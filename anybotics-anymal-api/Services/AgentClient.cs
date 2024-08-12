using AnymalGrpc;
using Grpc.Core;

namespace AnymalApi.Services;

public class AgentClient
{
    public required Agent Agent { get; set; }
    public IServerStreamWriter<RechargeBatteryEvent>? ClientStream { get; set; }
}
