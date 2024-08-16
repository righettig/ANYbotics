using AnymalGrpc;
using AnymalService = AnymalApi.Services.AnymalService;

namespace anybotics_anymal_api.Commands.CommandHandlers;

public abstract class CommandHandlerBase<TCommand>(AnymalService anymalService) : ICommandHandler<TCommand> where TCommand : ICommand
{
    public abstract Task<UpdateResponse> HandleAsync(TCommand command);
}
