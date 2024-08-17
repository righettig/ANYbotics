using AnymalGrpc;

namespace anybotics_anymal.CommandProcessors;

public class ShutdownCommandProcessor() : BaseCommandProcessor("Shutdown")
{
    public override void PerformCommand(Agent agent, Command response)
    {
        agent.Status = Status.Offline;
        Console.WriteLine($"Shutting down {agent.Name} (ID: {agent.Id})");
    }
}
