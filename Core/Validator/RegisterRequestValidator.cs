using Core.DTO;
using Core.ServiceContracts.Auth;
using Core.Services.Auth;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Validator
{
    public class RegisterRequestValidator: AbstractValidator<RegisterRequest>
    {
        public RegisterRequestValidator(IAuthService auth) { 
            RuleFor(r => r.PersonName)
                .NotEmpty().WithMessage("Person Name is required.")
                .MaximumLength(100).WithMessage("Person Name cannot exceed 100 characters.");
            RuleFor(r => r.Email)
                .NotEmpty().WithMessage("Email is required.")
                .MustAsync(async (email,CancellationToken) => !await auth.EmailExists(email)).WithMessage("Email already taken")
                .EmailAddress().WithMessage("Invalid email format.");
            RuleFor(r => r.Password)
                .NotEmpty().WithMessage("Password is required.")
                .MinimumLength(6).WithMessage("Password must be at least 6 characters long.");
            RuleFor(r => r.ConfirmPassword)
                .NotEmpty().WithMessage("Confirm Password is required.")
                .Equal(r => r.Password).WithMessage("Passwords do not match.");
            RuleFor(r => r.PhoneNumber)
                .NotEmpty().WithMessage("Phone Number is required.")
                .Matches(@"^\+?[1-9]\d{1,14}$").WithMessage("Invalid phone number format.");

        }
    }
}
