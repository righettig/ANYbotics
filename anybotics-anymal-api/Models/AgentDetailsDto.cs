using AnymalGrpc;

namespace anybotics_anymal_api.Models;

public class AgentDetailsDto
{
    public required string Id { get; set; }
    public required string Name { get; set; }
    public int BatteryLevel { get; set; }
    public Status Status { get; set; }
    public required GeneralInfo General { get; set; }
    public required HardwareInfo Hardware { get; set; }
    public required List<string> RecentImages { get; set; }
    public required List<CommandHistoryItem> CommandHistory { get; set; }
    public required List<StatusHistoryItem> StatusHistory { get; set; }
}

public class GeneralInfo
{
    public required string CurrentCommand { get; set; }
    public required string Model { get; set; }
    public required string FirmwareVersion { get; set; }
    public DateTime FirmwareLastUpdated { get; set; }
    public bool ManualModeOn { get; set; }
    public required Location Location { get; set; }
    public required string TrekkerVersion { get; set; }
    public DateTime TrekkerLastUpdated { get; set; }
}

public class Location
{
    public double X { get; set; }
    public double Y { get; set; }
    public double Z { get; set; }
}

public class HardwareInfo
{
    public required string TemperatureSensor { get; set; }
    public required string PressureSensor { get; set; }
    public required string Leg1Status { get; set; }
    public required string Leg2Status { get; set; }
    public required string Leg3Status { get; set; }
    public required string Leg4Status { get; set; }
    public required string Gps { get; set; }
    public required string Engine { get; set; }
    public required string Battery { get; set; }
    public required string LidarScanner { get; set; }
    public required string Wifi { get; set; }
    public required string Lte { get; set; }
    public required string Cpu1 { get; set; }
    public required string Cpu2 { get; set; }
    public required List<string> DepthCameras { get; set; }
    public required List<string> OpticalCameras { get; set; }
    public required string ThermalCamera { get; set; }
    public required string PanTiltUnit { get; set; }
    public required string Spotlight { get; set; }
    public required string UltrasonicMicrophone { get; set; }
}

public class CommandHistoryItem
{
    public string InitiatedBy { get; set; }
    public DateTime Timestamp { get; set; }
    public string Description { get; set; }
}

public class StatusHistoryItem
{
    public DateTime Timestamp { get; set; }
    public Status Status { get; set; }
}
