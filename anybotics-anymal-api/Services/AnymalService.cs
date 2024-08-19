using anybotics_anymal_common.Domain;
using AnymalGrpc;
using Grpc.Core;
using System.Collections.Concurrent;

namespace anybotics_anymal_api.Services;

public partial class AnymalService : AnymalGrpc.AnymalService.AnymalServiceBase
{
    private readonly ILogger<AnymalService> _logger;

    private static readonly ConcurrentDictionary<string, AgentClient> _agentClients = new();

    public AnymalService(ILogger<AnymalService> logger)
    {
        _logger = logger;
    }

    public override Task<RegistrationResponse> RegisterAgent(RegistrationRequest request, ServerCallContext context)
    {
        var agentClient = new AgentClient { Agent = new AnymalAgent(request.Id, request.Name) };
        _agentClients[request.Id] = agentClient;

        _logger.LogInformation($"Registered agent {request.Name} with ID {request.Id}.");
        return Task.FromResult(new RegistrationResponse { Success = true, Message = "Agent registered successfully." });
    }

    public override Task<UpdateResponse> UpdateBattery(BatteryUpdate request, ServerCallContext context)
        => UpdateAgentField(request.Id, agent => agent.BatteryLevel = request.BatteryLevel, "battery level", request.BatteryLevel);

    public override Task<UpdateResponse> UpdateStatus(StatusUpdate request, ServerCallContext context)
        => UpdateAgentField(request.Id, agent => agent.Status = request.Status, "status", request.Status);

    public override Task StreamCommands(CommandListener request,
                                        IServerStreamWriter<Command> responseStream,
                                        ServerCallContext context)
        => StreamEvents(request.Id, client => client.CommandStream = responseStream, context);

    public IEnumerable<AnymalAgent> GetAllAgents() => _agentClients.Values.Select(ac => ac.Agent);

    public AnymalAgent GetAgentById(string id) => _agentClients.GetValueOrDefault(id)?.Agent;

    private async Task StreamEvents<T>(string id,
                                       Func<AgentClient, IServerStreamWriter<T>> getStream,
                                       ServerCallContext context) where T : class
    {
        if (_agentClients.TryGetValue(id, out var agentClient))
        {
            _ = getStream(agentClient);

            try
            {
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
                _logger.LogError(ex, "Error occurred while streaming events.");
            }
            finally
            {
                _agentClients.TryRemove(id, out _);
                _logger.LogInformation($"Client {context.Peer} stopped streaming events.");
            }
        }
    }

    private Task<UpdateResponse> UpdateAgentField(string id,
                                                  Action<AnymalAgent> updateAction,
                                                  string fieldName,
                                                  object updatedValue)
    {
        if (_agentClients.TryGetValue(id, out var agentClient))
        {
            updateAction(agentClient.Agent);
            var message = $"{fieldName.CapitalizeFirstLetter()} updated to {updatedValue}.";
            return _logger.LogAndReturn(message, true);
        }
        return _logger.LogAndReturn("Agent not found.", false);
    }

    private Task<UpdateResponse> PerformAgentActionAsync(string id,
                                                         Func<AgentClient, Task> action,
                                                         string successMessage,
                                                         string failureMessage)
    {
        if (_agentClients.TryGetValue(id, out var agentClient))
        {
            try
            {
                action(agentClient).Wait();
                _logger.LogInformation(successMessage);
                return Task.FromResult(new UpdateResponse { Success = true, Message = successMessage });
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning(ex.Message);
                return Task.FromResult(new UpdateResponse { Success = false, Message = ex.Message });
            }
        }
        return Task.FromResult(new UpdateResponse { Success = false, Message = failureMessage });
    }
}
