using System;
using System.Net;
using System.Threading.Tasks;
using Campbelltech.TaxCalculation.DTO.Requests;
using Campbelltech.TaxCalculation.DTO.Responses;

namespace Campbelltech.TaxCalculation.Domain.Services
{
    public interface ITaxCalculationService
    {
        /// <summary>
        /// Main service method that facilitates the tax calculation request.
        /// </summary>
        /// <param name="request">TaxCalculationRequest object</param>
        /// <param name="requestBy">Username of person that requested the tax calculation</param>
        /// <returns>HTTP Status Code + TaxCalculationResponse</returns>
        Task<Tuple<HttpStatusCode, TaxCalculationResponse>> CalculateAsync(TaxCalculationRequest request, string requestedBy);
    }
}