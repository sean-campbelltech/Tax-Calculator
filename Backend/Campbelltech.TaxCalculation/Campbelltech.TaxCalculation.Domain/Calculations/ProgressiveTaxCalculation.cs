using System.Threading.Tasks;

namespace Campbelltech.TaxCalculation.Domain.Calculations
{
    public class ProgressiveTaxCalculation : ITaxCalculation
    {
        /// <summary>
        /// Calculates the progressive tax for the given annual income
        /// </summary>
        /// <param name="annualIncome">Total income earned per annum</param>
        /// <returns>Tax amount</returns>
        public Task<double> CalculateAsyc(double annualIncome)
        {
            throw new System.NotImplementedException();
        }
    }
}