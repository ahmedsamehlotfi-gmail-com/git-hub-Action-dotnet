using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [ApiController]
    public class BaseApiController : ControllerBase
    {
        public ISender _sender = null!;
        public ISender _mediator => _sender ??= HttpContext.RequestServices.GetRequiredService<ISender>();
    }
}
