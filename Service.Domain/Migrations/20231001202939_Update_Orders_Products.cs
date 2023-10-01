using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Service.Domain.Migrations
{
    /// <inheritdoc />
    public partial class Update_Orders_Products : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_DeliveryServices_Delivery_DeliveryServiceId",
                table: "Orders");

            migrationBuilder.RenameColumn(
                name: "Delivery_DeliveryServiceId",
                table: "Orders",
                newName: "Delivery_ServiceId");

            migrationBuilder.RenameIndex(
                name: "IX_Orders_Delivery_DeliveryServiceId",
                table: "Orders",
                newName: "IX_Orders_Delivery_ServiceId");

            migrationBuilder.AddColumn<DateTime>(
                name: "Created",
                table: "Products",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "CreatedById",
                table: "Products",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "PasswordHash", "PasswordSalt" },
                values: new object[] { new byte[] { 222, 2, 211, 103, 132, 215, 173, 28, 100, 209, 4, 22, 117, 178, 32, 143, 253, 219, 161, 33, 187, 47, 73, 225, 76, 7, 159, 106, 149, 101, 188, 252, 254, 33, 170, 61, 10, 235, 159, 124, 130, 15, 192, 14, 170, 52, 106, 165, 203, 9, 76, 95, 88, 139, 138, 131, 201, 116, 65, 138, 208, 212, 147, 171 }, new byte[] { 253, 97, 145, 209, 233, 66, 189, 199, 153, 82, 108, 57, 13, 15, 59, 101, 195, 109, 158, 96, 107, 169, 131, 228, 83, 17, 247, 186, 59, 161, 51, 15, 173, 206, 255, 86, 188, 39, 248, 185, 48, 107, 211, 98, 103, 21, 86, 112, 87, 245, 118, 76, 107, 11, 159, 233, 208, 229, 151, 6, 17, 54, 135, 98, 57, 13, 155, 135, 13, 66, 92, 77, 111, 251, 94, 164, 94, 196, 76, 244, 144, 113, 103, 72, 129, 81, 194, 247, 56, 27, 231, 169, 235, 158, 41, 19, 172, 44, 219, 18, 99, 4, 217, 65, 104, 115, 221, 248, 26, 215, 171, 142, 182, 101, 178, 104, 140, 43, 164, 38, 215, 109, 210, 50, 26, 246, 4, 202 } });

            migrationBuilder.CreateIndex(
                name: "IX_Products_CreatedById",
                table: "Products",
                column: "CreatedById");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_DeliveryServices_Delivery_ServiceId",
                table: "Orders",
                column: "Delivery_ServiceId",
                principalTable: "DeliveryServices",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Users_CreatedById",
                table: "Products",
                column: "CreatedById",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_DeliveryServices_Delivery_ServiceId",
                table: "Orders");

            migrationBuilder.DropForeignKey(
                name: "FK_Products_Users_CreatedById",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Products_CreatedById",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "Created",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "CreatedById",
                table: "Products");

            migrationBuilder.RenameColumn(
                name: "Delivery_ServiceId",
                table: "Orders",
                newName: "Delivery_DeliveryServiceId");

            migrationBuilder.RenameIndex(
                name: "IX_Orders_Delivery_ServiceId",
                table: "Orders",
                newName: "IX_Orders_Delivery_DeliveryServiceId");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "PasswordHash", "PasswordSalt" },
                values: new object[] { new byte[] { 52, 55, 155, 136, 186, 220, 138, 160, 241, 88, 246, 68, 19, 226, 178, 63, 242, 123, 223, 14, 168, 124, 8, 185, 42, 235, 186, 245, 129, 200, 25, 93, 164, 11, 87, 145, 183, 177, 241, 34, 215, 119, 167, 96, 10, 253, 227, 22, 190, 62, 35, 161, 145, 240, 125, 7, 150, 238, 150, 141, 53, 6, 171, 123 }, new byte[] { 173, 5, 65, 66, 199, 57, 159, 41, 213, 225, 63, 201, 21, 78, 130, 72, 222, 133, 67, 144, 107, 43, 29, 70, 114, 225, 201, 56, 149, 241, 170, 101, 133, 114, 161, 95, 51, 88, 226, 33, 78, 182, 237, 131, 43, 205, 31, 178, 238, 82, 169, 0, 67, 145, 1, 44, 206, 49, 171, 222, 214, 141, 103, 153, 188, 193, 0, 125, 44, 17, 159, 160, 65, 122, 66, 112, 155, 228, 28, 203, 217, 127, 217, 247, 21, 216, 104, 144, 227, 41, 63, 85, 37, 120, 15, 200, 25, 1, 151, 230, 229, 238, 203, 166, 174, 23, 30, 121, 102, 94, 231, 105, 248, 91, 113, 135, 182, 56, 24, 37, 131, 29, 107, 159, 34, 199, 73, 207 } });

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_DeliveryServices_Delivery_DeliveryServiceId",
                table: "Orders",
                column: "Delivery_DeliveryServiceId",
                principalTable: "DeliveryServices",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
