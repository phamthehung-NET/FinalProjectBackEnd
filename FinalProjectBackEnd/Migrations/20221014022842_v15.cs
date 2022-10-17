using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FinalProjectBackEnd.Migrations
{
    public partial class v15 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "8872e0b1-73c1-4b86-a4d6-34cb5dd61f3f", "AQAAAAEAACcQAAAAEN2SQmfY8+UCDG5z7rfZHB09rAJtNedDBVtJVvPHGMXAWXb7UK1lf5an7ol787muGw==", "8da6de25-9c01-433f-95a6-95ad043dce2d" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "2",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "cc243897-eff1-4423-aa68-a1c38dab1948", "AQAAAAEAACcQAAAAEELECRN4aAFoRhQA/D4iw4o3tKL5nmOo1yt0TMRb3/iR+vvthdESIAEgWfgzpCdk5g==", "928e00f8-d6d0-48d5-8f41-4770b13c9ffb" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
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
    }
}
