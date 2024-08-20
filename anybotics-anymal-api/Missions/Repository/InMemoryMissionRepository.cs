﻿using anybotics_anymal_api.Missions.Models;
using System.Collections.Concurrent;

namespace anybotics_anymal_api.Missions.Repository;

public class InMemoryMissionRepository : IMissionRepository
{
    private readonly ConcurrentDictionary<string, Mission> _missions = new();

    public Task<IEnumerable<Mission>> GetMissionsAsync()
    {
        var missions = _missions.Values.AsEnumerable();
        return Task.FromResult(missions);
    }

    public Task<Mission> GetMissionByIdAsync(string id)
    {
        _missions.TryGetValue(id, out var mission);
        return Task.FromResult(mission);
    }

    public Task AddMissionAsync(Mission mission)
    {
        _missions[mission.Id] = mission;
        return Task.CompletedTask;
    }

    public Task DeleteMissionAsync(string id)
    {
        _missions.TryRemove(id, out _);
        return Task.CompletedTask;
    }
}