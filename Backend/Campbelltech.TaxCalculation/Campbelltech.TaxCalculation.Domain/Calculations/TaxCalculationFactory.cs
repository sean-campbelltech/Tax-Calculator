using System;
using Campbelltech.TaxCalculation.Domain.Types;
using Microsoft.Extensions.DependencyInjection;

namespace Campbelltech.TaxCalculation.Domain.Calculations
{
    public class TaxCalculationFactory : ITaxCalculationFactory
    {
        /// <summary>
        /// Factory method for retrieving the correct ITaxCalculation implementation using DI to register 
        /// a Func which returns the correct ITaxCalculation implementation depending on the tax type.
        /// </summary>
        /// <param name="services">IServiceCollection which specifies the contract for a collection of service descriptors.</param>
        /// <returns>ITaxCalculation</returns>
        public ITaxCalculation Create(IServiceCollection services)
        {
            // register all possible implementations of ITaxCalculation
            services.AddScoped<ProgressiveTaxCalculation>();
            services.AddScoped<FlatValueTaxCalculation>();
            services.AddScoped<FlatRateTaxCalculation>();

            services.AddTransient<Func<TaxType, ITaxCalculation>>(serviceProvider => key =>
            {
                switch (key)
                {
                    case TaxType.Progressive:
                        return serviceProvider.GetService<ProgressiveTaxCalculation>();

                    case TaxType.FlatValue:
                        return serviceProvider.GetService<FlatValueTaxCalculation>();

                    case TaxType.FlatRate:
                        return serviceProvider.GetService<FlatRateTaxCalculation>();

                    default:
                        return null;
                }
            });

            return null;
        }
    }
}