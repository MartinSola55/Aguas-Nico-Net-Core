using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AguasNico.Migrations
{
    /// <inheritdoc />
    public partial class FixClientAbono : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ClientAbonos_Products_ProductID",
                table: "ClientAbonos");

            migrationBuilder.DropIndex(
                name: "IX_ClientAbonos_ProductID",
                table: "ClientAbonos");

            migrationBuilder.DropColumn(
                name: "ProductID",
                table: "ClientAbonos");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "ProductID",
                table: "ClientAbonos",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateIndex(
                name: "IX_ClientAbonos_ProductID",
                table: "ClientAbonos",
                column: "ProductID");

            migrationBuilder.AddForeignKey(
                name: "FK_ClientAbonos_Products_ProductID",
                table: "ClientAbonos",
                column: "ProductID",
                principalTable: "Products",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
