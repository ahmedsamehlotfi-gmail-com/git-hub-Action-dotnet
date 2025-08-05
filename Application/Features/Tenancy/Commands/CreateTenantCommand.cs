﻿using Application.Features.Tenancy.Model;
using Application.Models.Wrapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Tenancy.Commands
{

	public record CreateTenantCommand : IRequest<IResponseWrapper>
    {
        public CreateTenantRequest  CreateTenant { get; set; }
    }

	public class CreateTenantCommandHandler : IRequestHandler<CreateTenantCommand, IResponseWrapper>
	{
        private readonly ITenantService _tenantService;

        public CreateTenantCommandHandler(ITenantService tenantService)
        {
            this._tenantService = tenantService;
        }
        public async Task<IResponseWrapper> Handle(CreateTenantCommand request, CancellationToken cancellationToken)
		{
            var tenantId = await _tenantService.CreateTenantAsync(request.CreateTenant, cancellationToken);
            return await ResponseWrapper<string>.SuccessAsync(data: tenantId, message: "Tenant Created Successfully");
		}
	}
}
