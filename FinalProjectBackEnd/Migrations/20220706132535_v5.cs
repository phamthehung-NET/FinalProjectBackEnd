using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FinalProjectBackEnd.Migrations
{
    public partial class v5 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Type",
                table: "Notifications",
                type: "int",
                nullable: true);

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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Type",
                table: "Notifications");

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
        }
    }
}
