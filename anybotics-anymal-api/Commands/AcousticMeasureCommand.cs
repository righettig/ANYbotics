﻿namespace anybotics_anymal_api.Commands;

public class AcousticMeasureCommand(string agentId, string initiatedBy) : CommandBase(agentId, initiatedBy)
{
    public override string ToString() => "AcousticMeasure";
}