using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CafeAPI.Migrations
{
    public partial class Modify : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ChiTietGia_Mon_MaMon",
                table: "ChiTietGia");

            migrationBuilder.DropForeignKey(
                name: "FK_ChiTietHoaDon_HoaDon_SoHoaDon",
                table: "ChiTietHoaDon");

            migrationBuilder.DropForeignKey(
                name: "FK_ChiTietHoaDon_Mon_MaMon",
                table: "ChiTietHoaDon");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ChiTietHoaDon",
                table: "ChiTietHoaDon");

            migrationBuilder.DropIndex(
                name: "IX_ChiTietHoaDon_MaMon",
                table: "ChiTietHoaDon");

            migrationBuilder.AlterColumn<string>(
                name: "MaMon",
                table: "ChiTietHoaDon",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldMaxLength: 20);

            migrationBuilder.AddColumn<int>(
                name: "ID",
                table: "ChiTietHoaDon",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<string>(
                name: "Size",
                table: "ChiTietHoaDon",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_ChiTietHoaDon",
                table: "ChiTietHoaDon",
                column: "ID");

            migrationBuilder.CreateTable(
                name: "ChiTietTopping",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false),
                    TenTopping = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChiTietTopping", x => new { x.ID, x.TenTopping });
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ChiTietTopping");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ChiTietHoaDon",
                table: "ChiTietHoaDon");

            migrationBuilder.DropColumn(
                name: "ID",
                table: "ChiTietHoaDon");

            migrationBuilder.DropColumn(
                name: "Size",
                table: "ChiTietHoaDon");

            migrationBuilder.AlterColumn<string>(
                name: "MaMon",
                table: "ChiTietHoaDon",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldMaxLength: 20,
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_ChiTietHoaDon",
                table: "ChiTietHoaDon",
                columns: new[] { "SoHoaDon", "MaMon" });

            migrationBuilder.CreateIndex(
                name: "IX_ChiTietHoaDon_MaMon",
                table: "ChiTietHoaDon",
                column: "MaMon");

            migrationBuilder.AddForeignKey(
                name: "FK_ChiTietGia_Mon_MaMon",
                table: "ChiTietGia",
                column: "MaMon",
                principalTable: "Mon",
                principalColumn: "MaMon",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ChiTietHoaDon_HoaDon_SoHoaDon",
                table: "ChiTietHoaDon",
                column: "SoHoaDon",
                principalTable: "HoaDon",
                principalColumn: "SoHoaDon",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ChiTietHoaDon_Mon_MaMon",
                table: "ChiTietHoaDon",
                column: "MaMon",
                principalTable: "Mon",
                principalColumn: "MaMon",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
