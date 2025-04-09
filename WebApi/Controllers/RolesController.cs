using Application.Features.Identity.Roles;
using Application.Features.Identity.Roles.Command;
using Application.Features.Identity.Roles.Queries;
using Infrastructure.Identity.Auth;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static Infrastructure.Identity.Constants.PermissionConstants;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RolesController : BaseApiController
    {
        [HttpPost("add")]
        [ShouldHavePermission(SchoolAction.Create, SchoolFeature.Roles)]
        public async Task<IActionResult> AddRoleAsync([FromBody] CreateRolerequest createRolerequest)
        {
            var response = await _mediator.Send(new CreateRoleCommand { Rolerequest = createRolerequest });
            if (response.IsSuccessful)
            {
                return Ok(response);
            } 
            return BadRequest(response);
        }


        [HttpPut("update")]
        [ShouldHavePermission(SchoolAction.Update, SchoolFeature.Roles)]
        public async Task<IActionResult> UpdateRoleAsync([FromBody] UpdateRoleRequest updateRoleRequest)
        {
            var response = await _mediator.Send(new UpdateRoleCommand { UpdateRole = updateRoleRequest });
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }


        [HttpPut("update-permission")]
        [ShouldHavePermission(SchoolAction.Update, SchoolFeature.Roles)]
        public async Task<IActionResult> UpdateRoleClaimsAsync([FromBody] UpdateRolePermissions UpdateRoleClaims)
        {
            var response = await _mediator.Send(new UpdateRolePermissionsCommand {UpdateRolePermissions = UpdateRoleClaims });
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [HttpDelete("delete/{roleId}")]
        [ShouldHavePermission(SchoolAction.Delete, SchoolFeature.Roles)]
        public async Task<IActionResult> DeleteRoleAsync(string roleId)
        {
            var response = await _mediator.Send(new DeleteRoleCommand { RoleId = roleId });
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [HttpGet("all")]
        [ShouldHavePermission(SchoolAction.view, SchoolFeature.Roles)]
        public async Task<IActionResult> GetRoleAsync()
        {
            var response = await _mediator.Send(new GetRolesQuery());
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }


        [HttpGet("partial/{roleId}")]
        [ShouldHavePermission(SchoolAction.view, SchoolFeature.Roles)]
        public async Task<IActionResult> GetRoleByIdAsync(string roleId)
        {
            var response = await _mediator.Send(new GetRoleByIdQuery { RoleId = roleId });
            if (response.IsSuccessful)
            {
                return Ok(response);
            }

            return BadRequest(response);
        }



        [HttpGet("full/{roleId}")]
        [ShouldHavePermission(SchoolAction.view, SchoolFeature.Roles)]
        public async Task<IActionResult> GetDetailedRoleByIdAsync(string roleId)
        {
            var response = await _mediator.Send(new GetRoleWithPermissionQuery { RoleId = roleId });
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }
    }
}
