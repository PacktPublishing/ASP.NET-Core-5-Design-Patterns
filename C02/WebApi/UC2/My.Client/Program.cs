using My.Api.Contracts;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace My.Client
{
    public class Program
    {
        private static readonly HttpClient http = new HttpClient();

        static async Task Main(string[] args)
        {
            var uri = "https://localhost:44318/api/clients";

            // Read all summaries
            WriteTitle("All clients summaries");
            var clients = await FetchAndWriteFormattedJson<ClientSummaryDto[]>(uri);

            // Read all details
            foreach (var summary in clients)
            {
                WriteTitle($"Details of {summary.Name} (id: {summary.Id})");
                await FetchAndWriteFormattedJson<ClientDetailsDto>($"{uri}/{summary.Id}");
            }

            Console.ReadLine();
        }

        private static async Task<TContract> FetchAndWriteFormattedJson<TContract>(string uri)
        {
            var response = await http.GetStringAsync(uri);
            var deserializedObject = JsonConvert.DeserializeObject<TContract>(response);
            var formattedJson = JsonConvert.SerializeObject(deserializedObject, Formatting.Indented);
            Console.WriteLine(formattedJson);
            return deserializedObject;
        }

        private static void WriteTitle(string title)
        {
            var initialColor = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine();
            Console.WriteLine(title);
            Console.ForegroundColor = initialColor;
        }
    }
}
