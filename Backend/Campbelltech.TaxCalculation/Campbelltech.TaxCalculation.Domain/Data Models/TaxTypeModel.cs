using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Campbelltech.TaxCalculation.Domain.Types;

namespace Campbelltech.TaxCalculation.Domain.Data_Models
{
    [Table("TaxType")]
    public class TaxTypeModel
    {
        [Key]
        public TaxType TaxTypeId { get; set; }
        public string Description { get; set; }
    }
}