using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace C01.WebApi.Models
{
    public class Client
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public List<Contract> Contracts { get; set; }
    }

    public class Contract
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public ContractWork Work { get; set; }
        public ContactInformation PrimaryContact { get; set; }
    }

    public class ContractWork
    {
        public int Total { get; set; }
        public int Done { get; set; }

        public WorkState State => 
            Done == 0 ? WorkState.New : 
            Done == Total ? WorkState.Completed : 
            WorkState.InProgress;
    }

    public enum WorkState
    {
        New,
        InProgress,
        Completed
    }

    public class ContactInformation
    {
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Email { get; set; }
    }

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

    public class ClientDetailsDto
    {
        [JsonProperty("id")]
        public int Id { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("contracts")]
        public IEnumerable<ContractDetailsDto> Contracts { get; set; }
    }

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
