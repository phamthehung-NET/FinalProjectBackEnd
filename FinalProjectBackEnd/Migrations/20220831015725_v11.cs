using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FinalProjectBackEnd.Migrations
{
    public partial class v11 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsFirstLogin",
                table: "UserInfos",
                type: "bit",
                nullable: true);

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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsFirstLogin",
                table: "UserInfos");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "61baf01f-ea54-4781-9daa-28944ad1085a", "AQAAAAEAACcQAAAAEOgqx+K/lmiTwdOd9K86KftYzZqunDPUpifKB9IUfhHkowANY4J5bQuZ/COU/B+Yfw==", "d5953552-370a-432f-aedf-e8d134120ff5" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "2",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "89f55e06-5a30-4f19-a058-94fad47fc544", "AQAAAAEAACcQAAAAEFNUgAwbJfEJUdxPEtryH2QS6YuSKg+WINyhgfNEmUge5AO7bdwKIjkhWOsKPOEV+w==", "7fd7a640-73c0-481b-9e74-a8e3e6037be4" });
        }
    }
}
