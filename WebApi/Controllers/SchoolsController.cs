using Application.Features.Schools;
using Application.Features.Schools.Commands;
using Application.Features.Schools.Queries;
using Infrastructure.Identity.Auth;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static Infrastructure.Identity.Constants.PermissionConstants;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SchoolsController : BaseApiController
    {
        [HttpPost("add")]
        [ShouldHavePermission(SchoolAction.Create , SchoolFeature.Schools)]
        public async Task<IActionResult> CreateSchoolAsync(CreateSchoolRequest createSchool)
        {
            var response = await _mediator.Send(new CreateSchoolCommand { SchoolRequest = createSchool });
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }
        [HttpPut("update")]
        [ShouldHavePermission(SchoolAction.Update, SchoolFeature.Schools)]
        public async Task<IActionResult> UpdateSchoolAsync(UpdateSchoolRequest updateSchool)
        {
            var response = await _mediator.Send(new UpdateSchoolCommand { schoolRequest = updateSchool });
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [HttpDelete("delete/{schoolId}")]
        [ShouldHavePermission(SchoolAction.Delete, SchoolFeature.Schools)]
        public async Task<IActionResult> DeleteSchoolAsync(int schoolId)
        {
            var response = await _mediator.Send(new DeleteSchoolCommand { SchoolId = schoolId });
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [HttpGet("by-id/{schoolId}")]
        [ShouldHavePermission(SchoolAction.view, SchoolFeature.Schools)]
        public async Task<IActionResult> GetSchoolByIdAsync(int schoolId)
        {
            var response = await _mediator.Send(new GetSchoolByIdQuery { SchoolId = schoolId });
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return NotFound(response);
        }

        [HttpGet("by-Name/{schoolId}")]
        [ShouldHavePermission(SchoolAction.view, SchoolFeature.Schools)]
        public async Task<IActionResult> GetSchoolByNameAsync(string schoolName)
        {
            var response = await _mediator.Send(new GetSchoolByNameQuery { Name = schoolName });
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return NotFound(response);
        }

        [HttpGet("all")]
        [ShouldHavePermission(SchoolAction.view, SchoolFeature.Schools)]
        public async Task<IActionResult> GetSchoolAsync()
        {
            var response = await _mediator.Send(new GetSchoolsQuery());
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return NotFound(response);
        }
    }
}
