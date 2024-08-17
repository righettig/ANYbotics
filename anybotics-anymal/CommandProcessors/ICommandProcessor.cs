using AnymalGrpc;

namespace anybotics_anymal.CommandProcessors;

public interface ICommandProcessor
{
    string CommandId { get; }
    bool ConditionCheck(Agent agent);
    void PerformCommand(Agent agent, Command response);
}
