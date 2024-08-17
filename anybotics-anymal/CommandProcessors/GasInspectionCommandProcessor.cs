using AnymalGrpc;

namespace anybotics_anymal.CommandProcessors;

public class GasInspectionCommandProcessor() : BaseCommandProcessor("GasInspection")
{
    public override void PerformCommand(Agent agent, Command response)
    {
        Console.WriteLine($"Performing gas inspection {agent.Name} (ID: {agent.Id})");
    }
}