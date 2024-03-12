using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace UserManagement.Data.Migrations
{
    /// <inheritdoc />
    public partial class SeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Logs",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Changes = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Logs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Forename = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Surname = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DateOfBirth = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Logs",
                columns: new[] { "Id", "Changes", "CreatedAt", "Type", "UserId" },
                values: new object[,]
                {
                    { 1L, "Changes", new DateTime(2024, 3, 11, 13, 52, 36, 0, DateTimeKind.Unspecified), "Created User", 1L },
                    { 2L, "Changes", new DateTime(2024, 3, 11, 13, 52, 37, 0, DateTimeKind.Unspecified), "Created User", 2L },
                    { 3L, "Changes", new DateTime(2024, 3, 11, 13, 52, 38, 0, DateTimeKind.Unspecified), "Created User", 3L },
                    { 4L, "Changes", new DateTime(2024, 3, 11, 13, 52, 39, 0, DateTimeKind.Unspecified), "Created User", 4L },
                    { 5L, "Changes", new DateTime(2024, 3, 11, 13, 52, 40, 0, DateTimeKind.Unspecified), "Created User", 5L },
                    { 6L, "Changes", new DateTime(2024, 3, 11, 13, 52, 41, 0, DateTimeKind.Unspecified), "Created User", 6L },
                    { 7L, "Changes", new DateTime(2024, 3, 11, 13, 52, 42, 0, DateTimeKind.Unspecified), "Created User", 7L },
                    { 8L, "Changes", new DateTime(2024, 3, 11, 13, 52, 43, 0, DateTimeKind.Unspecified), "Created User", 8L },
                    { 9L, "Changes", new DateTime(2024, 3, 11, 13, 52, 44, 0, DateTimeKind.Unspecified), "Created User", 9L },
                    { 10L, "Changes", new DateTime(2024, 3, 11, 13, 52, 45, 0, DateTimeKind.Unspecified), "Created User", 10L },
                    { 11L, "Changes", new DateTime(2024, 3, 11, 13, 52, 46, 0, DateTimeKind.Unspecified), "Created User", 11L },
                    { 12L, "Changes for Update", new DateTime(2024, 3, 11, 13, 52, 36, 0, DateTimeKind.Unspecified), "Updated User", 3L },
                    { 13L, "Changes for Update", new DateTime(2024, 3, 11, 13, 52, 36, 0, DateTimeKind.Unspecified), "Updated User", 4L },
                    { 14L, "Changes for Deletion", new DateTime(2024, 3, 11, 13, 52, 36, 0, DateTimeKind.Unspecified), "Deleted User", 5L },
                    { 15L, "Changes for Deletion", new DateTime(2024, 3, 11, 13, 52, 36, 0, DateTimeKind.Unspecified), "Deleted User", 6L }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "DateOfBirth", "Email", "Forename", "IsActive", "Surname" },
                values: new object[,]
                {
                    { 1L, "1985-01-15", "ploew@example.com", "Peter", true, "Loew" },
                    { 2L, "1990-03-27", "bfgates@example.com", "Benjamin Franklin", true, "Gates" },
                    { 3L, "1976-06-08", "ctroy@example.com", "Castor", false, "Troy" },
                    { 4L, "2002-09-05", "mraines@example.com", "Memphis", true, "Raines" },
                    { 5L, "1995-12-20", "sgodspeed@example.com", "Stanley", true, "Goodspeed" },
                    { 6L, "2005-01-23", "himcdunnough@example.com", "H.I.", true, "McDunnough" },
                    { 7L, "1998-04-10", "cpoe@example.com", "Cameron", false, "Poe" },
                    { 8L, "1980-07-03", "emalus@example.com", "Edward", false, "Malus" },
                    { 9L, "2005-11-18", "dmacready@example.com", "Damon", false, "Macready" },
                    { 10L, "1972-02-28", "jblaze@example.com", "Johnny", true, "Blaze" },
                    { 11L, "1993-09-15", "rfeld@example.com", "Robin", true, "Feld" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Logs");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
