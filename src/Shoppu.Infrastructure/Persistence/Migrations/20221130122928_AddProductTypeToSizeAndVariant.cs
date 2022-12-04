using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Shoppu.Infrastructure.Persistence.Migrations
{
    public partial class AddProductTypeToSizeAndVariant : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ProductTypeId",
                table: "Sizes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Sizes_ProductTypeId",
                table: "Sizes",
                column: "ProductTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Sizes_ProductTypes_ProductTypeId",
                table: "Sizes",
                column: "ProductTypeId",
                principalTable: "ProductTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Sizes_ProductTypes_ProductTypeId",
                table: "Sizes");

            migrationBuilder.DropIndex(
                name: "IX_Sizes_ProductTypeId",
                table: "Sizes");

            migrationBuilder.DropColumn(
                name: "ProductTypeId",
                table: "Sizes");
        }
    }
}
