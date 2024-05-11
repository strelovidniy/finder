using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Finder.Data.Migrations
{
    /// <inheritdoc />
    public partial class Fix2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SearchOperation",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    Title = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    OperationType = table.Column<byte>(type: "smallint", nullable: false),
                    Tags = table.Column<string[]>(type: "text[]", nullable: true),
                    ShowContactInfo = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SearchOperation", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SearchOperation_Users_UserId",
                        column: x => x.UserId,
                        principalSchema: "dbo",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OperationImage",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Position = table.Column<int>(type: "integer", nullable: false),
                    ImageUrl = table.Column<string>(type: "text", nullable: false),
                    ImageThumbnailUrl = table.Column<string>(type: "text", nullable: false),
                    OperationId = table.Column<Guid>(type: "uuid", nullable: false),
                    SearchOperationId = table.Column<Guid>(type: "uuid", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OperationImage", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OperationImage_SearchOperation_SearchOperationId",
                        column: x => x.SearchOperationId,
                        principalTable: "SearchOperation",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_OperationImage_SearchOperationId",
                table: "OperationImage",
                column: "SearchOperationId");

            migrationBuilder.CreateIndex(
                name: "IX_SearchOperation_UserId",
                table: "SearchOperation",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OperationImage");

            migrationBuilder.DropTable(
                name: "SearchOperation");
        }
    }
}
