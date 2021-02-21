using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Campbelltech.TaxCalculation.Domain.Types;

namespace Campbelltech.TaxCalculation.Domain.Data_Models
{
    [Table("PostalCodeTax")]
    public class PostalCodeTaxModel
    {
        [Key]
        public string PostalCode { get; set; }
        public TaxType TaxTypeId { get; set; }
    }
}