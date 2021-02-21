using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace Campbelltech.TaxCalculator.UI.Clients
{
    public class GenericHttpClient<T> : IGenericHttpClient<T> where T : class
    {
        private readonly HttpClient _client = new HttpClient();
        private readonly Uri _endpointUri;

        public GenericHttpClient(Uri endpointUri)
        {
            if (endpointUri == null)
            {
                throw new UriFormatException("A valid URI is required.");
            }

            _endpointUri = endpointUri;
        }

        /// <summary>
        /// Generic GET method that makes an HTTP GET request to the given endpoint URI
        /// </summary>
        /// <returns>T</returns>
        public async Task<T> GetAsync()
        {
            var response = await _client.GetAsync(_endpointUri);

            if (response.StatusCode == HttpStatusCode.NotFound) return null;

            return response.Content.ReadAsAsync<T>().Result as T;
        }

        /// <summary>
        /// Generic POST method that makes an HTTP POST request to the given endpoint URI
        /// </summary>
        /// <param name="b">Generic JSON request body</param>
        /// <typeparam name="B">Generic type that will be serialised as a JSON request body</typeparam>
        /// <returns>T</returns>
        public async Task<T> PostAsync<B>(B b)
        {
            var response = await _client.PostAsJsonAsync(_endpointUri, b);

            if (response.StatusCode == HttpStatusCode.NotFound) return null;

            return response.Content.ReadAsAsync<T>().Result as T;
        }

        /// <summary>
        /// Dispose HTTP client
        /// </summary>
        public void Dispose()
        {
            _client.Dispose();
        }
    }
}