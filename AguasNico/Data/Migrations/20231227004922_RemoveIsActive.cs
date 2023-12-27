using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AguasNico.Migrations
{
    /// <inheritdoc />
    public partial class RemoveIsActive : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "PaymentMethods");

            migrationBuilder.UpdateData(
                table: "Clients",
                keyColumn: "ID",
                keyValue: 1L,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2023, 12, 26, 21, 49, 21, 731, DateTimeKind.Utc).AddTicks(3847), new DateTime(2023, 12, 26, 21, 49, 21, 731, DateTimeKind.Utc).AddTicks(3848) });

            migrationBuilder.UpdateData(
                table: "Clients",
                keyColumn: "ID",
                keyValue: 2L,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2023, 12, 26, 21, 49, 21, 731, DateTimeKind.Utc).AddTicks(3854), new DateTime(2023, 12, 26, 21, 49, 21, 731, DateTimeKind.Utc).AddTicks(3855) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ID",
                keyValue: 1L,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2023, 12, 26, 21, 49, 21, 731, DateTimeKind.Utc).AddTicks(3732), new DateTime(2023, 12, 26, 21, 49, 21, 731, DateTimeKind.Utc).AddTicks(3738) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ID",
                keyValue: 2L,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2023, 12, 26, 21, 49, 21, 731, DateTimeKind.Utc).AddTicks(3743), new DateTime(2023, 12, 26, 21, 49, 21, 731, DateTimeKind.Utc).AddTicks(3743) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ID",
                keyValue: 3L,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2023, 12, 26, 21, 49, 21, 731, DateTimeKind.Utc).AddTicks(3744), new DateTime(2023, 12, 26, 21, 49, 21, 731, DateTimeKind.Utc).AddTicks(3745) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ID",
                keyValue: 4L,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2023, 12, 26, 21, 49, 21, 731, DateTimeKind.Utc).AddTicks(3746), new DateTime(2023, 12, 26, 21, 49, 21, 731, DateTimeKind.Utc).AddTicks(3746) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ID",
                keyValue: 5L,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2023, 12, 26, 21, 49, 21, 731, DateTimeKind.Utc).AddTicks(3747), new DateTime(2023, 12, 26, 21, 49, 21, 731, DateTimeKind.Utc).AddTicks(3747) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ID",
                keyValue: 6L,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2023, 12, 26, 21, 49, 21, 731, DateTimeKind.Utc).AddTicks(3748), new DateTime(2023, 12, 26, 21, 49, 21, 731, DateTimeKind.Utc).AddTicks(3748) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ID",
                keyValue: 7L,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2023, 12, 26, 21, 49, 21, 731, DateTimeKind.Utc).AddTicks(3749), new DateTime(2023, 12, 26, 21, 49, 21, 731, DateTimeKind.Utc).AddTicks(3749) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ID",
                keyValue: 8L,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2023, 12, 26, 21, 49, 21, 731, DateTimeKind.Utc).AddTicks(3750), new DateTime(2023, 12, 26, 21, 49, 21, 731, DateTimeKind.Utc).AddTicks(3750) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ID",
                keyValue: 9L,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2023, 12, 26, 21, 49, 21, 731, DateTimeKind.Utc).AddTicks(3751), new DateTime(2023, 12, 26, 21, 49, 21, 731, DateTimeKind.Utc).AddTicks(3751) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ID",
                keyValue: 10L,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2023, 12, 26, 21, 49, 21, 731, DateTimeKind.Utc).AddTicks(3752), new DateTime(2023, 12, 26, 21, 49, 21, 731, DateTimeKind.Utc).AddTicks(3752) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ID",
                keyValue: 11L,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2023, 12, 26, 21, 49, 21, 731, DateTimeKind.Utc).AddTicks(3753), new DateTime(2023, 12, 26, 21, 49, 21, 731, DateTimeKind.Utc).AddTicks(3753) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Products",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "PaymentMethods",
                type: "bit",
                nullable: false,
                defaultValue: false);

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
                table: "PaymentMethods",
                keyColumn: "ID",
                keyValue: (short)1,
                column: "IsActive",
                value: true);

            migrationBuilder.UpdateData(
                table: "PaymentMethods",
                keyColumn: "ID",
                keyValue: (short)2,
                column: "IsActive",
                value: true);

            migrationBuilder.UpdateData(
                table: "PaymentMethods",
                keyColumn: "ID",
                keyValue: (short)3,
                column: "IsActive",
                value: true);

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ID",
                keyValue: 1L,
                columns: new[] { "CreatedAt", "IsActive", "UpdatedAt" },
                values: new object[] { new DateTime(2023, 12, 25, 19, 16, 35, 207, DateTimeKind.Utc).AddTicks(4280), true, new DateTime(2023, 12, 25, 19, 16, 35, 207, DateTimeKind.Utc).AddTicks(4290) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ID",
                keyValue: 2L,
                columns: new[] { "CreatedAt", "IsActive", "UpdatedAt" },
                values: new object[] { new DateTime(2023, 12, 25, 19, 16, 35, 207, DateTimeKind.Utc).AddTicks(4290), true, new DateTime(2023, 12, 25, 19, 16, 35, 207, DateTimeKind.Utc).AddTicks(4290) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ID",
                keyValue: 3L,
                columns: new[] { "CreatedAt", "IsActive", "UpdatedAt" },
                values: new object[] { new DateTime(2023, 12, 25, 19, 16, 35, 207, DateTimeKind.Utc).AddTicks(4300), true, new DateTime(2023, 12, 25, 19, 16, 35, 207, DateTimeKind.Utc).AddTicks(4300) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ID",
                keyValue: 4L,
                columns: new[] { "CreatedAt", "IsActive", "UpdatedAt" },
                values: new object[] { new DateTime(2023, 12, 25, 19, 16, 35, 207, DateTimeKind.Utc).AddTicks(4300), true, new DateTime(2023, 12, 25, 19, 16, 35, 207, DateTimeKind.Utc).AddTicks(4300) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ID",
                keyValue: 5L,
                columns: new[] { "CreatedAt", "IsActive", "UpdatedAt" },
                values: new object[] { new DateTime(2023, 12, 25, 19, 16, 35, 207, DateTimeKind.Utc).AddTicks(4300), true, new DateTime(2023, 12, 25, 19, 16, 35, 207, DateTimeKind.Utc).AddTicks(4300) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ID",
                keyValue: 6L,
                columns: new[] { "CreatedAt", "IsActive", "UpdatedAt" },
                values: new object[] { new DateTime(2023, 12, 25, 19, 16, 35, 207, DateTimeKind.Utc).AddTicks(4300), true, new DateTime(2023, 12, 25, 19, 16, 35, 207, DateTimeKind.Utc).AddTicks(4300) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ID",
                keyValue: 7L,
                columns: new[] { "CreatedAt", "IsActive", "UpdatedAt" },
                values: new object[] { new DateTime(2023, 12, 25, 19, 16, 35, 207, DateTimeKind.Utc).AddTicks(4300), true, new DateTime(2023, 12, 25, 19, 16, 35, 207, DateTimeKind.Utc).AddTicks(4300) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ID",
                keyValue: 8L,
                columns: new[] { "CreatedAt", "IsActive", "UpdatedAt" },
                values: new object[] { new DateTime(2023, 12, 25, 19, 16, 35, 207, DateTimeKind.Utc).AddTicks(4300), true, new DateTime(2023, 12, 25, 19, 16, 35, 207, DateTimeKind.Utc).AddTicks(4300) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ID",
                keyValue: 9L,
                columns: new[] { "CreatedAt", "IsActive", "UpdatedAt" },
                values: new object[] { new DateTime(2023, 12, 25, 19, 16, 35, 207, DateTimeKind.Utc).AddTicks(4300), true, new DateTime(2023, 12, 25, 19, 16, 35, 207, DateTimeKind.Utc).AddTicks(4300) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ID",
                keyValue: 10L,
                columns: new[] { "CreatedAt", "IsActive", "UpdatedAt" },
                values: new object[] { new DateTime(2023, 12, 25, 19, 16, 35, 207, DateTimeKind.Utc).AddTicks(4310), true, new DateTime(2023, 12, 25, 19, 16, 35, 207, DateTimeKind.Utc).AddTicks(4310) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ID",
                keyValue: 11L,
                columns: new[] { "CreatedAt", "IsActive", "UpdatedAt" },
                values: new object[] { new DateTime(2023, 12, 25, 19, 16, 35, 207, DateTimeKind.Utc).AddTicks(4310), true, new DateTime(2023, 12, 25, 19, 16, 35, 207, DateTimeKind.Utc).AddTicks(4310) });
        }
    }
}
