using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Inventory.Migrations
{
    /// <inheritdoc />
    public partial class chat : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "ChatbotResponses",
                columns: new[] { "Id", "Answer", "Question" },
                values: new object[] { 7, "You're Welcome!", "Thankyou" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "ChatbotResponses",
                keyColumn: "Id",
                keyValue: 7);
        }
    }
}
