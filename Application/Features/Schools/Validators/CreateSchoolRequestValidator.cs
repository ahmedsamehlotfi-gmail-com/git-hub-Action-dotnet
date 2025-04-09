using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Schools.Validators
{
    public class CreateSchoolRequestValidator : AbstractValidator<CreateSchoolRequest>
    {
        public CreateSchoolRequestValidator()
        {
            RuleFor(request => request.Name)
                .NotEmpty()
                .WithMessage("School Name is Required")
                .MaximumLength(60)
                .WithMessage("School Name Should be not exceed 60 charaters length");
            RuleFor(request => request.EstablishedOn)
                .LessThanOrEqualTo(DateTime.UtcNow)
                .WithMessage("Date established cannot be future date..");
        }
    }
}
