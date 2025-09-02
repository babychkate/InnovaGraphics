using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InnovaGraphics.Migrations
{
    /// <inheritdoc />
    public partial class ChangeUserandProfileConfig : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Profile_AvatarId",
                table: "Profile");

            migrationBuilder.CreateIndex(
                name: "IX_Profile_AvatarId",
                table: "Profile",
                column: "AvatarId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Profile_AvatarId",
                table: "Profile");

            migrationBuilder.CreateIndex(
                name: "IX_Profile_AvatarId",
                table: "Profile",
                column: "AvatarId",
                unique: true,
                filter: "[AvatarId] IS NOT NULL");
        }
    }
}
