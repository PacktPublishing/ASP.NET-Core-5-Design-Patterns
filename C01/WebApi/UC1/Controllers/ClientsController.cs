using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using C01.WebApi.Models;
using C01.WebApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace C01.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientsController : ControllerBase
    {
        private readonly ClientService _clientService = new ClientService();

        // GET api/values
        [HttpGet]
        public ActionResult<IEnumerable<ClientDto>> Get()
        {
            var clients = _clientService.ReadAll();
            return clients.Select(client => Convert(client)).ToArray();
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var client = _clientService.ReadOne(id);
            if (client == default(Client))
            {
                return NotFound();
            }
            var dto = Convert(client);
            return Ok(dto);
        }

        private ClientDto Convert(Client client)
        {
            return new ClientDto
            {
                Id = client.Id,
                Name = client.Name,
                Contracts = client.Contracts.Select(contract => new ContractDto
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
                })
            };
        }
    }
}
