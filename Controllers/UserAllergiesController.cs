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
    public class UserAllergiesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public UserAllergiesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: UserAllergies
        public async Task<IActionResult> Index()
{
    var userId = HttpContext.Session.GetInt32("UserId");

    if (userId == null)
    {
        return RedirectToAction("Login", "Users");
    }

    var applicationDbContext = _context.UserAllergies
        .Include(u => u.Allergy)
        .Include(u => u.User)
        .Where(u => u.UserId == userId.Value);

    return View(await applicationDbContext.ToListAsync());
}

        // GET: UserAllergies/Details/5
        public async Task<IActionResult> Details(int userId, int allergyId)
{
    var userAllergy = await _context.UserAllergies
        .Include(u => u.Allergy)
        .Include(u => u.User)
        .FirstOrDefaultAsync(m => m.UserId == userId && m.AllergyId == allergyId);

    if (userAllergy == null)
    {
        return NotFound();
    }

    return View(userAllergy);
}

        // GET: UserAllergies/Create
        public IActionResult Create()
{
    var userId = HttpContext.Session.GetInt32("UserId");

    if (userId == null)
    {
        return RedirectToAction("Login", "Users");
    }

    ViewData["AllergyId"] = new SelectList(_context.Allergies, "Id", "Name");

    return View();
}
        // POST: UserAllergies/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
[ValidateAntiForgeryToken]
public async Task<IActionResult> Create([Bind("AllergyId")] UserAllergy userAllergy)
{
    var userId = HttpContext.Session.GetInt32("UserId");

    if (userId == null)
    {
        return RedirectToAction("Login", "Users");
    }

    userAllergy.UserId = userId.Value;

    var exists = await _context.UserAllergies
        .AnyAsync(x => x.UserId == userAllergy.UserId &&
                       x.AllergyId == userAllergy.AllergyId);

    if (!exists)
    {
        _context.UserAllergies.Add(userAllergy);
        await _context.SaveChangesAsync();
    }

    return RedirectToAction(nameof(Index));
}

        // GET: UserAllergies/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userAllergy = await _context.UserAllergies.FindAsync(id);
            if (userAllergy == null)
            {
                return NotFound();
            }
            ViewData["AllergyId"] = new SelectList(_context.Allergies, "Id", "Name", userAllergy.AllergyId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "FullName", userAllergy.UserId);
            return View(userAllergy);
        }

        // POST: UserAllergies/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("UserId,AllergyId")] UserAllergy userAllergy)
        {
            if (id != userAllergy.UserId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(userAllergy);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserAllergyExists(userAllergy.UserId))
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
            ViewData["AllergyId"] = new SelectList(_context.Allergies, "Id", "Id", userAllergy.AllergyId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", userAllergy.UserId);
            return View(userAllergy);
        }

        // GET: UserAllergies/Delete/5
        public async Task<IActionResult> Delete(int userId, int allergyId)
{
    var userAllergy = await _context.UserAllergies
        .Include(u => u.Allergy)
        .Include(u => u.User)
        .FirstOrDefaultAsync(m => m.UserId == userId && m.AllergyId == allergyId);

    if (userAllergy == null)
    {
        return NotFound();
    }

    return View(userAllergy);
}

        // POST: UserAllergies/Delete/5
        [HttpPost, ActionName("Delete")]
[ValidateAntiForgeryToken]
public async Task<IActionResult> DeleteConfirmed(int userId, int allergyId)
{
    var userAllergy = await _context.UserAllergies
        .FirstOrDefaultAsync(m => m.UserId == userId && m.AllergyId == allergyId);

    if (userAllergy != null)
    {
        _context.UserAllergies.Remove(userAllergy);
        await _context.SaveChangesAsync();
    }

    return RedirectToAction(nameof(Index));
}

        private bool UserAllergyExists(int id)
        {
            return _context.UserAllergies.Any(e => e.UserId == id);
        }
    }
}
