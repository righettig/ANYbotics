using anybotics_anymal_api.Commands;
using anybotics_anymal_api.Helpers;
using anybotics_anymal_api.Models;
using anybotics_anymal_api.Services;
using Microsoft.AspNetCore.SignalR;

namespace anybotics_anymal_api.Hubs;

public class AgentsHub : Hub
{
    private readonly AnymalService _anymalService;
    private readonly ICommandRepository _commandRepository;

    public AgentsHub(AnymalService anymalService, ICommandRepository commandRepository)
    {
        _anymalService = anymalService;
        _commandRepository = commandRepository;
    }

    public async Task StreamAgentsData()
    {
        while (true)
        {
            var agents = _anymalService.GetAllAgents()
                .Select(agent => new AgentDto(agent.Id, agent.Name, agent.BatteryLevel, agent.Status))
                .ToList();

            await Clients.All.SendAsync("ReceiveAgentsData", agents);

            // Adjust the delay as needed
            await Task.Delay(1000);
        }
    }

    public async Task StreamAgentData(string id)
    {
        var userEmailsCache = new Dictionary<string, string>();

        while (true)
        {
            var agent = _anymalService.GetAgentById(id);

            if (agent is not null) 
            {
                var commandDtos = await _commandRepository.GetCommandsByAgentIdAsync(id);

                // Fetch emails and build the command history
                var commandHistoryTasks = commandDtos.Select(async c =>
                {
                    // Try to get the email from the cache
                    if (!userEmailsCache.TryGetValue(c.InitiatedBy, out var userEmail))
                    {
                        // Fetch email if not found in cache
                        userEmail = await FirebaseUserHelper.GetUserEmailAsync(c.InitiatedBy);
                        userEmailsCache[c.InitiatedBy] = userEmail;
                    }

                    // Return a new CommandHistoryItem
                    return new CommandHistoryItem
                    {
                        InitiatedBy = userEmail,
                        Timestamp = c.Timestamp,
                        Description = c.ToString(),
                    };
                });

                // Await all tasks to complete and order them
                var commandHistory = await Task.WhenAll(commandHistoryTasks);

                var agentDto = new AgentDetailsDto(agent, commandHistory.OrderByDescending(x => x.Timestamp));

                await Clients.All.SendAsync("ReceiveAgentData", agentDto);

                // Adjust the delay as needed
                await Task.Delay(1000);
            }
        }
    }
}
