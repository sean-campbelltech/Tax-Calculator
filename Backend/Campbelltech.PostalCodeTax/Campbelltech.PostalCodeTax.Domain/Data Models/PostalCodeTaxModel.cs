using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Campbelltech.PostalCodeTax.Domain.Data_Models
{
    [Table("PostalCodeTax")]
    public class PostalCodeTaxModel
    {
        [Key]
        public string PostalCode { get; set; }

        [ForeignKey("TaxTypeId")]
        public TaxTypeModel TaxType { get; set; }
    }
}
