using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InnovaGraphics.Migrations
{
    /// <inheritdoc />
    public partial class ShopItemTypefieldinShopItementity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.AlterColumn<int>(
                name: "Type",
                table: "ShopItem",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Material",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserTest");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "Material");

            migrationBuilder.AlterColumn<string>(
                name: "Type",
                table: "ShopItem",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "Name",
                table: "Material",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<string>(
                name: "ImagePath",
                table: "Certificate",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "LecturerName",
                table: "Certificate",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
