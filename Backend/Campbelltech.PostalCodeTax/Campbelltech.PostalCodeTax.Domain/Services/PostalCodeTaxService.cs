using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Campbelltech.PostalCodeTax.DTO.Responses;
using Campbelltech.PostalCodeTax.Domain.Mapping;
using Campbelltech.PostalCodeTax.Domain.Repositories;

namespace Campbelltech.PostalCodeTax.Domain.Services
{
    public class PostalCodeTaxService : IPostalCodeTaxService
    {
        private readonly IPostalCodeTaxRepository _postalCodeTaxRepository;
        private readonly ILogger<PostalCodeTaxService> _logger;
        private readonly IResponseMapper _responseMapper;

        public PostalCodeTaxService(
            IPostalCodeTaxRepository postalCodeTaxRepository,
            ILogger<PostalCodeTaxService> logger,
            IResponseMapper responseMapper)
        {
            _postalCodeTaxRepository = postalCodeTaxRepository;
            _responseMapper = responseMapper;
            _logger = logger;
        }

        /// <summary>
        /// Main service method that facilitates the postal code taxes retrieval request
        /// </summary>
        /// <returns>PostalCodeTaxResponse</returns>
        public async Task<PostalCodeTaxResponse> ListAsync()
        {
            try
            {
                var postalCodeTaxes = await _postalCodeTaxRepository.ListAsync();
                var response = _responseMapper.Map(postalCodeTaxes);

                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error while trying to retrieve a list of postal codes with their corresponding tax calculation types.");

                // rethrow to preserve stack details
                throw;
            }
        }
    }
}