using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ApiSults.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class PatchVarchar : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "TICKET_UNIT_NAME",
                table: "TICKET",
                type: "varchar(2000)",
                unicode: false,
                maxLength: 2000,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(400)",
                oldMaxLength: 400);

            migrationBuilder.AlterColumn<string>(
                name: "TICKET_TITLE",
                table: "TICKET",
                type: "varchar(2000)",
                unicode: false,
                maxLength: 2000,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(2000)",
                oldMaxLength: 2000);

            migrationBuilder.AlterColumn<string>(
                name: "TICKET_SUBJECT_NAME",
                table: "TICKET",
                type: "varchar(2000)",
                unicode: false,
                maxLength: 2000,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(400)",
                oldMaxLength: 400);

            migrationBuilder.AlterColumn<string>(
                name: "TICKET_RESPONSIBLE_NAME",
                table: "TICKET",
                type: "varchar(2000)",
                unicode: false,
                maxLength: 2000,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(400)",
                oldMaxLength: 400);

            migrationBuilder.AlterColumn<string>(
                name: "TICKET_EVALUATION_OBSERVATION",
                table: "TICKET",
                type: "varchar(2000)",
                unicode: false,
                maxLength: 2000,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(400)",
                oldMaxLength: 400,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "TICKET_DEPARTMENT_NAME",
                table: "TICKET",
                type: "varchar(2000)",
                unicode: false,
                maxLength: 2000,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(400)",
                oldMaxLength: 400);

            migrationBuilder.AlterColumn<string>(
                name: "TICKET_APPLICANT_NAME",
                table: "TICKET",
                type: "varchar(2000)",
                unicode: false,
                maxLength: 2000,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(400)",
                oldMaxLength: 400);

            migrationBuilder.AlterColumn<string>(
                name: "TAG_NAME",
                table: "TAG",
                type: "varchar(2000)",
                unicode: false,
                maxLength: 2000,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(400)",
                oldMaxLength: 400);

            migrationBuilder.AlterColumn<string>(
                name: "TAG_COLOR",
                table: "TAG",
                type: "varchar(2000)",
                unicode: false,
                maxLength: 2000,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(400)",
                oldMaxLength: 400);

            migrationBuilder.AlterColumn<string>(
                name: "SUPPORT_PERSON_NAME",
                table: "SUPPORT",
                type: "varchar(2000)",
                unicode: false,
                maxLength: 2000,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(400)",
                oldMaxLength: 400);

            migrationBuilder.AlterColumn<string>(
                name: "SUPPORT_DEPARTMENT_NAME",
                table: "SUPPORT",
                type: "varchar(2000)",
                unicode: false,
                maxLength: 2000,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(400)",
                oldMaxLength: 400);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "TICKET_UNIT_NAME",
                table: "TICKET",
                type: "nvarchar(400)",
                maxLength: 400,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(2000)",
                oldUnicode: false,
                oldMaxLength: 2000);

            migrationBuilder.AlterColumn<string>(
                name: "TICKET_TITLE",
                table: "TICKET",
                type: "nvarchar(2000)",
                maxLength: 2000,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(2000)",
                oldUnicode: false,
                oldMaxLength: 2000);

            migrationBuilder.AlterColumn<string>(
                name: "TICKET_SUBJECT_NAME",
                table: "TICKET",
                type: "nvarchar(400)",
                maxLength: 400,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(2000)",
                oldUnicode: false,
                oldMaxLength: 2000);

            migrationBuilder.AlterColumn<string>(
                name: "TICKET_RESPONSIBLE_NAME",
                table: "TICKET",
                type: "nvarchar(400)",
                maxLength: 400,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(2000)",
                oldUnicode: false,
                oldMaxLength: 2000);

            migrationBuilder.AlterColumn<string>(
                name: "TICKET_EVALUATION_OBSERVATION",
                table: "TICKET",
                type: "nvarchar(400)",
                maxLength: 400,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(2000)",
                oldUnicode: false,
                oldMaxLength: 2000,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "TICKET_DEPARTMENT_NAME",
                table: "TICKET",
                type: "nvarchar(400)",
                maxLength: 400,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(2000)",
                oldUnicode: false,
                oldMaxLength: 2000);

            migrationBuilder.AlterColumn<string>(
                name: "TICKET_APPLICANT_NAME",
                table: "TICKET",
                type: "nvarchar(400)",
                maxLength: 400,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(2000)",
                oldUnicode: false,
                oldMaxLength: 2000);

            migrationBuilder.AlterColumn<string>(
                name: "TAG_NAME",
                table: "TAG",
                type: "nvarchar(400)",
                maxLength: 400,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(2000)",
                oldUnicode: false,
                oldMaxLength: 2000);

            migrationBuilder.AlterColumn<string>(
                name: "TAG_COLOR",
                table: "TAG",
                type: "nvarchar(400)",
                maxLength: 400,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(2000)",
                oldUnicode: false,
                oldMaxLength: 2000);

            migrationBuilder.AlterColumn<string>(
                name: "SUPPORT_PERSON_NAME",
                table: "SUPPORT",
                type: "nvarchar(400)",
                maxLength: 400,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(2000)",
                oldUnicode: false,
                oldMaxLength: 2000);

            migrationBuilder.AlterColumn<string>(
                name: "SUPPORT_DEPARTMENT_NAME",
                table: "SUPPORT",
                type: "nvarchar(400)",
                maxLength: 400,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(2000)",
                oldUnicode: false,
                oldMaxLength: 2000);
        }
    }
}
