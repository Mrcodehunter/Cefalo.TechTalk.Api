
using Cefalo.TechTalk.Service.Utils.Contracts;
using Cefalo.TechTalk.Service.Utils.CustomErrorHandler;
using FluentValidation;
using NPOI.SS.Formula.Functions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cefalo.TechTalk.Service.Utils.DtoValidators
{

    public class BaseValidator<T> : AbstractValidator<T>
    {
        public virtual void ValidateDto(T dto)
        {
            var result = this.Validate(dto);
            string err = "";

            if(!result.IsValid)
            {
                foreach(var error in result.Errors) 
                    err += $"{error.PropertyName} : {error.ErrorMessage}\n";
                throw new BadRequestException(err);
            }
           
        }

    }
}
