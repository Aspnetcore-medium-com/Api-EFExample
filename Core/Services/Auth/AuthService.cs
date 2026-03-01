using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Core.Domain.IdentityEntities;
using Core.Domain.RepositoryContracts;
using Core.DTO;
using Core.ServiceContracts.Auth;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.JsonWebTokens;
namespace Core.Services.Auth
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signinManager;
        private readonly IJwtTokenGenerator _jwtTokenGenerator;
        private readonly IMapper _mapper;
        public AuthService(UserManager<ApplicationUser> userManager, IMapper mapper, SignInManager<ApplicationUser> signinManager, IJwtTokenGenerator jwtTokenGenerator)
        {
            _userManager = userManager;
            _mapper = mapper;
            _signinManager = signinManager;
            _jwtTokenGenerator = jwtTokenGenerator;
        }
        public async Task<SignInResponse?> RegisterUser(RegisterRequest registerDTO)
        {
            var user = _mapper.Map<ApplicationUser>(registerDTO);
            IdentityResult result = await _userManager.CreateAsync(user, registerDTO.Password);
            if (!result.Succeeded)
            {
                return null;
            }
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
            if (user != null)
            {
                var token = _jwtTokenGenerator.GenerateToken(user);
                var response = _mapper.Map<SignInResponse>(user);
                response.Token = token.Token;
                response.Expiry = token.ExpiryTime;
                return response;
            }
            return null;
        }

        public async Task SignOut()
        {
            await _signinManager.SignOutAsync();
        }

    }
}
