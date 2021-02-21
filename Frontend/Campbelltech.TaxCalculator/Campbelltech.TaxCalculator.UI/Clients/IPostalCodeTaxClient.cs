using System.Threading.Tasks;
using Campbelltech.PostalCodeTax.DTO.Responses;

namespace Campbelltech.TaxCalculator.UI.Clients
{
    public interface IPostalCodeTaxClient
    {
        Task<PostalCodeTaxResponse> GetAsync();
    }
}