using System.Threading.Tasks;
using Campbelltech.TaxCalculation.Domain.Types;

namespace Campbelltech.TaxCalculation.Domain.Repositories
{
    public interface IPostalCodeTaxRepository
    {
        Task<TaxType> GetAsync(string postalCode);
    }
}