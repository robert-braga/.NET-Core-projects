using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OrdersManager.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class init3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Orders",
                keyColumn: "OrderId",
                keyValue: new Guid("735886c0-faf3-49ca-9776-8a20b756f1cb"),
                column: "OrderDate",
                value: new DateTime(2025, 2, 7, 8, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "Orders",
                keyColumn: "OrderId",
                keyValue: new Guid("f4816224-70d6-4491-ac52-34f298ace16f"),
                column: "OrderDate",
                value: new DateTime(2025, 2, 6, 8, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Orders",
                keyColumn: "OrderId",
                keyValue: new Guid("735886c0-faf3-49ca-9776-8a20b756f1cb"),
                column: "OrderDate",
                value: new DateTime(2025, 2, 26, 12, 34, 52, 591, DateTimeKind.Local).AddTicks(6257));

            migrationBuilder.UpdateData(
                table: "Orders",
                keyColumn: "OrderId",
                keyValue: new Guid("f4816224-70d6-4491-ac52-34f298ace16f"),
                column: "OrderDate",
                value: new DateTime(2025, 2, 26, 12, 34, 52, 589, DateTimeKind.Local).AddTicks(9146));
        }
    }
}
