namespace AllergyNutritionPlatform.Models;

public class Allergy
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Type { get; set; } = string.Empty;
    public string? Description { get; set; }

public string? CommonFoods { get; set; }

public string? Recommendation { get; set; }

    public List<UserAllergy> UserAllergies { get; set; } = new();

    public List<Product> Products { get; set; } = new();
}