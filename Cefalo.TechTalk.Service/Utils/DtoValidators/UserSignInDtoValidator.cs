using Cefalo.TechTalk.Service.DTOs;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cefalo.TechTalk.Service.Utils.DtoValidators
{
    public class UserSignInDtoValidator : BaseValidator<UserSignInDto>
    {
        public UserSignInDtoValidator()
        {
            RuleFor(user => user.UserName)
                .NotNull()
                .WithMessage("Username Required");
            RuleFor(user => user.Password)
                .NotEmpty()
                .WithMessage("Provide Password");


        }
    }
}
