using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FinalProjectBackEnd.Migrations
{
    public partial class v2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsRedStar",
                table: "UserInfos");

            migrationBuilder.AddColumn<int>(
                name: "StudentRole",
                table: "UserInfos",
                type: "int",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "8374e823-7b96-4870-9602-e99455913cc3", "AQAAAAEAACcQAAAAEDvpxe9TZMBJ8wCWBm/4sPP6HN54BNuiSv0vlxXKP0ehfJqPR/aO1PcHVJS/VWQbOw==", "46a46775-1d32-4f0e-9431-09228a5d45c6" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "2",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "36ab8853-9fcc-4f8a-93ec-7a7ec9a7b784", "AQAAAAEAACcQAAAAEIGbNmFrdIlhI0DdTpPHrPuFV4LbCwz4E+yp3Enjd2+MmpBq61ArngXm+zNUSVlrrw==", "d49d3041-1ece-461f-9aa4-ebbfff2ba5b5" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "StudentRole",
                table: "UserInfos");

            migrationBuilder.AddColumn<bool>(
                name: "IsRedStar",
                table: "UserInfos",
                type: "bit",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "3b1ac964-21f8-4912-a870-d963f704db36", "AQAAAAEAACcQAAAAEERChIy6tqlOLecgxpdUi02YBPUZSrPL8nbeUy9UWvw4CazlHJh1jHWD+NDKTMqraQ==", "f3659d39-f0f9-4d49-b0f6-40bb68e16b7b" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "2",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "3e5b29e2-741b-439c-9771-0f6d102ef7d0", "AQAAAAEAACcQAAAAEDj9JX6lysq/j8GifnpMBBLNvcdLFxTLIyuG1D2B5RagkJAnan7THZaU76n+X02/Vw==", "14acbdfd-80bb-4a43-8923-7e6d21ee35af" });
        }
    }
}
