using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Campbelltech.TaxCalculation.Domain.Configuration;

namespace Campbelltech.TaxCalculation.Domain.Calculations
{
    public class FlatRateTaxCalculation : ITaxCalculation
    {
        private readonly ILogger<FlatRateTaxCalculation> _logger;
        private readonly decimal _flatRate;

        public FlatRateTaxCalculation(ILogger<FlatRateTaxCalculation> logger, IOptions<Config> config)
        {
            _logger = logger;
            _flatRate = config.Value.FlatRate;
        }

        /// <summary>
        /// Calculates the flat rate tax for the given annual income
        /// </summary>
        /// <param name="annualIncome">Total income earned per annum</param>
        /// <returns>Tax amount</returns>
        public async Task<decimal> CalculateAsyc(decimal annualIncome)
        {
            try
            {
                var taxAmount = annualIncome * (_flatRate / 100m);

                return await Task.FromResult(taxAmount);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error while trying to calculate flat tax rate for annual income - {annualIncome}.");

                // rethrow to preserve stack details
                throw;
            }
        }
    }
}