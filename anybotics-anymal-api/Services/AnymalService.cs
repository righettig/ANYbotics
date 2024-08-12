using AnymalGrpc;
using Grpc.Core;
using System.Collections.Concurrent;

namespace AnymalApi.Services;

public class AnymalService : AnymalGrpc.AnymalService.AnymalServiceBase
{
    private readonly ILogger<AnymalService> _logger;
    
    private static readonly ConcurrentDictionary<string, Agent> _agents = new();

    // Concurrent dictionary to keep track of connected clients
    private readonly ConcurrentDictionary<string, IServerStreamWriter<RechargeBatteryEvent>> _clients = new();

    public AnymalService(ILogger<AnymalService> logger)
    {
        _logger = logger;
    }

    public override Task<RegistrationResponse> RegisterAgent(Agent request, ServerCallContext context)
    {
        _agents[request.Id] = request;
        _logger.LogInformation($"Agent {request.Name} with ID {request.Id} registered.");

        return Task.FromResult(new RegistrationResponse
        {
            Success = true,
            Message = "Agent registered successfully."
        });
    }

    public override Task<UpdateResponse> UpdateBattery(BatteryUpdate request, ServerCallContext context)
    {
        if (_agents.TryGetValue(request.Id, out var agent))
        {
            agent.BatteryLevel = request.BatteryLevel;

            _logger.LogInformation($"Agent {agent.Name} (ID: {agent.Id}) battery updated to {agent.BatteryLevel}.");

            var message = agent.BatteryLevel > 0
                ? "Battery level updated."
                : "Battery level is now 0. Shutting down.";

            return Task.FromResult(new UpdateResponse
            {
                Success = true,
                Message = message
            });
        }

        return Task.FromResult(new UpdateResponse
        {
            Success = false,
            Message = "Agent not found."
        });
    }

    public override Task<UpdateResponse> UpdateStatus(StatusUpdate request, ServerCallContext context)
    {
        if (_agents.TryGetValue(request.Id, out var agent))
        {
            agent.Status = request.Status;

            _logger.LogInformation($"Agent {agent.Name} (ID: {agent.Id}) status updated to {agent.Status}.");

            return Task.FromResult(new UpdateResponse
            {
                Success = true,
                Message = "Status updated."
            });
        }

        return Task.FromResult(new UpdateResponse
        {
            Success = false,
            Message = "Agent not found."
        });
    }

    // StreamRechargeBatteryEvents: A method to register clients and stream recharge events
    public override async Task StreamRechargeBatteryEvents(
        RechargeBatteryEvent request, 
        IServerStreamWriter<RechargeBatteryEvent> responseStream, 
        ServerCallContext context)
    {
        // Register the client
        _clients[request.Id] = responseStream;

        // Keep the stream open
        while (!context.CancellationToken.IsCancellationRequested)
        {
            await Task.Delay(1000); // Keep alive, adjust as necessary
        }

        // Remove client on disconnection
        _clients.TryRemove(request.Id, out _);
    }

    // Method to send a recharge battery event to a specific client
    public async Task<UpdateResponse> NotifyRechargeBatteryAsync(string id)
    {
        if (_clients.TryGetValue(id, out var clientStream))
        {
            var rechargeEvent = new RechargeBatteryEvent
            {
                Id = id,
            };

            await clientStream.WriteAsync(rechargeEvent);

            if (_agents.TryGetValue(id, out var agent))
            {
                agent.BatteryLevel = 100;
                agent.Status = AnymalGrpc.Status.Active;

                _logger.LogInformation($"Agent {agent.Name} (ID: {agent.Id}) battery recharged to 100%.");
            }

            return new UpdateResponse
            {
                Success = true,
                Message = "Battery recharged successfully."
            };
        }
        else 
        {
            return new UpdateResponse
            {
                Success = false,
                Message = "Agent not found."
            };
        }
    }

    public IEnumerable<Agent> GetAllAgents() => _agents.Values;

    public Agent GetAgentById(string id) => _agents.GetValueOrDefault(id);
}
