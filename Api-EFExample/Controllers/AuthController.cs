using Api_EFExample.Filters.Actions;
using Core.DTO;
using Core.ServiceContracts.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ServiceContracts.DTO;

namespace Api_EFExample.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }
        /// <summary>
        /// Registers a new user with the provided registration details.
        /// </summary>
        /// <remarks>The request body must contain valid registration data as defined by <see
        /// cref="RegisterRequest"/>.  If the input data fails validation, a 400 Bad Request response is returned with
        /// validation details.</remarks>
        /// <param name="registerDTO">An object containing the user's registration information. Must not be <c>null</c> and must satisfy all
        /// validation requirements.</param>
        /// <returns>﻿An <see cref="IActionResult"/> indicating the result of the registration operation.  Returns <see
        /// cref="OkObjectResult"/> with the registration result if successful; otherwise, returns <see
        /// cref="BadRequestObjectResult"/> if the registration fails or the input is invalid.</returns>
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

        [HttpPost("generate-access-token")]
        public async Task<ActionResult<SignInResponse>> ReGenerateAccessToken(TokensRequest tokenRequest, CancellationToken cancellationToken = default)
        {
            SignInResponse? signInResponse = await _authService.RenewAccessToken(tokenRequest);
            if (signInResponse == null) { return BadRequest("Invalid Access token"); }
            return Ok(signInResponse);

        }
    }
}
