using Microsoft.EntityFrameworkCore;
using AllergyNutritionPlatform.Models;

namespace AllergyNutritionPlatform.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<User> Users { get; set; }

    public DbSet<Allergy> Allergies { get; set; }

    public DbSet<Product> Products { get; set; }


    public DbSet<Restaurant> Restaurants { get; set; }

    public DbSet<Review> Reviews { get; set; }

    public DbSet<Favorite> Favorites { get; set; }

    public DbSet<UserAllergy> UserAllergies { get; set; }
    public DbSet<Recipe> Recipes { get; set; }
    public DbSet<ProductAllergy> ProductAllergies { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<UserAllergy>()
            .HasKey(ua => new { ua.UserId, ua.AllergyId });
            modelBuilder.Entity<ProductAllergy>()
                 .HasKey(pa => new { pa.ProductId, pa.AllergyId });
    }
}