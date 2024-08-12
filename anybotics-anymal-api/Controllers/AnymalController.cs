using anybotics_anymal_api.Models;
using Microsoft.AspNetCore.Mvc;
using AnymalService = AnymalApi.Services.AnymalService;

namespace AnymalApi.Controllers;

[ApiController]
[Route("[controller]")]
public class AnymalController : ControllerBase
{
    private readonly AnymalService _anymalService;

    public AnymalController(AnymalService anymalService)
    {
        _anymalService = anymalService;
    }

    // GET: api/anymal
    [HttpGet]
    public ActionResult<IEnumerable<AgentDto>> GetAllAgents()
    {
        var agents = _anymalService.GetAllAgents()
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
        var agent = _anymalService.GetAgentById(id);

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

    // POST: api/anymal/recharge
    [HttpPost("recharge")]
    public async Task<IActionResult> RechargeAgentBattery([FromBody] string id)
    {
        if (string.IsNullOrEmpty(id))
        {
            return BadRequest("Invalid id.");
        }

        var response = await _anymalService.NotifyRechargeBatteryAsync(id);

        return Ok(response);
    }
}
