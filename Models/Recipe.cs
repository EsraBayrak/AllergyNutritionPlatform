namespace AllergyNutritionPlatform.Models;

public class Recipe
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Category { get; set; } = string.Empty;

    public string SuitableFor { get; set; } = string.Empty;

    public string Ingredients { get; set; } = string.Empty;

    public string Instructions { get; set; } = string.Empty;
    public string? ImageUrl { get; set; }
}