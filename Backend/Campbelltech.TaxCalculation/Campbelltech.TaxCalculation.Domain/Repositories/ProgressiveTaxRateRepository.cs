using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.EntityFrameworkCore;
using Campbelltech.TaxCalculation.Domain.Data_Models;
using Campbelltech.TaxCalculation.Domain.Configuration;

namespace Campbelltech.TaxCalculation.Domain.Repositories
{
    public class ProgressiveTaxRateRepository : IProgressiveTaxRateRepository
    {
        private readonly ILogger<ProgressiveTaxRateRepository> _logger;
        private readonly string _connectionString;

        public ProgressiveTaxRateRepository(ILogger<ProgressiveTaxRateRepository> logger, IOptions<Config> config)
        {
            _logger = logger;
            _connectionString = config?.Value?.SqlConnectionString;
        }

        /// <summary>
        /// Returns all the progressive tax rates in the database
        /// </summary>
        /// <returns>List of ProgressiveTaxRateModel</returns>
        public async Task<List<ProgressiveTaxRateModel>> ListAsync()
        {
            try
            {
                if (string.IsNullOrEmpty(_connectionString))
                    throw new ArgumentNullException($"The {nameof(ProgressiveTaxRateRepository)} was unable to retrieve the database connection string from config.");

                using (var context = new DataContext(_connectionString))
                {
                    var progressiveTaxRates = await context.ProgressiveTaxRates.AsNoTracking()
                                                 .OrderBy(x => x.Rate)
                                                ?.ToListAsync();

                    return progressiveTaxRates;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while trying to retrieve the progressive tax rates from the database.");

                // rethrow to preserve stack details
                throw;
            }
        }
    }
}