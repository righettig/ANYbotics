using anybotics_anymal_api.Models;
using Microsoft.AspNetCore.Mvc;
using AnymalService = AnymalApi.Services.AnymalService;

namespace AnymalApi.Controllers;

[ApiController]
[Route("[controller]")]
public class AnymalController : ControllerBase
{
    // GET: api/anymal
    [HttpGet]
    public ActionResult<IEnumerable<AgentDto>> GetAllAgents()
    {
        var agents = AnymalService.GetAllAgents()
            .Select(agent => new AgentDto
            {
                Id = agent.Id,
                Name = agent.Name,
                BatteryLevel = agent.BatteryLevel,
                Status = agent.Status
            })
            .ToList();

        return Ok(agents);
    }

    // GET: api/anymal/{id}
    [HttpGet("{id}")]
    public ActionResult<AgentDto> GetAgentById(string id)
    {
        var agent = AnymalService.GetAgentById(id);

        if (agent == null)
        {
            return NotFound();
        }

        var agentDto = new AgentDto
        {
            Id = agent.Id,
            Name = agent.Name,
            BatteryLevel = agent.BatteryLevel,
            Status = agent.Status
        };

        return Ok(agentDto);
    }
}
