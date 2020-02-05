using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace scripting.Scripts
{
    [Route("api/[controller]")]
    [ApiController]
    [ApiVersion("1.0")]
    public class ScriptsController : ControllerBase
    {
        private readonly IMediator mediator;

        public ScriptsController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpPost("execute")]
        public async Task<ActionResult<object>> Execute(ExecuteScript request)
        {
            try
            {
                return await mediator.Send(request);
        }catch(Microsoft.CodeAnalysis.Scripting.CompilationErrorException e)
            {
                return BadRequest(e.Message);
            }
}
    }
}
