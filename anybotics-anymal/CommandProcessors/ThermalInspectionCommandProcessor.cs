using AnymalGrpc;

namespace anybotics_anymal.CommandProcessors;

public class ThermalInspectionCommandProcessor() : BaseCommandProcessor("ThermalInspection")
{
    public override void PerformCommand(Agent agent, Command response)
    {
        Console.WriteLine($"Performing thermal inspection {agent.Name} (ID: {agent.Id})");
    }
}
