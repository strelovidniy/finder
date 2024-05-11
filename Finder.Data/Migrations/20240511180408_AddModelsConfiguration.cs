using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Finder.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddModelsConfiguration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OperationImage_SearchOperation_SearchOperationId",
                table: "OperationImage");

            migrationBuilder.DropIndex(
                name: "IX_OperationImage_SearchOperationId",
                table: "OperationImage");

            migrationBuilder.DropColumn(
                name: "SearchOperationId",
                table: "OperationImage");

            migrationBuilder.RenameTable(
                name: "SearchOperation",
                newName: "SearchOperation",
                newSchema: "dbo");

            migrationBuilder.RenameTable(
                name: "OperationImage",
                newName: "OperationImage",
                newSchema: "dbo");

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                schema: "dbo",
                table: "SearchOperation",
                type: "character varying(200)",
                maxLength: 200,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "Tags",
                schema: "dbo",
                table: "SearchOperation",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string[]),
                oldType: "text[]",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                schema: "dbo",
                table: "SearchOperation",
                type: "character varying(2000)",
                maxLength: 2000,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                schema: "dbo",
                table: "SearchOperation",
                type: "timestamp with time zone",
                nullable: false,
                defaultValueSql: "now() at time zone('utc')",
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                schema: "dbo",
                table: "SearchOperation",
                type: "uuid",
                nullable: false,
                defaultValueSql: "uuid_generate_v4()",
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AlterColumn<string>(
                name: "ImageUrl",
                schema: "dbo",
                table: "OperationImage",
                type: "character varying(200)",
                maxLength: 200,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "ImageThumbnailUrl",
                schema: "dbo",
                table: "OperationImage",
                type: "character varying(200)",
                maxLength: 200,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                schema: "dbo",
                table: "OperationImage",
                type: "timestamp with time zone",
                nullable: false,
                defaultValueSql: "now() at time zone('utc')",
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                schema: "dbo",
                table: "OperationImage",
                type: "uuid",
                nullable: false,
                defaultValueSql: "uuid_generate_v4()",
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.CreateIndex(
                name: "IX_OperationImage_OperationId",
                schema: "dbo",
                table: "OperationImage",
                column: "OperationId");

            migrationBuilder.AddForeignKey(
                name: "FK_OperationImage_SearchOperation_OperationId",
                schema: "dbo",
                table: "OperationImage",
                column: "OperationId",
                principalSchema: "dbo",
                principalTable: "SearchOperation",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OperationImage_SearchOperation_OperationId",
                schema: "dbo",
                table: "OperationImage");

            migrationBuilder.DropIndex(
                name: "IX_OperationImage_OperationId",
                schema: "dbo",
                table: "OperationImage");

            migrationBuilder.RenameTable(
                name: "SearchOperation",
                schema: "dbo",
                newName: "SearchOperation");

            migrationBuilder.RenameTable(
                name: "OperationImage",
                schema: "dbo",
                newName: "OperationImage");

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "SearchOperation",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(200)",
                oldMaxLength: 200);

            migrationBuilder.AlterColumn<string[]>(
                name: "Tags",
                table: "SearchOperation",
                type: "text[]",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "SearchOperation",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(2000)",
                oldMaxLength: 2000);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "SearchOperation",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldDefaultValueSql: "now() at time zone('utc')");

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "SearchOperation",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldDefaultValueSql: "uuid_generate_v4()");

            migrationBuilder.AlterColumn<string>(
                name: "ImageUrl",
                table: "OperationImage",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(200)",
                oldMaxLength: 200);

            migrationBuilder.AlterColumn<string>(
                name: "ImageThumbnailUrl",
                table: "OperationImage",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(200)",
                oldMaxLength: 200);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "OperationImage",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldDefaultValueSql: "now() at time zone('utc')");

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "OperationImage",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldDefaultValueSql: "uuid_generate_v4()");

            migrationBuilder.AddColumn<Guid>(
                name: "SearchOperationId",
                table: "OperationImage",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_OperationImage_SearchOperationId",
                table: "OperationImage",
                column: "SearchOperationId");

            migrationBuilder.AddForeignKey(
                name: "FK_OperationImage_SearchOperation_SearchOperationId",
                table: "OperationImage",
                column: "SearchOperationId",
                principalTable: "SearchOperation",
                principalColumn: "Id");
        }
    }
}
