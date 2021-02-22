using System;
using System.Threading.Tasks;
using Campbelltech.TaxCalculation.DTO.Requests;
using Campbelltech.TaxCalculation.DTO.Responses;
using Campbelltech.TaxCalculator.UI.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Campbelltech.TaxCalculator.UI.Clients
{
    public class TaxCalculationClient : ITaxCalculationClient
    {
        private readonly ILogger<TaxCalculationClient> _logger;
        private readonly Uri _apiGatewayUri;

        public TaxCalculationClient(ILogger<TaxCalculationClient> logger, IOptions<Config> config)
        {
            _logger = logger;
            _apiGatewayUri = new Uri(config.Value.ApiGatewayUrl);
        }

        /// <summary>
        /// Makes an HTTP request to the TaxCalculation API to calculate the tax ammount for a given postal code and annual income
        /// </summary>
        /// <param name="postalCode">Postal code</param>
        /// <param name="annualIncome">Total annual income</param>
        /// <returns>TaxCalculationResponse</returns>
        public async Task<TaxCalculationResponse> CalculateAsync(string postalCode, decimal annualIncome)
        {
            try
            {
                var endpointUri = new Uri($"{_apiGatewayUri}api/v1/listPostalCodeTaxes");
                var request = new TaxCalculationRequest
                {
                    PostalCode = postalCode,
                    AnnualIncome = annualIncome
                };

                using (var client = new GenericHttpClient<TaxCalculationResponse>(endpointUri))
                {
                    return await client.PostAsync(request);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while trying to tax caluclation request to the PostalCodeTax API.");

                // rethrow to preserve stack details
                throw;
            }
        }
    }
}