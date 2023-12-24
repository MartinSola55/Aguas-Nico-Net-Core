using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AguasNico.Migrations
{
    /// <inheritdoc />
    public partial class ClientDeliveryDay : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DeliveryDay",
                table: "Clients",
                type: "int",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Clients",
                keyColumn: "ID",
                keyValue: 1L,
                columns: new[] { "CreatedAt", "DeliveryDay", "UpdatedAt" },
                values: new object[] { new DateTime(2023, 12, 23, 23, 4, 29, 36, DateTimeKind.Utc).AddTicks(2060), null, new DateTime(2023, 12, 23, 23, 4, 29, 36, DateTimeKind.Utc).AddTicks(2060) });

            migrationBuilder.UpdateData(
                table: "Clients",
                keyColumn: "ID",
                keyValue: 2L,
                columns: new[] { "CreatedAt", "DeliveryDay", "UpdatedAt" },
                values: new object[] { new DateTime(2023, 12, 23, 23, 4, 29, 36, DateTimeKind.Utc).AddTicks(2060), null, new DateTime(2023, 12, 23, 23, 4, 29, 36, DateTimeKind.Utc).AddTicks(2060) });

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DeliveryDay",
                table: "Clients");

            migrationBuilder.UpdateData(
                table: "Clients",
                keyColumn: "ID",
                keyValue: 1L,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2023, 12, 23, 19, 19, 48, 107, DateTimeKind.Utc).AddTicks(3470), new DateTime(2023, 12, 23, 19, 19, 48, 107, DateTimeKind.Utc).AddTicks(3470) });

            migrationBuilder.UpdateData(
                table: "Clients",
                keyColumn: "ID",
                keyValue: 2L,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2023, 12, 23, 19, 19, 48, 107, DateTimeKind.Utc).AddTicks(3480), new DateTime(2023, 12, 23, 19, 19, 48, 107, DateTimeKind.Utc).AddTicks(3480) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ID",
                keyValue: 1L,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2023, 12, 23, 19, 19, 48, 107, DateTimeKind.Utc).AddTicks(3360), new DateTime(2023, 12, 23, 19, 19, 48, 107, DateTimeKind.Utc).AddTicks(3370) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ID",
                keyValue: 2L,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2023, 12, 23, 19, 19, 48, 107, DateTimeKind.Utc).AddTicks(3370), new DateTime(2023, 12, 23, 19, 19, 48, 107, DateTimeKind.Utc).AddTicks(3370) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ID",
                keyValue: 3L,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2023, 12, 23, 19, 19, 48, 107, DateTimeKind.Utc).AddTicks(3370), new DateTime(2023, 12, 23, 19, 19, 48, 107, DateTimeKind.Utc).AddTicks(3380) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ID",
                keyValue: 4L,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2023, 12, 23, 19, 19, 48, 107, DateTimeKind.Utc).AddTicks(3380), new DateTime(2023, 12, 23, 19, 19, 48, 107, DateTimeKind.Utc).AddTicks(3380) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ID",
                keyValue: 5L,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2023, 12, 23, 19, 19, 48, 107, DateTimeKind.Utc).AddTicks(3380), new DateTime(2023, 12, 23, 19, 19, 48, 107, DateTimeKind.Utc).AddTicks(3380) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ID",
                keyValue: 6L,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2023, 12, 23, 19, 19, 48, 107, DateTimeKind.Utc).AddTicks(3380), new DateTime(2023, 12, 23, 19, 19, 48, 107, DateTimeKind.Utc).AddTicks(3380) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ID",
                keyValue: 7L,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2023, 12, 23, 19, 19, 48, 107, DateTimeKind.Utc).AddTicks(3380), new DateTime(2023, 12, 23, 19, 19, 48, 107, DateTimeKind.Utc).AddTicks(3380) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ID",
                keyValue: 8L,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2023, 12, 23, 19, 19, 48, 107, DateTimeKind.Utc).AddTicks(3380), new DateTime(2023, 12, 23, 19, 19, 48, 107, DateTimeKind.Utc).AddTicks(3380) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ID",
                keyValue: 9L,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2023, 12, 23, 19, 19, 48, 107, DateTimeKind.Utc).AddTicks(3380), new DateTime(2023, 12, 23, 19, 19, 48, 107, DateTimeKind.Utc).AddTicks(3380) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ID",
                keyValue: 10L,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2023, 12, 23, 19, 19, 48, 107, DateTimeKind.Utc).AddTicks(3380), new DateTime(2023, 12, 23, 19, 19, 48, 107, DateTimeKind.Utc).AddTicks(3380) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ID",
                keyValue: 11L,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2023, 12, 23, 19, 19, 48, 107, DateTimeKind.Utc).AddTicks(3380), new DateTime(2023, 12, 23, 19, 19, 48, 107, DateTimeKind.Utc).AddTicks(3380) });
        }
    }
}
