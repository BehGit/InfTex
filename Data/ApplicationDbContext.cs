using Microsoft.EntityFrameworkCore;
using TimescaleDataApi.Models;

namespace TimescaleDataApi.Data
{
	public class ApplicationDbContext:DbContext
	{
		public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

		public DbSet<ExecutionData> Values { get; set; }
		public DbSet<Models.Results> Results { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<ExecutionData>()
				.ToTable("values")
				.HasKey(e => e.Date);

			modelBuilder.Entity<Models.Results>()
				.ToTable("results")
				.HasKey(r => r.FileName);
		}
	}
}
