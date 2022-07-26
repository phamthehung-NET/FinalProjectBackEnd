using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FinalProjectBackEnd.Migrations
{
    public partial class v7 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ConversationId",
                table: "Messages",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Conversations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    User1Id = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    User2Id = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Conversations", x => x.Id);
                });

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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Conversations");

            migrationBuilder.DropColumn(
                name: "ConversationId",
                table: "Messages");

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
    }
}
