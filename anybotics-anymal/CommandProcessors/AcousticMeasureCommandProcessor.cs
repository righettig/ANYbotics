using AnymalGrpc;

namespace anybotics_anymal.CommandProcessors;

public class AcousticMeasureCommandProcessor() : BaseCommandProcessor("Shutdown")
{
    public override void PerformCommand(Agent agent, Command response)
    {
        Console.WriteLine($"Performing acoustic measure {agent.Name} (ID: {agent.Id})");
    }
}
