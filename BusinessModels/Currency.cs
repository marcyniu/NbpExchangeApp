using System.Text.Json.Serialization;

namespace NbpExchangeApp.BusinessModels
{
    public class Currency
    {
        [JsonPropertyName("no")]
        public string? No { get; set; }

        [JsonPropertyName("effectiveDate")]
        public string? EffectiveDate { get; set; }

        [JsonPropertyName("mid")]
        public decimal? Mid { get; set; }
    }
}