using Application.Features.Tenancy.Model;
using Application.Models.Wrapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Tenancy.Queries
{

	public record GetTenantsQuery : IRequest<IResponseWrapper>
	{
	}

	public class GetTenantsQueryHandler : IRequestHandler<GetTenantsQuery, IResponseWrapper>
	{
        private readonly ITenantService _tenantService;

        public GetTenantsQueryHandler(ITenantService tenantService)
        {
           _tenantService = tenantService;
        }
        public async Task<IResponseWrapper> Handle(GetTenantsQuery request, CancellationToken cancellationToken)
		{
			var tenantInDb = await _tenantService.GetTenantAsync();
			return await ResponseWrapper<List<TenantDto>>.SuccessAsync(data: tenantInDb);
		}
	}
}
