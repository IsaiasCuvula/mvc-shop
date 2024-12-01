using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShopFullStack.Migrations
{
    /// <inheritdoc />
    public partial class CartItemProductIdType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CartItems_products_ProductId1",
                table: "CartItems");

            migrationBuilder.DropIndex(
                name: "IX_CartItems_ProductId1",
                table: "CartItems");

            migrationBuilder.DropColumn(
                name: "ProductId1",
                table: "CartItems");

            migrationBuilder.AlterColumn<long>(
                name: "product_id",
                table: "CartItems",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.CreateIndex(
                name: "IX_CartItems_product_id",
                table: "CartItems",
                column: "product_id");

            migrationBuilder.AddForeignKey(
                name: "FK_CartItems_products_product_id",
                table: "CartItems",
                column: "product_id",
                principalTable: "products",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CartItems_products_product_id",
                table: "CartItems");

            migrationBuilder.DropIndex(
                name: "IX_CartItems_product_id",
                table: "CartItems");

            migrationBuilder.AlterColumn<int>(
                name: "product_id",
                table: "CartItems",
                type: "integer",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AddColumn<long>(
                name: "ProductId1",
                table: "CartItems",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateIndex(
                name: "IX_CartItems_ProductId1",
                table: "CartItems",
                column: "ProductId1");

            migrationBuilder.AddForeignKey(
                name: "FK_CartItems_products_ProductId1",
                table: "CartItems",
                column: "ProductId1",
                principalTable: "products",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
