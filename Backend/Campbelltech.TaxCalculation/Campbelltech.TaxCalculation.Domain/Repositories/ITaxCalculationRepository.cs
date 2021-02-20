using System.Threading.Tasks;
using Campbelltech.TaxCalculation.Domain.Data_Models;

namespace Campbelltech.TaxCalculation.Domain.Repositories
{
    public interface ITaxCalculationRepository
    {
        /// <summary>
        /// Stores the tax results in the database
        /// </summary>
        /// <param name="model">TaxCalculationModel</param>
        /// <returns>Boolean result</returns>
        Task<bool> SaveAsync(TaxCalculationModel model);
    }
}