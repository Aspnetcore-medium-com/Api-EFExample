using Microsoft.AspNetCore.Mvc;
using ServiceContracts;
using ServiceContracts.DTO;
using Microsoft.AspNetCore.Http;

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
        public async Task<ActionResult<IEnumerable<PersonResponse>>> GetAll(CancellationToken cancellationToken = default)
        {
           var persons = await _personService.GetAllPersons(cancellationToken);
            return Ok(persons);
        }

        [HttpGet("{personId:guid}")]
        public async Task<ActionResult<PersonResponse>> GetById(Guid personId, CancellationToken cancellationToken = default)
        {
            PersonResponse? personResponse = await _personService.GetPersonById(personId,cancellationToken);
            if (personResponse == null)
            {
                return NotFound(new ProblemDetails
                {
                    Title = "Person Not Found",
                    Detail = $"{personId} was not found",
                });
            }
            return personResponse;
        }

        [HttpDelete("{personId:guid}")]
        public async Task<IActionResult> Delete(Guid personId,CancellationToken cancellationToken = default)
        {
            bool deleted = await _personService.DeletePerson(personId, cancellationToken);
            if (!deleted)
            {
                return NotFound(new ProblemDetails
                {
                    Title = "Person not found",
                    Detail = $"{personId} not found"
                });
            }
            return NoContent();
        }

        [HttpPost]
        public async Task<ActionResult<PersonResponse>> Add(PersonAddRequest personAddRequest, CancellationToken cancellationToken = default)
        {
            PersonResponse personResponse = await _personService.AddPerson(personAddRequest, cancellationToken);
            return CreatedAtAction(nameof(GetById),new {personId = personResponse.PersonId},personResponse);
        }
    }
}
