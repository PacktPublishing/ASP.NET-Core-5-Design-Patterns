using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace C01.WebApi.Models
{
    public class ContractDetailsDto
    {
        [JsonProperty("id")]
        public int Id { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("workTotal")]
        public int WorkTotal { get; set; }
        [JsonProperty("workDone")]
        public int WorkDone { get; set; }
        [JsonProperty("workState")]
        [JsonConverter(typeof(StringEnumConverter))]
        public WorkState WorkState { get; set; }

        [JsonProperty("primaryContactFirstname")]
        public string PrimaryContactFirstname { get; set; }
        [JsonProperty("primaryContactLastname")]
        public string PrimaryContactLastname { get; set; }
        [JsonProperty("primaryContactEmail")]
        public string PrimaryContactEmail { get; set; }
    }
}
