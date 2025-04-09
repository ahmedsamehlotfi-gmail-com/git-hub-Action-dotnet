using Application.Features.Identity.Users;
using Application.Features.Identity.Users.Commands;
using Application.Features.Identity.Users.Queries;
using Infrastructure.Identity.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using static Infrastructure.Identity.Constants.PermissionConstants;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : BaseApiController
    {
        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> RegisterUSerAsync( [FromBody] CreateUserRequest createuserRequest)
        {
            var response = await _mediator.Send(new CreateUserCommand { CreateUserRequests = createuserRequest });
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [HttpPut("update")]
        [ShouldHavePermission(SchoolAction.Update , SchoolFeature.Users)]
        public async Task<IActionResult> UpdateUSerAsync([FromBody] UpdateUserRequest  updateUser)
        {
            var response = await _mediator.Send(new UpdateUserCommand { UpdateUserRequest = updateUser });
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [HttpPut("update-status")]
        [ShouldHavePermission(SchoolAction.Update, SchoolFeature.Users)]
        public async Task<IActionResult> ChangeUserStatusAsync([FromBody] ChangeUserStatusRequest  changeUserStatus)
        {
            var response = await _mediator.Send(new UpdateUserStatusCommand { changeUserStatus = changeUserStatus });
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }


        [HttpPut("update-roles/{roleId}")]
        [ShouldHavePermission(SchoolAction.Update, SchoolFeature.UserRoles)]
        public async Task<IActionResult> UpdateUserAsync([FromBody] userRoleRequest userRoleRequest , string roleId)
        {
            var response = await _mediator.Send(new UpdateUserRolesCommand { userRoleRequest = userRoleRequest , RoleId = roleId });
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [HttpDelete("delete/{userId}")]
        [ShouldHavePermission(SchoolAction.Delete, SchoolFeature.Users)]
        public async Task<IActionResult> DeleteUserAsync(string userId)
        {
            var response = await _mediator.Send(new DeleteUserCommand { UserId = userId });
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }
        [HttpGet("all")]
        [ShouldHavePermission(SchoolAction.view, SchoolFeature.Users)]

        public async Task<IActionResult> GetUsersAsync()
        {
            var response = await _mediator.Send(new GetAllUsersQuery() { });
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [HttpGet("{userId}")]
        [ShouldHavePermission(SchoolAction.view, SchoolFeature.Users)]
        public async Task<IActionResult> GetUserByIdAsync(string userId)
        {
            var response = await _mediator.Send(new GetUserByIdQuery { UserId = userId});
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [HttpGet("permission/{userId}")]
        [ShouldHavePermission(SchoolAction.view, SchoolFeature.RolesClaims)]
        public async Task<IActionResult> GetUserPermissionsAsync(string userId)
        {
            var response = await _mediator.Send(new GetUserPermissionQuery { UserId = userId });
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [HttpGet("users-roles/{userId}")]
        [ShouldHavePermission(SchoolAction.view, SchoolFeature.UserRoles)]
        public async Task<IActionResult> GetUserRolesAsync(string userId)
        {
            var response = await _mediator.Send(new GetUserRolesQuery { UserId = userId });
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }
    }
}
