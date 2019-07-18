using Newtonsoft.Json;
using System.Collections.Generic;

WebApi.Models
{
    public class ClientDetailsDto
    {
        [JsonProperty("id")]
        public int Id { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("contracts")]
        public IEnumerable<ContractDetailsDto> Contracts { get; set; }
    }
}
