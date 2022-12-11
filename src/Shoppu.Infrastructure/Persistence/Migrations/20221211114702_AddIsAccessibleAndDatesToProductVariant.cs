using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Shoppu.Infrastructure.Persistence.Migrations
{
    public partial class AddIsAccessibleAndDatesToProductVariant : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DateCreated",
                table: "ProductVariants",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "DateSetAsAccessible",
                table: "ProductVariants",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsAccessible",
                table: "ProductVariants",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DateCreated",
                table: "ProductVariants");

            migrationBuilder.DropColumn(
                name: "DateSetAsAccessible",
                table: "ProductVariants");

            migrationBuilder.DropColumn(
                name: "IsAccessible",
                table: "ProductVariants");
        }
    }
}
