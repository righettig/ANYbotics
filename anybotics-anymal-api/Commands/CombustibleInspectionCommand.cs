﻿namespace anybotics_anymal_api.Commands;

public class CombustibleInspectionCommand(string agentId, string initiatedBy) : CommandBase(agentId, initiatedBy)
{
    public override string ToString() => "CombustibleInspection";
}
