namespace AllergyNutritionPlatform.Models;

public class UserAllergy
{
    public int UserId { get; set; }
    public User User { get; set; } = null!;

    public int AllergyId { get; set; }
    public Allergy Allergy { get; set; } = null!;
}