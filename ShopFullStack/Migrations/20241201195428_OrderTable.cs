using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShopFullStack.Migrations
{
    /// <inheritdoc />
    public partial class OrderTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_orders_customers_CustomerId",
                table: "orders");

            migrationBuilder.DropColumn(
                name: "customer_number",
                table: "orders");

            migrationBuilder.DropColumn(
                name: "order_date",
                table: "orders");

            migrationBuilder.RenameColumn(
                name: "CustomerId",
                table: "orders",
                newName: "customer_id");

            migrationBuilder.RenameColumn(
                name: "return_status",
                table: "orders",
                newName: "status");

            migrationBuilder.RenameColumn(
                name: "quantity",
                table: "orders",
                newName: "order_returned_status");

            migrationBuilder.RenameColumn(
                name: "product_number",
                table: "orders",
                newName: "cart_id");

            migrationBuilder.RenameColumn(
                name: "payment_date",
                table: "orders",
                newName: "created_at");

            migrationBuilder.RenameColumn(
                name: "group_order_id",
                table: "orders",
                newName: "shipping_address");

            migrationBuilder.RenameIndex(
                name: "IX_orders_CustomerId",
                table: "orders",
                newName: "IX_orders_customer_id");

            migrationBuilder.AlterColumn<long>(
                name: "customer_id",
                table: "orders",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "shipped_at",
                table: "orders",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "OrderId",
                table: "CartItems",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_orders_cart_id",
                table: "orders",
                column: "cart_id");

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

            migrationBuilder.AddForeignKey(
                name: "FK_orders_Carts_cart_id",
                table: "orders",
                column: "cart_id",
                principalTable: "Carts",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_orders_customers_customer_id",
                table: "orders",
                column: "customer_id",
                principalTable: "customers",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CartItems_orders_OrderId",
                table: "CartItems");

            migrationBuilder.DropForeignKey(
                name: "FK_orders_Carts_cart_id",
                table: "orders");

            migrationBuilder.DropForeignKey(
                name: "FK_orders_customers_customer_id",
                table: "orders");

            migrationBuilder.DropIndex(
                name: "IX_orders_cart_id",
                table: "orders");

            migrationBuilder.DropIndex(
                name: "IX_CartItems_OrderId",
                table: "CartItems");

            migrationBuilder.DropColumn(
                name: "shipped_at",
                table: "orders");

            migrationBuilder.DropColumn(
                name: "OrderId",
                table: "CartItems");

            migrationBuilder.RenameColumn(
                name: "customer_id",
                table: "orders",
                newName: "CustomerId");

            migrationBuilder.RenameColumn(
                name: "status",
                table: "orders",
                newName: "return_status");

            migrationBuilder.RenameColumn(
                name: "shipping_address",
                table: "orders",
                newName: "group_order_id");

            migrationBuilder.RenameColumn(
                name: "order_returned_status",
                table: "orders",
                newName: "quantity");

            migrationBuilder.RenameColumn(
                name: "created_at",
                table: "orders",
                newName: "payment_date");

            migrationBuilder.RenameColumn(
                name: "cart_id",
                table: "orders",
                newName: "product_number");

            migrationBuilder.RenameIndex(
                name: "IX_orders_customer_id",
                table: "orders",
                newName: "IX_orders_CustomerId");

            migrationBuilder.AlterColumn<long>(
                name: "CustomerId",
                table: "orders",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AddColumn<long>(
                name: "customer_number",
                table: "orders",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<DateTime>(
                name: "order_date",
                table: "orders",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddForeignKey(
                name: "FK_orders_customers_CustomerId",
                table: "orders",
                column: "CustomerId",
                principalTable: "customers",
                principalColumn: "id");
        }
    }
}
