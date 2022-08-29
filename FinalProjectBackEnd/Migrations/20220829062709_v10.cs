using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FinalProjectBackEnd.Migrations
{
    public partial class v10 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<double>(
                name: "Mark",
                table: "Marks",
                type: "float",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)",
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "ReducedMark",
                table: "MarkHistories",
                type: "float",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)",
                oldNullable: true);

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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "Mark",
                table: "Marks",
                type: "decimal(18,2)",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "float",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "ReducedMark",
                table: "MarkHistories",
                type: "decimal(18,2)",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "float",
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "c897615c-34d0-4b19-a4bd-ca86c344678b", "AQAAAAEAACcQAAAAEOpb8bJLKarnGAwawZhl5VaVip364WByZTZWqthNmfA4qyPxgbFqPJak6XkPqhbYhg==", "68ec687d-0329-40fe-8614-af9c2fc0383c" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "2",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "12471d38-4229-4608-a537-6e4669d703bc", "AQAAAAEAACcQAAAAENmKCf1XjSrXfqskyuUBreN2X6P6E4lcsxEQDFh84v3INrWT7GK7u/XyNijxjwGREA==", "5ed154fd-a29b-4436-ba33-b8ec4d657b4a" });
        }
    }
}
