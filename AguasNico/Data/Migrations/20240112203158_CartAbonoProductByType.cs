using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AguasNico.Migrations
{
    /// <inheritdoc />
    public partial class CartAbonoProductByType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CartAbonoProducts_AbonoRenewalProducts_AbonoRenewalProductID",
                table: "CartAbonoProducts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CartAbonoProducts",
                table: "CartAbonoProducts");

            migrationBuilder.DropIndex(
                name: "IX_CartAbonoProducts_AbonoRenewalProductID",
                table: "CartAbonoProducts");

            migrationBuilder.DropColumn(
                name: "AbonoRenewalProductID",
                table: "CartAbonoProducts");

            migrationBuilder.AddColumn<int>(
                name: "Type",
                table: "CartAbonoProducts",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_CartAbonoProducts",
                table: "CartAbonoProducts",
                columns: new[] { "CartID", "Type" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_CartAbonoProducts",
                table: "CartAbonoProducts");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "CartAbonoProducts");

            migrationBuilder.AddColumn<long>(
                name: "AbonoRenewalProductID",
                table: "CartAbonoProducts",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddPrimaryKey(
                name: "PK_CartAbonoProducts",
                table: "CartAbonoProducts",
                columns: new[] { "CartID", "AbonoRenewalProductID" });

            migrationBuilder.CreateIndex(
                name: "IX_CartAbonoProducts_AbonoRenewalProductID",
                table: "CartAbonoProducts",
                column: "AbonoRenewalProductID");

            migrationBuilder.AddForeignKey(
                name: "FK_CartAbonoProducts_AbonoRenewalProducts_AbonoRenewalProductID",
                table: "CartAbonoProducts",
                column: "AbonoRenewalProductID",
                principalTable: "AbonoRenewalProducts",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
