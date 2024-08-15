using anybotics_anymal_api.Models;
using AnymalApi.Services;
using Microsoft.AspNetCore.SignalR;

namespace anybotics_anymal_api.Hubs;

public class AgentsHub : Hub
{
    private readonly AnymalService _anymalService;

    public AgentsHub(AnymalService anymalService)
    {
        _anymalService = anymalService;
    }

    public async Task StreamAgentsData()
    {
        while (true)
        {
            var agents = _anymalService.GetAllAgents()
                                        .Select(agent => new AgentDto
                                        {
                                            Id = agent.Id,
                                            Name = agent.Name,
                                            BatteryLevel = agent.BatteryLevel,
                                            Status = agent.Status
                                        }).ToList();

            await Clients.All.SendAsync("ReceiveAgentsData", agents);

            // Adjust the delay as needed
            await Task.Delay(1000);
        }
    }

    public async Task StreamAgentData(string id)
    {
        while (true)
        {
            var agent = _anymalService.GetAgentById(id);

            var agentDto = GetAgentDetailsStub(agent.Id, agent.Name, agent.BatteryLevel, agent.Status);

            await Clients.All.SendAsync("ReceiveAgentData", agentDto);

            // Adjust the delay as needed
            await Task.Delay(1000);
        }
    }

    private AgentDetailsDto GetAgentDetailsStub(string id, string name, int batteryLevel, AnymalGrpc.Status status)
    {
        return new AgentDetailsDto
        {
            Id = id,
            Name = name,
            BatteryLevel = batteryLevel,
            Status = status,
            General = new GeneralInfo
            {
                CurrentCommand = "Move Forward",
                Model = "ANYmal X",
                FirmwareVersion = "1.2.3",
                FirmwareLastUpdated = new DateTime(2024, 7, 15, 12, 0, 0, DateTimeKind.Utc),
                ManualModeOn = false,
                Location = new Location
                {
                    X = 123.45,
                    Y = 67.89,
                    Z = 10.11
                },
                TrekkerVersion = "2.3.4",
                TrekkerLastUpdated = new DateTime(2024, 7, 14, 15, 0, 0, DateTimeKind.Utc)
            },
            Hardware = new HardwareInfo
            {
                TemperatureSensor = "Running",
                PressureSensor = "Anomaly_detected",
                Leg1Status = "Running",
                Leg2Status = "Running",
                Leg3Status = "Failed",
                Leg4Status = "Running",
                Gps = "Running",
                Engine = "Running",
                Battery = "Anomaly_detected",
                LidarScanner = "Running",
                Wifi = "Failed",
                Lte = "Running",
                Cpu1 = "Running",
                Cpu2 = "Running",
                DepthCameras = new List<string>
                {
                    "Running",
                    "Running",
                    "Failed",
                    "Running",
                    "Running",
                    "Running"
                },
                OpticalCameras = new List<string>
                {
                    "Running",
                    "Anomaly_detected"
                },
                ThermalCamera = "Running",
                PanTiltUnit = "Failed",
                Spotlight = "Running",
                UltrasonicMicrophone = "Running"
            },
            RecentImages = new List<string>
            {
                "https://placehold.co/400?text=Room\\n1",
                "https://placehold.co/400?text=Room\\n2",
                "https://placehold.co/400?text=Room\\n3",
                "https://placehold.co/400?text=Room\\n4",
                "https://placehold.co/400?text=Room\\n5"
            },
            CommandHistory = new List<string>
            {
                "Initialized system",
                "Started navigation",
                "Executed pathfinding",
                "Battery check completed"
            },
            StatusHistory = new List<StatusHistoryItem>
            {
                new StatusHistoryItem
                {
                    Timestamp = new DateTime(2024, 8, 10, 8, 0, 0, DateTimeKind.Utc),
                    Status = AnymalGrpc.Status.Active
                },
                new StatusHistoryItem
                {
                    Timestamp = new DateTime(2024, 8, 11, 9, 30, 0, DateTimeKind.Utc),
                    Status = AnymalGrpc.Status.Offline
                },
                new StatusHistoryItem
                {
                    Timestamp = new DateTime(2024, 8, 12, 10, 45, 0, DateTimeKind.Utc),
                    Status = AnymalGrpc.Status.Unavailable
                },
                new StatusHistoryItem
                {
                    Timestamp = new DateTime(2024, 8, 13, 11, 50, 0, DateTimeKind.Utc),
                    Status = AnymalGrpc.Status.Active
                }
            }
        };
    }
}
