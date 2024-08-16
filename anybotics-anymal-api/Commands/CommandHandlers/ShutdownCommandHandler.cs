using AnymalGrpc;
using AnymalService = AnymalApi.Services.AnymalService;

namespace anybotics_anymal_api.Commands.CommandHandlers;

public class ShutdownCommandHandler(AnymalService anymalService) : CommandHandlerBase<ShutdownCommand>(anymalService)
{
    public override async Task<UpdateResponse> HandleAsync(ShutdownCommand command)
    {
        return await anymalService.ShutdownAsync(command.AgentId);
    }
}
