using Application.Models.Wrapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Identity.Users.Queries
{

	public record GetUserPermissionQuery : IRequest<IResponseWrapper>
	{
        public string UserId { get; set; }
    }

	public class GetUserPermissionQueryHandler : IRequestHandler<GetUserPermissionQuery, IResponseWrapper>
	{
        private readonly IUserService _userService;

        public GetUserPermissionQueryHandler(IUserService userService)
        {
            _userService = userService;
        }
        public async Task<IResponseWrapper> Handle(GetUserPermissionQuery request, CancellationToken cancellationToken)
		{
            var permissions = await _userService.GetPermissionsAsync(request.UserId, cancellationToken);
            return await ResponseWrapper<List<string>>.SuccessAsync(data: permissions);
		}
	}
}
