
using AnymalGrpc;

namespace anybotics_anymal.CommandProcessors;

public class MoveForwardCommandProcessor() : BaseCommandProcessor("MoveForward")
{
    public override void PerformCommand(Agent agent, Command response)
    {
        agent.Location.Y = agent.Location.Y + 1;
        Console.WriteLine($"Moving forward {agent.Name} (ID: {agent.Id})");
    }
}
