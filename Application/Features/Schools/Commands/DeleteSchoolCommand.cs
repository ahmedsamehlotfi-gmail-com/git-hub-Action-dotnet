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

	public record DeleteSchoolCommand : IRequest<IResponseWrapper>
	{
        public int SchoolId { get; set; }
    }

	public class DeleteSchoolCommandHandler : IRequestHandler<DeleteSchoolCommand , IResponseWrapper> , IValidateMe
	{
        private readonly ISchoolService _schoolService;

        public DeleteSchoolCommandHandler(ISchoolService schoolService)
        {
            _schoolService = schoolService;
        }
        public async Task<IResponseWrapper> Handle(DeleteSchoolCommand request, CancellationToken cancellationToken)
		{
			var schoolInDb = await _schoolService.GetSchoolByIdAsync(request.SchoolId);
            var deleteSchoolId = await _schoolService.DeleteSchoolAsync(schoolInDb);

            return await ResponseWrapper<int>.SuccessAsync(data: deleteSchoolId, message: "School deleted successfully");
		}
	}
}
