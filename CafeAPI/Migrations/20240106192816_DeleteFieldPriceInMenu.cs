using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CafeAPI.Migrations
{
    public partial class DeleteFieldPriceInMenu : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GiaBan",
                table: "Mon");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "GiaBan",
                table: "Mon",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }
    }
}
