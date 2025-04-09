using Application.Features.Tenancy.Commands;
using Application.Features.Tenancy.Model;
using Application.Features.Tenancy.Queries;
using Infrastructure.Identity.Auth;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static Infrastructure.Identity.Constants.PermissionConstants;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TenantsController : BaseApiController
    {
        [HttpPost("add")]
        [ShouldHavePermission(SchoolAction.Create, SchoolFeature.Tenants)]

        public async Task<IActionResult> CreateTenantAsync(CreateTenantRequest createTenant)
        {
            var response = await _mediator.Send(new CreateTenantCommand { CreateTenant = createTenant });
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [HttpPut("{tenantId}/activate")]
        [ShouldHavePermission(SchoolAction.Update, SchoolFeature.Tenants)]

        public async Task<IActionResult> ActivateTenantAsync(string tenantId)
        {
            var response = await _mediator.Send(new ActivateTenantCommand { TenantId = tenantId });
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [HttpPut("{tenantId}/deactivate")]
        [ShouldHavePermission(SchoolAction.Update, SchoolFeature.Tenants)]

        public async Task<IActionResult> DeActivateTenantAsync(string tenantId)
        {
            var response = await _mediator.Send(new DeactivateTenantCommand { TenantId = tenantId });
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [HttpPut("upgrade/{tenantId}/{newExpiryDate}")]
        [ShouldHavePermission(SchoolAction.Update, SchoolFeature.Tenants)]

        public async Task<IActionResult> UpgradeTenantSubscriptionAsync(UpdateTenantSubscriptionRequest updateTenant)
        {
            var response = await _mediator.Send(new UpdateTenantSubscriptionCommand { SubscriptionRequest = updateTenant });
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [HttpGet("{tenantId}")]
        [ShouldHavePermission(SchoolAction.view, SchoolFeature.Tenants)]
        public async Task<IActionResult> GetTenantByIdAsync(string tenantId)
        {
            var response = await _mediator.Send(new GetTenantByIdQuery { TenantId = tenantId });
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [HttpGet("all")]
        [ShouldHavePermission(SchoolAction.view, SchoolFeature.Tenants)]
        public async Task<IActionResult> GetTenantAsync(string tenantId)
        {
            var response = await _mediator.Send(new GetTenantsQuery() {  });
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }
    }
}
