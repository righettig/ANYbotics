
using AnymalGrpc;

namespace anybotics_anymal.CommandProcessors;

public class MoveBackwardCommandProcessor() : BaseCommandProcessor("MoveBackward")
{
    public override void PerformCommand(Agent agent, Command response)
    {
        agent.Location.Y = agent.Location.Y - 1;
        Console.WriteLine($"Moving backward {agent.Name} (ID: {agent.Id})");
    }
}
