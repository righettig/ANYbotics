using AnymalGrpc;

namespace anybotics_anymal.CommandProcessors;

public class SetManualModeCommandProcessor() : BaseCommandProcessor("SetManualMode")
{
    public override void PerformCommand(Agent agent, Command response)
    {
        var payload = response.Payload.Unpack<SetManualModeRequest>();

        agent.ManualMode = payload.ManualMode;
        Console.WriteLine($"Setting up manual mode for {agent.Name} (ID: {agent.Id}) with value {agent.ManualMode}");
    }
}
