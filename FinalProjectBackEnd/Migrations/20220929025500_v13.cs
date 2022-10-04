using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FinalProjectBackEnd.Migrations
{
    public partial class v13 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "EvidenceLink",
                table: "MarkHistories",
                type: "nvarchar(max)",
                nullable: true);

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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EvidenceLink",
                table: "MarkHistories");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "37a866dc-b887-472d-9c60-03e5c1f39108", "AQAAAAEAACcQAAAAEKcnX1rYOauogFm0mCfu+/oAwQKlN7KOU7JCQDR2yb2vg0Iclwc4BDTZJK/5jBHLGQ==", "75357a44-6d09-485b-a76d-21430d1f3625" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "2",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "710ae7c7-909f-463b-ad41-621c0980af4c", "AQAAAAEAACcQAAAAEJI6NYYf1dJra2t6fNu7PTf2MWrKWHhsJwN+mWwJytg6qcYGGL6mqK99nWAOd1OK5g==", "68ec1f6e-c7bc-4b6a-845b-6c9c377f4a31" });
        }
    }
}
