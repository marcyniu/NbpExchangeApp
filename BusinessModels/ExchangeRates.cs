using System.Text.Json.Serialization;

namespace NbpExchangeApp.BusinessModels
{
    public class ExchangeRates
    {
        [JsonPropertyName("table")]
        public string? Table { get; set; }

        [JsonPropertyName("no")]
        public string? No { get; set; }

        [JsonPropertyName("effectiveDate")]
        public string? EffectiveDate { get; set; }

        [JsonPropertyName("tradingDate")]
        public string? TradingDate { get; set; }

        [JsonPropertyName("rates")]
        public Rate[]? Rates { get; set; }
    }
}