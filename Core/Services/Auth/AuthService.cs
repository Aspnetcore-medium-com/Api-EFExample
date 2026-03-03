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
        public AuthService(UserManager<ApplicationUser> userManager, IMapper mapper, SignInManager<ApplicationUser> signinManager, IJwtTokenGenerator jwtTokenGenerator ,IConfiguration configuration)
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

        public async Task<SignInResponse?> RenewAccessToken(TokensRequest tokensRequest)
        {
            ClaimsPrincipal claimsPrincipal = GetClientsPrincipalFromAccessToken(tokensRequest);

            if (claimsPrincipal == null)
                return null;
            string? email = claimsPrincipal.FindFirst(ClaimTypes.Email)?.Value;
            if (email == null) return null;
            ApplicationUser? applicationUser = await _userManager.FindByEmailAsync(email);
            if (applicationUser == null || applicationUser.RefreshToken != tokensRequest.RefreshToken || applicationUser.RefreshTokenValidity <= DateTime.UtcNow)
            {
                return null;
            }
            SignInResponse response = GenerateAccessToken(applicationUser);
            return response;

        }

        public  ClaimsPrincipal GetClientsPrincipalFromAccessToken(TokensRequest tokensRequest)
        {
            var accessToken = tokensRequest.AccessToken;

            var tokenValidationParams = new TokenValidationParameters()
            {
                ValidateIssuer = true,
                ValidIssuer = _config["Jwt:Issuer"],
                ValidateAudience = true,
                ValidAudience = _config["Jwt:Audience"],
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey( UTF8Encoding.UTF8.GetBytes(_config["Jwt:SecretKey"]) ),
                ValidateLifetime = false
            };

            JwtSecurityTokenHandler jwtSecurityTokenHandler = new JwtSecurityTokenHandler();

            ClaimsPrincipal claimsPrincipal =  jwtSecurityTokenHandler.ValidateToken(accessToken, tokenValidationParams, out SecurityToken securityToken);

            if (securityToken is not JwtSecurityToken jwtSecurityToken || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase)) {
                throw new SecurityTokenException("invalid token");
            }

            return claimsPrincipal;
        }

    }
}
