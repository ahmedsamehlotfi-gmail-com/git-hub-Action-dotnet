using Application.Models.Wrapper;
using Application.PipeLines;
using Domain.Entities;
using Mapster;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Schools.Commands
{

	public record CreateSchoolCommand : IRequest<IResponseWrapper> , IValidateMe
	{
        public CreateSchoolRequest  SchoolRequest { get; set; }
    }

	public class CreateSchoolCommandHandler : IRequestHandler<CreateSchoolCommand , IResponseWrapper>
	{
        private readonly ISchoolService _schoolService;

        public CreateSchoolCommandHandler(ISchoolService schoolService)
        {
            _schoolService = schoolService;
        }
        public async Task<IResponseWrapper> Handle(CreateSchoolCommand request, CancellationToken cancellationToken)
		{
            var schoolMapped = request.SchoolRequest.Adapt<School>();
            var schoolId = await _schoolService.CreateSchoolAsync(schoolMapped);
            return await ResponseWrapper<int>.SuccessAsync(data: schoolId, message: "School Created Successfully");
		}
	}
}
