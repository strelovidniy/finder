using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Finder.Data.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "dbo");

            migrationBuilder.CreateTable(
                name: "Addresses",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_generate_v4()"),
                    AddressLine1 = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    AddressLine2 = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    City = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    State = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    PostalCode = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Country = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now() at time zone('utc')"),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Addresses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ContactInfos",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_generate_v4()"),
                    PhoneNumber = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Telegram = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    Skype = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    LinkedIn = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    Instagram = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    Other = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now() at time zone('utc')"),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContactInfos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_generate_v4()"),
                    Name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    IsHidden = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    Type = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    CanDeleteUsers = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    CanRestoreUsers = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    CanEditUsers = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    CanCreateRoles = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    CanEditRoles = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    CanDeleteRoles = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    CanSeeAllUsers = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    CanSeeUsers = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    CanSeeRoles = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    CanSeeAllRoles = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    CanMaintainSystem = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    CanCreateHelpRequest = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    CanSeeHelpRequests = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now() at time zone('utc')"),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_generate_v4()"),
                    FirstName = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    LastName = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    Email = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    PasswordHash = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    RegistrationToken = table.Column<Guid>(type: "uuid", nullable: true),
                    VerificationCode = table.Column<Guid>(type: "uuid", nullable: true),
                    RefreshToken = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Status = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false, defaultValue: "Pending"),
                    RefreshTokenExpiresAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    RoleId = table.Column<Guid>(type: "uuid", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now() at time zone('utc')"),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Users_Roles_RoleId",
                        column: x => x.RoleId,
                        principalSchema: "dbo",
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "NotificationSettings",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    EnableNotifications = table.Column<bool>(type: "boolean", nullable: false),
                    EnableTagFiltration = table.Column<bool>(type: "boolean", nullable: false),
                    FilterTags = table.Column<string[]>(type: "text[]", nullable: true),
                    EnableTitleFiltration = table.Column<bool>(type: "boolean", nullable: false),
                    FilterTitles = table.Column<string[]>(type: "text[]", nullable: true),
                    EnableUpdateNotifications = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NotificationSettings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NotificationSettings_Users_UserId",
                        column: x => x.UserId,
                        principalSchema: "dbo",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PushSubscriptions",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_generate_v4()"),
                    Endpoint = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    ExpirationTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    P256dh = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    Auth = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now() at time zone('utc')"),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PushSubscriptions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PushSubscriptions_Users_UserId",
                        column: x => x.UserId,
                        principalSchema: "dbo",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserDetails",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_generate_v4()"),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    ImageUrl = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    ImageThumbnailUrl = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    AddressId = table.Column<Guid>(type: "uuid", nullable: true),
                    ContactInfoId = table.Column<Guid>(type: "uuid", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now() at time zone('utc')"),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserDetails_Addresses_AddressId",
                        column: x => x.AddressId,
                        principalSchema: "dbo",
                        principalTable: "Addresses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserDetails_ContactInfos_ContactInfoId",
                        column: x => x.ContactInfoId,
                        principalSchema: "dbo",
                        principalTable: "ContactInfos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserDetails_Users_UserId",
                        column: x => x.UserId,
                        principalSchema: "dbo",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                schema: "dbo",
                table: "Roles",
                columns: new[] { "Id", "CanCreateHelpRequest", "CanCreateRoles", "CanDeleteRoles", "CanDeleteUsers", "CanEditRoles", "CanEditUsers", "CanMaintainSystem", "CanRestoreUsers", "CanSeeAllRoles", "CanSeeAllUsers", "CanSeeHelpRequests", "CanSeeRoles", "CanSeeUsers", "CreatedAt", "DeletedAt", "IsHidden", "Name", "Type", "UpdatedAt" },
                values: new object[] { new Guid("00000000-0000-0000-0000-000000000001"), true, true, true, true, true, true, true, true, true, true, true, true, true, new DateTime(2022, 11, 11, 1, 6, 0, 0, DateTimeKind.Utc), null, true, "Admin", "Admin", null });

            migrationBuilder.InsertData(
                schema: "dbo",
                table: "Roles",
                columns: new[] { "Id", "CanCreateHelpRequest", "CreatedAt", "DeletedAt", "IsHidden", "Name", "Type", "UpdatedAt" },
                values: new object[] { new Guid("a0a80c03-abbc-eb11-cabb-0022480a1c0a"), true, new DateTime(2022, 11, 11, 1, 6, 0, 0, DateTimeKind.Utc), null, true, "User", "User", null });

            migrationBuilder.InsertData(
                schema: "dbo",
                table: "Users",
                columns: new[] { "Id", "DeletedAt", "Email", "FirstName", "LastName", "PasswordHash", "RefreshToken", "RefreshTokenExpiresAt", "RegistrationToken", "RoleId", "Status", "UpdatedAt", "VerificationCode" },
                values: new object[,]
                {
                    { new Guid("00a68a21-7b01-2211-abbc-0022480a1c03"), null, "roma.dan2001@gmail.com", "Roman", "Danylevych", "2576c639ea2309626fee6232e624ba921afada44537b9fa6592f03d5a1da7dd375fbd17b2af56655323327e8fd75a46d4932d54c4df61595844bc95fd5979c56", null, null, null, new Guid("00000000-0000-0000-0000-000000000001"), "Active", null, null },
                    { new Guid("07afd050-0126-4610-8dae-854efa9fcfde"), null, "boredarthur@gmail.com", "Arthur", "Zavolovych", "2576c639ea2309626fee6232e624ba921afada44537b9fa6592f03d5a1da7dd375fbd17b2af56655323327e8fd75a46d4932d54c4df61595844bc95fd5979c56", null, null, null, new Guid("a0a80c03-abbc-eb11-cabb-0022480a1c0a"), "Active", null, null },
                    { new Guid("6ebd3c1c-4a07-4c53-8a71-493386f28261"), null, "nazariy.chetvertukha@gmail.com", "Nazariy", "Chetvertukha", "2576c639ea2309626fee6232e624ba921afada44537b9fa6592f03d5a1da7dd375fbd17b2af56655323327e8fd75a46d4932d54c4df61595844bc95fd5979c56", null, null, null, new Guid("a0a80c03-abbc-eb11-cabb-0022480a1c0a"), "Active", null, null },
                    { new Guid("bafee45c-1874-4c40-b781-5221133b4c30"), null, "annstepaniuk12@gmail.com", "Ann", "Stepaniuk", "2576c639ea2309626fee6232e624ba921afada44537b9fa6592f03d5a1da7dd375fbd17b2af56655323327e8fd75a46d4932d54c4df61595844bc95fd5979c56", null, null, null, new Guid("a0a80c03-abbc-eb11-cabb-0022480a1c0a"), "Active", null, null },
                    { new Guid("e823f508-ce4e-498f-a401-dbf40c892dbb"), null, "roman.sadokha01@gmail.com", "Roman", "Sadokha", "2576c639ea2309626fee6232e624ba921afada44537b9fa6592f03d5a1da7dd375fbd17b2af56655323327e8fd75a46d4932d54c4df61595844bc95fd5979c56", null, null, null, new Guid("a0a80c03-abbc-eb11-cabb-0022480a1c0a"), "Active", null, null }
                });

            migrationBuilder.CreateIndex(
                name: "IX_NotificationSettings_UserId",
                table: "NotificationSettings",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PushSubscriptions_UserId",
                schema: "dbo",
                table: "PushSubscriptions",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Roles_Name",
                schema: "dbo",
                table: "Roles",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserDetails_AddressId",
                schema: "dbo",
                table: "UserDetails",
                column: "AddressId");

            migrationBuilder.CreateIndex(
                name: "IX_UserDetails_ContactInfoId",
                schema: "dbo",
                table: "UserDetails",
                column: "ContactInfoId");

            migrationBuilder.CreateIndex(
                name: "IX_UserDetails_UserId",
                schema: "dbo",
                table: "UserDetails",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email",
                schema: "dbo",
                table: "Users",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_RoleId",
                schema: "dbo",
                table: "Users",
                column: "RoleId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "NotificationSettings");

            migrationBuilder.DropTable(
                name: "PushSubscriptions",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "UserDetails",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "Addresses",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "ContactInfos",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "Users",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "Roles",
                schema: "dbo");
        }
    }
}
