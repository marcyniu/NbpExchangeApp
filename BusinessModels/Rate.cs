using System.Text.Json.Serialization;

namespace NbpExchangeApp.BusinessModels
{
    public class Rate
    {
        [JsonPropertyName("currency")]
        public string? Currency { get; set; }

        [JsonPropertyName("code")]
        public string? Code { get; set; }

        [JsonPropertyName("mid")]
        public decimal? Mid { get; set; }

        [JsonPropertyName("bid")]
        public decimal? Bid { get; set; }

        [JsonPropertyName("ask")]
        public decimal? Ask { get; set; }
    }
}