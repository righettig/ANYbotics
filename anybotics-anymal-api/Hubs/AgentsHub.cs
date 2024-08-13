using anybotics_anymal_api.Models;
using AnymalApi.Services;
using Microsoft.AspNetCore.SignalR;

namespace anybotics_anymal_api.Hubs;

public class AgentsHub : Hub
{
    private readonly AnymalService _anymalService;

    public AgentsHub(AnymalService anymalService)
    {
        _anymalService = anymalService;
    }

    public async Task StreamAgentsData()
    {
        while (true)
        {
            var agents = _anymalService.GetAllAgents()
                                        .Select(agent => new AgentDto
                                        {
                                            Id = agent.Id,
                                            Name = agent.Name,
                                            BatteryLevel = agent.BatteryLevel,
                                            Status = agent.Status
                                        }).ToList();

            await Clients.All.SendAsync("ReceiveAgentsData", agents);

            // Adjust the delay as needed
            await Task.Delay(1000);
        }
    }

    public async Task StreamAgentData(string id)
    {
        while (true)
        {
            var agent = _anymalService.GetAgentById(id);

            var agentDto = new AgentDto
            {
                Id = agent.Id,
                Name = agent.Name,
                BatteryLevel = agent.BatteryLevel,
                Status = agent.Status
            };

            await Clients.All.SendAsync("ReceiveAgentData", agentDto);

            // Adjust the delay as needed
            await Task.Delay(1000);
        }
    }
}
