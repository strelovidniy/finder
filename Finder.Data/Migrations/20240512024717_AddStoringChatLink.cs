using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Finder.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddStoringChatLink : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "OperationType",
                schema: "dbo",
                table: "SearchOperation",
                type: "integer",
                nullable: false,
                oldClrType: typeof(byte),
                oldType: "smallint");

            migrationBuilder.AlterColumn<int>(
                name: "OperationStatus",
                schema: "dbo",
                table: "SearchOperation",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(byte),
                oldType: "smallint",
                oldDefaultValue: (byte)0);

            migrationBuilder.AddColumn<string>(
                name: "ChatLink",
                schema: "dbo",
                table: "SearchOperation",
                type: "character varying(2000)",
                maxLength: 2000,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ChatLink",
                schema: "dbo",
                table: "SearchOperation");

            migrationBuilder.AlterColumn<byte>(
                name: "OperationType",
                schema: "dbo",
                table: "SearchOperation",
                type: "smallint",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<byte>(
                name: "OperationStatus",
                schema: "dbo",
                table: "SearchOperation",
                type: "smallint",
                nullable: false,
                defaultValue: (byte)0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldDefaultValue: 0);
        }
    }
}
