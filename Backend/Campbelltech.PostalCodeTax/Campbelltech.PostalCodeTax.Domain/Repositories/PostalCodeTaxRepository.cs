using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.EntityFrameworkCore;
using Campbelltech.PostalCodeTax.Domain.Data_Models;
using Campbelltech.PostalCodeTax.Domain.Configuration;

namespace Campbelltech.PostalCodeTax.Domain.Repositories
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
        /// Retrieves all the postal codes with their corresponding tax calculation types 
        /// <returns>List of PostalCodeTaxModel</returns>
        public async Task<List<PostalCodeTaxModel>> ListAsync()
        {
            try
            {
                if (string.IsNullOrEmpty(_connectionString))
                    throw new ArgumentNullException($"The {nameof(PostalCodeTaxRepository)} was unable to retrieve the database connection string from config.");

                using (var context = new DataContext(_connectionString))
                {
                    var postalCodeTaxes = await context.PostalCodeTaxes.AsNoTracking()
                                                .Include(i => i.TaxType)
                                               ?.ToListAsync();

                    return postalCodeTaxes;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while trying to retrieve all postal code with their corresponding tax calculation types.");

                // rethrow to preserve stack details
                throw;
            }
        }
    }
}