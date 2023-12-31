using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AguasNico.Migrations
{
    /// <inheritdoc />
    public partial class ReturnedProductType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ReturnedProducts_Products_ProductID",
                table: "ReturnedProducts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ReturnedProducts",
                table: "ReturnedProducts");

            migrationBuilder.DropColumn(
                name: "ProductID",
                table: "ReturnedProducts");

            migrationBuilder.AddColumn<int>(
                name: "Type",
                table: "ReturnedProducts",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_ReturnedProducts",
                table: "ReturnedProducts",
                columns: new[] { "Type", "CartID" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_ReturnedProducts",
                table: "ReturnedProducts");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "ReturnedProducts");

            migrationBuilder.AddColumn<long>(
                name: "ProductID",
                table: "ReturnedProducts",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddPrimaryKey(
                name: "PK_ReturnedProducts",
                table: "ReturnedProducts",
                columns: new[] { "ProductID", "CartID" });

            migrationBuilder.AddForeignKey(
                name: "FK_ReturnedProducts_Products_ProductID",
                table: "ReturnedProducts",
                column: "ProductID",
                principalTable: "Products",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
