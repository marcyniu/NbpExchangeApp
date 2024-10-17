using System.Text.Json;
using NbpExchangeApp.BusinessModels;
using Microsoft.Extensions.Caching.Memory;

namespace NbpExchangeApp.Services
{
    public class NbpExchangeRatesService(IMemoryCache memoryCache)
    {

        private const string _apiHost = "https://api.nbp.pl";

        private readonly IMemoryCache _memoryCache = memoryCache;
        private readonly NbpApiService _nbpApiService = new();

        public async Task<string?> GetDataFromCacheOrApiAsync(string? path = null)
        {
            string cacheKeyOrRequestString = $"{_apiHost}{path}";

            if (!_memoryCache.TryGetValue(cacheKeyOrRequestString, out string? jsonString))
            {
                jsonString = await _nbpApiService.GetExchangeTableRatesAsync(cacheKeyOrRequestString).ConfigureAwait(false);

                var cacheEntryOptions = new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromMinutes(60)); // Cache for 60 minutes

                _memoryCache.Set(cacheKeyOrRequestString, jsonString, cacheEntryOptions);
            }

            return jsonString;
        }

        public async Task<ExchangeRates?> GetExchangeTableRatesAsync(
            string tableType = "A",
            string? tableDateString = null
        )
        {
            ExchangeRates exchangeRates = new();
            string path = $"/api/exchangerates/tables/{tableType}";
            path += tableDateString != null ? $"/{tableDateString}" : "";
            path += $"?format=json";

            string? data = await GetDataFromCacheOrApiAsync(path).ConfigureAwait(false);
            
            if (data != null)
            {
                exchangeRates = JsonSerializer.Deserialize<List<ExchangeRates>>(data)?.FirstOrDefault() ?? exchangeRates;

                if (exchangeRates == null)
                {
                    throw new HttpRequestException("Failed to deserialize exchange rates.");
                }
            }

            return exchangeRates;
        }
    }
}