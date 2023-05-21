using FluentMigrator;

namespace Migration;

[Migration(20180430121801)]
public class CreatePostgresTable : FluentMigrator.Migration
{
    public override void Up()
    {
        Create.Table("inbox_messages")
            .WithColumn("id").AsGuid().PrimaryKey()
            .WithColumn("telegram_message").AsCustom("jsonb")
            .WithColumn("status").AsString().WithDefaultValue("processing");
    }

    public override void Down()
    {
        Delete.Table("inbox_messages");
    }
}