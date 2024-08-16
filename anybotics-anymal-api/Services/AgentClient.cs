using AnymalGrpc;
using Grpc.Core;

namespace anybotics_anymal_api.Services;

public class AgentClient
{
    public required Agent Agent { get; set; }
    public IServerStreamWriter<RechargeBatteryEvent>? RechargeBatteryStream { get; set; }
    public IServerStreamWriter<ShutdownEvent>? ShutdownStream { get; set; }
    public IServerStreamWriter<WakeupEvent>? WakeupStream { get; set; }
}
