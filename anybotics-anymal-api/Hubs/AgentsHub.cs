using anybotics_anymal_api.Commands;
using anybotics_anymal_api.Helpers;
using anybotics_anymal_api.Models;
using anybotics_anymal_api.Services;
using Microsoft.AspNetCore.SignalR;

namespace anybotics_anymal_api.Hubs;

public class AgentsHub : Hub
{
    private readonly AnymalService _anymalService;
    private readonly ICommandRepository _commandRepository;

    public AgentsHub(AnymalService anymalService, ICommandRepository commandRepository)
    {
        _anymalService = anymalService;
        _commandRepository = commandRepository;
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
        var userEmailsCache = new Dictionary<string, string>();

        while (true)
        {
            var agent = _anymalService.GetAgentById(id);

            var agentDto = GetAgentDetailsStub(agent.Id,
                                               agent.Name,
                                               agent.BatteryLevel,
                                               agent.ManualMode,
                                               agent.Status);

            var commandDtos = await _commandRepository.GetCommandsByAgentIdAsync(id);

            // Fetch emails and build the command history
            var commandHistoryTasks = commandDtos.Select(async c =>
            {
                // Try to get the email from the cache
                if (!userEmailsCache.TryGetValue(c.InitiatedBy, out var userEmail))
                {
                    // Fetch email if not found in cache
                    userEmail = await FirebaseUserHelper.GetUserEmailAsync(c.InitiatedBy);
                    userEmailsCache[c.InitiatedBy] = userEmail;
                }

                // Return a new CommandHistoryItem
                return new CommandHistoryItem
                {
                    InitiatedBy = userEmail,
                    Timestamp = c.Timestamp,
                    Description = c.ToString(),
                };
            });

            // Await all tasks to complete and order them
            var commandHistory = await Task.WhenAll(commandHistoryTasks);
            agentDto.CommandHistory = commandHistory.OrderByDescending(x => x.Timestamp).ToList();

            await Clients.All.SendAsync("ReceiveAgentData", agentDto);

            // Adjust the delay as needed
            await Task.Delay(1000);
        }
    }

    private AgentDetailsDto GetAgentDetailsStub(string id, string name, int batteryLevel, bool manualMode, AnymalGrpc.Status status)
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
                ManualModeOn = manualMode,
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
                DepthCameras =
                [
                    "Running",
                    "Running",
                    "Failed",
                    "Running",
                    "Running",
                    "Running"
                ],
                OpticalCameras =
                [
                    "Running",
                    "Anomaly_detected"
                ],
                ThermalCamera = "Running",
                PanTiltUnit = "Failed",
                Spotlight = "Running",
                UltrasonicMicrophone = "Running"
            },
            RecentImages =
            [
                "https://placehold.co/400?text=Room\\n1",
                "https://placehold.co/400?text=Room\\n2",
                "https://placehold.co/400?text=Room\\n3",
                "https://placehold.co/400?text=Room\\n4",
                "https://placehold.co/400?text=Room\\n5"
            ],
            CommandHistory = [],
            StatusHistory =
            [
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
            ]
        };
    }
}
