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

namespace Api_EFExample.Controllers.v2
{
    //[Route("api/v2/[controller]")]
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    [ApiVersion("2.0")]
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
        [MapToApiVersion("2.0")]
        public async Task<ActionResult<IEnumerable<string>>> GetAll(CancellationToken cancellationToken = default)
        {
           var persons = await _personService.GetAllPersons(cancellationToken);
           var personList = persons.Select(p => p.PersonName).ToList();
           return Ok(personList);
        }

        
    }
}
