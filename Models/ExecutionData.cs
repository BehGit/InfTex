using System;

namespace TimescaleDataApi.Models
{
	public class ExecutionData
	{
		public int Id { get; set; }
		public string? Date { get; set; }
		public double ExecutionTime { get; set; }
		public double Value { get; set; }
	}
}
