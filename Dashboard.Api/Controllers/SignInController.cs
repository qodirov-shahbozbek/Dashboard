using Dashboard.Application.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Dashboard.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SignInController : ControllerBase
    {
        private readonly IMediator _mediator;
        public SignInController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login(SignInCommand command)
        {
            var token = await _mediator.Send(command);

            return Ok(token);
        }
    }
}
