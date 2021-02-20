using System;
using System.Net;
using System.Threading.Tasks;
using Campbelltech.TaxCalculation.DTO.Requests;
using Campbelltech.TaxCalculation.DTO.Responses;

namespace Campbelltech.TaxCalculation.Domain.Services
{
    public interface ITaxCalculationService
    {
        Task<Tuple<HttpStatusCode, TaxCalculationResponse>> CalculateAsync(TaxCalculationRequest request);
    }
}