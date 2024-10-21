using System.Text.Json.Serialization;

namespace NbpExchangeApp.BusinessModels
{
    public class ExchangeCurrency
    {
        [JsonPropertyName("table")]
        public string? Table { get; set; }

        [JsonPropertyName("currency")]
        public string? Currency { get; set; }

        [JsonPropertyName("code")]
        public string? Code { get; set; }

        [JsonPropertyName("rates")]
        public Currency[]? Rates { get; set; }
    }
}