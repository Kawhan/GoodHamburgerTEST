using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace GoodHamburgerProject.Migrations
{
    /// <inheritdoc />
    public partial class InitialPostgreSQL : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Accompaniments",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Price = table.Column<decimal>(type: "numeric(10,2)", precision: 10, scale: 2, nullable: false),
                    Active = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Accompaniments", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Burgers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Price = table.Column<decimal>(type: "numeric(10,2)", precision: 10, scale: 2, nullable: false),
                    Active = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Burgers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Discounts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    Percentage = table.Column<decimal>(type: "numeric(5,2)", precision: 5, scale: 2, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Active = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Discounts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    TotalAmount = table.Column<decimal>(type: "numeric(10,2)", precision: 10, scale: 2, nullable: false),
                    Discount = table.Column<decimal>(type: "numeric(10,2)", precision: 10, scale: 2, nullable: false),
                    FinalAmount = table.Column<decimal>(type: "numeric(10,2)", precision: 10, scale: 2, nullable: false),
                    Active = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DiscountItems",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    DiscountId = table.Column<Guid>(type: "uuid", nullable: false),
                    ProductId = table.Column<Guid>(type: "uuid", nullable: false),
                    ProductType = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DiscountItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DiscountItems_Discounts_DiscountId",
                        column: x => x.DiscountId,
                        principalTable: "Discounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OrderItems",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    OrderId = table.Column<Guid>(type: "uuid", nullable: false),
                    ProductId = table.Column<Guid>(type: "uuid", nullable: false),
                    ProductType = table.Column<int>(type: "integer", nullable: false),
                    Price = table.Column<decimal>(type: "numeric(10,2)", precision: 10, scale: 2, nullable: false),
                    Active = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrderItems_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Accompaniments",
                columns: new[] { "Id", "Active", "Name", "Price" },
                values: new object[,]
                {
                    { new Guid("44444444-4444-4444-4444-444444444444"), true, "Batata Frita", 2.00m },
                    { new Guid("55555555-5555-5555-5555-555555555555"), true, "Refrigerante", 2.50m }
                });

            migrationBuilder.InsertData(
                table: "Burgers",
                columns: new[] { "Id", "Active", "Name", "Price" },
                values: new object[,]
                {
                    { new Guid("11111111-1111-1111-1111-111111111111"), true, "X Burger", 5.00m },
                    { new Guid("22222222-2222-2222-2222-222222222222"), true, "X Egg", 4.50m },
                    { new Guid("33333333-3333-3333-3333-333333333333"), true, "X Bacon", 7.00m }
                });

            migrationBuilder.InsertData(
                table: "Discounts",
                columns: new[] { "Id", "Active", "CreatedAt", "DeletedAt", "Name", "Percentage" },
                values: new object[,]
                {
                    { new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"), true, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Combo Completo", 0.20m },
                    { new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"), true, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Combo Burger + Refri", 0.15m },
                    { new Guid("cccccccc-cccc-cccc-cccc-cccccccccccc"), true, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Combo Burger + Batata", 0.10m },
                    { new Guid("d1111111-1111-1111-1111-111111111111"), true, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Combo X Egg Completo", 0.20m },
                    { new Guid("d2222222-2222-2222-2222-222222222222"), true, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Combo X Egg + Refri", 0.15m },
                    { new Guid("d3333333-3333-3333-3333-333333333333"), true, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Combo X Egg + Batata", 0.10m },
                    { new Guid("d4444444-4444-4444-4444-444444444444"), true, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Combo X Bacon Completo", 0.20m },
                    { new Guid("d5555555-5555-5555-5555-555555555555"), true, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Combo X Bacon + Refri", 0.15m },
                    { new Guid("d6666666-6666-6666-6666-666666666666"), true, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Combo X Bacon + Batata", 0.10m }
                });

            migrationBuilder.InsertData(
                table: "DiscountItems",
                columns: new[] { "Id", "DiscountId", "ProductId", "ProductType" },
                values: new object[,]
                {
                    { new Guid("d1d1d1d1-d1d1-d1d1-d1d1-d1d1d1d1d1d1"), new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"), new Guid("11111111-1111-1111-1111-111111111111"), 1 },
                    { new Guid("d2d2d2d2-d2d2-d2d2-d2d2-d2d2d2d2d2d2"), new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"), new Guid("44444444-4444-4444-4444-444444444444"), 2 },
                    { new Guid("d3d3d3d3-d3d3-d3d3-d3d3-d3d3d3d3d3d3"), new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"), new Guid("55555555-5555-5555-5555-555555555555"), 2 },
                    { new Guid("e1111111-1111-1111-1111-111111111111"), new Guid("d1111111-1111-1111-1111-111111111111"), new Guid("22222222-2222-2222-2222-222222222222"), 1 },
                    { new Guid("e1111111-2222-1111-1111-111111111111"), new Guid("d1111111-1111-1111-1111-111111111111"), new Guid("44444444-4444-4444-4444-444444444444"), 2 },
                    { new Guid("e1111111-3333-1111-1111-111111111111"), new Guid("d1111111-1111-1111-1111-111111111111"), new Guid("55555555-5555-5555-5555-555555555555"), 2 },
                    { new Guid("e1e1e1e1-e1e1-e1e1-e1e1-e1e1e1e1e1e1"), new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"), new Guid("11111111-1111-1111-1111-111111111111"), 1 },
                    { new Guid("e2222222-1111-2222-2222-222222222222"), new Guid("d2222222-2222-2222-2222-222222222222"), new Guid("22222222-2222-2222-2222-222222222222"), 1 },
                    { new Guid("e2222222-2222-2222-2222-222222222222"), new Guid("d2222222-2222-2222-2222-222222222222"), new Guid("55555555-5555-5555-5555-555555555555"), 2 },
                    { new Guid("e2e2e2e2-e2e2-e2e2-e2e2-e2e2e2e2e2e2"), new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"), new Guid("55555555-5555-5555-5555-555555555555"), 2 },
                    { new Guid("e3333333-1111-3333-3333-333333333333"), new Guid("d3333333-3333-3333-3333-333333333333"), new Guid("22222222-2222-2222-2222-222222222222"), 1 },
                    { new Guid("e3333333-2222-3333-3333-333333333333"), new Guid("d3333333-3333-3333-3333-333333333333"), new Guid("44444444-4444-4444-4444-444444444444"), 2 },
                    { new Guid("f1111111-1111-1111-1111-111111111111"), new Guid("d4444444-4444-4444-4444-444444444444"), new Guid("33333333-3333-3333-3333-333333333333"), 1 },
                    { new Guid("f1111111-2222-1111-1111-111111111111"), new Guid("d4444444-4444-4444-4444-444444444444"), new Guid("44444444-4444-4444-4444-444444444444"), 2 },
                    { new Guid("f1111111-3333-1111-1111-111111111111"), new Guid("d4444444-4444-4444-4444-444444444444"), new Guid("55555555-5555-5555-5555-555555555555"), 2 },
                    { new Guid("f1f1f1f1-f1f1-f1f1-f1f1-f1f1f1f1f1f1"), new Guid("cccccccc-cccc-cccc-cccc-cccccccccccc"), new Guid("11111111-1111-1111-1111-111111111111"), 1 },
                    { new Guid("f2222222-1111-2222-2222-222222222222"), new Guid("d5555555-5555-5555-5555-555555555555"), new Guid("33333333-3333-3333-3333-333333333333"), 1 },
                    { new Guid("f2222222-2222-2222-2222-222222222222"), new Guid("d5555555-5555-5555-5555-555555555555"), new Guid("55555555-5555-5555-5555-555555555555"), 2 },
                    { new Guid("f2f2f2f2-f2f2-f2f2-f2f2-f2f2f2f2f2f2"), new Guid("cccccccc-cccc-cccc-cccc-cccccccccccc"), new Guid("44444444-4444-4444-4444-444444444444"), 2 },
                    { new Guid("f3333333-1111-3333-3333-333333333333"), new Guid("d6666666-6666-6666-6666-666666666666"), new Guid("33333333-3333-3333-3333-333333333333"), 1 },
                    { new Guid("f3333333-2222-3333-3333-333333333333"), new Guid("d6666666-6666-6666-6666-666666666666"), new Guid("44444444-4444-4444-4444-444444444444"), 2 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_DiscountItems_DiscountId_ProductId",
                table: "DiscountItems",
                columns: new[] { "DiscountId", "ProductId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Discounts_Active",
                table: "Discounts",
                column: "Active");

            migrationBuilder.CreateIndex(
                name: "IX_OrderItems_OrderId_ProductId",
                table: "OrderItems",
                columns: new[] { "OrderId", "ProductId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Accompaniments");

            migrationBuilder.DropTable(
                name: "Burgers");

            migrationBuilder.DropTable(
                name: "DiscountItems");

            migrationBuilder.DropTable(
                name: "OrderItems");

            migrationBuilder.DropTable(
                name: "Discounts");

            migrationBuilder.DropTable(
                name: "Orders");
        }
    }
}
