using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Shoppu.Infrastructure.Persistence.Migrations
{
    public partial class AddSpecificGenderToProductCategory : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SpecificGender",
                table: "ProductCategories",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SpecificGender",
                table: "ProductCategories");
        }
    }
}
