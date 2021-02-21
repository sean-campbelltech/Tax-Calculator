using System.Threading.Tasks;
using Campbelltech.PostalCodeTax.DTO.Responses;

namespace Campbelltech.PostalCodeTax.Domain.Services
{
    public interface IPostalCodeTaxService
    {
        Task<PostalCodeTaxResponse> ListAsync();
    }
}