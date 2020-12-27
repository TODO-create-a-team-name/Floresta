using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Floresta.Migrations
{
    public partial class changenewsmodel2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageByte",
                table: "News");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "ImageByte",
                table: "News",
                type: "varbinary(max)",
                nullable: true);
        }
    }
}
