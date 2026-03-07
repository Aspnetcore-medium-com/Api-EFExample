using Api_EFExample.Filters.Actions;
using Asp.Versioning;
using Core.Validator;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ServiceContracts;
using ServiceContracts.DTO;
using Services.Entities;
using System.ComponentModel.DataAnnotations;

namespace Api_EFExample.Controllers.v1
{

    //[Route("api/v1/[controller]")]
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    [ApiVersion("1.0")]

    public class PersonsController : ControllerBase
    {
        private readonly IPersonService _personService;
        private readonly IValidator<PersonAddRequest> _validator;
        private readonly ILogger<PersonsController> _logger;
        public PersonsController(IPersonService personService,IValidator<PersonAddRequest> validator, ILogger<PersonsController> logger)
        {
            _personService = personService;
            _validator = validator;
            _logger = logger;
        }
        [HttpGet]
        //[TypeFilter(typeof(ResponseHeaderFilter))]
        [MapToApiVersion("1.0")]
        public async Task<ActionResult<IEnumerable<PersonResponse>>> GetAll(CancellationToken cancellationToken = default)
        {
           var persons = await _personService.GetAllPersons(cancellationToken);
            return Ok(persons);
        }

        [HttpGet("{personId:guid}")]
        public async Task<ActionResult<PersonResponse>> GetById(Guid personId, CancellationToken cancellationToken = default)
        {
            _logger.LogInformation($"{nameof(GetById)} called");
            _logger.LogDebug($"{personId} passed for {nameof(GetById)}");
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
        [ServiceFilter(typeof(ValidationFilter<PersonAddRequest>))]
        public async Task<ActionResult<PersonResponse>> Add(PersonAddRequest personAddRequest, CancellationToken cancellationToken = default)
        {
            //var result = await _validator.ValidateAsync(personAddRequest);
            //if (!result.IsValid)
            //    return BadRequest(result.Errors.Select(e => new {
            //        e.PropertyName,
            //        e.ErrorMessage
            //    }));
            PersonResponse personResponse = await _personService.AddPerson(personAddRequest, cancellationToken);
            return CreatedAtAction(nameof(GetById),new {personId = personResponse.PersonId},personResponse);
        }
    }
}
