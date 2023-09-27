using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyShop.Migrations
{
    /// <inheritdoc />
    public partial class userFix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Carts_AspNetUsers_ShopAppWebUserId",
                table: "Carts");

            migrationBuilder.AlterColumn<string>(
                name: "ShopAppWebUserId",
                table: "Carts",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Carts_AspNetUsers_ShopAppWebUserId",
                table: "Carts",
                column: "ShopAppWebUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Carts_AspNetUsers_ShopAppWebUserId",
                table: "Carts");

            migrationBuilder.AlterColumn<string>(
                name: "ShopAppWebUserId",
                table: "Carts",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddForeignKey(
                name: "FK_Carts_AspNetUsers_ShopAppWebUserId",
                table: "Carts",
                column: "ShopAppWebUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
