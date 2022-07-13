using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FinalProjectBackEnd.Migrations
{
    public partial class v6 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Visibility",
                table: "Posts",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Marks",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "Marks",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "Marks",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdatedBy",
                table: "Marks",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "d6770d41-e5cb-4fcd-b671-db0ed5788aa8", "AQAAAAEAACcQAAAAECJVeoa2qI3vv+OoV1/adlsKQ0Shyryhu/MBJ2HeO83y818qDy+nYGlXjwzH5TRRZw==", "e468d2e7-b681-46be-93c0-e6685257642d" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "2",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "aa16f29a-c90d-494b-a3ed-098c04bddc52", "AQAAAAEAACcQAAAAEBV1bb7rFpcMQYOQgiZpTW8JLX5QNQSOYdoLgkRjrn3u3fmGyYs63aD8c9uNICRYtg==", "9384120b-29e9-4d71-9be8-f95a2329a302" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Visibility",
                table: "Posts");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Marks");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Marks");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "Marks");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "Marks");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "bf8512c8-580e-4d10-b502-695e500a1fa5", "AQAAAAEAACcQAAAAEBcATD2wv5c2+YwYPHn51vHdB5RwWlHoU3ScV3l9wLp7NDMjFJTuU2PVFu/y4QViXg==", "d39604aa-5513-4389-91d6-7b6f7ea0a066" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "2",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "a0049013-36cf-4ab2-9423-dcc7563683c7", "AQAAAAEAACcQAAAAEAri4jeBXU9mv17M0QNewJvD/rRkSDL3yingtSkjPVmxwcnUr3elTRK8tLPQ2jYIfg==", "7593391a-ca1d-40d8-a83c-07adaf552bd0" });
        }
    }
}
