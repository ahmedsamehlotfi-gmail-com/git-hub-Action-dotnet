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

    public record CreateRoleCommand : IRequest<IResponseWrapper>
    {
        public CreateRolerequest Rolerequest { get; set; }
    }

    public class CreateRoleCommandHandler : IRequestHandler<CreateRoleCommand, IResponseWrapper>
    {
        private readonly IRoleService _roleService;

        public CreateRoleCommandHandler(IRoleService roleService)
        {
            _roleService = roleService;
        }
        public async Task<IResponseWrapper> Handle(CreateRoleCommand request, CancellationToken cancellationToken)
        {
            var newRoleId = await _roleService.CreateAsync(request.Rolerequest);
            return await ResponseWrapper<string>.SuccessAsync(data: newRoleId, message: newRoleId);
        }
    }
}
