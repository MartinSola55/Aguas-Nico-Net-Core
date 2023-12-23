using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AguasNico.Migrations
{
    /// <inheritdoc />
    public partial class DispatchedProduct : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Day",
                table: "Routes",
                newName: "DayOfWeek");

            migrationBuilder.CreateTable(
                name: "DispatchedProducts",
                columns: table => new
                {
                    RouteID = table.Column<long>(type: "bigint", nullable: false),
                    Bottle = table.Column<int>(type: "int", nullable: false),
                    ProductName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DispatchedProducts", x => new { x.RouteID, x.Bottle });
                    table.ForeignKey(
                        name: "FK_DispatchedProducts_Routes_RouteID",
                        column: x => x.RouteID,
                        principalTable: "Routes",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "Clients",
                keyColumn: "ID",
                keyValue: 1L,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2023, 12, 23, 18, 11, 38, 735, DateTimeKind.Utc).AddTicks(4690), new DateTime(2023, 12, 23, 18, 11, 38, 735, DateTimeKind.Utc).AddTicks(4690) });

            migrationBuilder.UpdateData(
                table: "Clients",
                keyColumn: "ID",
                keyValue: 2L,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2023, 12, 23, 18, 11, 38, 735, DateTimeKind.Utc).AddTicks(4690), new DateTime(2023, 12, 23, 18, 11, 38, 735, DateTimeKind.Utc).AddTicks(4690) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ID",
                keyValue: 1L,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2023, 12, 23, 18, 11, 38, 735, DateTimeKind.Utc).AddTicks(4520), new DateTime(2023, 12, 23, 18, 11, 38, 735, DateTimeKind.Utc).AddTicks(4530) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ID",
                keyValue: 2L,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2023, 12, 23, 18, 11, 38, 735, DateTimeKind.Utc).AddTicks(4530), new DateTime(2023, 12, 23, 18, 11, 38, 735, DateTimeKind.Utc).AddTicks(4530) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ID",
                keyValue: 3L,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2023, 12, 23, 18, 11, 38, 735, DateTimeKind.Utc).AddTicks(4530), new DateTime(2023, 12, 23, 18, 11, 38, 735, DateTimeKind.Utc).AddTicks(4530) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ID",
                keyValue: 4L,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2023, 12, 23, 18, 11, 38, 735, DateTimeKind.Utc).AddTicks(4530), new DateTime(2023, 12, 23, 18, 11, 38, 735, DateTimeKind.Utc).AddTicks(4540) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ID",
                keyValue: 5L,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2023, 12, 23, 18, 11, 38, 735, DateTimeKind.Utc).AddTicks(4540), new DateTime(2023, 12, 23, 18, 11, 38, 735, DateTimeKind.Utc).AddTicks(4540) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ID",
                keyValue: 6L,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2023, 12, 23, 18, 11, 38, 735, DateTimeKind.Utc).AddTicks(4540), new DateTime(2023, 12, 23, 18, 11, 38, 735, DateTimeKind.Utc).AddTicks(4540) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ID",
                keyValue: 7L,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2023, 12, 23, 18, 11, 38, 735, DateTimeKind.Utc).AddTicks(4540), new DateTime(2023, 12, 23, 18, 11, 38, 735, DateTimeKind.Utc).AddTicks(4540) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ID",
                keyValue: 8L,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2023, 12, 23, 18, 11, 38, 735, DateTimeKind.Utc).AddTicks(4540), new DateTime(2023, 12, 23, 18, 11, 38, 735, DateTimeKind.Utc).AddTicks(4540) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ID",
                keyValue: 9L,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2023, 12, 23, 18, 11, 38, 735, DateTimeKind.Utc).AddTicks(4540), new DateTime(2023, 12, 23, 18, 11, 38, 735, DateTimeKind.Utc).AddTicks(4540) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ID",
                keyValue: 10L,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2023, 12, 23, 18, 11, 38, 735, DateTimeKind.Utc).AddTicks(4540), new DateTime(2023, 12, 23, 18, 11, 38, 735, DateTimeKind.Utc).AddTicks(4540) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ID",
                keyValue: 11L,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2023, 12, 23, 18, 11, 38, 735, DateTimeKind.Utc).AddTicks(4540), new DateTime(2023, 12, 23, 18, 11, 38, 735, DateTimeKind.Utc).AddTicks(4540) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DispatchedProducts");

            migrationBuilder.RenameColumn(
                name: "DayOfWeek",
                table: "Routes",
                newName: "Day");

            migrationBuilder.UpdateData(
                table: "Clients",
                keyColumn: "ID",
                keyValue: 1L,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2023, 12, 22, 2, 27, 22, 869, DateTimeKind.Utc).AddTicks(933), new DateTime(2023, 12, 22, 2, 27, 22, 869, DateTimeKind.Utc).AddTicks(934) });

            migrationBuilder.UpdateData(
                table: "Clients",
                keyColumn: "ID",
                keyValue: 2L,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2023, 12, 22, 2, 27, 22, 869, DateTimeKind.Utc).AddTicks(941), new DateTime(2023, 12, 22, 2, 27, 22, 869, DateTimeKind.Utc).AddTicks(941) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ID",
                keyValue: 1L,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2023, 12, 22, 2, 27, 22, 869, DateTimeKind.Utc).AddTicks(831), new DateTime(2023, 12, 22, 2, 27, 22, 869, DateTimeKind.Utc).AddTicks(835) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ID",
                keyValue: 2L,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2023, 12, 22, 2, 27, 22, 869, DateTimeKind.Utc).AddTicks(840), new DateTime(2023, 12, 22, 2, 27, 22, 869, DateTimeKind.Utc).AddTicks(840) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ID",
                keyValue: 3L,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2023, 12, 22, 2, 27, 22, 869, DateTimeKind.Utc).AddTicks(842), new DateTime(2023, 12, 22, 2, 27, 22, 869, DateTimeKind.Utc).AddTicks(843) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ID",
                keyValue: 4L,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2023, 12, 22, 2, 27, 22, 869, DateTimeKind.Utc).AddTicks(844), new DateTime(2023, 12, 22, 2, 27, 22, 869, DateTimeKind.Utc).AddTicks(844) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ID",
                keyValue: 5L,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2023, 12, 22, 2, 27, 22, 869, DateTimeKind.Utc).AddTicks(845), new DateTime(2023, 12, 22, 2, 27, 22, 869, DateTimeKind.Utc).AddTicks(845) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ID",
                keyValue: 6L,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2023, 12, 22, 2, 27, 22, 869, DateTimeKind.Utc).AddTicks(846), new DateTime(2023, 12, 22, 2, 27, 22, 869, DateTimeKind.Utc).AddTicks(846) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ID",
                keyValue: 7L,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2023, 12, 22, 2, 27, 22, 869, DateTimeKind.Utc).AddTicks(847), new DateTime(2023, 12, 22, 2, 27, 22, 869, DateTimeKind.Utc).AddTicks(847) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ID",
                keyValue: 8L,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2023, 12, 22, 2, 27, 22, 869, DateTimeKind.Utc).AddTicks(848), new DateTime(2023, 12, 22, 2, 27, 22, 869, DateTimeKind.Utc).AddTicks(848) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ID",
                keyValue: 9L,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2023, 12, 22, 2, 27, 22, 869, DateTimeKind.Utc).AddTicks(849), new DateTime(2023, 12, 22, 2, 27, 22, 869, DateTimeKind.Utc).AddTicks(849) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ID",
                keyValue: 10L,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2023, 12, 22, 2, 27, 22, 869, DateTimeKind.Utc).AddTicks(850), new DateTime(2023, 12, 22, 2, 27, 22, 869, DateTimeKind.Utc).AddTicks(850) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ID",
                keyValue: 11L,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2023, 12, 22, 2, 27, 22, 869, DateTimeKind.Utc).AddTicks(851), new DateTime(2023, 12, 22, 2, 27, 22, 869, DateTimeKind.Utc).AddTicks(851) });
        }
    }
}
