using System;
using System.Net;
using System.Threading.Tasks;
using Campbelltech.TaxCalculation.Domain.Calculations;
using Campbelltech.TaxCalculation.Domain.Repositories;
using Campbelltech.TaxCalculation.Domain.Types;
using Campbelltech.TaxCalculation.DTO.Requests;
using Campbelltech.TaxCalculation.DTO.Responses;
using Microsoft.Extensions.Logging;

namespace Campbelltech.TaxCalculation.Domain.Services
{
    public class TaxCalculationService : ITaxCalculationService
    {
        private readonly Func<TaxType, ITaxCalculation> _taxCalculationResolver;
        private readonly IPostalCodeTaxRepository _postalCodeTaxRepository;
        private readonly ILogger<TaxCalculationService> _logger;

        public TaxCalculationService(
            Func<TaxType, ITaxCalculation> taxCalculationResolver,
            IPostalCodeTaxRepository postalCodeTaxRepository,
            ILogger<TaxCalculationService> logger)
        {
            _postalCodeTaxRepository = postalCodeTaxRepository;
            _taxCalculationResolver = taxCalculationResolver;
            _logger = logger;
        }

        /// <summary>
        /// Main method that facilitates the tax calculation request.
        /// </summary>
        /// <param name="request">TaxCalculationRequest object</param>
        /// <returns>HTTP Status Code + TaxCalculationResponse</returns>
        public async Task<Tuple<HttpStatusCode, TaxCalculationResponse>> CalculateAsync(TaxCalculationRequest request)
        {
            try
            {
                var postalCode = request.PostalCode;

                // first retrieve the tax type for the given postal Code
                var taxType = await _postalCodeTaxRepository.GetAsync(request.PostalCode);

                // if unknown, it means it is a client error, thus respond with 400 - Bad Request
                if (taxType == TaxType.Unknown)
                    return new Tuple<HttpStatusCode, TaxCalculationResponse>(
                        HttpStatusCode.BadRequest,
                        new TaxCalculationResponse
                        {
                            Message = $"The {nameof(postalCode)}: {postalCode} that you have provided is not a valid {nameof(postalCode)}"
                        });

                // then, let factory method resolve the correct implementation of ITaxCalculation and calculate tax
                var taxCalculation = _taxCalculationResolver(taxType);
                var taxAmount = await taxCalculation.CalculateAsyc(request.AnnualIncome);

                throw new NotImplementedException();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error while trying to calculate tax for postal code - {request?.PostalCode ?? ("Unknown")}.");

                // rethrow to preserve stack details
                throw;
            }
        }
    }
}
