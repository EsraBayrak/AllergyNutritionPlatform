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
    public class ProductsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProductsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Products
        public async Task<IActionResult> Index(string searchString, string statusFilter)
{
    var userId = HttpContext.Session.GetInt32("UserId");
List<int> userAllergyIds = new();

if (userId != null)
{
    userAllergyIds = await _context.UserAllergies
        .Where(ua => ua.UserId == userId.Value)
        .Select(ua => ua.AllergyId)
        .ToListAsync();
}
ViewBag.UserAllergyIds = userAllergyIds;
ViewBag.IsLoggedIn = userId != null;

var products = _context.Products
    .Include(p => p.ProductAllergies)
    .ThenInclude(pa => pa.Allergy)
    .AsQueryable();

if (!string.IsNullOrEmpty(searchString))
{
    searchString = searchString.ToLower();

    products = products.Where(p =>
        p.Name.ToLower().Contains(searchString) ||
        p.Brand.ToLower().Contains(searchString) ||
        p.Category.ToLower().Contains(searchString));
}

 

    ViewData["CurrentSearch"] = searchString;
    ViewData["CurrentStatus"] = statusFilter;
ViewBag.UserAllergyIds = userAllergyIds;
ViewBag.IsLoggedIn = userId != null;
    return View(await products.ToListAsync());
}

        // GET: Products/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products
    .Include(p => p.ProductAllergies)
    .ThenInclude(pa => pa.Allergy)
    .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }
            var userId = HttpContext.Session.GetInt32("UserId");

if (userId == null)
{
    return RedirectToAction("Login", "Users");
}
            var userAllergyIds = await _context.UserAllergies
    .Where(ua => ua.UserId == userId.Value)
    .Select(ua => ua.AllergyId)
    .ToListAsync();

var productAllergyIds = product.ProductAllergies
    .Select(pa => pa.AllergyId)
    .ToList();

bool isNotSuitable = productAllergyIds.Any(id => userAllergyIds.Contains(id));

ViewBag.IsNotSuitable = isNotSuitable;

            return View(product);
        }

        // GET: Products/Create
        public IActionResult Create()
        {
            ViewBag.Allergies = _context.Allergies.ToList();
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Brand,Category,Description")] Product product, int[] selectedAllergyIds)
        {
            if (ModelState.IsValid)
            {
                _context.Add(product);
                await _context.SaveChangesAsync();
                foreach (var allergyId in selectedAllergyIds)
{
    _context.ProductAllergies.Add(new ProductAllergy
    {
        ProductId = product.Id,
        AllergyId = allergyId
    });
}

await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(product);
        }

        // GET: Products/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            ViewBag.Allergies = _context.Allergies.ToList();
ViewBag.SelectedAllergyIds = _context.ProductAllergies
    .Where(pa => pa.ProductId == product.Id)
    .Select(pa => pa.AllergyId)
    .ToList();
            return View(product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
       public async Task<IActionResult> Edit(
    int id,
    [Bind("Id,Name,Brand,Category,Description")] Product product,
    int[] selectedAllergyIds)
        {
            if (id != product.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(product);
await _context.SaveChangesAsync();

var oldAllergies = _context.ProductAllergies
    .Where(pa => pa.ProductId == product.Id);

_context.ProductAllergies.RemoveRange(oldAllergies);

foreach (var allergyId in selectedAllergyIds)
{
    _context.ProductAllergies.Add(new ProductAllergy
    {
        ProductId = product.Id,
        AllergyId = allergyId
    });
}

await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.Id))
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
            ViewBag.Allergies = _context.Allergies.ToList();

ViewBag.SelectedAllergyIds = _context.ProductAllergies
    .Where(pa => pa.ProductId == product.Id)
    .Select(pa => pa.AllergyId)
    .ToList();
            return View(product);
        }

        // GET: Products/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product != null)
            {
                _context.Products.Remove(product);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(int id)
        {
            return _context.Products.Any(e => e.Id == id);
        }
    }
}
