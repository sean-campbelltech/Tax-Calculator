using System;
using System.Linq;
using System.Threading.Tasks;
using Campbelltech.TaxCalculation.Domain.Configuration;
using Campbelltech.TaxCalculation.Domain.Data_Models;
using Campbelltech.TaxCalculation.Domain.Types;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Campbelltech.TaxCalculation.Domain.Repositories
{
    public class PostalCodeTaxRepository : IPostalCodeTaxRepository
    {
        private readonly ILogger<PostalCodeTaxRepository> _logger;
        private readonly string _connectionString;

        public PostalCodeTaxRepository(ILogger<PostalCodeTaxRepository> logger, IOptions<Config> config)
        {
            _logger = logger;
            _connectionString = config?.Value?.SqlConnectionString;
        }

        /// <summary>
        /// Retrieves the tax type for a given postal code
        /// </summary>
        /// <param name="postalCode">Postal code of an individual</param>
        /// <returns>TaxType enum</returns>
        public async Task<TaxType> GetAsync(string postalCode)
        {
            try
            {
                if (string.IsNullOrEmpty(_connectionString))
                    throw new ArgumentNullException($"The {nameof(PostalCodeTaxRepository)} was unable to retrieve the database connection string from config.");

                using (var context = new DataContext(_connectionString))
                {
                    var postalCodeTax = await context.PostalCodeTaxes.AsNoTracking()
                                                .Include(i => i.TaxType).AsNoTracking()
                                                .Where(x => x.PostalCode.Equals(postalCode))
                                                .FirstOrDefaultAsync();

                    var taxType = postalCodeTax?.TaxType?.TaxTypeId ?? TaxType.Unknown;

                    return taxType;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error while trying to retrieve tax type for postal code - {postalCode}.");

                // re-throw to preserve the original call stack
                throw;
            }
        }
    }
}