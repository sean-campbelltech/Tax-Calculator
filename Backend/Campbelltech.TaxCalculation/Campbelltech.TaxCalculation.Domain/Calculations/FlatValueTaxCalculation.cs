using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Campbelltech.TaxCalculation.Domain.Configuration;

namespace Campbelltech.TaxCalculation.Domain.Calculations
{
    public class FlatValueTaxCalculation : ITaxCalculation
    {
        private readonly ILogger<FlatValueTaxCalculation> _logger;
        private readonly FlatValueTaxConfig _flatValueConfig;

        public FlatValueTaxCalculation(ILogger<FlatValueTaxCalculation> logger, IOptions<Config> config)
        {
            _logger = logger;
            _flatValueConfig = config.Value.FlatValueTax;
        }

        /// <summary>
        /// Calculates the flat value tax for the given annual income
        /// </summary>
        /// <param name="annualIncome">Total income earned per annum</param>
        /// <returns>Tax amount</returns>
        public async Task<decimal> CalculateAsyc(decimal annualIncome)
        {
            try
            {
                var taxAmount = annualIncome >= _flatValueConfig.MinAnnualIncome
                    ? _flatValueConfig.FlatValue
                    : annualIncome * (_flatValueConfig.Rate / 100m);

                return await Task.FromResult(taxAmount);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error while trying to calculate flat tax value for annual income - {annualIncome}.");

                // rethrow to preserve stack details
                throw;
            }
        }
    }
}