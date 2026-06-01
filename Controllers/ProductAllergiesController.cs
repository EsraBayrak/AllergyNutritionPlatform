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
    public class ProductAllergiesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProductAllergiesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: ProductAllergies
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.ProductAllergies.Include(p => p.Allergy).Include(p => p.Product);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: ProductAllergies/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productAllergy = await _context.ProductAllergies
                .Include(p => p.Allergy)
                .Include(p => p.Product)
                .FirstOrDefaultAsync(m => m.ProductId == id);
            if (productAllergy == null)
            {
                return NotFound();
            }

            return View(productAllergy);
        }

        // GET: ProductAllergies/Create
        public IActionResult Create()
        {
            ViewData["AllergyId"] = new SelectList(_context.Allergies, "Id", "Name");
            ViewData["ProductId"] = new SelectList(_context.Products, "Id", "Name");
            return View();
        }

        // POST: ProductAllergies/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ProductId,AllergyId")] ProductAllergy productAllergy)
        {
            ModelState.Remove("Product");
            ModelState.Remove("Allergy");
            if (ModelState.IsValid)
            {
                _context.Add(productAllergy);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["AllergyId"] = new SelectList(_context.Allergies, "Id", "Name", productAllergy.AllergyId);
            ViewData["ProductId"] = new SelectList(_context.Products, "Id", "Name", productAllergy.ProductId);
            return View(productAllergy);
        }

        // GET: ProductAllergies/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productAllergy = await _context.ProductAllergies.FindAsync(id);
            if (productAllergy == null)
            {
                return NotFound();
            }
            ViewData["AllergyId"] = new SelectList(_context.Allergies, "Id", "Name", productAllergy.AllergyId);
            ViewData["ProductId"] = new SelectList(_context.Products, "Id", "Name", productAllergy.ProductId);
            return View(productAllergy);
        }

        // POST: ProductAllergies/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ProductId,AllergyId")] ProductAllergy productAllergy)
        {
            if (id != productAllergy.ProductId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(productAllergy);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductAllergyExists(productAllergy.ProductId))
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
            ViewData["AllergyId"] = new SelectList(_context.Allergies, "Id", "Name", productAllergy.AllergyId);
            ViewData["ProductId"] = new SelectList(_context.Products, "Id", "Name", productAllergy.ProductId);
            return View(productAllergy);
        }

        // GET: ProductAllergies/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productAllergy = await _context.ProductAllergies
                .Include(p => p.Allergy)
                .Include(p => p.Product)
                .FirstOrDefaultAsync(m => m.ProductId == id);
            if (productAllergy == null)
            {
                return NotFound();
            }

            return View(productAllergy);
        }

        // POST: ProductAllergies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var productAllergy = await _context.ProductAllergies.FindAsync(id);
            if (productAllergy != null)
            {
                _context.ProductAllergies.Remove(productAllergy);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductAllergyExists(int id)
        {
            return _context.ProductAllergies.Any(e => e.ProductId == id);
        }
    }
}
