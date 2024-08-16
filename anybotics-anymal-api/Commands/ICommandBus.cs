using AnymalGrpc;

namespace anybotics_anymal_api.Commands;

public interface ICommandBus
{
    Task<UpdateResponse> SendAsync<TCommand>(TCommand command) where TCommand : ICommand;
}
