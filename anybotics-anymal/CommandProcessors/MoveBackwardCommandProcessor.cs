
using anybotics_anymal_common.Domain;
using AnymalGrpc;

namespace anybotics_anymal.CommandProcessors;

public class MoveBackwardCommandProcessor() : BaseCommandProcessor("MoveBackward")
{
    public override void PerformCommand(AnymalAgent agent, Command response)
    {
        agent.General.Location.Y = agent.General.Location.Y - 1;
        Console.WriteLine($"Moving backward {agent.Name} (ID: {agent.Id})");
    }
}
