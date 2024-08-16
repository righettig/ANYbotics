namespace anybotics_anymal_api.Commands;

public interface ICommandRepository
{
    Task SaveCommandAsync<TCommand>(TCommand command) where TCommand : ICommand;
    Task<IEnumerable<ICommand>> GetCommandsByAgentIdAsync(string agentId);
}
