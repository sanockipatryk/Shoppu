using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Shoppu.Infrastructure.Persistence.Migrations
{
    public partial class AddSizeTypes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Sizes_ProductCategories_ProductCategoryId",
                table: "Sizes");

            migrationBuilder.RenameColumn(
                name: "ProductCategoryId",
                table: "Sizes",
                newName: "SizeTypeId");

            migrationBuilder.RenameIndex(
                name: "IX_Sizes_ProductCategoryId",
                table: "Sizes",
                newName: "IX_Sizes_SizeTypeId");

            migrationBuilder.AddColumn<int>(
                name: "SizeTypeId",
                table: "Products",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "SizeTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SizeTypes", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Products_SizeTypeId",
                table: "Products",
                column: "SizeTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_SizeTypes_SizeTypeId",
                table: "Products",
                column: "SizeTypeId",
                principalTable: "SizeTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Sizes_SizeTypes_SizeTypeId",
                table: "Sizes",
                column: "SizeTypeId",
                principalTable: "SizeTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_SizeTypes_SizeTypeId",
                table: "Products");

            migrationBuilder.DropForeignKey(
                name: "FK_Sizes_SizeTypes_SizeTypeId",
                table: "Sizes");

            migrationBuilder.DropTable(
                name: "SizeTypes");

            migrationBuilder.DropIndex(
                name: "IX_Products_SizeTypeId",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "SizeTypeId",
                table: "Products");

            migrationBuilder.RenameColumn(
                name: "SizeTypeId",
                table: "Sizes",
                newName: "ProductCategoryId");

            migrationBuilder.RenameIndex(
                name: "IX_Sizes_SizeTypeId",
                table: "Sizes",
                newName: "IX_Sizes_ProductCategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Sizes_ProductCategories_ProductCategoryId",
                table: "Sizes",
                column: "ProductCategoryId",
                principalTable: "ProductCategories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
