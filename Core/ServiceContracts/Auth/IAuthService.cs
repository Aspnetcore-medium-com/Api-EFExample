using Core.DTO;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.ServiceContracts.Auth
{
    public interface IAuthService
    {
        /// <summary>
        /// Registers a new user account using the specified registration details.
        /// </summary>
        /// <param name="registerDTO">An object containing the user's registration information, such as username, password, and email address.
        /// Cannot be <c>null</c>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IdentityResult"/>
        /// indicating whether the registration succeeded and any associated errors.</returns>
        Task<SignInResponse?> RegisterUser(RegisterRequest registerDTO);
        /// <summary>
        /// Determines whether a user account with the specified email address exists.
        /// </summary>
        /// <param name="email">The email address to check for existence. Cannot be null or empty.</param>
        /// <returns>A task that represents the asynchronous operation. The task result is <see langword="true"/> if an account
        /// with the specified email exists; otherwise, <see langword="false"/>.</returns>
        Task<bool> EmailExists(string email);

        Task<SignInResponse?> SignInWithPassword(LoginRequest loginRequest);

        Task SignOut();
    }
}
