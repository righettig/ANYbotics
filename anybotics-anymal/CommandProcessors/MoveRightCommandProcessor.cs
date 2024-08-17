
using AnymalGrpc;

namespace anybotics_anymal.CommandProcessors;

public class MoveRightCommandProcessor() : BaseCommandProcessor("MoveRight")
{
    public override void PerformCommand(Agent agent, Command response)
    {
        agent.Location.X = agent.Location.X + 1;
        Console.WriteLine($"Moving right {agent.Name} (ID: {agent.Id})");
    }
}
