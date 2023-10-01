using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Service.Domain.Migrations
{
    /// <inheritdoc />
    public partial class DeliveryAdd : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Delivery_Address",
                table: "Orders",
                type: "nvarchar(150)",
                maxLength: 150,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "Delivery_DeliveryServiceId",
                table: "Orders",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<DateTime>(
                name: "DateChange",
                table: "OrderChangeStatusLogs",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "GETDATE()",
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValueSql: "GATDATE()");

            migrationBuilder.CreateTable(
                name: "DeliveryServices",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DeliveryName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeliveryServices", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "DeliveryServices",
                columns: new[] { "Id", "DeliveryName" },
                values: new object[,]
                {
                    { 1, "Nova Poshta" },
                    { 2, "Ukr Poshta" },
                    { 3, "Meest Express" },
                    { 4, "Samoviviz" }
                });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "PasswordHash", "PasswordSalt" },
                values: new object[] { new byte[] { 52, 55, 155, 136, 186, 220, 138, 160, 241, 88, 246, 68, 19, 226, 178, 63, 242, 123, 223, 14, 168, 124, 8, 185, 42, 235, 186, 245, 129, 200, 25, 93, 164, 11, 87, 145, 183, 177, 241, 34, 215, 119, 167, 96, 10, 253, 227, 22, 190, 62, 35, 161, 145, 240, 125, 7, 150, 238, 150, 141, 53, 6, 171, 123 }, new byte[] { 173, 5, 65, 66, 199, 57, 159, 41, 213, 225, 63, 201, 21, 78, 130, 72, 222, 133, 67, 144, 107, 43, 29, 70, 114, 225, 201, 56, 149, 241, 170, 101, 133, 114, 161, 95, 51, 88, 226, 33, 78, 182, 237, 131, 43, 205, 31, 178, 238, 82, 169, 0, 67, 145, 1, 44, 206, 49, 171, 222, 214, 141, 103, 153, 188, 193, 0, 125, 44, 17, 159, 160, 65, 122, 66, 112, 155, 228, 28, 203, 217, 127, 217, 247, 21, 216, 104, 144, 227, 41, 63, 85, 37, 120, 15, 200, 25, 1, 151, 230, 229, 238, 203, 166, 174, 23, 30, 121, 102, 94, 231, 105, 248, 91, 113, 135, 182, 56, 24, 37, 131, 29, 107, 159, 34, 199, 73, 207 } });

            migrationBuilder.CreateIndex(
                name: "IX_Orders_Delivery_DeliveryServiceId",
                table: "Orders",
                column: "Delivery_DeliveryServiceId");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_DeliveryServices_Delivery_DeliveryServiceId",
                table: "Orders",
                column: "Delivery_DeliveryServiceId",
                principalTable: "DeliveryServices",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_DeliveryServices_Delivery_DeliveryServiceId",
                table: "Orders");

            migrationBuilder.DropTable(
                name: "DeliveryServices");

            migrationBuilder.DropIndex(
                name: "IX_Orders_Delivery_DeliveryServiceId",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "Delivery_Address",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "Delivery_DeliveryServiceId",
                table: "Orders");

            migrationBuilder.AlterColumn<DateTime>(
                name: "DateChange",
                table: "OrderChangeStatusLogs",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "GATDATE()",
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValueSql: "GETDATE()");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "PasswordHash", "PasswordSalt" },
                values: new object[] { new byte[] { 39, 86, 233, 9, 72, 127, 202, 3, 58, 254, 226, 232, 195, 68, 33, 203, 131, 63, 135, 132, 22, 50, 39, 104, 241, 117, 252, 179, 242, 218, 192, 138, 229, 38, 28, 217, 238, 202, 57, 163, 36, 177, 44, 86, 176, 140, 220, 65, 0, 111, 85, 131, 227, 63, 106, 189, 153, 178, 186, 253, 117, 244, 45, 72 }, new byte[] { 174, 0, 229, 85, 253, 69, 92, 189, 230, 218, 216, 15, 1, 1, 185, 66, 72, 45, 144, 28, 108, 146, 168, 191, 198, 12, 238, 58, 47, 145, 214, 218, 215, 12, 57, 95, 188, 124, 77, 252, 95, 184, 218, 249, 177, 73, 37, 201, 46, 48, 221, 247, 238, 103, 57, 138, 105, 45, 62, 241, 55, 132, 193, 67, 219, 140, 130, 231, 79, 125, 49, 176, 236, 81, 245, 117, 219, 127, 138, 65, 106, 198, 71, 170, 132, 179, 186, 154, 216, 190, 243, 72, 173, 23, 178, 8, 230, 245, 209, 41, 54, 93, 205, 251, 54, 211, 245, 85, 108, 69, 63, 9, 84, 249, 205, 176, 240, 62, 241, 210, 128, 225, 185, 139, 177, 171, 11, 203 } });
        }
    }
}
