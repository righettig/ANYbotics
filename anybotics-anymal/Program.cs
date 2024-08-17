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
            _ = MonitorCommandsAsync(client, agent);

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

    static async Task MonitorCommandsAsync(AnymalService.AnymalServiceClient client, Agent agent)
    {
        using var call = client.StreamCommands(new CommandListener { Id = agent.Id });

        await foreach (var response in call.ResponseStream.ReadAllAsync())
        {
            if (response.Id == agent.Id)
            {
                // TODO: introduce command processor(s)
                // it should have a ConditionCheck(agent, response) and if it returns true then it invokes PerformCommand
                if (response.CommandId == "Shutdown")
                {
                    if (agent.Status == AnymalGrpc.Status.Active) 
                    {
                        agent.Status = AnymalGrpc.Status.Offline;
                        Console.WriteLine($"Shutting down {agent.Name} (ID: {agent.Id})");
                    }
                }
                else if (response.CommandId == "Wakeup") 
                {
                    if (agent.Status == AnymalGrpc.Status.Offline && agent.BatteryLevel > 0) 
                    {
                        agent.Status = AnymalGrpc.Status.Active;
                        Console.WriteLine($"Waking up {agent.Name} (ID: {agent.Id})");
                    }
                }
                else if (response.CommandId == "RechargeBattery")
                {
                    if (agent.Status == AnymalGrpc.Status.Active) 
                    {
                        agent.BatteryLevel = 100;
                        Console.WriteLine($"Battery recharged to 100% for Agent {agent.Name} (ID: {agent.Id})");
                    }
                }
                else if (response.CommandId == "SetManualMode")
                {
                    if (agent.Status == AnymalGrpc.Status.Active) 
                    {
                        var payload = response.Payload.Unpack<SetManualModeRequest>();

                        agent.ManualMode = payload.ManualMode;
                        Console.WriteLine($"Setting up manual mode for {agent.Name} (ID: {agent.Id}) with value {agent.ManualMode}");
                    }
                }
                else if (response.CommandId == "ThermalInspection")
                {
                    if (agent.Status == AnymalGrpc.Status.Active)
                    {
                        Console.WriteLine($"Performing thermal inspection {agent.Name} (ID: {agent.Id})");
                    }
                }
                else if (response.CommandId == "CombustibleInspection")
                {
                    if (agent.Status == AnymalGrpc.Status.Active)
                    {
                        Console.WriteLine($"Performing combustible inspection {agent.Name} (ID: {agent.Id})");
                    }
                }
                else if (response.CommandId == "GasInspection")
                {
                    if (agent.Status == AnymalGrpc.Status.Active)
                    {
                        Console.WriteLine($"Performing gas inspection {agent.Name} (ID: {agent.Id})");
                    }
                }
                else if (response.CommandId == "AcousticMeasure")
                {
                    if (agent.Status == AnymalGrpc.Status.Active)
                    {
                        Console.WriteLine($"Performing acoustic measure {agent.Name} (ID: {agent.Id})");
                    }
                }
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
