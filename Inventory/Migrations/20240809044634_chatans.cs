using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Inventory.Migrations
{
    /// <inheritdoc />
    public partial class chatans : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "ChatbotResponses",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Answer", "Question" },
                values: new object[] { "I'm unable to help with finding specific categories, but you can browse through our website or use the search feature to find the products you're looking for.", "Can you help me find a product?" });

            migrationBuilder.UpdateData(
                table: "ChatbotResponses",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Answer", "Question" },
                values: new object[] { "We offer various shipping options including standard, expedited, and express delivery. You can choose the one that best fits your needs during checkout.", "What are your shipping options?" });

            migrationBuilder.UpdateData(
                table: "ChatbotResponses",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "Answer", "Question" },
                values: new object[] { "To track your order, please use the tracking number sent to you via email. You can enter this number on our website's order tracking page to see the current status of your shipment.", "How can I track my order?" });

            migrationBuilder.UpdateData(
                table: "ChatbotResponses",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "Answer", "Question" },
                values: new object[] { "We offer a 30-day return policy for most items. If you're not satisfied with your purchase, you can return it within 30 days for a full refund or exchange.", "What is your return policy?" });

            migrationBuilder.UpdateData(
                table: "ChatbotResponses",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "Answer", "Question" },
                values: new object[] { "Yes, we frequently offer discounts and promotions. Be sure to check our website's promotions page or subscribe to our newsletter for the latest deals.", "Do you offer discounts or promotions?" });

            migrationBuilder.UpdateData(
                table: "ChatbotResponses",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "Answer", "Question" },
                values: new object[] { "You can contact our customer support team via email at support@example.com. We will be happy to assist you with any questions or concerns.", "How can I contact customer support?" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "ChatbotResponses",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Answer", "Question" },
                values: new object[] { "I am a chatbot!", "What is your name?" });

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
                columns: new[] { "Answer", "Question" },
                values: new object[] { "You can add the products and then from the cart yo can select the products and check out to place the order.", "How to order?" });

            migrationBuilder.UpdateData(
                table: "ChatbotResponses",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "Answer", "Question" },
                values: new object[] { "It takes  2-3 working days for the products to be delivered to your location.", "How long for the product to arive to my loacation ?" });

            migrationBuilder.UpdateData(
                table: "ChatbotResponses",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "Answer", "Question" },
                values: new object[] { "I answer questions.", "What do you do?" });

            migrationBuilder.UpdateData(
                table: "ChatbotResponses",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "Answer", "Question" },
                values: new object[] { "welcome.Fell free to ask any other questions", "Thankyou" });
        }
    }
}
