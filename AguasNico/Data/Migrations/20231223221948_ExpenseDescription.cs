using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AguasNico.Migrations
{
    /// <inheritdoc />
    public partial class ExpenseDescription : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Expenses",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "Expenses");

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
    }
}
