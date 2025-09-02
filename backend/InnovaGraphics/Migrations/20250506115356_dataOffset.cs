using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InnovaGraphics.Migrations
{
    /// <inheritdoc />
    public partial class dataOffset : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "Expires",
                table: "TokenManager",
                type: "datetimeoffset",
                nullable: false,
                oldClrType: typeof(TimeOnly),
                oldType: "time");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<TimeOnly>(
                name: "Expires",
                table: "TokenManager",
                type: "time",
                nullable: false,
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetimeoffset");
        }
    }
}
