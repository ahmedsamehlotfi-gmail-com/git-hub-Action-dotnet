using Application.Models.Wrapper;
using Application.PipeLines;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Schools.Commands
{

	public record UpdateSchoolCommand : IRequest<IResponseWrapper>
	{
        public UpdateSchoolRequest  schoolRequest { get; set; }
    }

	public class UpdateSchoolCommandHandler : IRequestHandler<UpdateSchoolCommand , IResponseWrapper> , IValidateMe
	{
        private readonly ISchoolService _schoolService;

        public UpdateSchoolCommandHandler(ISchoolService schoolService)
        {
            _schoolService = schoolService;
        }
        public async Task<IResponseWrapper> Handle(UpdateSchoolCommand request, CancellationToken cancellationToken)
		{
            var schoolInDb = await _schoolService.GetSchoolByIdAsync(request.schoolRequest.Id);
            schoolInDb.Name =  request.schoolRequest.Name;
            schoolInDb.EstablishedOn = request.schoolRequest.EstablishedOn;
            var updateSchoolId = await _schoolService.UpdateSchoolAsync(schoolInDb);
            return await ResponseWrapper<int>.SuccessAsync(data: updateSchoolId, message: "School Update Successfully");
		}
	}
}
