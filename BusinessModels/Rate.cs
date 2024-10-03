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
    }
}