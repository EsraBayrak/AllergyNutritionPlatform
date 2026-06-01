using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AllergyNutritionPlatform.Migrations
{
    /// <inheritdoc />
    public partial class AddAllergyInfoFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CommonFoods",
                table: "Allergies",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Allergies",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Recommendation",
                table: "Allergies",
                type: "TEXT",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CommonFoods",
                table: "Allergies");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Allergies");

            migrationBuilder.DropColumn(
                name: "Recommendation",
                table: "Allergies");
        }
    }
}
