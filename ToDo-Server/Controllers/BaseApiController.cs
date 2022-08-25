using MediatR;

using Microsoft.AspNetCore.Mvc;

using ToDo_Server.Application.Core;

namespace ToDo_Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BaseApiController : ControllerBase
    {
        private IMediator? _mediator;
        protected IMediator? Mediator => _mediator ??= HttpContext.RequestServices.GetService<IMediator>();

        //* handle custom return type 
        protected ActionResult HandleResult<T>(Result<T> res)
            => res == null
            ? NotFound()
            : res.IsSuccess && res.Value != null
                ? Ok(res.Value)
                : res.IsSuccess && res.Value == null
                    ? NotFound()
                    : BadRequest(res.Error);
    }
}
