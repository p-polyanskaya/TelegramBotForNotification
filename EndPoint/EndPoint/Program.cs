using Application;
using Consumers;
using EndPoint;
using Migration;
using Options;
using Postgres;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHostedService<TelegramConsumer>();
builder.Services.Configure<PostgresConnection>(builder.Configuration.GetSection(nameof(PostgresConnection)));

builder.Services.Configure<ConsumersSettings>(builder.Configuration.GetSection(nameof(ConsumersSettings)));
builder.Services.Configure<BotSettings>(builder.Configuration.GetSection(nameof(BotSettings)));

builder.Services.AddMediatR(x => x.RegisterServicesFromAssemblies(typeof(BotSenderMessageCommand.Handler).Assembly));
builder.Services.SetTelegramBotClient();

//настройка миграций постгреса
var connectionString = builder
    .Configuration
    .GetSection("PostgresConnection:Connection")
    .Get<string>();

if (connectionString is null)
{
    throw new InvalidOperationException("Cannot find connection string");
}

builder.Services.SetPostgres(connectionString);

builder.Services.AddScoped<MessagesRepository>();

var app = builder.Build();

app.Migrate();

app.Run();