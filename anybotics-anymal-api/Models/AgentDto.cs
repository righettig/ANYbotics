using AnymalGrpc;

namespace anybotics_anymal_api.Models;

public class AgentDto
{
    public string Id { get; set; }
    public string Name { get; set; }
    public int BatteryLevel { get; set; }
    public Status Status { get; set; }
}
