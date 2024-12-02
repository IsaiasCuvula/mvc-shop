using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShopFullStack.Migrations
{
    /// <inheritdoc />
    public partial class AddedMapeamento : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CartItems_orders_OrderId",
                table: "CartItems");

            migrationBuilder.DropIndex(
                name: "IX_CartItems_OrderId",
                table: "CartItems");

            migrationBuilder.DropColumn(
                name: "OrderId",
                table: "CartItems");

            migrationBuilder.AddForeignKey(
                name: "FK_CartItems_orders_cart_id",
                table: "CartItems",
                column: "cart_id",
                principalTable: "orders",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CartItems_orders_cart_id",
                table: "CartItems");

            migrationBuilder.AddColumn<long>(
                name: "OrderId",
                table: "CartItems",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_CartItems_OrderId",
                table: "CartItems",
                column: "OrderId");

            migrationBuilder.AddForeignKey(
                name: "FK_CartItems_orders_OrderId",
                table: "CartItems",
                column: "OrderId",
                principalTable: "orders",
                principalColumn: "id");
        }
    }
}
