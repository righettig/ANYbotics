// Create a channel to the gRPC server
using AnymalGrpc;
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
    Status = Status.Active
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

// When battery reaches 0, update status to Unavailable and notify the server
agent.Status = Status.Unavailable;

var statusUpdateResponse = await client.UpdateStatusAsync(
    new StatusUpdate
    {
        Id = agent.Id,
        Status = agent.Status
    });

Console.WriteLine($"Status Update: {statusUpdateResponse.Message}");
