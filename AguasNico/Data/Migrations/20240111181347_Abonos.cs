using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AguasNico.Migrations
{
    /// <inheritdoc />
    public partial class Abonos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Abonos",
                columns: table => new
                {
                    ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Price = table.Column<decimal>(type: "money", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Abonos", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "AbonoProducts",
                columns: table => new
                {
                    AbonoID = table.Column<long>(type: "bigint", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AbonoProducts", x => new { x.AbonoID, x.Type });
                    table.ForeignKey(
                        name: "FK_AbonoProducts_Abonos_AbonoID",
                        column: x => x.AbonoID,
                        principalTable: "Abonos",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ClientAbonos",
                columns: table => new
                {
                    ClientID = table.Column<long>(type: "bigint", nullable: false),
                    AbonoID = table.Column<long>(type: "bigint", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ProductID = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClientAbonos", x => new { x.ClientID, x.AbonoID });
                    table.ForeignKey(
                        name: "FK_ClientAbonos_Abonos_AbonoID",
                        column: x => x.AbonoID,
                        principalTable: "Abonos",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ClientAbonos_Products_ProductID",
                        column: x => x.ProductID,
                        principalTable: "Products",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ClientAbonos_AbonoID",
                table: "ClientAbonos",
                column: "AbonoID");

            migrationBuilder.CreateIndex(
                name: "IX_ClientAbonos_ProductID",
                table: "ClientAbonos",
                column: "ProductID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AbonoProducts");

            migrationBuilder.DropTable(
                name: "ClientAbonos");

            migrationBuilder.DropTable(
                name: "Abonos");
        }
    }
}
