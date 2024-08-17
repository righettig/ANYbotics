
using anybotics_anymal_api.Commands;
using anybotics_anymal_api.CustomAttributes;
using Microsoft.AspNetCore.Mvc;

namespace anybotics_anymal_api.Controllers;

[ApiController]
[Route("anymal/moveLeft")]
public class MoveLeftController(ICommandBus commandBus) : BaseAnymalCommandController(commandBus)
{
    [HttpPost]
    [Deny("guest")]
    public async Task<IActionResult> MoveLeft([FromBody] string id)
    {
        var validationResult = ValidateId(id);
        if (validationResult != null)
        {
            return validationResult;
        }

        var result = await commandBus.SendAsync(new MoveLeftCommand(id, UserUid));
        return Ok(result);
    }
}
