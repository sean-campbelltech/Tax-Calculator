using System;
using System.Threading.Tasks;
using Campbelltech.PostalCodeTax.DTO.Responses;
using Campbelltech.TaxCalculator.UI.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Campbelltech.TaxCalculator.UI.Clients
{
    public class PostalCodeTaxClient : IPostalCodeTaxClient
    {
        private readonly ILogger<PostalCodeTaxClient> _logger;
        private readonly Uri _apiGatewayUri;

        public PostalCodeTaxClient(ILogger<PostalCodeTaxClient> logger, IOptions<Config> config)
        {
            _logger = logger;
            _apiGatewayUri = new Uri(config.Value.ApiGatewayUrl);
        }

        /// <summary>
        /// Makes a request to the PostalCodeTax API to return a list of postal codes with
        /// their corresponding tax calculation types
        /// </summary>
        /// <returns></returns>
        public async Task<PostalCodeTaxResponse> GetAsync()
        {
            try
            {
                var endpointUri = new Uri($"{_apiGatewayUri}api/v1/listPostalCodeTaxes");

                using (var client = new GenericHttpClient<PostalCodeTaxResponse>(endpointUri))
                {
                    return await client.GetAsync();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while trying to retrieve the postal codes with their corresponding tax types from the PostalCodeTax API.");

                // rethrow to preserve stack details
                throw;
            }
        }
    }
}