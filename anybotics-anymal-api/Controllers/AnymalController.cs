using anybotics_anymal_api.Commands;
using anybotics_anymal_api.CustomAttributes;
using anybotics_anymal_api.Models;
using AnymalGrpc;
using Microsoft.AspNetCore.Mvc;
using AnymalService = anybotics_anymal_api.Services.AnymalService;

namespace anybotics_anymal_api.Controllers;

//[Authorize] // TODO: this requires more setup!
[ApiController]
[Route("[controller]")]
public class AnymalController(ICommandBus commandBus,
                              ICommandRepository commandRepository,
                              AnymalService anymalService) : ControllerBase
{
    // GET: api/anymal
    [HttpGet]
    public ActionResult<IEnumerable<AgentDto>> GetAllAgents()
    {
        var agentDtos = anymalService.GetAllAgents()
            .Select(MapToDto)
            .ToList();

        return Ok(agentDtos);
    }

    // GET: api/anymal/{id}
    [HttpGet("{id}")]
    public ActionResult<AgentDto> GetAgentById(string id)
    {
        var agent = anymalService.GetAgentById(id);

        if (agent == null)
        {
            return NotFound();
        }

        var agentDto = MapToDto(agent);

        return Ok(agentDto);
    }

    // POST: api/anymal/shutdown
    [HttpPost("shutdown")]
    [Deny("guest")]
    public async Task<IActionResult> ShutdownAgent([FromBody] string id)
    {
        if (string.IsNullOrEmpty(id))
        {
            return BadRequest("Invalid id.");
        }

        var result = await commandBus.SendAsync(new ShutdownCommand(id, UserUid));

        return Ok(result);
    }

    // POST: api/anymal/wakeup
    [HttpPost("wakeup")]
    [Deny("guest")]
    public async Task<IActionResult> WakeupAgent([FromBody] string id)
    {
        if (string.IsNullOrEmpty(id))
        {
            return BadRequest("Invalid id.");
        }

        var result = await commandBus.SendAsync(new WakeUpCommand(id, UserUid));

        return Ok(result);
    }

    // POST: api/anymal/recharge
    [HttpPost("recharge")]
    [Deny("guest")]
    public async Task<IActionResult> RechargeAgentBattery([FromBody] string id)
    {
        if (string.IsNullOrEmpty(id))
        {
            return BadRequest("Invalid id.");
        }

        var result = await commandBus.SendAsync(new RechargeBatteryCommand(id, UserUid));

        return Ok(result);
    }

    [HttpGet("{agentId}/commands")]
    public async Task<IActionResult> GetCommands(string agentId)
    {
        var commandDtos = (await commandRepository.GetCommandsByAgentIdAsync(agentId))
            .Select(x => new
            {
                x.AgentId,
                x.InitiatedBy,
                x.Timestamp,
                Description = x.ToString(),
            })
            .OrderByDescending(x => x.Timestamp);

        return Ok(commandDtos);
    }

    private AgentDto MapToDto(Agent agent)
    {
        return new AgentDto
        {
            Id = agent.Id,
            Name = agent.Name,
            BatteryLevel = agent.BatteryLevel,
            Status = agent.Status
        };
    }

    private string? UserUid => HttpContext.Items["UserUid"] as string;
}
