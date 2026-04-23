using AguasNico.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AguasNico.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20260423123000_Client_Email")]
    public partial class Client_Email : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Clients",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Email",
                table: "Clients");
        }

        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            new SnapshotAccessor().Populate(modelBuilder);
        }

        private sealed class SnapshotAccessor : ApplicationDbContextModelSnapshot
        {
            public void Populate(ModelBuilder modelBuilder)
            {
                BuildModel(modelBuilder);
            }
        }
    }
}
