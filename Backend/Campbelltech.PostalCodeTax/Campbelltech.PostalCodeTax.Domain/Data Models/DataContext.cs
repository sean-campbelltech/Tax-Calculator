using System;
using Microsoft.EntityFrameworkCore;

namespace Campbelltech.PostalCodeTax.Domain.Data_Models
{
    public class DataContext : DbContext
    {
        private readonly string _connectionString;

        public DataContext(string connectionString)
        {
            _connectionString = connectionString;
        }

        public DbSet<PostalCodeTaxModel> PostalCodeTaxes { get; set; }

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
    }
}