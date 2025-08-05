using Application.Models.Wrapper;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Identity.Roles.Queries
{

	public record GetRoleByIdQuery : IRequest<IResponseWrapper>
	{
        public string RoleId { get; set; }
    }

	public class GetRoleByIdQueryHandler : IRequestHandler<GetRoleByIdQuery, IResponseWrapper>
	{
        private readonly IRoleService _roleService;
        private readonly ILogger<GetRoleByIdQueryHandler> _logger;

        public GetRoleByIdQueryHandler(IRoleService roleService , ILogger<GetRoleByIdQueryHandler> logger)
        {
            _roleService = roleService;
             _logger = logger;
        }
        public async Task<IResponseWrapper> Handle(GetRoleByIdQuery request, CancellationToken cancellationToken)
		{
			var role = await _roleService.GetRoleByIdAsync(request.RoleId , cancellationToken);
            _logger.LogInformation($"Role with id {request.RoleId} is fetched successfully");
            return await ResponseWrapper<RoleDto>.SuccessAsync(data: role);
		}
	}
}
