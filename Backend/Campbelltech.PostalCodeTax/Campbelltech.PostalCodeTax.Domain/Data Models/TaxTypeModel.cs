using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Campbelltech.PostalCodeTax.Domain.Data_Models
{
    [Table("TaxType")]
    public class TaxTypeModel
    {
        [Key]
        public int TaxTypeId { get; set; }
        public string Description { get; set; }
    }
}