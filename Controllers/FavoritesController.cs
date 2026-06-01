using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AllergyNutritionPlatform.Data;
using AllergyNutritionPlatform.Models;

namespace AllergyNutritionPlatform.Controllers
{
    public class FavoritesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public FavoritesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Favorites
        public async Task<IActionResult> Index()
{
    var userId = HttpContext.Session.GetInt32("UserId");

    if (userId == null)
    {
        return RedirectToAction("Login", "Users");
    }

    var favorites = _context.Favorites
        .Include(f => f.Product)
        .Where(f => f.UserId == userId.Value);

    return View(await favorites.ToListAsync());
}

        // GET: Favorites/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var favorite = await _context.Favorites
                .Include(f => f.Product)
                .Include(f => f.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (favorite == null)
            {
                return NotFound();
            }

            return View(favorite);
        }

        // GET: Favorites/Create
        public IActionResult Create()
        {
            ViewData["ProductId"] = new SelectList(_context.Products, "Id", "Name");
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "FullName");
            return View();
        }

        // POST: Favorites/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,UserId,ProductId")] Favorite favorite)
        {
            ModelState.Remove("User");
            ModelState.Remove("Product");
            if (ModelState.IsValid)
            {
                _context.Add(favorite);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ProductId"] = new SelectList(_context.Products, "Id", "Name", favorite.ProductId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "FullName", favorite.UserId);
            return View(favorite);
        }

        // GET: Favorites/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var favorite = await _context.Favorites.FindAsync(id);
            if (favorite == null)
            {
                return NotFound();
            }
            ViewData["ProductId"] = new SelectList(_context.Products, "Id", "Name", favorite.ProductId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "FullName", favorite.UserId);
            return View(favorite);
        }

        // POST: Favorites/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,UserId,ProductId")] Favorite favorite)
        {
            if (id != favorite.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(favorite);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FavoriteExists(favorite.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["ProductId"] = new SelectList(_context.Products, "Id", "Name", favorite.ProductId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "FullName", favorite.UserId);
            return View(favorite);
        }
     
      public async Task<IActionResult> AddToFavorites(int productId)
{
    var userId = HttpContext.Session.GetInt32("UserId");

    if (userId == null)
    {
        return RedirectToAction("Login", "Users");
    }

    var product = await _context.Products.FindAsync(productId);
    if (product == null)
{
    return NotFound();
}

if (product.IsSafe)
{
        var exists = await _context.Favorites
            .AnyAsync(f => f.UserId == userId.Value && f.ProductId == productId);
        if (!exists)
        {
            _context.Favorites.Add(new Favorite
            {
                UserId = userId.Value,
                ProductId = productId
            });

            await _context.SaveChangesAsync();
        }
    }

    return RedirectToAction("Index", "Products");
}
        // GET: Favorites/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var favorite = await _context.Favorites
                .Include(f => f.Product)
                .Include(f => f.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (favorite == null)
            {
                return NotFound();
            }

            return View(favorite);
        }

        // POST: Favorites/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var favorite = await _context.Favorites.FindAsync(id);
            if (favorite != null)
            {
                _context.Favorites.Remove(favorite);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        

        private bool FavoriteExists(int id)
        {
            return _context.Favorites.Any(e => e.Id == id);
        }
    }
}
