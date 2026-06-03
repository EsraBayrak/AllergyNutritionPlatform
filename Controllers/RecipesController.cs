using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AllergyNutritionPlatform.Data;
using AllergyNutritionPlatform.Models;

namespace AllergyNutritionPlatform.Controllers;

public class RecipesController : Controller
{
    private readonly ApplicationDbContext _context;

    public RecipesController(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        return View(await _context.Recipes.ToListAsync());
    }

    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var recipe = await _context.Recipes
            .FirstOrDefaultAsync(m => m.Id == id);

        if (recipe == null)
        {
            return NotFound();
        }

        return View(recipe);
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Id,Name,Category,SuitableFor,Ingredients,Instructions,ImageUrl")] Recipe recipe)
    {
        if (ModelState.IsValid)
        {
            _context.Add(recipe);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        return View(recipe);
    }

    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var recipe = await _context.Recipes.FindAsync(id);

        if (recipe == null)
        {
            return NotFound();
        }

        return View(recipe);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Category,SuitableFor,Ingredients,Instructions,ImageUrl")] Recipe recipe)
    {
        if (id != recipe.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            _context.Update(recipe);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        return View(recipe);
    }

    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var recipe = await _context.Recipes
            .FirstOrDefaultAsync(m => m.Id == id);

        if (recipe == null)
        {
            return NotFound();
        }

        return View(recipe);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var recipe = await _context.Recipes.FindAsync(id);

        if (recipe != null)
        {
            _context.Recipes.Remove(recipe);
            await _context.SaveChangesAsync();
        }

        return RedirectToAction(nameof(Index));
    }
}