using System.Collections.Generic;
using Campbelltech.PostalCodeTax.Domain.Data_Models;
using Campbelltech.PostalCodeTax.DTO.Responses;

namespace Campbelltech.PostalCodeTax.Domain.Mapping
{
    public interface IResponseMapper
    {
        /// <summary>
        /// Maps a list of PostalCodeTaxModel to the PostalCodeTaxResponse object
        /// </summary>
        /// <param name="postalCodeTaxModels">List of PostalCodeTaxModel</param>
        /// <returns>PostalCodeTaxResponse</returns>
        PostalCodeTaxResponse Map(List<PostalCodeTaxModel> postalCodeTaxModels);
    }
}