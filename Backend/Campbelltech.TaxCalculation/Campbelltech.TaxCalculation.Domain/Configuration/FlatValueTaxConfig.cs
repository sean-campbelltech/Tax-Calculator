namespace Campbelltech.TaxCalculation.Domain.Configuration
{
    public class FlatValueTaxConfig
    {
        public decimal FlatValue { get; set; }
        public decimal MinAnnualIncome { get; set; }
        public decimal Rate { get; set; }
    }
}