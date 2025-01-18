using System;

namespace TimescaleDataApi.Models
{
	public class Results
	{
		public int Id { get; set; }	
		public string? FileName { get; set; } 
		public string? MinDate { get; set; }
		public string? MaxDate { get; set; }
		public double AverageExecutionTime { get; set; }
		public double AverageValue { get; set; }
		public double MedianValue { get; set; }
		public double MaxValue { get; set; }
		public double MinValue { get; set; }

		public double DeltaTime { get; set; }
	}
}
