using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ApiSults.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddTriggerLimitConfigurationInsert : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                CREATE TRIGGER TR_LIMIT_CONFIGURATION_INSERT
                ON CONFIGURATION
                INSTEAD OF INSERT
                AS
                BEGIN
                    -- Verifica se já existe um registro na tabela
                    IF EXISTS (SELECT 1 FROM CONFIGURATION)
                    BEGIN
                        -- Se já existir, impede a inserção
                        RAISERROR ('Apenas um registro pode existir na tabela CONFIGURATION.', 16, 1);
                        ROLLBACK TRANSACTION;
                        RETURN;
                    END

                    -- Se não existir, insere normalmente
                    INSERT INTO CONFIGURATION (CONFIGURATION_ID, CONFIGURATION_KEY, CONFIGURATION_ATUALIZATION_INTERVAL, CONFIGURATION_ATUALIZATION_ENABLED, CONFIGURATION_LAST_ATUALIZATION)
                    SELECT CONFIGURATION_ID, CONFIGURATION_KEY, CONFIGURATION_ATUALIZATION_INTERVAL, CONFIGURATION_ATUALIZATION_ENABLED, CONFIGURATION_LAST_ATUALIZATION
                    FROM inserted;
                END;");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP TRIGGER TR_LIMIT_CONFIGURATION_INSERT;");
        }
    }
}
