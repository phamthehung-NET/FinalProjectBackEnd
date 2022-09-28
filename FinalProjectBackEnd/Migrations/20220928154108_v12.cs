using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FinalProjectBackEnd.Migrations
{
    public partial class v12 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "RelatedId",
                table: "MarkHistories",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RelatedType",
                table: "MarkHistories",
                type: "int",
                nullable: true);

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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RelatedId",
                table: "MarkHistories");

            migrationBuilder.DropColumn(
                name: "RelatedType",
                table: "MarkHistories");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "7df7e1cf-5ba0-45c9-badb-e40bb4b22ea5", "AQAAAAEAACcQAAAAEJOTlvmE97VbiwNFlfMqaKaXlg3r0K41IKPIogktY/qkNaYjyOj2JOvGWzpjqBDpIA==", "10b6b60c-fd88-44d9-b2d4-b85caaca3671" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "2",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "3be17a2f-a27b-462a-a164-652927a422df", "AQAAAAEAACcQAAAAEHpKIl90EapV2AvvjeqU08XiWFNFDInlN88PG4fHpu+LnYyr29kwCOiR1H1x+UMG2g==", "e5a196b0-c7ae-406f-9b29-3c3b579504cb" });
        }
    }
}
