using AnymalGrpc;

namespace anybotics_anymal.CommandProcessors;

public class WakeupCommandProcessor() : BaseCommandProcessor("Wakeup")
{
    public override void PerformCommand(Agent agent, Command response)
    {
        agent.Status = Status.Active;
        Console.WriteLine($"Waking up {agent.Name} (ID: {agent.Id})");
    }
}
