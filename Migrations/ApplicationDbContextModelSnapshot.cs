﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using TimescaleDataApi.Data;

#nullable disable

namespace TimescaleDataApi.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("TimescaleDataApi.Models.ExecutionData", b =>
                {
                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.Property<double>("ExecutionTime")
                        .HasColumnType("float");

                    b.Property<double>("Value")
                        .HasColumnType("float");

                    b.HasKey("Date");

                    b.ToTable("values", (string)null);
                });

            modelBuilder.Entity("TimescaleDataApi.Models.Results", b =>
                {
                    b.Property<string>("FileName")
                        .HasColumnType("nvarchar(450)");

                    b.Property<double>("AverageExecutionTime")
                        .HasColumnType("float");

                    b.Property<double>("AverageValue")
                        .HasColumnType("float");

                    b.Property<DateTime>("MaxDate")
                        .HasColumnType("datetime2");

                    b.Property<double>("MaxValue")
                        .HasColumnType("float");

                    b.Property<double>("MedianValue")
                        .HasColumnType("float");

                    b.Property<DateTime>("MinDate")
                        .HasColumnType("datetime2");

                    b.Property<double>("MinValue")
                        .HasColumnType("float");

                    b.Property<double>("TimeDelta")
                        .HasColumnType("float");

                    b.HasKey("FileName");

                    b.ToTable("results", (string)null);
                });
#pragma warning restore 612, 618
        }
    }
}
