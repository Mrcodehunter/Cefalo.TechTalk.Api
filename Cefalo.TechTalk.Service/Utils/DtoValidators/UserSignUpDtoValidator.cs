using Cefalo.TechTalk.Service.DTOs;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cefalo.TechTalk.Service.Utils.DtoValidators
{
    public class UserSignUpDtoValidator : BaseValidator<UserSignUpDto>
    {
        public UserSignUpDtoValidator()
        {
            RuleFor(user => user.Name)
                .NotEmpty()
                .WithMessage("Name of User is Mandatory to SignUp.");
            RuleFor(user => user.UserName)
               .NotEmpty()
               .WithMessage("Username Required");
            RuleFor(user => user.Password)
                .NotEmpty()
                .WithMessage("Provide Password")
                .Matches("^(?=.*[A-Za-z])(?=.*\\d)[A-Za-z\\d]{8,}$")
                .WithMessage("Password Required Minimum eight characters, at least one letter and one number.");
            RuleFor(user => user.Email)
                .NotEmpty()
                .EmailAddress()
                .WithMessage("Please Provide a Valid Email.");
        }
    }
}
