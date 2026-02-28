using Api_EFExample.Filters.Actions;
using Core.DTO;
using Core.ServiceContracts.Auth;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ServiceContracts.DTO;

namespace Api_EFExample.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }
        [HttpPost("register")]
        [ServiceFilter(typeof(ValidationFilter<RegisterRequest>))]
        public async Task<IActionResult> Register(RegisterRequest registerDTO)
        {
           if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }
           var result = await _authService.RegisterUser(registerDTO);
            if (!result.Succeeded)
            {
                var errors = result.Errors
                    .GroupBy(e => e.Code)
                    .ToDictionary(
                        g => g.Key,
                        g => g.Select(e => e.Description).ToArray()
                    );

                var problemDetails = new ValidationProblemDetails(errors)
                {
                    Title = "Validation failed",
                };
                return BadRequest(problemDetails);

            }
            else {
                return Ok(new { message = "User created"});
            }

        }
    }
}
