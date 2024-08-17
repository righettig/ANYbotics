using AnymalGrpc;

namespace anybotics_anymal.CommandProcessors;

public class RechargeBatteryCommandProcessor() : BaseCommandProcessor("RechargeBattery")
{
    public override void PerformCommand(Agent agent, Command response)
    {
        agent.BatteryLevel = 100;
        Console.WriteLine($"Battery recharged to 100% for Agent {agent.Name} (ID: {agent.Id})");
    }
}
