using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using NbpExchangeApp.BusinessModels;

namespace NbpExchangeApp.Services
{
    public class NbpApiService
    {
        private readonly HttpClient _httpClient;

        public NbpApiService()
        {
            _httpClient = new HttpClient();
        }

        public NbpApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<ExchangeRates> GetExchangeRatesAsync()
        {
            var response = await _httpClient.GetAsync("https://api.nbp.pl/api/exchangerates/tables/A?format=json");
            
            // Add code to get response content
            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException($"Failed to fetch exchange rates. Status code: {response.StatusCode}");
            }

            var content = await response.Content.ReadAsStringAsync();
            var exchangeRates = JsonSerializer.Deserialize<List<ExchangeRates>>(content)?.FirstOrDefault();

            if (exchangeRates == null)
            {
                throw new HttpRequestException("Failed to deserialize exchange rates.");
            }

            return exchangeRates;
        }
    }
}
