namespace NbpExchangeApp.Services
{
    public class NbpApiService
    {
        private readonly HttpClient _httpClient;

        public NbpApiService()
        {
            _httpClient = new HttpClient();
        }

        public async Task<string> GetExchangeTableRatesAsync(string? cacheKeyOrRequestString = null)
        {
            string? jsonString = null;

            if (cacheKeyOrRequestString == null)
            {
                throw new ArgumentNullException(nameof(cacheKeyOrRequestString));
            }

            HttpResponseMessage response = await _httpClient.GetAsync(cacheKeyOrRequestString);

            // Add code to get response content
            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException($"Failed to fetch exchange rates. Status code: {response.StatusCode}");
            }

            jsonString = await response.Content.ReadAsStringAsync();

            return jsonString;
        }
    }
}
