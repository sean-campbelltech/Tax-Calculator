using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Campbelltech.TaxCalculation.Domain.Data_Models;
using Campbelltech.TaxCalculation.Domain.Configuration;

namespace Campbelltech.TaxCalculation.Domain.Repositories
{
    public class TaxCalculationRepository : ITaxCalculationRepository
    {
        private readonly ILogger<TaxCalculationRepository> _logger;
        private readonly string _connectionString;

        public TaxCalculationRepository(ILogger<TaxCalculationRepository> logger, IOptions<Config> config)
        {
            _logger = logger;
            _connectionString = config?.Value?.SqlConnectionString;
        }

        /// <summary>
        /// Stores the tax calculation results in the database
        /// </summary>
        /// <param name="model">TaxCalculationModel</param>
        /// <returns>Boolean result</returns>
        public async Task<bool> SaveAsync(TaxCalculationModel model)
        {
            try
            {
                if (string.IsNullOrEmpty(_connectionString))
                    throw new ArgumentNullException($"The {nameof(TaxCalculationRepository)} was unable to retrieve the database connection string from config.");

                using (var context = new DataContext(_connectionString))
                {
                    context.Add(model);
                    var result = await context.SaveChangesAsync();

                    return result > 0;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while trying to store tax calculation.");

                // rethrow to preserve stack details
                throw;
            }
        }
    }
}