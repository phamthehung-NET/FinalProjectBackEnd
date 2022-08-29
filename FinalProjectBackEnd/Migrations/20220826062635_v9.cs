using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FinalProjectBackEnd.Migrations
{
    public partial class v9 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "GraduateYear",
                table: "UserInfos",
                type: "int",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "MarkHistories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MarkId = table.Column<int>(type: "int", nullable: true),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Priority = table.Column<int>(type: "int", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ReducedMark = table.Column<decimal>(type: "decimal(18,2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MarkHistories", x => x.Id);
                });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "c897615c-34d0-4b19-a4bd-ca86c344678b", "AQAAAAEAACcQAAAAEOpb8bJLKarnGAwawZhl5VaVip364WByZTZWqthNmfA4qyPxgbFqPJak6XkPqhbYhg==", "68ec687d-0329-40fe-8614-af9c2fc0383c" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "2",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "12471d38-4229-4608-a537-6e4669d703bc", "AQAAAAEAACcQAAAAENmKCf1XjSrXfqskyuUBreN2X6P6E4lcsxEQDFh84v3INrWT7GK7u/XyNijxjwGREA==", "5ed154fd-a29b-4436-ba33-b8ec4d657b4a" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MarkHistories");

            migrationBuilder.AlterColumn<string>(
                name: "GraduateYear",
                table: "UserInfos",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "79d6bdfe-a60d-43a6-b741-b9b61f9a7977", "AQAAAAEAACcQAAAAEE7zGKTiXDLzMnBpTBXLqYVkUJRk1p+43R6JxLJml684NmdpcVs9YHfp+o55W98AbA==", "9a0ef694-06a1-4ad8-a258-9a151fbcf2b5" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "2",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "315ab927-a13d-4994-b58a-9e713ae06eb7", "AQAAAAEAACcQAAAAEG6f3bBfoA53lUwTcMs4BbDD12MQehBg6gRN/i2GPJDy/12MjwGlYN5QMapgxtdP1A==", "573fcb1b-8c37-4572-b85a-f0ab8eff37b7" });
        }
    }
}
