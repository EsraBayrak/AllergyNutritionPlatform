namespace AllergyNutritionPlatform.Models;

public class Restaurant
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string City { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public bool HasGlutenFreeOptions { get; set; }

    public bool HasVeganOptions { get; set; }

    public double Rating { get; set; }
}