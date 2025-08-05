using Application.Models.Wrapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Identity.Users.Commands
{

	public record UpdateUserRolesCommand : IRequest<IResponseWrapper>
	{
        public string RoleId { get; set; }
        public userRoleRequest  userRoleRequest { get; set; }
    }

	public class UpdateUserRolesCommandHandler : IRequestHandler<UpdateUserRolesCommand , IResponseWrapper>
	{
        private readonly IUserService _userService;

        public UpdateUserRolesCommandHandler(IUserService userService)
        {
           _userService = userService;
        }
        public async Task<IResponseWrapper> Handle(UpdateUserRolesCommand request, CancellationToken cancellationToken)
		{
            var userId = await _userService.AssignRolesAsync(request.RoleId, request.userRoleRequest);
            return await ResponseWrapper<string>.SuccessAsync(data: userId, message: "User Role Updated Successfully");
		}
	}
}
