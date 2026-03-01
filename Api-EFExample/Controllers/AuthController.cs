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
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _authService.RegisterUser(registerDTO);
            if (result == null)
            {
                //var errors = result.Errors
                //    .GroupBy(e => e.Code)
                //    .ToDictionary(
                //        g => g.Key,
                //        g => g.Select(e => e.Description).ToArray()
                //    );

                //var problemDetails = new ValidationProblemDetails(errors)
                //{
                //    Title = "Validation failed",
                //};
                return BadRequest();

            }
            else
            {
                return Ok(result);
            }

        }

        [HttpPost("login")]
        [ServiceFilter(typeof(ValidationFilter<LoginRequest>))]
        public async Task<IActionResult> Login(LoginRequest loginRequest)
        {
            var response = await _authService.SignInWithPassword(loginRequest);
            if (response == null)
            {
                return Unauthorized(new ProblemDetails
                {
                    Title = "Invalid credentials",
                    Detail = "The email or password provided is incorrect."
                });
            }
            return Ok(response);
        }

        [HttpGet("logout")]
        public async Task<IActionResult> Logout()
        {
            await _authService.SignOut();
            return NoContent();
        }
    }
}
