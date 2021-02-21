using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Campbelltech.TaxCalculation.Domain.Types;
using Campbelltech.TaxCalculation.DTO.Requests;
using Campbelltech.TaxCalculation.DTO.Responses;
using Campbelltech.TaxCalculation.Domain.Mapping;
using Campbelltech.TaxCalculation.Domain.Repositories;
using Campbelltech.TaxCalculation.Domain.Calculations;

namespace Campbelltech.TaxCalculation.Domain.Services
{
    public class TaxCalculationService : ITaxCalculationService
    {
        private readonly Func<TaxType, ITaxCalculation> _taxCalculationResolver;
        private readonly ITaxCalculationRepository _taxCalculationRepository;
        private readonly IPostalCodeTaxRepository _postalCodeTaxRepository;
        private readonly ILogger<TaxCalculationService> _logger;

        public TaxCalculationService(
            Func<TaxType, ITaxCalculation> taxCalculationResolver,
            ITaxCalculationRepository taxCalculationRepository,
            IPostalCodeTaxRepository postalCodeTaxRepository,
            ILogger<TaxCalculationService> logger)
        {
            _taxCalculationRepository = taxCalculationRepository;
            _postalCodeTaxRepository = postalCodeTaxRepository;
            _taxCalculationResolver = taxCalculationResolver;
            _logger = logger;
        }

        /// <summary>
        /// Main service method that facilitates the tax calculation request.
        /// </summary>
        /// <param name="request">TaxCalculationRequest object</param>
        /// <param name="requestBy">Username of person that requested the tax calculation</param>
        /// <returns>HTTP Status Code + TaxCalculationResponse</returns>
        public async Task<Tuple<HttpStatusCode, TaxCalculationResponse>> CalculateAsync(TaxCalculationRequest request, string requestedBy)
        {
            try
            {
                var postalCode = request.PostalCode;

                // first retrieve the tax type for the given postal Code
                var taxType = await _postalCodeTaxRepository.GetAsync(postalCode);

                // if unknown, it means it is a client error, thus respond with 400 - Bad Request
                if (taxType == TaxType.Unknown)
                    return new Tuple<HttpStatusCode, TaxCalculationResponse>(
                        HttpStatusCode.BadRequest,
                        new TaxCalculationResponse
                        {
                            Message = $"The {nameof(postalCode)}: {postalCode} that was provided is not a valid {nameof(postalCode)}"
                        });

                // let the factory method resolve the correct implementation of ITaxCalculation without exposing the creation logic of the factory to the client 
                var taxCalculation = _taxCalculationResolver(taxType);
                var annualIncome = request.AnnualIncome;

                // execute main tax calculation logic
                var taxAmount = await taxCalculation.CalculateAsyc(annualIncome);

                // use a builder to separate the construction of the TaxCalculationModel from its representation
                var model = new TaxCalculationModelBuilder()
                            .AddPostalCode(postalCode)
                            .AddAnnualIncome(annualIncome)
                            .AddTaxAmount(taxAmount)
                            .AddRequestedBy(requestedBy)
                            .Build();

                // then store the tax results to the database
                var persisted = await _taxCalculationRepository.SaveAsync(model);

                // use a builder to separate the construction of the complex Tuple result object from its representation
                var result = new ResultBuilder(persisted)
                            .AddTaxAmount(taxAmount)
                            .AddTaxType(taxType)
                            .Build();

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error while trying to calculate tax for postal code - {request?.PostalCode ?? ("Unknown")} and annual income - {request.AnnualIncome}.");

                // rethrow to preserve stack details
                throw;
            }
        }
    }
}
