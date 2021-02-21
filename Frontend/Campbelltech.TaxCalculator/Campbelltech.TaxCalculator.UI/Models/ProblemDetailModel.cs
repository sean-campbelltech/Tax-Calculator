using System;
using Newtonsoft.Json;

namespace Campbelltech.TaxCalculator.UI.Models
{
    public class ProblemDetailModel
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("status")]
        public int Status { get; set; }

        [JsonProperty("traceId")]
        public string TraceId { get; set; }

        [JsonProperty("errors")]
        public Object Errors { get; set; }
    }
}