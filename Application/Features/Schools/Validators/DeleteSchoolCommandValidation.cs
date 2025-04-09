using Application.Features.Schools.Commands;
using Domain.Entities;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Schools.Validators
{
    public class DeleteSchoolCommandValidation : AbstractValidator<DeleteSchoolCommand>
    {
        public DeleteSchoolCommandValidation(ISchoolService schoolService)
        {
            RuleFor(request => request.SchoolId).NotEmpty()
              .MustAsync(async (id, ct) => await schoolService.GetSchoolByIdAsync(id) is School schoolInDb && schoolInDb.Id == id)
              .WithMessage("School dose Not Exists");
        }
    }
}
