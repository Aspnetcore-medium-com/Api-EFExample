using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Core.Domain.IdentityEntities;
using Core.DTO;
using Core.ServiceContracts.Auth;
using Microsoft.AspNetCore.Identity;

namespace Core.Services.Auth
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signinManager;

        private readonly IMapper _mapper;
        public AuthService(UserManager<ApplicationUser> userManager, IMapper mapper, SignInManager<ApplicationUser> signinManager)
        {
            _userManager = userManager;
            _mapper = mapper;
            _signinManager = signinManager;
        }
        public async Task<IdentityResult> RegisterUser(RegisterRequest registerDTO)
        {
            var user = _mapper.Map<ApplicationUser>(registerDTO);
            IdentityResult result = await _userManager.CreateAsync(user, registerDTO.Password);
            if (result.Succeeded)
            {
               await SigninUser(user);
            }
            return result;
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
    }
}
