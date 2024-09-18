using System.Net.Http.Headers;
using API.Models;
using API.Services;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientsController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        //create constructor
        public ClientsController(ApplicationDbContext context)
        {
            this.context = context;
        }

        [HttpGet]
        public List<Client> GetClients()
        {
            return context.Clients.OrderByDescending(c => c.Id).ToList();
        }

        [HttpGet("{id}")]
        public IActionResult GetClient(int id)
        {
            var client = context.Clients.Find(id);
            if(client == null)
            {
                return NotFound();
            }

            return Ok(client);
        }

        [HttpPost]
        public IActionResult CreateClient(ClientDto clientDto)
        {
            //submited data is valid
            var otherClient = context.Clients.FirstOrDefault(c => c.Email == clientDto.Email);
            if(otherClient != null) //is same email address
            {
                ModelState.AddModelError("Email" , "The Email Address is already used");
                var validation = new ValidationProblemDetails(ModelState); //then add to validate and return
                return BadRequest(validation);
            }

            //else
            var client = new Client
            {
                FirstName = clientDto.FirstName,
                LastName = clientDto.LastName,
                Email = clientDto.Email,
                Phone = clientDto.Phone,
                Address = clientDto.Address,
                Status = clientDto.Status,
                CreatedAt = DateTime.Now,
            };

            //cors database
            context.Clients.Add(client);
            context.SaveChanges();

            return Ok();
        }

        [HttpPut("{id}")]
        public IActionResult EditClient(int id, ClientDto clientDto)
        {
            //submitted data is valid
            var otherClient = context.Clients.FirstOrDefault(c => c.Id != id && c.Email == clientDto.Email);
            if(otherClient !=null)
            {
                ModelState.AddModelError("Email", "The Email Address is already used");
                var validation = new ValidationProblemDetails(ModelState);
                return BadRequest(validation);
            }

            var client = context.Clients.Find(id);
            if(client == null)
            {
                return NotFound();
            }

            client.FirstName = clientDto.FirstName;
            client.LastName = clientDto.LastName;
            client.Email = clientDto.Email;
            client.Phone = clientDto.Phone ?? "";
            client.Address = clientDto.Address ?? "";
            client.Status = clientDto.Status;

            context.SaveChanges();
            return Ok(client);
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteClient(int id)
        {
            var client = context.Clients.Find(id);
            if(client == null)
            {
                return NotFound();
            }

            context.Clients.Remove(client);
            context.SaveChanges();

            return Ok();
        }

        
    }
}