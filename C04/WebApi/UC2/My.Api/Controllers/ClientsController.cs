using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using My.Api.Models;
using My.Api.Services;
using Microsoft.AspNetCore.Mvc;
using My.Api.Contracts;

namespace My.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientsController : ControllerBase
    {
        private readonly ClientService _clientService = new ClientService();

        // GET api/clients
        [HttpGet]
        public ActionResult<IEnumerable<ClientSummaryDto>> Get()
        {
            var clients = _clientService.ReadAll();
            var dto = clients.Select(client => ConvertToSummary(client)).ToArray();
            return dto;
        }

        // GET api/clients/1
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var client = _clientService.ReadOne(id);
            if (client == default(Client))
            {
                return NotFound();
            }
            var dto = ConvertToDetails(client);
            return Ok(dto);
        }

        //
        // Model to DTO conversion
        //
        private ClientSummaryDto ConvertToSummary(Client client)
        {
            return new ClientSummaryDto
            {
                Id = client.Id,
                Name = client.Name,
                TotalNumberOfContracts = client.Contracts.Count,
                NumberOfOpenContracts = client.Contracts.Count(x => x.Work.State != WorkState.Completed)
            };
        }

        private ClientDetailsDto ConvertToDetails(Client client)
        {
            return new ClientDetailsDto
            {
                Id = client.Id,
                Name = client.Name,
                Contracts = client.Contracts.Select(contract => ConvertToDetails(contract))
            };
        }

        private static ContractDetailsDto ConvertToDetails(Contract contract)
        {
            return new ContractDetailsDto
            {
                Id = contract.Id,
                Name = contract.Name,
                Description = contract.Description,

                // Flattening PrimaryContact
                PrimaryContactEmail = contract.PrimaryContact.Email,
                PrimaryContactFirstname = contract.PrimaryContact.Firstname,
                PrimaryContactLastname = contract.PrimaryContact.Lastname,

                // Flattening Work
                WorkDone = contract.Work.Done,
                WorkState = contract.Work.State,
                WorkTotal = contract.Work.Total
            };
        }
    }
}
