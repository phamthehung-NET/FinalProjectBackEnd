using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FinalProjectBackEnd.Migrations
{
    public partial class v4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "b1625f29-121d-456b-a4be-5e3035fcedee", "AQAAAAEAACcQAAAAEJwzrdY3q1KfM6KeQNXdpHdkmL4PNK2cuLi+ST7ra3xlYoLkpsXObwmKFNLMjKtqvQ==", "d98cc223-411e-4e12-b121-e005891369db" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "2",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "9e6714d8-d462-4159-b01b-3ca1a043732b", "AQAAAAEAACcQAAAAEKCvA5rbPaUMxkYrMkWHHf/2tbXOlzXrljjQrlAKs0H2EIe/nTm3M6ySDMvplbseEA==", "9596cc84-b4b5-464d-9756-80634772c249" });

            migrationBuilder.UpdateData(
                table: "UserInfos",
                keyColumn: "Id",
                keyValue: 1,
                column: "FullName",
                value: "Principal");

            migrationBuilder.UpdateData(
                table: "UserInfos",
                keyColumn: "Id",
                keyValue: 2,
                column: "FullName",
                value: "Vice-principal");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
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

            migrationBuilder.UpdateData(
                table: "UserInfos",
                keyColumn: "Id",
                keyValue: 1,
                column: "FullName",
                value: null);

            migrationBuilder.UpdateData(
                table: "UserInfos",
                keyColumn: "Id",
                keyValue: 2,
                column: "FullName",
                value: null);
        }
    }
}
