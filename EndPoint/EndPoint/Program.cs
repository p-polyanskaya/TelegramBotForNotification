using Application;
using Consumers;
using CronJobs;
using MediatR;
using Quartz;

var builder = WebApplication.CreateBuilder(args);

//builder.Services.AddHostedService<TelegramConsumer>();
builder.Services.AddMediatR(x => x.RegisterServicesFromAssemblies(typeof(BotSenderMessageCommand.Handler).Assembly));

builder.Services.AddQuartz(q =>  
{
    q.UseMicrosoftDependencyInjectionScopedJobFactory();

    // Create a "key" for the job
    var jobKey = new JobKey("HelloWorldJob");

    // Register the job with the DI container
    q.AddJob<RetryJob>(opts => opts.WithIdentity(jobKey));
                
    // Create a trigger for the job
    q.AddTrigger(opts => opts
        .ForJob(jobKey) // link to the HelloWorldJob
        .WithIdentity("HelloWorldJob-trigger") // give the trigger a unique name
        .WithCronSchedule("0/5 * * * * ?")); // run every 5 seconds

});
builder.Services.AddQuartzHostedService(q => q.WaitForJobsToComplete = true);

var app = builder.Build();

app.Run();