using System.Threading.Tasks;

namespace Campbelltech.TaxCalculation.Domain.Calculations
{
    public interface ITaxCalculation
    {
        /// <summary>
        /// Calculates the tax that is payable for the given annual income
        /// </summary>
        /// <param name="annualIncome">Total income earned per annum</param>
        /// <returns>Tax amount</returns>
        Task<decimal> CalculateAsyc(decimal annualIncome);
    }
}