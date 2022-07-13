using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FinalProjectBackEnd.Migrations
{
    public partial class v3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Notifications",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreateAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    AuthorId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PostId = table.Column<int>(type: "int", nullable: true),
                    CommentId = table.Column<int>(type: "int", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: true),
                    Link = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notifications", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserFollows",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FollowerId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FolloweeId = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserFollows", x => x.Id);
                });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "d58d9783-b1e2-46c5-be40-977f68fd15af", "AQAAAAEAACcQAAAAENQA1AjXWz869Evt9rMW9By7ULMKDhk+c4sBd4B61WQke6XRLbWCWDIo+0pD4yj0/Q==", "8ca6c483-1d53-4c5a-99db-3ed933e4308a" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "2",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "ac7c8450-4c3f-4353-9000-d88b9175ab1a", "AQAAAAEAACcQAAAAEI2lxmXVS4hkX037RflmJzvbBQaMvvIVFts1w33CKZiYGsJM/3v2xyXnYaNFfol53w==", "8c1d01e1-844d-4842-971e-720a1d17c900" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Notifications");

            migrationBuilder.DropTable(
                name: "UserFollows");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "764fe516-d0d0-4079-9a9a-4898d657072c", "AQAAAAEAACcQAAAAEAP4IW7J0xMjeyyhfGby8ReX01t1Xc3wVDg/gJEV7Q1fInx/hDun73oZyN0iUjkIow==", "c72d47e3-66f3-41a8-a3f4-ba7beabfdcfd" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "2",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "95316c6a-06a1-4eb0-bb39-0fc9db358515", "AQAAAAEAACcQAAAAEKkCi7/SFkYnMZrgOPICTW8B4iqAMwaYUc14IYw45ePahrXoRAJogVbQR8Br3iIKBg==", "b562ae9a-1da6-4b05-abd1-c9757f1f4877" });
        }
    }
}
