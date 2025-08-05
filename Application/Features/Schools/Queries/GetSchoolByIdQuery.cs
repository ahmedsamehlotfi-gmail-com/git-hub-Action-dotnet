using Application.Models.Wrapper;
using Domain.Entities;
using Mapster;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Schools.Queries
{

	public record GetSchoolByIdQuery : IRequest<IResponseWrapper>
	{
        public int SchoolId { get; set; }
    }

	public class GetSchoolByIdQueryHandler : IRequestHandler<GetSchoolByIdQuery, IResponseWrapper>
	{
        private readonly ISchoolService _schoolService;

        public GetSchoolByIdQueryHandler(ISchoolService schoolService)
        {
           _schoolService = schoolService;
        }
        public async Task<IResponseWrapper> Handle(GetSchoolByIdQuery request, CancellationToken cancellationToken)
		{
			var schoolInDb = (await _schoolService.GetSchoolByIdAsync(request.SchoolId)).Adapt<SchoolResponse>();
            if(schoolInDb is not null)
            {
                return await ResponseWrapper<SchoolResponse>.SuccessAsync(data: schoolInDb);

            }
            return await ResponseWrapper<SchoolResponse>.FailAsync(message: "School dose not Exist");
        }
	}
}
