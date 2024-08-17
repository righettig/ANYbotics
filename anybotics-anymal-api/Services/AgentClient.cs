using AnymalGrpc;
using Grpc.Core;

namespace anybotics_anymal_api.Services;

public class AgentClient
{
    public required Agent Agent { get; set; }
    public IServerStreamWriter<Command>? CommandStream { get; set; }
}
