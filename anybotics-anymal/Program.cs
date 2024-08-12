// Create a channel to the gRPC server
using AnymalGrpc;
using Grpc.Net.Client;

using var channel = GrpcChannel.ForAddress("https://localhost:7272");

var client = new AnymalService.AnymalServiceClient(channel);

var agent = new Agent
{
    Id = Guid.NewGuid().ToString(),
    Name = "Anymal",
    BatteryLevel = 100
};

// Register the agent
var registrationResponse = await client.RegisterAgentAsync(agent);

Console.WriteLine($"Registration: {registrationResponse.Message}");

// Battery decrease loop
while (agent.BatteryLevel > 0)
{
    await Task.Delay(10000); // 10 seconds delay

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