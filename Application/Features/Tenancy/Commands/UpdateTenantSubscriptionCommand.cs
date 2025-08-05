using Application.Features.Tenancy.Model;
using Application.Models.Wrapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Tenancy.Commands
{

	public record UpdateTenantSubscriptionCommand : IRequest<IResponseWrapper>
	{
        public UpdateTenantSubscriptionRequest  SubscriptionRequest { get; set; }
    }

	public class UpdateTenantSubscriptionCommandHandler : IRequestHandler<UpdateTenantSubscriptionCommand , IResponseWrapper>
	{
        private readonly ITenantService _tenantService;

        public UpdateTenantSubscriptionCommandHandler(ITenantService tenantService)
        {
            _tenantService = tenantService;
        }
        public async Task<IResponseWrapper> Handle(UpdateTenantSubscriptionCommand request, CancellationToken cancellationToken)
		{
			var tenantId = await _tenantService.UpdateSubscriptionAsync(request.SubscriptionRequest.TenantId , request.SubscriptionRequest.NewExpiryDate);
            return await ResponseWrapper<string>.SuccessAsync(data: tenantId, "Tenant subscription update successfully.");
		}
	}
}
