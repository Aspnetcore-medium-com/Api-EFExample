using FluentValidation;
using ServiceContracts.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Validator
{
    public class PersonValidator: AbstractValidator<PersonAddRequest>
    {
        public PersonValidator() { 
            RuleFor(x => x.PersonName)
                .NotEmpty().WithMessage("Person Name is required.")
                .MaximumLength(100).WithMessage("Person Name cannot exceed 100 characters.");
            RuleFor(x => x.Email)
                .EmailAddress().WithMessage("Invalid email format.");
            RuleFor(x => x.DateOfBirth)
                .LessThan(DateTime.Now).WithMessage("Date of Birth must be in the past.");
            
        }
    }
}
