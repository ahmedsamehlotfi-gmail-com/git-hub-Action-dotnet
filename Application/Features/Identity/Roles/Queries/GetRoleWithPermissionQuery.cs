using Application.Models.Wrapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Identity.Roles.Queries
{

	public record GetRoleWithPermissionQuery : IRequest<IResponseWrapper>
	{
        public string RoleId { get; set; }
    }

	public class GetRoleWithPermissionQueryHandler : IRequestHandler<GetRoleWithPermissionQuery, IResponseWrapper>
	{
        private readonly IRoleService _roleService;

        public GetRoleWithPermissionQueryHandler(IRoleService roleService)
        {
            _roleService = roleService;
        }
        public async Task<IResponseWrapper> Handle(GetRoleWithPermissionQuery request, CancellationToken cancellationToken)
		{
            var role = await _roleService.GetRoleWithPermissionsAsync(request.RoleId, cancellationToken);
            return await ResponseWrapper<RoleDto>.SuccessAsync(data: role);
		} 
	}
}
