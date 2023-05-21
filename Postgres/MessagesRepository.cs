using System.Text.Json;
using Dapper;
using Domain;
using Microsoft.Extensions.Options;
using Npgsql;
using Options;

namespace Postgres;

public class MessagesRepository
{
    private readonly IOptions<PostgresConnection> _postgresOptions;

    private const string GetStatusByMessageId = @"--MessagesRepository.GetStatusById
                                          select status from inbox_messages where id = @Id;";

    private const string InsertSqlScript = @"--MessagesRepository.InsertSqlScript
                                             insert into inbox_messages (id, telegram_message) 
                                             values (@Id, @TelegramMessage::jsonb)";
    
    private const string UpdateSqlScript = @"--MessagesRepository.InsertSqlScript
                                             update inbox_messages 
                                             set status = @Status
                                             where Id = @Id;";

    public MessagesRepository(IOptions<PostgresConnection> postgresOptions)
    {
        _postgresOptions = postgresOptions;
    }

    public async Task<string?> GetStatusById(Guid id)
    {
        using (var connection = new NpgsqlConnection(_postgresOptions.Value.Connection))
        {
            var status = (await connection.QueryAsync<string>(GetStatusByMessageId, new
            {
                Id = id
            })).FirstOrDefault();
            
            return status;
        }
    }

    public async Task Insert(InboxMessage message)
    {
        using (var connection = new NpgsqlConnection(_postgresOptions.Value.Connection))
        {
            await connection.ExecuteAsync(InsertSqlScript,
                new
                {
                    Id = message.Id,
                    TelegramMessage = JsonSerializer.Serialize(message.TelegramMessage)
                });
        }
    }
    
    public async Task Update(Guid id, string status)
    {
        using (var connection = new NpgsqlConnection(_postgresOptions.Value.Connection))
        {
            await connection.ExecuteAsync(UpdateSqlScript,
                new
                {
                    Id = id,
                    Status = status
                });
        }
    }
}