using AnymalGrpc;

namespace anybotics_anymal.CommandProcessors;

public abstract class BaseCommandProcessor(string commandId) : ICommandProcessor
{
    public string CommandId { get; } = commandId;

    public virtual bool ConditionCheck(Agent agent) => agent.Status == Status.Active;

    public abstract void PerformCommand(Agent agent, Command response);
}