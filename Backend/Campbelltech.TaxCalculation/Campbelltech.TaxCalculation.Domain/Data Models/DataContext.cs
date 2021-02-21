using System;
using Microsoft.EntityFrameworkCore;

namespace Campbelltech.TaxCalculation.Domain.Data_Models
{
    public class DataContext : DbContext
    {
        private readonly string _connectionString;

        public DataContext(string connectionString)
        {
            _connectionString = connectionString;
        }

        public DbSet<PostalCodeTaxModel> PostalCodeTaxes { get; set; }
        public DbSet<TaxCalculationModel> TaxCalculations { get; set; }
        public DbSet<ProgressiveTaxRateModel> ProgressiveTaxRates { get; set; }

        /// <summary>
        /// Used to configure the database context
        /// </summary>
        /// <param name="optionsBuilder">Provides a simple API surface for configuring DbContextOptions</param>
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder
                .UseSqlServer(
                    _connectionString,
                    options => options.EnableRetryOnFailure(
                        maxRetryCount: 10,
                        maxRetryDelay: TimeSpan.FromSeconds(30),
                        errorNumbersToAdd: null
                    ));
        }

        /// <summary>
        /// Used to configure entity models
        /// </summary>
        /// <param name="modelBuilder">Provides a simple API surface for configuring a Microsoft.EntityFrameworkCore.Metadata.IMutableModel that defines the shape of your entities, the relationships between them, and how they map to the database.</param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PostalCodeTaxModel>()
                .Property(c => c.TaxTypeId)
                .HasConversion<int>();
        }
    }
}