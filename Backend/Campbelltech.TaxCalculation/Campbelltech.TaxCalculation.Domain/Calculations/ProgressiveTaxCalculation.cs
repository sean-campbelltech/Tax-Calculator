using System;
using System.Linq;
using System.Threading.Tasks;
using Campbelltech.TaxCalculation.Domain.Repositories;
using Microsoft.Extensions.Logging;

namespace Campbelltech.TaxCalculation.Domain.Calculations
{
    public class ProgressiveTaxCalculation : ITaxCalculation
    {
        private readonly ILogger<ProgressiveTaxCalculation> _logger;
        private readonly IProgressiveTaxRateRepository _progressiveRatesRepository;

        public ProgressiveTaxCalculation(ILogger<ProgressiveTaxCalculation> logger, IProgressiveTaxRateRepository progressiveRatesRepository)
        {
            _logger = logger;
            _progressiveRatesRepository = progressiveRatesRepository;
        }

        /// <summary>
        /// Calculates the progressive tax for the given annual income
        /// </summary>
        /// <param name="annualIncome">Total income earned per annum</param>
        /// <returns>Tax amount</returns>
        public async Task<decimal> CalculateAsyc(decimal annualIncome)
        {
            try
            {
                var progressiveTaxRates = await _progressiveRatesRepository.ListAsync();

                if (progressiveTaxRates == null || !progressiveTaxRates.Any())
                    throw new ArgumentNullException("Unable to retrieve progressive tax rates from the database.");

                var taxAmmount = 0m;

                foreach (var rate in progressiveTaxRates)
                {
                    if (annualIncome > rate.FromAmount)
                    {
                        var rateBracket = rate.ToAmount - rate.FromAmount;
                        var amountTaxableAtRate = Math.Min(rateBracket, annualIncome - rate.FromAmount);
                        var taxAtRate = amountTaxableAtRate * rate.Rate;
                        taxAmmount += taxAtRate;
                    }
                }

                return taxAmmount;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error while trying to calculate progressive tax for and annual income - {annualIncome}.");

                // rethrow to preserve stack details
                throw;
            }
        }
    }
}