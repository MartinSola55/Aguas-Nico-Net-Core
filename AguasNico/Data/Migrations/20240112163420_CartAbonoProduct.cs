using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AguasNico.Migrations
{
    /// <inheritdoc />
    public partial class CartAbonoProduct : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_AbonoRenewalProducts",
                table: "AbonoRenewalProducts");

            migrationBuilder.DropColumn(
                name: "Quantity",
                table: "AbonoRenewalProducts");

            migrationBuilder.AddColumn<long>(
                name: "ID",
                table: "AbonoRenewalProducts",
                type: "bigint",
                nullable: false,
                defaultValue: 0L)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AbonoRenewalProducts",
                table: "AbonoRenewalProducts",
                column: "ID");

            migrationBuilder.CreateTable(
                name: "CartAbonoProducts",
                columns: table => new
                {
                    CartID = table.Column<long>(type: "bigint", nullable: false),
                    AbonoRenewalProductID = table.Column<long>(type: "bigint", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CartAbonoProducts", x => new { x.CartID, x.AbonoRenewalProductID });
                    table.ForeignKey(
                        name: "FK_CartAbonoProducts_AbonoRenewalProducts_AbonoRenewalProductID",
                        column: x => x.AbonoRenewalProductID,
                        principalTable: "AbonoRenewalProducts",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CartAbonoProducts_Carts_CartID",
                        column: x => x.CartID,
                        principalTable: "Carts",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AbonoRenewalProducts_AbonoRenewalID",
                table: "AbonoRenewalProducts",
                column: "AbonoRenewalID");

            migrationBuilder.CreateIndex(
                name: "IX_CartAbonoProducts_AbonoRenewalProductID",
                table: "CartAbonoProducts",
                column: "AbonoRenewalProductID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CartAbonoProducts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AbonoRenewalProducts",
                table: "AbonoRenewalProducts");

            migrationBuilder.DropIndex(
                name: "IX_AbonoRenewalProducts_AbonoRenewalID",
                table: "AbonoRenewalProducts");

            migrationBuilder.DropColumn(
                name: "ID",
                table: "AbonoRenewalProducts");

            migrationBuilder.AddColumn<int>(
                name: "Quantity",
                table: "AbonoRenewalProducts",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_AbonoRenewalProducts",
                table: "AbonoRenewalProducts",
                columns: new[] { "AbonoRenewalID", "Type" });
        }
    }
}
