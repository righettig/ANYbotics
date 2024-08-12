using AnymalGrpc;
using Grpc.Core;
using System.Collections.Concurrent;

namespace AnymalApi.Services;

public class AnymalService : AnymalGrpc.AnymalService.AnymalServiceBase
{
    private readonly ILogger<AnymalService> _logger;

    private static readonly ConcurrentDictionary<string, AgentClient> _agentClients = new();

    public AnymalService(ILogger<AnymalService> logger)
    {
        _logger = logger;
    }

    public override Task<RegistrationResponse> RegisterAgent(Agent request, ServerCallContext context)
    {
        var agentClient = new AgentClient { Agent = request };
        _agentClients[request.Id] = agentClient;

        _logger.LogInformation($"Registered agent {request.Name} with ID {request.Id}.");
        return Task.FromResult(new RegistrationResponse { Success = true, Message = "Agent registered successfully." });
    }

    public override Task<UpdateResponse> UpdateBattery(BatteryUpdate request, ServerCallContext context)
        => UpdateAgentField(request.Id, agent => agent.BatteryLevel = request.BatteryLevel, "battery level", request.BatteryLevel);

    public override Task<UpdateResponse> UpdateStatus(StatusUpdate request, ServerCallContext context)
        => UpdateAgentField(request.Id, agent => agent.Status = request.Status, "status", request.Status);

    public override async Task StreamRechargeBatteryEvents(RechargeBatteryEvent request, IServerStreamWriter<RechargeBatteryEvent> responseStream, ServerCallContext context)
    {
        if (_agentClients.TryGetValue(request.Id, out var agentClient))
        {
            agentClient.ClientStream = responseStream;

            try
            {
                // Keep the stream open until the client disconnects
                while (!context.CancellationToken.IsCancellationRequested)
                {
                    await Task.Delay(1000, context.CancellationToken);
                }
            }
            catch (TaskCanceledException)
            {
                _logger.LogWarning($"Stream for {context.Peer} was canceled.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while streaming recharge battery events.");
            }
            finally
            {
                _agentClients.TryRemove(request.Id, out _);

                _logger.LogInformation($"Client {context.Peer} stopped streaming recharge events.");
            }
        }
    }

    public async Task<UpdateResponse> NotifyRechargeBatteryAsync(string id)
    {
        if (_agentClients.TryGetValue(id, out var agentClient))
        {
            var rechargeEvent = new RechargeBatteryEvent { Id = id };

            if (agentClient.ClientStream != null)
            {
                await agentClient.ClientStream.WriteAsync(rechargeEvent);
            }

            agentClient.Agent.BatteryLevel = 100;
            agentClient.Agent.Status = AnymalGrpc.Status.Active;

            _logger.LogInformation($"Recharged agent {agentClient.Agent.Name} (ID: {agentClient.Agent.Id}) to 100%.");

            return new UpdateResponse { Success = true, Message = "Battery recharged successfully." };
        }
        return new UpdateResponse { Success = false, Message = "Agent not found." };
    }

    public IEnumerable<Agent> GetAllAgents() => _agentClients.Values.Select(ac => ac.Agent);

    public Agent GetAgentById(string id) => _agentClients.GetValueOrDefault(id)?.Agent;

    private Task<UpdateResponse> UpdateAgentField(string id, Action<Agent> updateAction, string fieldName, object updatedValue)
    {
        if (_agentClients.TryGetValue(id, out var agentClient))
        {
            // Apply the update action to the agent
            updateAction(agentClient.Agent);

            // Log the updated value
            _logger.LogInformation($"Updated agent {agentClient.Agent.Name} (ID: {agentClient.Agent.Id}) {fieldName}. New value: {updatedValue}.");

            // Prepare the message based on the fieldName and updated value
            var message = fieldName == "battery" && agentClient.Agent.BatteryLevel == 0
                ? "Battery level is now 0. Shutting down."
                : $"{fieldName.CapitalizeFirstLetter()} updated to {updatedValue}.";

            return Task.FromResult(new UpdateResponse { Success = true, Message = message });
        }
        return Task.FromResult(new UpdateResponse { Success = false, Message = "Agent not found." });
    }
}
