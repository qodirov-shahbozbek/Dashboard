using Dashboard.Application.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Dashboard.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ClientsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllClients(GetAllClientsQuery query)
        {
            var result = await _mediator.Send(query);
            return Ok(result);
        }
    }
}
