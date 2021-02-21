using System.Collections.Generic;
using System.Threading.Tasks;
using Campbelltech.PostalCodeTax.Domain.Data_Models;

namespace Campbelltech.PostalCodeTax.Domain.Repositories
{
    public interface IPostalCodeTaxRepository
    {
        Task<List<PostalCodeTaxModel>> ListAsync();
    }
}