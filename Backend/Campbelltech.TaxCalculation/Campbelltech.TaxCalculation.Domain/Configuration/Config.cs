namespace Campbelltech.TaxCalculation.Domain.Configuration
{
    public class Config
    {
        public string SqlConnectionString { get; set; }
        public FlatValueTaxConfig FlatValueTax { get; set; }
        public decimal FlatRate { get; set; }
    }
}