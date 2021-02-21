using System.Threading.Tasks;
using System.Collections.Generic;
using Campbelltech.TaxCalculation.Domain.Data_Models;

namespace Campbelltech.TaxCalculation.Domain.Repositories
{
    public interface IProgressiveTaxRateRepository
    {
        Task<List<ProgressiveTaxRateModel>> ListAsync();
    }
}