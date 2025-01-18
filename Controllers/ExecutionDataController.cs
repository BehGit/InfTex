using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TimescaleDataApi.Data;
using TimescaleDataApi.Models;
using CsvHelper;
using CsvHelper.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace TimescaleDataApi.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ExecutionDataController : Controller
	{
		private readonly ApplicationDbContext _context;

		public ExecutionDataController(ApplicationDbContext context)
		{
			_context = context;
		}

// ---------------------------- 1 Метод

		[HttpGet]
		[Route("take-csv")]
		public async Task<IActionResult> UploadCsv(IFormFile file)
		{
			if (file == null || file.Length == 0)
				return BadRequest("Файл не выбран");

			if (!file.FileName.EndsWith(".csv"))
				return BadRequest("Неверный формат файла");

			var values = new List<ExecutionData>();

			using (var reader = new StreamReader(file.OpenReadStream()))
			using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
			{
				var records = csv.GetRecords<ExecutionData>().ToList();

				if (records.Count < 1 || records.Count > 10000)
					return BadRequest("Количество строк от 1 до 10000");

				foreach (var record in records)
				{

					if (!DateTime.TryParse(record.Date, out var parsedDate) ||
						parsedDate > DateTime.UtcNow ||
						parsedDate < new DateTime(2000, 1, 1))
						return BadRequest("Дата должна быть в диапазоне от 01.01.2000 до текущего времени");

					if (record.ExecutionTime < 0)
						return BadRequest("Время выполнения не может быть меньше 0");

					if (record.Value < 0)
						return BadRequest("Значение показателя не может быть меньше 0");

					values.Add(record);
				}
	
			}

			var existingResults = await _context.Results.FirstOrDefaultAsync(r => r.FileName == file.FileName);
			if (existingResults != null)
			{
				_context.Results.Remove(existingResults);
				var existingValues = await _context.Values.Where(v => v.Date == existingResults.MinDate).ToListAsync();
				_context.Values.RemoveRange(existingValues);
			}

			await _context.Values.AddRangeAsync(values);
			await _context.SaveChangesAsync();

			var result = new Models.Results
			{
				FileName = file.FileName,
				DeltaTime = (values.Max(v => DateTime.Parse(v.Date)) - values.Min(v => DateTime.Parse(v.Date))).TotalSeconds,
				MinDate = values.Min(v => v.Date),
				AverageExecutionTime = values.Average(v => v.ExecutionTime),
				AverageValue = values.Average(v => v.Value),
				MedianValue = CalculateMedian(values.Select(v => v.Value).ToList()),
				MaxValue = values.Max(v => v.Value),
				MinValue = values.Min(v => v.Value),
			};

			await _context.Results.AddAsync(result);
			await _context.SaveChangesAsync();

			return Ok("Данные загружены");
		}

		private double CalculateMedian(List<double> values)
		{
			int count = values.Count();
			var sortedValues = values.OrderBy(n => n).ToList();
			if (count % 2 == 0)
			{
				return (sortedValues[count / 2 - 1] + sortedValues[count / 2]) / 2.0;
			}
			return sortedValues[count / 2];
		}

// ------------------- 2 Метод

		[HttpGet("get-results")]
		public async Task<IActionResult> GetResults(string fileName, string startDate, string endDate, double? avgValueMin, double? avgValueMax, double? avgExecutionTimeMin, double? avgExecutionTimeMax)
		{
			var query = _context.Results.AsQueryable();

			if (!string.IsNullOrEmpty(fileName))
				query = query.Where(r => r.FileName.Contains(fileName));

			if (!string.IsNullOrEmpty(startDate))
				query = query.Where(r => DateTime.Parse(r.MinDate) >= DateTime.Parse(startDate));

			if (!string.IsNullOrEmpty(endDate))
				query = query.Where(r => DateTime.Parse(r.MinDate) <= DateTime.Parse(endDate));

			if (avgValueMin.HasValue)
				query = query.Where(r => r.AverageValue >= avgValueMin.Value);

			if (avgValueMax.HasValue)
				query = query.Where(r => r.AverageValue <= avgValueMax.Value);

			if (avgExecutionTimeMin.HasValue)
				query = query.Where(r => r.AverageExecutionTime >= avgExecutionTimeMin.Value);

			if (avgExecutionTimeMax.HasValue)
				query = query.Where(r => r.AverageExecutionTime <= avgExecutionTimeMax.Value);

			var results = await query.ToListAsync();
			return Ok(results);
		}

// ---------------------- 3 Метод

		[HttpGet("last-values")]
		public async Task<IActionResult> GetLastValues(string fileName)
		{
			var lastValues = await _context.Values
				.Where(v => v.Date == fileName)
				.OrderByDescending(v => DateTime.Parse(v.Date))
				.Take(10)
				.ToListAsync();

			return Ok(lastValues);
		}
	}
}

