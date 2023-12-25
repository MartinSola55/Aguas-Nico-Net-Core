using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AguasNico.Migrations
{
    /// <inheritdoc />
    public partial class RemoveBottles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Bottle",
                table: "Products");

            migrationBuilder.UpdateData(
                table: "Clients",
                keyColumn: "ID",
                keyValue: 1L,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2023, 12, 25, 19, 16, 35, 207, DateTimeKind.Utc).AddTicks(4390), new DateTime(2023, 12, 25, 19, 16, 35, 207, DateTimeKind.Utc).AddTicks(4390) });

            migrationBuilder.UpdateData(
                table: "Clients",
                keyColumn: "ID",
                keyValue: 2L,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2023, 12, 25, 19, 16, 35, 207, DateTimeKind.Utc).AddTicks(4410), new DateTime(2023, 12, 25, 19, 16, 35, 207, DateTimeKind.Utc).AddTicks(4410) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ID",
                keyValue: 1L,
                columns: new[] { "CreatedAt", "Type", "UpdatedAt" },
                values: new object[] { new DateTime(2023, 12, 25, 19, 16, 35, 207, DateTimeKind.Utc).AddTicks(4280), 4, new DateTime(2023, 12, 25, 19, 16, 35, 207, DateTimeKind.Utc).AddTicks(4290) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ID",
                keyValue: 2L,
                columns: new[] { "CreatedAt", "Type", "UpdatedAt" },
                values: new object[] { new DateTime(2023, 12, 25, 19, 16, 35, 207, DateTimeKind.Utc).AddTicks(4290), 2, new DateTime(2023, 12, 25, 19, 16, 35, 207, DateTimeKind.Utc).AddTicks(4290) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ID",
                keyValue: 3L,
                columns: new[] { "CreatedAt", "Type", "UpdatedAt" },
                values: new object[] { new DateTime(2023, 12, 25, 19, 16, 35, 207, DateTimeKind.Utc).AddTicks(4300), 1, new DateTime(2023, 12, 25, 19, 16, 35, 207, DateTimeKind.Utc).AddTicks(4300) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ID",
                keyValue: 4L,
                columns: new[] { "CreatedAt", "Type", "UpdatedAt" },
                values: new object[] { new DateTime(2023, 12, 25, 19, 16, 35, 207, DateTimeKind.Utc).AddTicks(4300), 3, new DateTime(2023, 12, 25, 19, 16, 35, 207, DateTimeKind.Utc).AddTicks(4300) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ID",
                keyValue: 5L,
                columns: new[] { "CreatedAt", "Type", "UpdatedAt" },
                values: new object[] { new DateTime(2023, 12, 25, 19, 16, 35, 207, DateTimeKind.Utc).AddTicks(4300), 1, new DateTime(2023, 12, 25, 19, 16, 35, 207, DateTimeKind.Utc).AddTicks(4300) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ID",
                keyValue: 6L,
                columns: new[] { "CreatedAt", "Type", "UpdatedAt" },
                values: new object[] { new DateTime(2023, 12, 25, 19, 16, 35, 207, DateTimeKind.Utc).AddTicks(4300), 1, new DateTime(2023, 12, 25, 19, 16, 35, 207, DateTimeKind.Utc).AddTicks(4300) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ID",
                keyValue: 7L,
                columns: new[] { "CreatedAt", "Type", "UpdatedAt" },
                values: new object[] { new DateTime(2023, 12, 25, 19, 16, 35, 207, DateTimeKind.Utc).AddTicks(4300), 4, new DateTime(2023, 12, 25, 19, 16, 35, 207, DateTimeKind.Utc).AddTicks(4300) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ID",
                keyValue: 8L,
                columns: new[] { "CreatedAt", "Type", "UpdatedAt" },
                values: new object[] { new DateTime(2023, 12, 25, 19, 16, 35, 207, DateTimeKind.Utc).AddTicks(4300), 4, new DateTime(2023, 12, 25, 19, 16, 35, 207, DateTimeKind.Utc).AddTicks(4300) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ID",
                keyValue: 9L,
                columns: new[] { "CreatedAt", "Type", "UpdatedAt" },
                values: new object[] { new DateTime(2023, 12, 25, 19, 16, 35, 207, DateTimeKind.Utc).AddTicks(4300), 1, new DateTime(2023, 12, 25, 19, 16, 35, 207, DateTimeKind.Utc).AddTicks(4300) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ID",
                keyValue: 10L,
                columns: new[] { "CreatedAt", "Type", "UpdatedAt" },
                values: new object[] { new DateTime(2023, 12, 25, 19, 16, 35, 207, DateTimeKind.Utc).AddTicks(4310), 1, new DateTime(2023, 12, 25, 19, 16, 35, 207, DateTimeKind.Utc).AddTicks(4310) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ID",
                keyValue: 11L,
                columns: new[] { "CreatedAt", "Type", "UpdatedAt" },
                values: new object[] { new DateTime(2023, 12, 25, 19, 16, 35, 207, DateTimeKind.Utc).AddTicks(4310), 1, new DateTime(2023, 12, 25, 19, 16, 35, 207, DateTimeKind.Utc).AddTicks(4310) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Bottle",
                table: "Products",
                type: "int",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Clients",
                keyColumn: "ID",
                keyValue: 1L,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2023, 12, 25, 16, 17, 48, 927, DateTimeKind.Utc).AddTicks(8360), new DateTime(2023, 12, 25, 16, 17, 48, 927, DateTimeKind.Utc).AddTicks(8360) });

            migrationBuilder.UpdateData(
                table: "Clients",
                keyColumn: "ID",
                keyValue: 2L,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2023, 12, 25, 16, 17, 48, 927, DateTimeKind.Utc).AddTicks(8360), new DateTime(2023, 12, 25, 16, 17, 48, 927, DateTimeKind.Utc).AddTicks(8360) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ID",
                keyValue: 1L,
                columns: new[] { "Bottle", "CreatedAt", "Type", "UpdatedAt" },
                values: new object[] { null, new DateTime(2023, 12, 25, 16, 17, 48, 927, DateTimeKind.Utc).AddTicks(8250), 0, new DateTime(2023, 12, 25, 16, 17, 48, 927, DateTimeKind.Utc).AddTicks(8260) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ID",
                keyValue: 2L,
                columns: new[] { "Bottle", "CreatedAt", "Type", "UpdatedAt" },
                values: new object[] { 2, new DateTime(2023, 12, 25, 16, 17, 48, 927, DateTimeKind.Utc).AddTicks(8270), 0, new DateTime(2023, 12, 25, 16, 17, 48, 927, DateTimeKind.Utc).AddTicks(8270) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ID",
                keyValue: 3L,
                columns: new[] { "Bottle", "CreatedAt", "Type", "UpdatedAt" },
                values: new object[] { 1, new DateTime(2023, 12, 25, 16, 17, 48, 927, DateTimeKind.Utc).AddTicks(8270), 0, new DateTime(2023, 12, 25, 16, 17, 48, 927, DateTimeKind.Utc).AddTicks(8270) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ID",
                keyValue: 4L,
                columns: new[] { "Bottle", "CreatedAt", "Type", "UpdatedAt" },
                values: new object[] { null, new DateTime(2023, 12, 25, 16, 17, 48, 927, DateTimeKind.Utc).AddTicks(8270), 0, new DateTime(2023, 12, 25, 16, 17, 48, 927, DateTimeKind.Utc).AddTicks(8270) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ID",
                keyValue: 5L,
                columns: new[] { "Bottle", "CreatedAt", "Type", "UpdatedAt" },
                values: new object[] { 1, new DateTime(2023, 12, 25, 16, 17, 48, 927, DateTimeKind.Utc).AddTicks(8270), 0, new DateTime(2023, 12, 25, 16, 17, 48, 927, DateTimeKind.Utc).AddTicks(8270) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ID",
                keyValue: 6L,
                columns: new[] { "Bottle", "CreatedAt", "Type", "UpdatedAt" },
                values: new object[] { 1, new DateTime(2023, 12, 25, 16, 17, 48, 927, DateTimeKind.Utc).AddTicks(8270), 0, new DateTime(2023, 12, 25, 16, 17, 48, 927, DateTimeKind.Utc).AddTicks(8270) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ID",
                keyValue: 7L,
                columns: new[] { "Bottle", "CreatedAt", "Type", "UpdatedAt" },
                values: new object[] { null, new DateTime(2023, 12, 25, 16, 17, 48, 927, DateTimeKind.Utc).AddTicks(8270), 0, new DateTime(2023, 12, 25, 16, 17, 48, 927, DateTimeKind.Utc).AddTicks(8270) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ID",
                keyValue: 8L,
                columns: new[] { "Bottle", "CreatedAt", "Type", "UpdatedAt" },
                values: new object[] { null, new DateTime(2023, 12, 25, 16, 17, 48, 927, DateTimeKind.Utc).AddTicks(8270), 0, new DateTime(2023, 12, 25, 16, 17, 48, 927, DateTimeKind.Utc).AddTicks(8270) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ID",
                keyValue: 9L,
                columns: new[] { "Bottle", "CreatedAt", "Type", "UpdatedAt" },
                values: new object[] { 1, new DateTime(2023, 12, 25, 16, 17, 48, 927, DateTimeKind.Utc).AddTicks(8280), 0, new DateTime(2023, 12, 25, 16, 17, 48, 927, DateTimeKind.Utc).AddTicks(8280) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ID",
                keyValue: 10L,
                columns: new[] { "Bottle", "CreatedAt", "Type", "UpdatedAt" },
                values: new object[] { 1, new DateTime(2023, 12, 25, 16, 17, 48, 927, DateTimeKind.Utc).AddTicks(8280), 0, new DateTime(2023, 12, 25, 16, 17, 48, 927, DateTimeKind.Utc).AddTicks(8280) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ID",
                keyValue: 11L,
                columns: new[] { "Bottle", "CreatedAt", "Type", "UpdatedAt" },
                values: new object[] { 1, new DateTime(2023, 12, 25, 16, 17, 48, 927, DateTimeKind.Utc).AddTicks(8280), 0, new DateTime(2023, 12, 25, 16, 17, 48, 927, DateTimeKind.Utc).AddTicks(8280) });
        }
    }
}
