using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Finder.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddAbilityToApplyForSearchOperation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SearchOperation_Users_UserId",
                schema: "dbo",
                table: "SearchOperation");

            migrationBuilder.RenameColumn(
                name: "UserId",
                schema: "dbo",
                table: "SearchOperation",
                newName: "CreatorUserId");

            migrationBuilder.RenameIndex(
                name: "IX_SearchOperation_UserId",
                schema: "dbo",
                table: "SearchOperation",
                newName: "IX_SearchOperation_CreatorUserId");

            migrationBuilder.CreateTable(
                name: "UserSearchOperation",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    SearchOperationId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserSearchOperation", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserSearchOperation_SearchOperation_SearchOperationId",
                        column: x => x.SearchOperationId,
                        principalSchema: "dbo",
                        principalTable: "SearchOperation",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserSearchOperation_Users_UserId",
                        column: x => x.UserId,
                        principalSchema: "dbo",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserSearchOperation_SearchOperationId",
                table: "UserSearchOperation",
                column: "SearchOperationId");

            migrationBuilder.CreateIndex(
                name: "IX_UserSearchOperation_UserId",
                table: "UserSearchOperation",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_SearchOperation_Users_CreatorUserId",
                schema: "dbo",
                table: "SearchOperation",
                column: "CreatorUserId",
                principalSchema: "dbo",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SearchOperation_Users_CreatorUserId",
                schema: "dbo",
                table: "SearchOperation");

            migrationBuilder.DropTable(
                name: "UserSearchOperation");

            migrationBuilder.RenameColumn(
                name: "CreatorUserId",
                schema: "dbo",
                table: "SearchOperation",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_SearchOperation_CreatorUserId",
                schema: "dbo",
                table: "SearchOperation",
                newName: "IX_SearchOperation_UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_SearchOperation_Users_UserId",
                schema: "dbo",
                table: "SearchOperation",
                column: "UserId",
                principalSchema: "dbo",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
