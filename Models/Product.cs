namespace AllergyNutritionPlatform.Models;

public class Product
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Brand { get; set; } = string.Empty;

    public string Category { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public bool IsSafe { get; set; }

    public ICollection<ProductAllergy> ProductAllergies { get; set; } = new List<ProductAllergy>();

    public List<Allergy> Allergies { get; set; } = new();
    
}