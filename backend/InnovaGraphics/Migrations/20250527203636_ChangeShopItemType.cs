using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InnovaGraphics.Migrations
{
    public partial class ChangeShopItemType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // 1. Якщо необхідно — змінити стовпець на nullable int
            migrationBuilder.AlterColumn<int>(
                name: "Type",
                table: "ShopItem",
                type: "int",
                nullable: true);

            // (Тут можна додати SQL скрипт, щоб оновити дані у колонці через migrationBuilder.Sql)

            // 2. Після оновлення даних встановити NOT NULL
            migrationBuilder.AlterColumn<int>(
                name: "Type",
                table: "ShopItem",
                type: "int",
                nullable: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Type",
                table: "ShopItem",
                type: "nvarchar(max)",
                nullable: false);
        }

    }
}
