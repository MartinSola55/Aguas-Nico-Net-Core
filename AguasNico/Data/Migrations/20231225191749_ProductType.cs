using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AguasNico.Migrations
{
    /// <inheritdoc />
    public partial class ProductType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Clients");

            migrationBuilder.RenameColumn(
                name: "Bottle",
                table: "DispatchedProducts",
                newName: "Type");

            migrationBuilder.AddColumn<int>(
                name: "Type",
                table: "Products",
                type: "int",
                nullable: false,
                defaultValue: 0);

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
                columns: new[] { "CreatedAt", "Type", "UpdatedAt" },
                values: new object[] { new DateTime(2023, 12, 25, 16, 17, 48, 927, DateTimeKind.Utc).AddTicks(8250), 0, new DateTime(2023, 12, 25, 16, 17, 48, 927, DateTimeKind.Utc).AddTicks(8260) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ID",
                keyValue: 2L,
                columns: new[] { "CreatedAt", "Type", "UpdatedAt" },
                values: new object[] { new DateTime(2023, 12, 25, 16, 17, 48, 927, DateTimeKind.Utc).AddTicks(8270), 0, new DateTime(2023, 12, 25, 16, 17, 48, 927, DateTimeKind.Utc).AddTicks(8270) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ID",
                keyValue: 3L,
                columns: new[] { "CreatedAt", "Type", "UpdatedAt" },
                values: new object[] { new DateTime(2023, 12, 25, 16, 17, 48, 927, DateTimeKind.Utc).AddTicks(8270), 0, new DateTime(2023, 12, 25, 16, 17, 48, 927, DateTimeKind.Utc).AddTicks(8270) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ID",
                keyValue: 4L,
                columns: new[] { "CreatedAt", "Type", "UpdatedAt" },
                values: new object[] { new DateTime(2023, 12, 25, 16, 17, 48, 927, DateTimeKind.Utc).AddTicks(8270), 0, new DateTime(2023, 12, 25, 16, 17, 48, 927, DateTimeKind.Utc).AddTicks(8270) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ID",
                keyValue: 5L,
                columns: new[] { "CreatedAt", "Type", "UpdatedAt" },
                values: new object[] { new DateTime(2023, 12, 25, 16, 17, 48, 927, DateTimeKind.Utc).AddTicks(8270), 0, new DateTime(2023, 12, 25, 16, 17, 48, 927, DateTimeKind.Utc).AddTicks(8270) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ID",
                keyValue: 6L,
                columns: new[] { "CreatedAt", "Type", "UpdatedAt" },
                values: new object[] { new DateTime(2023, 12, 25, 16, 17, 48, 927, DateTimeKind.Utc).AddTicks(8270), 0, new DateTime(2023, 12, 25, 16, 17, 48, 927, DateTimeKind.Utc).AddTicks(8270) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ID",
                keyValue: 7L,
                columns: new[] { "CreatedAt", "Type", "UpdatedAt" },
                values: new object[] { new DateTime(2023, 12, 25, 16, 17, 48, 927, DateTimeKind.Utc).AddTicks(8270), 0, new DateTime(2023, 12, 25, 16, 17, 48, 927, DateTimeKind.Utc).AddTicks(8270) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ID",
                keyValue: 8L,
                columns: new[] { "CreatedAt", "Type", "UpdatedAt" },
                values: new object[] { new DateTime(2023, 12, 25, 16, 17, 48, 927, DateTimeKind.Utc).AddTicks(8270), 0, new DateTime(2023, 12, 25, 16, 17, 48, 927, DateTimeKind.Utc).AddTicks(8270) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ID",
                keyValue: 9L,
                columns: new[] { "CreatedAt", "Type", "UpdatedAt" },
                values: new object[] { new DateTime(2023, 12, 25, 16, 17, 48, 927, DateTimeKind.Utc).AddTicks(8280), 0, new DateTime(2023, 12, 25, 16, 17, 48, 927, DateTimeKind.Utc).AddTicks(8280) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ID",
                keyValue: 10L,
                columns: new[] { "CreatedAt", "Type", "UpdatedAt" },
                values: new object[] { new DateTime(2023, 12, 25, 16, 17, 48, 927, DateTimeKind.Utc).AddTicks(8280), 0, new DateTime(2023, 12, 25, 16, 17, 48, 927, DateTimeKind.Utc).AddTicks(8280) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ID",
                keyValue: 11L,
                columns: new[] { "CreatedAt", "Type", "UpdatedAt" },
                values: new object[] { new DateTime(2023, 12, 25, 16, 17, 48, 927, DateTimeKind.Utc).AddTicks(8280), 0, new DateTime(2023, 12, 25, 16, 17, 48, 927, DateTimeKind.Utc).AddTicks(8280) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Type",
                table: "Products");

            migrationBuilder.RenameColumn(
                name: "Type",
                table: "DispatchedProducts",
                newName: "Bottle");

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Clients",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "Clients",
                keyColumn: "ID",
                keyValue: 1L,
                columns: new[] { "CreatedAt", "IsActive", "UpdatedAt" },
                values: new object[] { new DateTime(2023, 12, 23, 23, 4, 29, 36, DateTimeKind.Utc).AddTicks(2060), true, new DateTime(2023, 12, 23, 23, 4, 29, 36, DateTimeKind.Utc).AddTicks(2060) });

            migrationBuilder.UpdateData(
                table: "Clients",
                keyColumn: "ID",
                keyValue: 2L,
                columns: new[] { "CreatedAt", "IsActive", "UpdatedAt" },
                values: new object[] { new DateTime(2023, 12, 23, 23, 4, 29, 36, DateTimeKind.Utc).AddTicks(2060), true, new DateTime(2023, 12, 23, 23, 4, 29, 36, DateTimeKind.Utc).AddTicks(2060) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ID",
                keyValue: 1L,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2023, 12, 23, 23, 4, 29, 36, DateTimeKind.Utc).AddTicks(1940), new DateTime(2023, 12, 23, 23, 4, 29, 36, DateTimeKind.Utc).AddTicks(1950) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ID",
                keyValue: 2L,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2023, 12, 23, 23, 4, 29, 36, DateTimeKind.Utc).AddTicks(1950), new DateTime(2023, 12, 23, 23, 4, 29, 36, DateTimeKind.Utc).AddTicks(1950) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ID",
                keyValue: 3L,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2023, 12, 23, 23, 4, 29, 36, DateTimeKind.Utc).AddTicks(1950), new DateTime(2023, 12, 23, 23, 4, 29, 36, DateTimeKind.Utc).AddTicks(1950) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ID",
                keyValue: 4L,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2023, 12, 23, 23, 4, 29, 36, DateTimeKind.Utc).AddTicks(1970), new DateTime(2023, 12, 23, 23, 4, 29, 36, DateTimeKind.Utc).AddTicks(1970) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ID",
                keyValue: 5L,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2023, 12, 23, 23, 4, 29, 36, DateTimeKind.Utc).AddTicks(1970), new DateTime(2023, 12, 23, 23, 4, 29, 36, DateTimeKind.Utc).AddTicks(1970) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ID",
                keyValue: 6L,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2023, 12, 23, 23, 4, 29, 36, DateTimeKind.Utc).AddTicks(1970), new DateTime(2023, 12, 23, 23, 4, 29, 36, DateTimeKind.Utc).AddTicks(1970) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ID",
                keyValue: 7L,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2023, 12, 23, 23, 4, 29, 36, DateTimeKind.Utc).AddTicks(1970), new DateTime(2023, 12, 23, 23, 4, 29, 36, DateTimeKind.Utc).AddTicks(1970) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ID",
                keyValue: 8L,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2023, 12, 23, 23, 4, 29, 36, DateTimeKind.Utc).AddTicks(1980), new DateTime(2023, 12, 23, 23, 4, 29, 36, DateTimeKind.Utc).AddTicks(1980) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ID",
                keyValue: 9L,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2023, 12, 23, 23, 4, 29, 36, DateTimeKind.Utc).AddTicks(1980), new DateTime(2023, 12, 23, 23, 4, 29, 36, DateTimeKind.Utc).AddTicks(1980) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ID",
                keyValue: 10L,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2023, 12, 23, 23, 4, 29, 36, DateTimeKind.Utc).AddTicks(1980), new DateTime(2023, 12, 23, 23, 4, 29, 36, DateTimeKind.Utc).AddTicks(1980) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ID",
                keyValue: 11L,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2023, 12, 23, 23, 4, 29, 36, DateTimeKind.Utc).AddTicks(1980), new DateTime(2023, 12, 23, 23, 4, 29, 36, DateTimeKind.Utc).AddTicks(1980) });
        }
    }
}
