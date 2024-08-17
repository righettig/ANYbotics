
using AnymalGrpc;

namespace anybotics_anymal.CommandProcessors;

public class MoveLeftCommandProcessor() : BaseCommandProcessor("MoveLeft")
{
    public override void PerformCommand(Agent agent, Command response)
    {
        agent.Location.X = agent.Location.X - 1;
        Console.WriteLine($"Moving Left {agent.Name} (ID: {agent.Id})");
    }
}
