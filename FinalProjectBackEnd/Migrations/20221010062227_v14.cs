using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FinalProjectBackEnd.Migrations
{
    public partial class v14 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "MarkHistories",
                type: "bit",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "e8fa80df-6a3d-4fd9-8792-2182d46ffb3a", "AQAAAAEAACcQAAAAEIP/1rWXqzBbASACVxIgNUy5qKhd5lJ6Ok39USqeBgf5i0ArNiavVTWW/Moj01HJgA==", "99892d38-d94a-405a-a658-cd5a5e2beb88" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "2",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "876685c5-b506-43b6-a811-ca92404cf62a", "AQAAAAEAACcQAAAAEESNW7xtor3NMfhWQzw/kCqIUV7jMqCRqrW1WdOgMbO4lMOD5pw+WBrUmI33OTUQNg==", "37bc8c0c-c96e-4a7c-bba7-f3d2f9cf2abd" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "MarkHistories");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "26ece6c4-73b8-42ac-8328-97595d48bd01", "AQAAAAEAACcQAAAAEM/Ap+RUmES1Es8QglXUd7BnWsJuaWlPsKkreGywnQNMuQryVMmrXelPem7uzBUeVw==", "3a837f60-335b-45b6-a937-d3a9318b121a" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "2",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "1b30ee58-7a4f-485c-843a-194fe89df762", "AQAAAAEAACcQAAAAEABZ2wk1S1R+Lc69HqKovgCjlo/GyDRnUdHBxMWVJtKcKmaAbaARAUe5XLBMXwKHQw==", "7b693878-8c00-4ce6-b057-a341cd1bf0bf" });
        }
    }
}
