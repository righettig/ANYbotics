using AnymalGrpc;
using Microsoft.AspNetCore.Mvc;
using AnymalService = AnymalApi.Services.AnymalService;

namespace AnymalApi.Controllers;

[ApiController]
[Route("[controller]")]
public class AnymalController : ControllerBase
{
    [HttpGet]
    public IEnumerable<Agent> GetAllAgents() => AnymalService.GetAllAgents();

    [HttpGet("{id}")]
    public ActionResult<Agent> GetAgentById(string id)
    {
        var agent = AnymalService.GetAgentById(id);
        if (agent == null)
        {
            return NotFound();
        }

        return agent;
    }
}
