using AnymalGrpc;
using Grpc.Core;
using System.Collections.Concurrent;

namespace anybotics_anymal_api.Services;

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

    public override Task StreamRechargeBatteryEvents(RechargeBatteryEvent request,
                                                     IServerStreamWriter<RechargeBatteryEvent> responseStream,
                                                     ServerCallContext context)
        => StreamEvents(request.Id, client => client.RechargeBatteryStream = responseStream, context);

    public override Task StreamShutdownEvents(ShutdownEvent request,
                                              IServerStreamWriter<ShutdownEvent> responseStream,
                                              ServerCallContext context)
        => StreamEvents(request.Id, client => client.ShutdownStream = responseStream, context);

    public override Task StreamWakeupEvents(WakeupEvent request,
                                            IServerStreamWriter<WakeupEvent> responseStream,
                                            ServerCallContext context)
        => StreamEvents(request.Id, client => client.WakeupStream = responseStream, context);

    public override Task StreamSetManualModeEvents(SetManualModeEvent request,
                                                   IServerStreamWriter<SetManualModeEvent> responseStream,
                                                   ServerCallContext context)
        => StreamEvents(request.Id, client => client.SetManualModeStream = responseStream, context);

    public override Task StreamThermalInspectionEvents(ThermalInspectionEvent request,
                                                       IServerStreamWriter<ThermalInspectionEvent> responseStream,
                                                       ServerCallContext context)
        => StreamEvents(request.Id, client => client.ThermalInspectionStream = responseStream, context);

    public override Task StreamCombustibleInspectionEvents(CombustibleInspectionEvent request,
                                                           IServerStreamWriter<CombustibleInspectionEvent> responseStream,
                                                           ServerCallContext context)
        => StreamEvents(request.Id, client => client.CombustibleInspectionStream = responseStream, context);

    public override Task StreamGasInspectionEvents(GasInspectionEvent request,
                                                   IServerStreamWriter<GasInspectionEvent> responseStream,
                                                   ServerCallContext context)
        => StreamEvents(request.Id, client => client.GasInspectionStream = responseStream, context);

    public override Task StreamAcousticMeasureEvents(AcousticMeasureEvent request,
                                                     IServerStreamWriter<AcousticMeasureEvent> responseStream,
                                                     ServerCallContext context)
        => StreamEvents(request.Id, client => client.AcousticMeasureStream = responseStream, context);

    public Task<UpdateResponse> RechargeBatteryAsync(string id)
        => PerformAgentActionAsync(id, async agentClient =>
        {
            if (agentClient.Agent.Status == AnymalGrpc.Status.Offline)
            {
                throw new InvalidOperationException("Agent is Offline. Recharge requests are ignored.");
            }

            var @event = new RechargeBatteryEvent { Id = id };
            await agentClient.RechargeBatteryStream?.WriteAsync(@event);

            agentClient.Agent.BatteryLevel = 100;
            agentClient.Agent.Status = AnymalGrpc.Status.Active;
        },
        $"Recharged agent {id} to 100%.", "Agent not found.");

    public Task<UpdateResponse> ShutdownAsync(string id)
        => PerformAgentActionAsync(id, async agentClient =>
        {
            if (agentClient.Agent.Status == AnymalGrpc.Status.Offline)
            {
                throw new InvalidOperationException("Agent is Offline. Shutdown requests are ignored.");
            }

            var @event = new ShutdownEvent { Id = id };
            await agentClient.ShutdownStream?.WriteAsync(@event);

            agentClient.Agent.Status = AnymalGrpc.Status.Offline;
        },
        $"Shutting down agent {id}.", "Agent not found.");

    public Task<UpdateResponse> WakeupAsync(string id)
        => PerformAgentActionAsync(id, async agentClient =>
        {
            if (agentClient.Agent.Status == AnymalGrpc.Status.Unavailable ||
                agentClient.Agent.Status == AnymalGrpc.Status.Active)
            {
                throw new InvalidOperationException("Agent is either already Active or Unavailable. Wake up requests are ignored.");
            }

            var @event = new WakeupEvent { Id = id };
            await agentClient.WakeupStream?.WriteAsync(@event);

            agentClient.Agent.Status = AnymalGrpc.Status.Active;
        },
        $"Waking up agent {id}.", "Agent not found.");

    public Task<UpdateResponse> SetManualModeAsync(string id, bool manualMode)
        => PerformAgentActionAsync(id, async agentClient =>
        {
            if (agentClient.Agent.Status == AnymalGrpc.Status.Unavailable)
            {
                throw new InvalidOperationException("Agent is Unavailable. Set manual mode requests are ignored.");
            }

            var @event = new SetManualModeEvent { Id = id, ManualMode = manualMode };
            await agentClient.SetManualModeStream?.WriteAsync(@event);

            agentClient.Agent.ManualMode = manualMode;
        },
        $"Setting up manual mode for agent {id} with value {manualMode}", "Agent not found.");

    public Task<UpdateResponse> ThermalInspectionAsync(string id)
        => PerformAgentActionAsync(id, async agentClient =>
        {
            if (agentClient.Agent.Status == AnymalGrpc.Status.Offline)
            {
                throw new InvalidOperationException("Agent is Offline. ThermalInspection requests are ignored.");
            }

            var @event = new ThermalInspectionEvent { Id = id };
            await agentClient.ThermalInspectionStream?.WriteAsync(@event);
        },
        $"Performing thermal inspection agent {id}.", "Agent not found.");

    public Task<UpdateResponse> CombustibleInspectionAsync(string id)
        => PerformAgentActionAsync(id, async agentClient =>
        {
            if (agentClient.Agent.Status == AnymalGrpc.Status.Offline)
            {
                throw new InvalidOperationException("Agent is Offline. CombustibleInspection requests are ignored.");
            }

            var @event = new CombustibleInspectionEvent { Id = id };
            await agentClient.CombustibleInspectionStream?.WriteAsync(@event);
        },
        $"Performing combustible inspection agent {id}.", "Agent not found.");

    public Task<UpdateResponse> GasInspectionAsync(string id)
        => PerformAgentActionAsync(id, async agentClient =>
        {
            if (agentClient.Agent.Status == AnymalGrpc.Status.Offline)
            {
                throw new InvalidOperationException("Agent is Offline. GasInspection requests are ignored.");
            }

            var @event = new GasInspectionEvent { Id = id };
            await agentClient.GasInspectionStream?.WriteAsync(@event);
        },
        $"Performing gas inspection agent {id}.", "Agent not found.");

    public Task<UpdateResponse> AcousticMeasureAsync(string id)
        => PerformAgentActionAsync(id, async agentClient =>
        {
            if (agentClient.Agent.Status == AnymalGrpc.Status.Offline)
            {
                throw new InvalidOperationException("Agent is Offline. AcousticMeasure requests are ignored.");
            }

            var @event = new AcousticMeasureEvent { Id = id };
            await agentClient.AcousticMeasureStream?.WriteAsync(@event);
        },
        $"Performing acoustic measure agent {id}.", "Agent not found.");

    public IEnumerable<Agent> GetAllAgents() => _agentClients.Values.Select(ac => ac.Agent);

    public Agent GetAgentById(string id) => _agentClients.GetValueOrDefault(id)?.Agent;

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
                                                  Action<Agent> updateAction,
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
