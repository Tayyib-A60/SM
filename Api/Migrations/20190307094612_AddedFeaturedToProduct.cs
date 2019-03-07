using Microsoft.EntityFrameworkCore.Migrations;

namespace SkineroMotors.Migrations
{
    public partial class AddedFeaturedToProduct : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Featured",
                table: "Vehicles",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Featured",
                table: "Vehicles");
        }
    }
}
