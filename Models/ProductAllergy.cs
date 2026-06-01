namespace AllergyNutritionPlatform.Models;

public class ProductAllergy
{
    public int ProductId { get; set; }
    public Product Product { get; set; } = null!;

    public int AllergyId { get; set; }
    public Allergy Allergy { get; set; } = null!;
}