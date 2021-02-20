using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Campbelltech.TaxCalculation.Domain.Data_Models
{
    [Table("ProgressiveTaxRate")]
    public class ProgressiveTaxRateModel
    {
        [Key]
        public decimal Rate { get; set; }
        public decimal FromAmount { get; set; }
        public decimal ToAmount { get; set; }
    }
}