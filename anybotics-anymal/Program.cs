using AnymalGrpc;
using Grpc.Core;
using Grpc.Net.Client;

class Program
{
    static async Task Main(string[] args)
    {
        // Parse the agent name from the command-line arguments
        string agentName = args.Length > 0 ? args[0] : "Anymal";

        // Create a channel to the gRPC server
        using var channel = GrpcChannel.ForAddress("https://localhost:7272");
        var client = new AnymalService.AnymalServiceClient(channel);

        var agent = new Agent
        {
            Id = Guid.NewGuid().ToString(),
            Name = agentName,
            BatteryLevel = 100,
            Status = AnymalGrpc.Status.Active
        };

        try
        {
            // Register the agent
            await RegisterAgentAsync(client, agent);

            // Start monitoring events
            _ = MonitorBatteryRechargeEventsAsync(client, agent);
            _ = MonitorShutdownEventsAsync(client, agent);
            _ = MonitorWakeupEventsAsync(client, agent);
            _ = MonitorSetManualModeEventsAsync(client, agent);
            _ = MonitorThermalInspectionEventsAsync(client, agent);
            _ = MonitorCombustibleInspectionEventsAsync(client, agent);
            _ = MonitorGasInspectionEventsAsync(client, agent);
            _ = MonitorAcousticMeasureEventsAsync(client, agent);

            // Start battery decrease loop
            await BatteryDecreaseLoopAsync(client, agent);
        }
        catch (RpcException ex) when (ex.StatusCode == StatusCode.Unavailable)
        {
            Console.WriteLine($"Error: Server is unavailable.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Unexpected error: {ex.Message}");
        }
        finally
        {
            await channel.ShutdownAsync();
        }
    }

    static async Task RegisterAgentAsync(AnymalService.AnymalServiceClient client, Agent agent)
    {
        var registrationResponse = await client.RegisterAgentAsync(agent);
        Console.WriteLine($"Registration: {registrationResponse.Message} (ID: {agent.Id})");
    }

    static async Task MonitorBatteryRechargeEventsAsync(AnymalService.AnymalServiceClient client, Agent agent)
    {
        using var call = client.StreamRechargeBatteryEvents(new RechargeBatteryEvent { Id = agent.Id });

        await foreach (var response in call.ResponseStream.ReadAllAsync())
        {
            if (response.Id == agent.Id && agent.Status == AnymalGrpc.Status.Active)
            {
                agent.BatteryLevel = 100;
                Console.WriteLine($"Battery recharged to 100% for Agent {agent.Name} (ID: {agent.Id})");
            }
        }
    }

    static async Task MonitorShutdownEventsAsync(AnymalService.AnymalServiceClient client, Agent agent)
    {
        using var call = client.StreamShutdownEvents(new ShutdownEvent { Id = agent.Id });

        await foreach (var response in call.ResponseStream.ReadAllAsync())
        {
            if (response.Id == agent.Id && agent.Status == AnymalGrpc.Status.Active)
            {
                agent.Status = AnymalGrpc.Status.Offline;
                Console.WriteLine($"Shutting down {agent.Name} (ID: {agent.Id})");
            }
        }
    }

    static async Task MonitorWakeupEventsAsync(AnymalService.AnymalServiceClient client, Agent agent)
    {
        using var call = client.StreamWakeupEvents(new WakeupEvent { Id = agent.Id });

        await foreach (var response in call.ResponseStream.ReadAllAsync())
        {
            if (response.Id == agent.Id && agent.Status == AnymalGrpc.Status.Offline && agent.BatteryLevel > 0)
            {
                agent.Status = AnymalGrpc.Status.Active;
                Console.WriteLine($"Waking up {agent.Name} (ID: {agent.Id})");
            }
        }
    }

    static async Task MonitorSetManualModeEventsAsync(AnymalService.AnymalServiceClient client, Agent agent)
    {
        using var call = client.StreamSetManualModeEvents(new SetManualModeEvent { Id = agent.Id });

        await foreach (var response in call.ResponseStream.ReadAllAsync())
        {
            if (response.Id == agent.Id && agent.Status == AnymalGrpc.Status.Active)
            {
                agent.ManualMode = response.ManualMode;
                Console.WriteLine($"Setting up manual mode for {agent.Name} (ID: {agent.Id}) with value {response.ManualMode}");
            }
        }
    }

