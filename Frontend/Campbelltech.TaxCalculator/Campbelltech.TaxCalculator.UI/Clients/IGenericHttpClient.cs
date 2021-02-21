using System;
using System.Threading.Tasks;

namespace Campbelltech.TaxCalculator.UI.Clients
{
    public interface IGenericHttpClient<T> : IDisposable
    {
        Task<T> GetAsync();
        Task<T> PostAsync<B>(B b);
    }
}