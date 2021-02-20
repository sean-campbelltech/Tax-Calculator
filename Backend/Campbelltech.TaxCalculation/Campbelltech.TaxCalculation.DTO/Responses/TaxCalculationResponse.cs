namespace Campbelltech.TaxCalculation.DTO.Responses
{
    public class TaxCalculationResponse
    {
        public decimal TaxAmount { get; set; }
        public string Message { get; set; }
        public string CalculationTypeUsed { get; set; }
    }
}