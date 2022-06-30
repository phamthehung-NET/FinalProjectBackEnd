using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FinalProjectBackEnd.Migrations
{
    public partial class v2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

            migrationBuilder.InsertData(
                table: "UserInfos",
                columns: new[] { "Id", "Address", "Avatar", "DoB", "EndDate", "FullName", "GraduateYear", "IsDeleted", "SchoolYear", "StartDate", "Status", "StudentRole", "UserId" },
                values: new object[,]
                {
                    { 1, null, null, null, null, null, null, null, null, null, null, null, "1" },
                    { 2, null, null, null, null, null, null, null, null, null, null, null, "2" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "UserInfos",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "UserInfos",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "abef532a-786c-4ec4-856b-c286df0d7c59", "AQAAAAEAACcQAAAAEALrq9Zc1wNSpctAsT1IUobNvu4rTHUGIqcWQWJ8rwaWNy0sZHyfm2cA1UkMu2yc9A==", "b44b495d-4f74-4790-be3b-42dd0ffd1462" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "2",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "2a244981-d923-46d8-85c3-8b5c364874b1", "AQAAAAEAACcQAAAAEMaj0j9DTS2kPJWSS5tyMKdPLPeXgdnmttNxlt8PylPPMnBexquIJ97Sq1DY0E58cw==", "b234a01e-9b0f-4fa7-8928-e797bc8d7cd8" });
        }
    }
}
