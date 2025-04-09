using Application.Models.Wrapper;
using Mapster;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Schools.Queries
{

	public record GetSchoolByNameQuery : IRequest<IResponseWrapper>
	{
        public string Name { get; set; }
    }

	public class GetSchoolByNameQueryHandler : IRequestHandler<GetSchoolByNameQuery, IResponseWrapper>
	{
        private readonly ISchoolService _schoolService;

        public GetSchoolByNameQueryHandler(ISchoolService  schoolService)
        {
            _schoolService = schoolService;
        }
        public async Task<IResponseWrapper> Handle(GetSchoolByNameQuery request, CancellationToken cancellationToken)
		{
            var schoolInDb = (await _schoolService.GetSchoolByNameAsync(request.Name)).Adapt<SchoolResponse>();
            if (schoolInDb is not null)
            {
                return await ResponseWrapper<SchoolResponse>.SuccessAsync(data: schoolInDb);

            }
            return await ResponseWrapper<SchoolResponse>.FailAsync(message: "School dose not Exist");
        }
	}
}
