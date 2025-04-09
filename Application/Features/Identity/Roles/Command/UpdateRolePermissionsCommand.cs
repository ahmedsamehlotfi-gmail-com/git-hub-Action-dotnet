using Application.Features.Identity.Roles;
using Application.Models.Wrapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Identity.Roles.Command
{

    public record UpdateRolePermissionsCommand : IRequest<IResponseWrapper>
    {
        public UpdateRolePermissions UpdateRolePermissions { get; set; }
    }

    public class UpdateRolePermissionsCommandHandler : IRequestHandler<UpdateRolePermissionsCommand, IResponseWrapper>
    {
        private readonly IRoleService _roleService;

        public UpdateRolePermissionsCommandHandler(IRoleService roleService)
        {
            _roleService = roleService;
        }
        public async Task<IResponseWrapper> Handle(UpdateRolePermissionsCommand request, CancellationToken cancellationToken)
        {
            var message = await _roleService.UpdatePermissionsAsync(request.UpdateRolePermissions);
            return await ResponseWrapper<string>.SuccessAsync(message: message);
        }
    }
}
