using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FinalProjectBackEnd.Migrations
{
    public partial class v8 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "GroupChats",
                type: "nvarchar(max)",
                nullable: true);

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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "GroupChats");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "5641406e-27d3-4612-aae1-91a9c55e45a4", "AQAAAAEAACcQAAAAEOr1D0xkURYj+Me3pmtY2DZiwEHk1SGvTU5XdNBqAF9eYwosbYRwimDoZaKrO3fHbw==", "ef8184d0-365d-4730-98db-7ba8ae61b085" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "2",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "170f9288-5dff-4483-9d12-6108438c48bf", "AQAAAAEAACcQAAAAEI+6JIymL9LHrgGHRwGY+co3wk2qzrlfGa11sAvd0S2e6Zshqnk7DDS4W766LbN4vg==", "4d7883b4-b237-4437-b70d-6f0409b0f76d" });
        }
    }
}
