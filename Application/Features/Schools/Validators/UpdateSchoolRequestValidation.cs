using Domain.Entities;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Schools.Validators
{
    public class UpdateSchoolRequestValidation : AbstractValidator<UpdateSchoolRequest>
    {
        public UpdateSchoolRequestValidation(ISchoolService schoolService)
        {
            RuleFor(request => request.Id).NotEmpty()
                .MustAsync(async (id, ct) => await schoolService.GetSchoolByIdAsync(id) is School schoolInDb && schoolInDb.Id == id)
                .WithMessage("School dose Not Exists");

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
