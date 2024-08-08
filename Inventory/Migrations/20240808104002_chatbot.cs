using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Inventory.Migrations
{
    /// <inheritdoc />
    public partial class chatbot : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ChatbotResponses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Question = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Answer = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChatbotResponses", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "ChatbotResponses",
                columns: new[] { "Id", "Answer", "Question" },
                values: new object[,]
                {
                    { 1, "I am a chatbot!", "What is your name?" },
                    { 2, "we have differnet types of products from different catgorise from elcetrical to clothes you can surf around to find more products .", "What producs do you have?" },
                    { 3, "You can add the products and then from the cart yo can select the products and check out to place the order.", "How to order?" },
                    { 4, "It takes  2-3 working days for the products to be delivered to your location.", "How long for the product to arive to my loacation ?" },
                    { 5, "I answer questions.", "What do you do?" },
                    { 6, "welcome.Fell free to ask any other questions", "Thankyou" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ChatbotResponses");
        }
    }
}
