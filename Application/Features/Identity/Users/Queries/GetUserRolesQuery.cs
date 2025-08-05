using Application.Models.Wrapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Identity.Users.Queries
{

	public record GetUserRolesQuery : IRequest<IResponseWrapper>
	{
        public string UserId { get; set; }
    }

	public class GetUserRolesQueryHandler : IRequestHandler<GetUserRolesQuery, IResponseWrapper>
	{
        private readonly IUserService _userService;

        public GetUserRolesQueryHandler(IUserService userService)
        {
            _userService = userService;
        }
        public async Task<IResponseWrapper> Handle(GetUserRolesQuery request, CancellationToken cancellationToken)
		{
			var userRoles = await _userService.GetUserRolesAsync(request.UserId , cancellationToken);
            return await ResponseWrapper<List<UserRoleDto>>.SuccessAsync(data: userRoles);  
		}
	}
}
