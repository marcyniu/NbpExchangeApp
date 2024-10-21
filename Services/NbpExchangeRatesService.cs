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
            var errorMessage = string.Empty;
            var jsonString = string.Empty;
            var cacheEntryOptions = new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromMinutes(60)); // Cache for 60 minutes
            string cacheKeyOrRequestString = $"{_apiHost}{path}";

            if (!_memoryCache.TryGetValue(cacheKeyOrRequestString, out string? responseString))
            {
                (jsonString, errorMessage) = await _nbpApiService.GetExchangeTableRatesAsync(cacheKeyOrRequestString).ConfigureAwait(false);

                if (!string.IsNullOrEmpty(jsonString))
                {
                    _memoryCache.Set(cacheKeyOrRequestString, jsonString, cacheEntryOptions);
                    responseString = jsonString;
                }
                else if (!string.IsNullOrEmpty(errorMessage))
                {
                    _memoryCache.Set(cacheKeyOrRequestString, errorMessage, cacheEntryOptions);
                    responseString = errorMessage;
                }
            }

            return responseString;
        }

        public async Task<(ExchangeRates?, string?)> GetExchangeTableRatesAsync(
            string tableType = "A",
            string? tableDateString = null
        )
        {
            ExchangeRates exchangeRates = new();
            string? errorMessage = null;
            string path = $"/api/exchangerates/tables/{tableType}";
            path += tableDateString != null ? $"/{tableDateString}" : "";
            path += $"?format=json";

            string? data = await GetDataFromCacheOrApiAsync(path).ConfigureAwait(false);
            
            if (data != null)
            {
                if (IsValidJson(data)) {
                    exchangeRates = JsonSerializer.Deserialize<List<ExchangeRates>>(data)?.FirstOrDefault() ?? exchangeRates;

                    if (exchangeRates == null)
                    {
                        errorMessage = "Failed to deserialize exchange rates.";
                    }
                }
                else
                {
                    errorMessage = data;
                }
            }

            return (exchangeRates, errorMessage);
        }

        public async Task<(ExchangeCurrency?, string?)> GetExchangeCurrencyForTableAndPeriodAsync(
            string currencyCode,
            string tableType = "A",
            string? startDateString = null,
            string? endDateString = null
        )
        {
            ExchangeCurrency exchangeCurrency = new();
            string? errorMessage = null;
            string path = $"/api/exchangerates/rates/{tableType}/{currencyCode}";
            path += startDateString != null ? $"/{startDateString}" : "";
            path += endDateString != null ? $"/{endDateString}" : "";
            path += $"?format=json";

            string? data = await GetDataFromCacheOrApiAsync(path).ConfigureAwait(false);

            if (data != null)
            {
                if (IsValidJson(data))
                {
                    exchangeCurrency = JsonSerializer.Deserialize<ExchangeCurrency>(data) ?? exchangeCurrency;

                    if (exchangeCurrency == null)
                    {
                        errorMessage = "Failed to deserialize exchange currency.";
                    }
                }
                else
                {
                    errorMessage = data;
                }
            }

            return (exchangeCurrency, errorMessage);
        }

        private static bool IsValidJson(string? jsonString)
        {
            if (string.IsNullOrWhiteSpace(jsonString))
            {
                return false;
            }

            try
            {
                JsonDocument.Parse(jsonString);
                return true;
            }
            catch (JsonException)
            {
                return false;
            }
        }
    }
}