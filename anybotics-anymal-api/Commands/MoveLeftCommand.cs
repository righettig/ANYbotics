
using anybotics_anymal_api.Commands.Core;

namespace anybotics_anymal_api.Commands;

public class MoveLeftCommand(string agentId, string initiatedBy) : CommandBase(agentId, initiatedBy)
{
    public override string ToString() => "MoveLeft";
}
