using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Finder.Data.Migrations
{
    /// <inheritdoc />
    public partial class RenameColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CanCreateHelpRequest",
                schema: "dbo",
                table: "Roles",
                newName: "CanCreateSearchOperation");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CanCreateSearchOperation",
                schema: "dbo",
                table: "Roles",
                newName: "CanCreateHelpRequest");
        }
    }
}
