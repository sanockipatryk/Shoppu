using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Shoppu.Infrastructure.Persistence.Migrations
{
    public partial class FixProductVariantIdInProductVariantSize : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductVariantSizes_ProductVariants_ProductVariantId",
                table: "ProductVariantSizes");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductVariantSizes_Variants_VariantId",
                table: "ProductVariantSizes");

            migrationBuilder.DropIndex(
                name: "IX_ProductVariantSizes_VariantId",
                table: "ProductVariantSizes");

            migrationBuilder.DropColumn(
                name: "VariantId",
                table: "ProductVariantSizes");

            migrationBuilder.AlterColumn<int>(
                name: "ProductVariantId",
                table: "ProductVariantSizes",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductVariantSizes_ProductVariants_ProductVariantId",
                table: "ProductVariantSizes",
                column: "ProductVariantId",
                principalTable: "ProductVariants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductVariantSizes_ProductVariants_ProductVariantId",
                table: "ProductVariantSizes");

            migrationBuilder.AlterColumn<int>(
                name: "ProductVariantId",
                table: "ProductVariantSizes",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "VariantId",
                table: "ProductVariantSizes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_ProductVariantSizes_VariantId",
                table: "ProductVariantSizes",
                column: "VariantId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductVariantSizes_ProductVariants_ProductVariantId",
                table: "ProductVariantSizes",
                column: "ProductVariantId",
                principalTable: "ProductVariants",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductVariantSizes_Variants_VariantId",
                table: "ProductVariantSizes",
                column: "VariantId",
                principalTable: "Variants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
