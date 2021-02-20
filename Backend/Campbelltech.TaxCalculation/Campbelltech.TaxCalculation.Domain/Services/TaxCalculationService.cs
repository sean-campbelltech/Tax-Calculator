using System;
using Campbelltech.TaxCalculation.Domain.Calculations;
using Campbelltech.TaxCalculation.Domain.Types;
using Microsoft.Extensions.Logging;

namespace Campbelltech.TaxCalculation.Domain.Services
{
    public class TaxCalculationService : ITaxCalculationService
    {
        private readonly Func<TaxType, ITaxCalculation> _taxCalculation;
        private readonly ILogger<TaxCalculationService> _logger;

        public TaxCalculationService(Func<TaxType, ITaxCalculation> taxCalculation, ILogger<TaxCalculationService> logger)
        {
            _taxCalculation = taxCalculation;
            _logger = logger;
        }

    }
}
