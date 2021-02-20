using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Campbelltech.TaxCalculation.Domain.Data_Models
{
    [Table("TaxCalculation")]
    public class TaxCalculationModel
    {
        [Key]
        public int CalculationId { get; set; }
        public DateTime CalculatedAt { get; set; }
        public string PostalCode { get; set; }
        public decimal AnnualIncome { get; set; }
        public decimal TaxAmount { get; set; }
        public string RequestedBy { get; set; }
    }
}