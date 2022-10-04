using Cefalo.TechTalk.Service.DTOs;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cefalo.TechTalk.Service.Utils.DtoValidators
{
    public class BlogDetailsDtoValidator : BaseValidator<BlogDetailsDto>
    {
        public BlogDetailsDtoValidator()
        {
            RuleFor(blog => blog.Id)
                .NotEmpty()
                .WithMessage("Story Id is Required.");

            RuleFor(blog => blog.AuthorName)
                .NotEmpty()
                .WithMessage("No Author Name Included in Blog Details.");
            RuleFor(blog => blog.Title)
                .NotEmpty()
                .WithMessage("Blog Title is Mandatory in Story Details.");
            RuleFor(blog => blog.Body)
                .NotEmpty()
                .WithMessage("Blog Body is Mandatory in Story Details.");
            RuleFor(blog => blog.CreatedAt)
                .NotEmpty()
                .LessThanOrEqualTo(DateTime.UtcNow)
                .WithMessage("Story Creation Date Must Be Less or Equal Present Date.");
            RuleFor(blog => blog.ModifiedAt)
                .NotEmpty()
                .LessThanOrEqualTo(DateTime.UtcNow)
                .WithMessage("Story Last Modification Date Must Be Less or Equal Present Date.");
        }

    }
}
