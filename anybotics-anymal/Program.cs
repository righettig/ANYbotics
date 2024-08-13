// Create a channel to the gRPC server
using AnymalGrpc;
using Grpc.Core;
using Grpc.Net.Client;

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

var batteryDecreaseLoop = async () =>
{
    // Battery decrease loop
    while (agent.BatteryLevel > 0)
    {
        await Task.Delay(10 * 1000); // 10 seconds delay

        if (agent.Status == AnymalGrpc.Status.Active) 
        {
            agent.BatteryLevel--;

            var updateResponse = await client.UpdateBatteryAsync(
                new BatteryUpdate
                {
                    Id = agent.Id,
                    BatteryLevel = agent.BatteryLevel
                });

            Console.WriteLine($"Battery Update: {updateResponse.Message}");
        }
    }

    // Notify the server when the battery reaches 0
    var finalUpdateResponse = await client.UpdateBatteryAsync(
        new BatteryUpdate
        {
            Id = agent.Id,
            BatteryLevel = agent.BatteryLevel
        });

    Console.WriteLine($"Final Battery Update: {finalUpdateResponse.Message}");

    // When battery reaches 0, update status to Unavailable and notify the server
    agent.Status = AnymalGrpc.Status.Unavailable;

    var statusUpdateResponse = await client.UpdateStatusAsync(
        new StatusUpdate
        {
            Id = agent.Id,
            Status = agent.Status
        });

    Console.WriteLine($"Status Update: {statusUpdateResponse.Message}");
};

// Register the agent
try
{
    var registrationResponse = await client.RegisterAgentAsync(agent);
    Console.WriteLine($"Registration: {registrationResponse.Message} (ID: {agent.Id})");

    // Start monitoring for battery recharge notifications
    _ = Task.Run(async () =>
    {
        using var call = client.StreamRechargeBatteryEvents(new RechargeBatteryEvent { Id = agent.Id });

        await foreach (var response in call.ResponseStream.ReadAllAsync())
        {
            if (response.Id == agent.Id && agent.Status == AnymalGrpc.Status.Active)
            {
                agent.BatteryLevel = 100;
                agent.Status = AnymalGrpc.Status.Active;

                Console.WriteLine($"Battery recharged to 100% for Agent {agent.Name} (ID: {agent.Id})");
            }
        }
    });

    // Shutdown
    _ = Task.Run(async () =>
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
    });

    // Wakeup
    _ = Task.Run(async () =>
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
    });

    await batteryDecreaseLoop();
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
    channel.ShutdownAsync().Wait();
}
