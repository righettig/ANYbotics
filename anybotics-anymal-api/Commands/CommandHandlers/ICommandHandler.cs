using AnymalGrpc;

namespace anybotics_anymal_api.Commands.CommandHandlers;

internal interface ICommandHandler<TCommand> where TCommand : ICommand
{
    Task<UpdateResponse> HandleAsync(TCommand command);
}