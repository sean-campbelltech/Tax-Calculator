using Microsoft.Extensions.DependencyInjection;

namespace Campbelltech.TaxCalculation.Domain.Calculations
{
    public interface ITaxCalculationFactory
    {
        /// <summary>
        /// Factory method for retrieving the correct ITaxCalculation implementation using DI to register 
        /// a Func which returns the correct ITaxCalculation implementation depending on the tax type.
        /// </summary>
        /// <param name="services">IServiceCollection which specifies the contract for a collection of service descriptors.</param>
        /// <returns>ITaxCalculation</returns>
        ITaxCalculation Create(IServiceCollection services);
    }
}