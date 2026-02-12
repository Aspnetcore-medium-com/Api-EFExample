using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ServiceContracts;
using ServiceContracts.DTO;

namespace Api_EFExample.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PersonController : ControllerBase
    {
        private readonly IPersonService _personService;

        public PersonController(IPersonService personService)
        {
            _personService = personService;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllPersons()
        {
           return Ok(await _personService.GetAllPersons());
        }

        [HttpGet("{guid}")]
        public async Task<IActionResult> GetPerson(Guid guid)
        {
            return Ok(await _personService.GetPersonById(guid));
        }

        [HttpDelete("{guid}")]
        public async Task<IActionResult> DeletePerson(Guid guid)
        {
            return Ok(await _personService.DeletePerson(guid));
        }
    }
}
