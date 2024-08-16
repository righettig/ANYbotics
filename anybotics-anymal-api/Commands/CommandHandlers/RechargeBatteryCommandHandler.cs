using AnymalGrpc;
using AnymalService = AnymalApi.Services.AnymalService;

namespace anybotics_anymal_api.Commands.CommandHandlers;

public class RechargeBatteryCommandHandler(AnymalService anymalService) : CommandHandlerBase<RechargeBatteryCommand>(anymalService)
{
    public override async Task<UpdateResponse> HandleAsync(RechargeBatteryCommand command)
    {
        return await anymalService.RechargeBatteryAsync(command.AgentId);
    }
}
