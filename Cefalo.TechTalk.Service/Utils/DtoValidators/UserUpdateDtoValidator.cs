using Cefalo.TechTalk.Database.Models;
using Cefalo.TechTalk.Service.DTOs;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cefalo.TechTalk.Service.Utils.DtoValidators
{
    public class UserUpdateDtoValidator : BaseValidator<UserUpdateDto>
    {
        public UserUpdateDtoValidator()
        {
            RuleFor(user => user.Name)
               .NotEmpty()
               .WithMessage("Name of User is Mandatory to Update.");
            RuleFor(user => user.UserName)
               .NotNull()
               .WithMessage("Username Required");
           
            //RuleFor(user => user.Password )
               // .Matches("^(?=.*[A-Za-z])(?=.*\\d)[A-Za-z\\d]{8,}$")
               // .WithMessage("Password Required Minimum eight characters, at least one letter and one number.");
            RuleFor(user => user.Email)
                .NotEmpty()
                .WithMessage("Please Provide an Email.");
        }
    }
}
