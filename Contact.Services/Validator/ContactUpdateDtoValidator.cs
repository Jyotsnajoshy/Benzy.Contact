using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Contact.Services.Dtos;
using FluentValidation;

namespace Contact.Services.Validator
{
    public class ContactUpdateDtoValidator : AbstractValidator<ContactUpdateDto>
    {
        public ContactUpdateDtoValidator()
        {
            RuleFor(x => x.Email).EmailAddress().When(x => x.Email != null);
            RuleFor(x => x.PhoneNumber).MaximumLength(15).When(x => x.PhoneNumber != null);
        }
    }
}
