using Newtonsoft.Json;

namespace My.Api.Contracts
{
    public class ClientSummaryDto
    {
        [JsonProperty("id")]
        public int Id { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("totalNumberOfContracts")]
        public int TotalNumberOfContracts { get; set; }
        [JsonProperty("numberOfOpenContracts")]
        public int NumberOfOpenContracts { get; set; }
    }
}
