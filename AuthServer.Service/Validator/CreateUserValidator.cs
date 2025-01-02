using AuthServer.Core.DTOs;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthServer.Service.Validator
{
    public class CreateUserValidator : AbstractValidator<CreateUserDto>
    {
        public CreateUserValidator()
        {
            RuleFor(x => x.Email).NotEmpty().WithMessage("email boş olamaz.").EmailAddress().WithMessage("email is wrong");
            RuleFor(x => x.PassWord).NotEmpty().WithMessage("şifre boş olamaz.");
            RuleFor(x => x.UserName).NotEmpty().WithMessage("username boş olamaz.");
            //RuleFor(x => x.UserName).NotNull().WithMessage("username boş olamaz.");

        }
    }
}
