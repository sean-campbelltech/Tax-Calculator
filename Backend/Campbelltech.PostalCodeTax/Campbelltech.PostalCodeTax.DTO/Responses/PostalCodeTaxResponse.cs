using System.Collections.Generic;

namespace Campbelltech.PostalCodeTax.DTO.Responses
{
    public class PostalCodeTaxResponse
    {
        public List<Postal_Code_Taxes.PostalCodeTax> PostalCodeTaxes { get; set; }
        public string Message { get; set; }
    }
}