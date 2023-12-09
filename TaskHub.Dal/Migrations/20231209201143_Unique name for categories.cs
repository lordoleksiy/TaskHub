using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TaskHub.Dal.Migrations
{
    public partial class Uniquenameforcategories : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("531dd788-09d6-461c-aea5-af0c4236ed0c"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("9758814a-ad76-4a5d-8843-ec352a2577cc"));

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { new Guid("44753e1a-3681-4855-b196-86ed66a16b9a"), "8014ea9a-3f38-4cb4-8b1c-118536588a7a", "User", "USER" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { new Guid("6aec0ddf-ca0d-42b4-8c85-164f3b6fd024"), "019b945b-9800-4239-b333-650233ffa0c6", "Admin", "ADMIN" });

            migrationBuilder.CreateIndex(
                name: "IX_Categories_Name",
                table: "Categories",
                column: "Name",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Categories_Name",
                table: "Categories");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("44753e1a-3681-4855-b196-86ed66a16b9a"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("6aec0ddf-ca0d-42b4-8c85-164f3b6fd024"));

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { new Guid("531dd788-09d6-461c-aea5-af0c4236ed0c"), "b452b1fc-6894-402f-9df7-87542e26f70a", "Admin", "ADMIN" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { new Guid("9758814a-ad76-4a5d-8843-ec352a2577cc"), "69b959c4-9290-4be6-af79-13f61b0a5eb5", "User", "USER" });
        }
    }
}
