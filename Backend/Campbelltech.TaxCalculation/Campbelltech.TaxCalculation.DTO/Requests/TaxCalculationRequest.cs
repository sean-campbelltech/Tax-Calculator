using System.ComponentModel.DataAnnotations;

namespace Campbelltech.TaxCalculation.DTO.Requests
{
    public class TaxCalculationRequest
    {
        [Required]
        [StringLength(4)]
        public string PostalCode { get; set; }

        [Required]
        [Range(1, double.MaxValue, ErrorMessage = "Please enter an annual amount that is bigger than 0.")]
        public decimal AnnualIncome { get; set; }
    }
}