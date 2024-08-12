// Create a channel to the gRPC server
using AnymalGrpc;
using Grpc.Core;
using Grpc.Net.Client;

// Parse the agent name from the command-line arguments
string agentName = args.Length > 0 ? args[0] : "Anymal";

using var channel = GrpcChannel.ForAddress("https://localhost:7272");

var client = new AnymalService.AnymalServiceClient(channel);

var agent = new Agent
{
    Id = Guid.NewGuid().ToString(),
    Name = agentName,
    BatteryLevel = 100,
    Status = AnymalGrpc.Status.Active
};

// Register the agent
var registrationResponse = await client.RegisterAgentAsync(agent);

Console.WriteLine($"Registration: {registrationResponse.Message}");

// Start monitoring for battery recharge notifications
Task.Run(async () =>
{
    using (var call = client.StreamRechargeBatteryEvents(new RechargeBatteryEvent { Id = agent.Id }))
    {
        await foreach (var response in call.ResponseStream.ReadAllAsync())
        {
            var rechargeEvent = call.ResponseStream.Current;

            // Handle the recharge event (e.g., update battery level)
            if (rechargeEvent.Id == agent.Id)
            {
                agent.BatteryLevel = 100;
                agent.Status = AnymalGrpc.Status.Active;

                Console.WriteLine($"Battery recharged to 100% for Agent {agent.Name} (ID: {agent.Id})");
            }
        }
    }
});

// Battery decrease loop
while (agent.BatteryLevel > 0)
{
    await Task.Delay(10 * 1000); // 10 seconds delay

    agent.BatteryLevel--;

    var updateResponse = await client.UpdateBatteryAsync(
        new BatteryUpdate
        {
            Id = agent.Id,
            BatteryLevel = agent.BatteryLevel
        });

    Console.WriteLine($"Battery Update: {updateResponse.Message}");
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
