﻿namespace ev_sim;

public interface IBackgroundTaskQueue
{
    void QueueBackgroundWorkItem(string id, Func<CancellationToken, Task> workItem);
    Task<Func<CancellationToken, Task>> DequeueAsync(CancellationToken cancellationToken);
    bool TryCancelSimulation(string id);
}
