using System.Threading.Tasks;
using Campbelltech.PostalCodeTax.DTO.Responses;

namespace Campbelltech.PostalCodeTax.Domain.Services
{
    public interface IPostalCodeTaxService
    {
        /// <summary>
        /// Main service method that facilitates the postal code taxes retrieval request
        /// </summary>
        /// <returns>PostalCodeTaxResponse</returns>
        Task<PostalCodeTaxResponse> ListAsync();
    }
}