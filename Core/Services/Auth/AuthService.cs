using AutoMapper;
using Core.Domain.IdentityEntities;
using Core.Domain.RepositoryContracts;
using Core.DTO;
using Core.ServiceContracts.Auth;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
namespace Core.Services.Auth
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signinManager;
        private readonly IJwtTokenGenerator _jwtTokenGenerator;
        private readonly IMapper _mapper;
        private readonly IConfiguration _config;
        public AuthService(UserManager<ApplicationUser> userManager, IMapper mapper, SignInManager<ApplicationUser> signinManager, IJwtTokenGenerator jwtTokenGenerator, IConfiguration configuration)
        {
            _userManager = userManager;
            _mapper = mapper;
            _signinManager = signinManager;
            _jwtTokenGenerator = jwtTokenGenerator;
            _config = configuration;
        }
        public async Task<SignInResponse?> RegisterUser(RegisterRequest registerDTO)
        {
            var user = _mapper.Map<ApplicationUser>(registerDTO);
            IdentityResult result = await _userManager.CreateAsync(user, registerDTO.Password);
            if (!result.Succeeded)
            {
                return null;
            }
            SignInResponse response = GenerateAccessToken(user);
            return response;
        }

        private SignInResponse GenerateAccessToken(ApplicationUser user)
        {
            var token = _jwtTokenGenerator.GenerateToken(user);
            var response = _mapper.Map<SignInResponse>(user);
            response.Token = token.Token;
            response.Expiry = token.ExpiryTime;
            return response;
        }

        public async Task<bool> EmailExists(string email)
        {
            var result = await _userManager.FindByEmailAsync(email);
            if (result == null)
            {
                return false;
            }
            return true;
        }

        public async Task SigninUser(ApplicationUser applicationUser)
        {
            await _signinManager.SignInAsync(applicationUser, isPersistent: true);
        }

        public async Task<SignInResponse?> SignInWithPassword(LoginRequest loginRequest)
        {
            var user = await _userManager.FindByEmailAsync(loginRequest.Email);
            if (user == null)
                return null;

            var result = await _signinManager.CheckPasswordSignInAsync(user, loginRequest.Password, lockoutOnFailure: false);
            if (!result.Succeeded)
                return null;

            var token = _jwtTokenGenerator.GenerateToken(user);
            user.RefreshToken = token.RefreshToken;
            user.RefreshTokenValidity = token.RefreshTokenExpiry;
            await _userManager.UpdateAsync(user);

            var response = _mapper.Map<SignInResponse>(user);
            response.Token = token.Token;
            response.Expiry = token.ExpiryTime;
            return response;
        }

        public async Task SignOut()
        {
            await _signinManager.SignOutAsync();
        }

        // This method is used to generate a NEW access token using
        // an existing (possibly expired) access token + refresh token.
        public async Task<SignInResponse?> RenewAccessToken(TokensRequest tokensRequest)
        {
            // Step 1: Extract the ClaimsPrincipal from the expired access token.
            // We ignore token lifetime validation because access token may already be expired.
            ClaimsPrincipal claimsPrincipal = GetClientsPrincipalFromAccessToken(tokensRequest);

            // If token validation failed, return null (invalid request)
            if (claimsPrincipal == null)
                return null;

            // Step 2: Extract email claim from token
            string? email = claimsPrincipal.FindFirst(ClaimTypes.Email)?.Value;

            // If email claim is missing, token is invalid
            if (email == null)
                return null;

            // Step 3: Find the user from database using email
            ApplicationUser? applicationUser = await _userManager.FindByEmailAsync(email);

            // Step 4: Validate refresh token
            // Conditions checked:
            // 1. User must exist
            // 2. Stored refresh token must match the incoming refresh token
            // 3. Refresh token must NOT be expired
            if (applicationUser == null ||
                applicationUser.RefreshToken != tokensRequest.RefreshToken ||
                applicationUser.RefreshTokenValidity <= DateTime.UtcNow)
            {
                return null; // Refresh token invalid or expired
            }

            // Step 5: Generate a new access token (and optionally new refresh token)
            SignInResponse response = GenerateAccessToken(applicationUser);

            return response;
        }

        // This method extracts the ClaimsPrincipal from the given access token.
        // It is mainly used during refresh token flow.
        // NOTE: Lifetime validation is intentionally disabled because
        // the access token may already be expired.
        public ClaimsPrincipal GetClientsPrincipalFromAccessToken(TokensRequest tokensRequest)
        {
            // Extract access token from request
            var accessToken = tokensRequest.AccessToken;

            // Basic guard clause (recommended in production)
            if (string.IsNullOrWhiteSpace(accessToken))
                throw new SecurityTokenException("Access token is missing.");

            // Configure how the token should be validated
            var tokenValidationParams = new TokenValidationParameters()
            {
                // Validate that token was issued by expected issuer
                ValidateIssuer = true,
                ValidIssuer = _config["Jwt:Issuer"],

                // Validate that token is meant for expected audience
                ValidateAudience = true,
                ValidAudience = _config["Jwt:Audience"],

                // Ensure the token was signed with the correct secret key
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(
                    UTF8Encoding.UTF8.GetBytes(_config["Jwt:SecretKey"])
                ),

                // IMPORTANT:
                // We disable lifetime validation because this method
                // is used in refresh token flow.
                // The token may already be expired.
                ValidateLifetime = false
            };

            // Handler responsible for validating and reading JWT tokens
            JwtSecurityTokenHandler jwtSecurityTokenHandler = new JwtSecurityTokenHandler();

            try
            {
                // Validate token signature and extract claims
                ClaimsPrincipal claimsPrincipal = jwtSecurityTokenHandler.ValidateToken(
                    accessToken,
                    tokenValidationParams,
                    out SecurityToken securityToken
                );

                // Extra security validation:
                // Ensure token is actually JWT and uses expected algorithm (HmacSha256)
                if (securityToken is not JwtSecurityToken jwtSecurityToken ||
                    !jwtSecurityToken.Header.Alg.Equals(
                        SecurityAlgorithms.HmacSha256,
                        StringComparison.InvariantCultureIgnoreCase))
                {
                    throw new SecurityTokenException("Invalid token algorithm.");
                }

                return claimsPrincipal;
            }
            catch (Exception)
            {
                // Optional: log error here using Serilog or ILogger
                // _logger.LogError(ex, "Invalid access token during refresh");

                throw new SecurityTokenException("Invalid access token.");
            }
        }

    }
}
