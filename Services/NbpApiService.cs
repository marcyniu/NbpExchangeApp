using System.Text.Json;
using NbpExchangeApp.BusinessModels;
using Microsoft.Extensions.Caching.Memory;

namespace NbpExchangeApp.Services
{
    public class NbpApiService
    {
        private readonly HttpClient _httpClient;
        private readonly IMemoryCache _memoryCache;

        public NbpApiService(HttpClient httpClient, IMemoryCache memoryCache)
        {
            _httpClient = httpClient;
            _memoryCache = memoryCache;
        }

        public async Task<ExchangeRates?> GetExchangeTableRatesAsync(string tableType = "A")
        {
            string cacheKeyOrRequestString = $"https://api.nbp.pl/api/exchangerates/tables/{tableType}?format=json";

            ExchangeRates? exchangeRates = null;
            if (!_memoryCache.TryGetValue(cacheKeyOrRequestString, out exchangeRates))
            {
                var response = await _httpClient.GetAsync(cacheKeyOrRequestString);
                
                // Add code to get response content
                if (!response.IsSuccessStatusCode)
                {
                    throw new HttpRequestException($"Failed to fetch exchange rates. Status code: {response.StatusCode}");
                }

                var content = await response.Content.ReadAsStringAsync();
                exchangeRates = JsonSerializer.Deserialize<List<ExchangeRates>>(content)?.FirstOrDefault() ?? new ExchangeRates();

                if (exchangeRates == null)
                {
                    throw new HttpRequestException("Failed to deserialize exchange rates.");
                }

                var cacheEntryOptions = new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromMinutes(60)); // Cache for 60 minutes

                _memoryCache.Set(cacheKeyOrRequestString, exchangeRates, cacheEntryOptions);
            }

            return exchangeRates;
        }
    }
}
