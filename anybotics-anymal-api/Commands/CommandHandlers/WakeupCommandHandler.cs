using AnymalGrpc;
using AnymalService = AnymalApi.Services.AnymalService;

namespace anybotics_anymal_api.Commands.CommandHandlers;

public class WakeupCommandHandler(AnymalService anymalService) : CommandHandlerBase<WakeUpCommand>(anymalService)
{
    public override async Task<UpdateResponse> HandleAsync(WakeUpCommand command)
    {
        return await anymalService.WakeupAsync(command.AgentId);
    }
}