using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ApiSults.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class Start : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CONFIGURATION",
                columns: table => new
                {
                    CONFIGURATION_ID = table.Column<long>(type: "bigint", nullable: false),
                    CONFIGURATION_KEY = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    CONFIGURATION_ATUALIZATION_INTERVAL = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    CONFIGURATION_ATUALIZATION_ENABLED = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    CONFIGURATION_LAST_ATUALIZATION = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValue: new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified))
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CONFIGURATION", x => x.CONFIGURATION_ID);
                });

            migrationBuilder.CreateTable(
                name: "Log",
                columns: table => new
                {
                    LOG_ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LOG_MESSAGE = table.Column<string>(type: "varchar(4000)", unicode: false, maxLength: 4000, nullable: false),
                    LOG_DATE = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LOG_LEVEL = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    LOG_SOURCE = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Log", x => x.LOG_ID);
                });

            migrationBuilder.CreateTable(
                name: "TICKET",
                columns: table => new
                {
                    TICKET_ID = table.Column<long>(type: "bigint", nullable: false),
                    TICKET_TITLE = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    TICKET_APPLICANT_NAME = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    TICKET_APPLICANT_ID = table.Column<long>(type: "bigint", nullable: false),
                    TICKET_RESPONSIBLE_NAME = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    TICKET_RESPONSIBLE_ID = table.Column<long>(type: "bigint", nullable: false),
                    TICKET_UNIT_NAME = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    TICKET_UNIT_ID = table.Column<long>(type: "bigint", nullable: false),
                    TICKET_DEPARTMENT_NAME = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    TICKET_DEPARTMENT_ID = table.Column<long>(type: "bigint", nullable: false),
                    TICKET_SUBJECT_NAME = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    TICKET_SUBJECT_ID = table.Column<long>(type: "bigint", nullable: false),
                    TICKET_TYPE = table.Column<long>(type: "bigint", nullable: false),
                    TICKET_OPEN = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TICKET_RESOLVED = table.Column<DateTime>(type: "datetime2", nullable: true),
                    TICKET_COMPLETED = table.Column<DateTime>(type: "datetime2", nullable: true),
                    TICKET_SOLVE_PLANNED = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TICKET_RESOLVE_STIPULATED = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TICKET_FIRST_INTERACTION = table.Column<DateTime>(type: "datetime2", nullable: true),
                    TICKET_LAST_CHANGE = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TICKET_EVALUATION_NOTE = table.Column<int>(type: "int", nullable: true),
                    TICKET_EVALUATION_OBSERVATION = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TICKET_STATUS = table.Column<int>(type: "int", nullable: false),
                    TICKET_COUNT_PUBLIC_INTERACTION = table.Column<int>(type: "int", nullable: false),
                    TICKET_COUNT_INTERACTION_INTERNAL = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TICKET", x => x.TICKET_ID);
                });

            migrationBuilder.CreateTable(
                name: "SUPPORT",
                columns: table => new
                {
                    SUPPORT_ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SUPPORT_PERSON_NAME = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    SUPPORT_PERSON_ID = table.Column<long>(type: "bigint", nullable: false),
                    SUPPORT_DEPARTMENT_NAME = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    SUPPORT_DEPARTMENT_ID = table.Column<long>(type: "bigint", nullable: false),
                    SUPPORT_PERSON_UNIT = table.Column<bool>(type: "bit", nullable: false),
                    TICKET_ID = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SUPPORT", x => x.SUPPORT_ID);
                    table.ForeignKey(
                        name: "FK_SUPPORT_TICKET_TICKET_ID",
                        column: x => x.TICKET_ID,
                        principalTable: "TICKET",
                        principalColumn: "TICKET_ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TAG",
                columns: table => new
                {
                    TAG_ID = table.Column<long>(type: "bigint", nullable: false),
                    TICKET_ID = table.Column<long>(type: "bigint", nullable: false),
                    TAG_NAME = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    TAG_COLOR = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TAG", x => new { x.TICKET_ID, x.TAG_ID });
                    table.ForeignKey(
                        name: "FK_TAG_TICKET_TICKET_ID",
                        column: x => x.TICKET_ID,
                        principalTable: "TICKET",
                        principalColumn: "TICKET_ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "CONFIGURATION",
                columns: new[] { "CONFIGURATION_ID", "CONFIGURATION_ATUALIZATION_ENABLED", "CONFIGURATION_ATUALIZATION_INTERVAL", "CONFIGURATION_KEY", "CONFIGURATION_LAST_ATUALIZATION" },
                values: new object[] { 1L, true, 1, null, new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.CreateIndex(
                name: "IX_SUPPORT_TICKET_ID",
                table: "SUPPORT",
                column: "TICKET_ID");

            migrationBuilder.CreateIndex(
                name: "IX_TICKET_DEPARTMENT_ID",
                table: "TICKET",
                column: "TICKET_DEPARTMENT_ID");

            migrationBuilder.CreateIndex(
                name: "TICKET_ID",
                table: "TICKET",
                column: "TICKET_ID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CONFIGURATION");

            migrationBuilder.DropTable(
                name: "Log");

            migrationBuilder.DropTable(
                name: "SUPPORT");

            migrationBuilder.DropTable(
                name: "TAG");

            migrationBuilder.DropTable(
                name: "TICKET");
        }
    }
}
