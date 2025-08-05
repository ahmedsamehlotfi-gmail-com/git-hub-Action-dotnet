﻿using Application.Models.Wrapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Tenancy.Commands
{

	public record DeactivateTenantCommand : IRequest<IResponseWrapper>
	{
        public string TenantId { get; set; }
    }

	public class DeactivateTenantCommandHandler : IRequestHandler<DeactivateTenantCommand , IResponseWrapper>
	{
        private readonly ITenantService _tenantService;

        public DeactivateTenantCommandHandler(ITenantService tenantService)
        {
            _tenantService = tenantService;
        }
        public async Task<IResponseWrapper> Handle(DeactivateTenantCommand request, CancellationToken cancellationToken)
		{
            var tenantId = await _tenantService.ActivateAsync(request.TenantId);
            return await ResponseWrapper<string>.SuccessAsync(data: tenantId, "Tenant de-activated successfully");
        }
	}
}
