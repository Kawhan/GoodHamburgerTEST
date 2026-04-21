using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace GoodHamburgerProject.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Accompaniments",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Price = table.Column<decimal>(type: "TEXT", precision: 10, scale: 2, nullable: false),
                    Active = table.Column<bool>(type: "INTEGER", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Accompaniments", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Burgers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Price = table.Column<decimal>(type: "TEXT", precision: 10, scale: 2, nullable: false),
                    Active = table.Column<bool>(type: "INTEGER", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Burgers", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Accompaniments",
                columns: new[] { "Id", "Active", "Name", "Price" },
                values: new object[,]
                {
                    { new Guid("11111111-1111-1111-1111-111111111111"), true, "X Burger", 5.00m },
                    { new Guid("22222222-2222-2222-2222-222222222222"), true, "X Egg", 4.50m },
                    { new Guid("33333333-3333-3333-3333-333333333333"), true, "X Bacon", 7.00m }
                });

            migrationBuilder.InsertData(
                table: "Burgers",
                columns: new[] { "Id", "Active", "Name", "Price" },
                values: new object[,]
                {
                    { new Guid("44444444-4444-4444-4444-444444444444"), true, "Batata Frita", 2.00m },
                    { new Guid("55555555-5555-5555-5555-555555555555"), true, "Refrigerante", 2.50m }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Accompaniments");

            migrationBuilder.DropTable(
                name: "Burgers");
        }
    }
}
