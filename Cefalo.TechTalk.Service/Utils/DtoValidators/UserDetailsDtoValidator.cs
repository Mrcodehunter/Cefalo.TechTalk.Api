using Cefalo.TechTalk.Service.DTOs;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cefalo.TechTalk.Service.Utils.DtoValidators
{
    public class UserDetailsDtoValidator : BaseValidator<UserDetailsDto>
    {
        public UserDetailsDtoValidator()
        {
            RuleFor(user => user.Id)
               .NotEmpty()
               .WithMessage("User Id Is Required.");

            RuleFor(user => user.Name)
                .NotEmpty()
                .WithMessage("User Name Not Mentioned.");
            RuleFor(user => user.UserName)
                .NotEmpty()
                .WithMessage("User's username Not Mentioned.");
            RuleFor(user =>user.Email)
                .NotEmpty()
                .WithMessage("User's Email Not Mentioned.");
            
        }
    }
}
