using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FinalProjectBackEnd.Migrations
{
    public partial class v16 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "GroupId",
                table: "Posts",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Groups",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClassId = table.Column<int>(type: "int", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Groups", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserAndGroups",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    GroupId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserAndGroups", x => x.Id);
                });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "b240bf91-df31-4331-8303-8b69380a5424", "AQAAAAEAACcQAAAAEGMaA6p62c49zfhnaGkCAk8Mm5GDFAaMoR7dRvkx4AT9jJ295AL4StMQs3rAeqYIWg==", "810ab967-b88e-4527-a22c-68ef761a926d" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "2",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "d8d08913-1dc3-4968-8cb4-a9f07fb6df11", "AQAAAAEAACcQAAAAEKEWXGMErE6xvoNonCNIe52AdcDBO3s9qIp6cMw8sk5Rf+XzWmeFDFb0qL8j9Gyv0A==", "e0a29443-ea29-4ff7-9976-751f038f1ac2" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Groups");

            migrationBuilder.DropTable(
                name: "UserAndGroups");

            migrationBuilder.DropColumn(
                name: "GroupId",
                table: "Posts");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "8872e0b1-73c1-4b86-a4d6-34cb5dd61f3f", "AQAAAAEAACcQAAAAEN2SQmfY8+UCDG5z7rfZHB09rAJtNedDBVtJVvPHGMXAWXb7UK1lf5an7ol787muGw==", "8da6de25-9c01-433f-95a6-95ad043dce2d" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "2",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "cc243897-eff1-4423-aa68-a1c38dab1948", "AQAAAAEAACcQAAAAEELECRN4aAFoRhQA/D4iw4o3tKL5nmOo1yt0TMRb3/iR+vvthdESIAEgWfgzpCdk5g==", "928e00f8-d6d0-48d5-8f41-4770b13c9ffb" });
        }
    }
}
