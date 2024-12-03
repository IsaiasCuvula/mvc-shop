using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShopFullStack.Migrations
{
    /// <inheritdoc />
    public partial class AddedDeliveryDate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "delivered_at",
                table: "orders",
                type: "timestamp with time zone",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "delivered_at",
                table: "orders");
        }
    }
}
