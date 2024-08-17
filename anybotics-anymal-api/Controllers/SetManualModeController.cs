using anybotics_anymal_api.Commands;
using anybotics_anymal_api.CustomAttributes;
using Microsoft.AspNetCore.Mvc;

namespace anybotics_anymal_api.Controllers;

public class SetManualModeRequest
{
    public string Id { get; set; }
    public bool ManualMode { get; set; }
}

[ApiController]
[Route("anymal/setmanualmode")]
public class SetManualModeController(ICommandBus commandBus) : BaseAnymalCommandController(commandBus)
{
    [HttpPost]
    [Deny("guest")]
    public async Task<IActionResult> SetManualMode([FromBody] SetManualModeRequest request)
    {
        var validationResult = ValidateId(request.Id);
        if (validationResult != null)
        {
            return validationResult;
        }

        var result = await commandBus.SendAsync(new SetManualModeCommand(request.Id, UserUid, request.ManualMode));
        return Ok(result);
    }
}
