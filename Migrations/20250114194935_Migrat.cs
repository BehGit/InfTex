using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TimescaleDataApi.Migrations
{
    /// <inheritdoc />
    public partial class Migrat : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "results",
                columns: table => new
                {
                    FileName = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    MinDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    MaxDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TimeDelta = table.Column<double>(type: "float", nullable: false),
                    AverageExecutionTime = table.Column<double>(type: "float", nullable: false),
                    AverageValue = table.Column<double>(type: "float", nullable: false),
                    MedianValue = table.Column<double>(type: "float", nullable: false),
                    MaxValue = table.Column<double>(type: "float", nullable: false),
                    MinValue = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_results", x => x.FileName);
                });

            migrationBuilder.CreateTable(
                name: "values",
                columns: table => new
                {
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ExecutionTime = table.Column<double>(type: "float", nullable: false),
                    Value = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_values", x => x.Date);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "results");

            migrationBuilder.DropTable(
                name: "values");
        }
    }
}
