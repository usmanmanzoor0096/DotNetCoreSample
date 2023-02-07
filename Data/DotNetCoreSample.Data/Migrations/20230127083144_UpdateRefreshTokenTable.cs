using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AuthService.Data.Migrations
{
    public partial class UpdateRefreshTokenTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ClientId",
                table: "RefreshToken",
                newName: "UserId");

           
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
           
            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "RefreshToken",
                newName: "ClientId");

        }
    }
}
