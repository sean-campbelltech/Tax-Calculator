using System.Threading.Tasks;
using System.Collections.Generic;
using Campbelltech.PostalCodeTax.Domain.Data_Models;

namespace Campbelltech.PostalCodeTax.Domain.Repositories
{
    public interface IPostalCodeTaxRepository
    {
        /// <summary>
        /// Retrieves all the postal codes with their corresponding tax calculation types 
        /// <returns>List of PostalCodeTaxModel</returns>
        Task<List<PostalCodeTaxModel>> ListAsync();
    }
}