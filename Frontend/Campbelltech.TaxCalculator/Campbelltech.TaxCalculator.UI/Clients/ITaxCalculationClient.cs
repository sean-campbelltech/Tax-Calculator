using System.Threading.Tasks;
using Campbelltech.TaxCalculation.DTO.Responses;

namespace Campbelltech.TaxCalculator.UI.Clients
{
    public interface ITaxCalculationClient
    {
        Task<TaxCalculationResponse> CalculateAsync(string postalCode, decimal annualIncome);
    }
}