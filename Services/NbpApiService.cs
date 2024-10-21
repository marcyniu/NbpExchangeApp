namespace NbpExchangeApp.Services
{
    public class NbpApiService
    {
        private readonly HttpClient _httpClient;

        public NbpApiService()
        {
            _httpClient = new HttpClient();
        }

        public async Task<(string, string)> GetExchangeTableRatesAsync(string? cacheKeyOrRequestString = null)
        {
            string jsonString = string.Empty;
            string errorMessage = string.Empty;

            ArgumentNullException.ThrowIfNull(cacheKeyOrRequestString);

            HttpResponseMessage response = await _httpClient.GetAsync(cacheKeyOrRequestString);

            // Add code to get response content
            if (response.IsSuccessStatusCode)
            {
                jsonString = await response.Content.ReadAsStringAsync();
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                errorMessage = "Exchange rates not available for the given date!";
            }
            else
            {
                throw new HttpRequestException($"Failed to fetch exchange rates. Status code: {response.StatusCode}");
            }

            return (jsonString, errorMessage);
        }
    }
}
