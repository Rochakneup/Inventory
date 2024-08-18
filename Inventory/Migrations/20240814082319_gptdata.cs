using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Inventory.Migrations
{
    /// <inheritdoc />
    public partial class gptdata : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "Price",
                table: "Products",
                type: "decimal(18,4)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AlterColumn<decimal>(
                name: "UnitPrice",
                table: "OrderItems",
                type: "decimal(18,4)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AlterColumn<decimal>(
                name: "UnitPrice",
                table: "CartItems",
                type: "decimal(18,4)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.UpdateData(
                table: "ChatbotResponses",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Answer", "Question" },
                values: new object[] { "We have different types of products from different categories, from electrical to clothes. You can surf around to find more products.", "What products do you have?" });

            migrationBuilder.UpdateData(
                table: "ChatbotResponses",
                keyColumn: "Id",
                keyValue: 3,
                column: "Answer",
                value: "You can add the products, and then from the cart, you can select the products and check out to place the order.");

            migrationBuilder.UpdateData(
                table: "ChatbotResponses",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "Answer", "Question" },
                values: new object[] { "It takes 2-3 working days for the products to be delivered to your location.", "How long for the product to arrive at my location?" });

            migrationBuilder.UpdateData(
                table: "ChatbotResponses",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "Answer", "Question" },
                values: new object[] { "You're welcome. Feel free to ask any other questions.", "Thank you" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "Price",
                table: "Products",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,4)");

            migrationBuilder.AlterColumn<decimal>(
                name: "UnitPrice",
                table: "OrderItems",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,4)");

            migrationBuilder.AlterColumn<decimal>(
                name: "UnitPrice",
                table: "CartItems",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,4)");

            migrationBuilder.UpdateData(
                table: "ChatbotResponses",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Answer", "Question" },
                values: new object[] { "we have differnet types of products from different catgorise from elcetrical to clothes you can surf around to find more products .", "What producs do you have?" });

            migrationBuilder.UpdateData(
                table: "ChatbotResponses",
                keyColumn: "Id",
                keyValue: 3,
                column: "Answer",
                value: "You can add the products and then from the cart yo can select the products and check out to place the order.");

            migrationBuilder.UpdateData(
                table: "ChatbotResponses",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "Answer", "Question" },
                values: new object[] { "It takes  2-3 working days for the products to be delivered to your location.", "How long for the product to arive to my loacation ?" });

            migrationBuilder.UpdateData(
                table: "ChatbotResponses",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "Answer", "Question" },
                values: new object[] { "welcome.Fell free to ask any other questions", "Thankyou" });
        }
    }
}
