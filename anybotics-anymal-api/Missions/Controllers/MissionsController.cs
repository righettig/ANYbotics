using anybotics_anymal_api.Commands;
using anybotics_anymal_api.CustomAttributes;
using anybotics_anymal_api.Missions.Models;
using anybotics_anymal_api.Missions.Repository;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;

namespace anybotics_anymal_api.Missions.Controllers;

public class ExecuteMissionRequest 
{
    public string AgentId { get; set; }
    public string MissionId { get; set; }
}

[ApiController]
[Route("[controller]")]
public class MissionsController(IMissionRepository missionRepository, ICommandBus commandBus) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Mission>>> GetMissions()
    {
        var missions = await missionRepository.GetMissionsAsync();
        return Ok(missions);
    }

    [HttpPost("create")]
    [Deny("guest")]
    public async Task<ActionResult> CreateMission([FromBody] Mission mission)
    {
        mission.Id = Guid.NewGuid().ToString();
        await missionRepository.AddMissionAsync(mission);
        return Ok(mission);
    }

    [HttpDelete]
    [Deny("guest")]
    public async Task<ActionResult> DeleteMission(string missionId)
    {
        await missionRepository.DeleteMissionAsync(missionId);
        return Ok();
    }

    [HttpPost("execute")]
    [Deny("guest")]
    public async Task<IActionResult> ExecuteMission([FromBody] ExecuteMissionRequest request)
    {
        try
        {
            // Retrieve the mission from the repository
            var mission = await missionRepository.GetMissionByIdAsync(request.MissionId);
            if (mission == null)
            {
                return NotFound("Mission not found");
            }

            // Iterate through each command specified in the mission
            foreach (var commandName in mission.Commands)
            {
                // Find the command type based on the name
                var commandType = Assembly.GetExecutingAssembly().GetTypes()
                    .FirstOrDefault(t => t.Name == commandName && typeof(ICommand).IsAssignableFrom(t));

                if (commandType != null)
                {
                    // Check if the command type has the expected constructor
                    var constructor = commandType.GetConstructors()
                        .FirstOrDefault(c => c.GetParameters().Length == 2 &&
                                             c.GetParameters()[0].ParameterType == typeof(string) &&
                                             c.GetParameters()[1].ParameterType == typeof(string));

                    if (constructor != null)
                    {
                        // Create an instance of the command using the constructor
                        var command = constructor.Invoke([request.AgentId, UserUid]);

                        // Get the SendAsync method info
                        var sendAsyncMethod = 
                            typeof(ICommandBus).GetMethod("SendAsync").MakeGenericMethod(commandType);

                        // Invoke SendAsync with the created command instance
                        await (Task)sendAsyncMethod.Invoke(commandBus, [command]);
                    }
                    else
                    {
                        // Return a bad request if the constructor is not found
                        return BadRequest($"Command {commandName} does not have the expected constructor.");
                    }
                }
                else
                {
                    // Return a bad request if the command type is not found
                    return BadRequest($"Command {commandName} not found.");
                }
            }

            return Ok();
        }
        catch (Exception ex)
        {
            // Log the exception (ensure you have a logging mechanism set up)
            // For example: _logger.LogError(ex, "An error occurred while executing the mission.");
            return StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred.");
        }
    }

    private string? UserUid => HttpContext.Items["UserUid"] as string;
}
