﻿namespace anybotics_anymal_api.Commands;

public class GasInspectionCommand(string agentId, string initiatedBy) : CommandBase(agentId, initiatedBy)
{
    public override string ToString() => "GasInspection";
}
