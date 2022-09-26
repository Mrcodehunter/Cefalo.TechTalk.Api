using Cefalo.TechTalk.Service.DTOs;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cefalo.TechTalk.Service.Utils.DtoValidators
{
    public class BlogUpdateDtoValidator : BaseValidator<BlogUpdateDto>
    {
        public BlogUpdateDtoValidator()
        {
            RuleFor(blog => blog.Title)
                .NotEmpty()
                .WithMessage("Blog Title Is Required To Update A Blog.");
            RuleFor(blog => blog.Title)
                .NotEmpty()
                .WithMessage("Blog Body Is Required To Update A Blog.");
        }
    }
}