    static async Task MonitorThermalInspectionEventsAsync(AnymalService.AnymalServiceClient client, Agent agent)
    {
        using var call = client.StreamThermalInspectionEvents(new ThermalInspectionEvent { Id = agent.Id });

        await foreach (var response in call.ResponseStream.ReadAllAsync())
        {
            if (response.Id == agent.Id && agent.Status == AnymalGrpc.Status.Active && agent.BatteryLevel > 0)
            {
                Console.WriteLine($"Performing thermal inspection {agent.Name} (ID: {agent.Id})");
            }
        }
    }

    static async Task MonitorCombustibleInspectionEventsAsync(AnymalService.AnymalServiceClient client, Agent agent)
    {
        using var call = client.StreamCombustibleInspectionEvents(new CombustibleInspectionEvent { Id = agent.Id });

        await foreach (var response in call.ResponseStream.ReadAllAsync())
        {
            if (response.Id == agent.Id && agent.Status == AnymalGrpc.Status.Active && agent.BatteryLevel > 0)
            {
                Console.WriteLine($"Performing combustible inspection {agent.Name} (ID: {agent.Id})");
            }
        }
    }

    static async Task MonitorGasInspectionEventsAsync(AnymalService.AnymalServiceClient client, Agent agent)
    {
        using var call = client.StreamGasInspectionEvents(new GasInspectionEvent { Id = agent.Id });

        await foreach (var response in call.ResponseStream.ReadAllAsync())
        {
            if (response.Id == agent.Id && agent.Status == AnymalGrpc.Status.Active && agent.BatteryLevel > 0)
            {
                Console.WriteLine($"Performing gas inspection {agent.Name} (ID: {agent.Id})");
            }
        }
    }

    static async Task MonitorAcousticMeasureEventsAsync(AnymalService.AnymalServiceClient client, Agent agent)
    {
        using var call = client.StreamAcousticMeasureEvents(new AcousticMeasureEvent { Id = agent.Id });

        await foreach (var response in call.ResponseStream.ReadAllAsync())
        {
            if (response.Id == agent.Id && agent.Status == AnymalGrpc.Status.Active && agent.BatteryLevel > 0)
            {
                Console.WriteLine($"Performing acoustic measure {agent.Name} (ID: {agent.Id})");
            }
        }
    }

    static async Task BatteryDecreaseLoopAsync(AnymalService.AnymalServiceClient client, Agent agent)
    {
        while (agent.BatteryLevel > 0)
        {
            await Task.Delay(10 * 1000); // 10 seconds delay

            if (agent.Status == AnymalGrpc.Status.Active)
            {
                agent.BatteryLevel--;

                var updateResponse = await client.UpdateBatteryAsync(new BatteryUpdate
                {
                    Id = agent.Id,
                    BatteryLevel = agent.BatteryLevel
                });

                Console.WriteLine($"Battery Update: {updateResponse.Message}");
            }
        }

        await NotifyBatteryDepletionAsync(client, agent);
    }

    static async Task NotifyBatteryDepletionAsync(AnymalService.AnymalServiceClient client, Agent agent)
    {
        // Final battery update when battery reaches 0
        var finalUpdateResponse = await client.UpdateBatteryAsync(new BatteryUpdate
        {
            Id = agent.Id,
            BatteryLevel = agent.BatteryLevel
        });

        Console.WriteLine($"Final Battery Update: {finalUpdateResponse.Message}");

        // Update status to Unavailable and notify the server
        agent.Status = AnymalGrpc.Status.Unavailable;

        var statusUpdateResponse = await client.UpdateStatusAsync(new StatusUpdate
        {
            Id = agent.Id,
            Status = agent.Status
        });

        Console.WriteLine($"Status Update: {statusUpdateResponse.Message}");
    }
}
