using Microsoft.Extensions.Logging;
using Quartz;

namespace CronJobs;

public class RetryJob : IJob
{
    private readonly ILogger<RetryJob> _logger;
    public RetryJob(ILogger<RetryJob> logger)
    {
        _logger = logger;
    }

    public Task Execute(IJobExecutionContext context)
    {
        _logger.LogInformation("Hello world!End "+ DateTime.UtcNow);
        while (true)
        {
            
        }
        return Task.CompletedTask;
    }
}