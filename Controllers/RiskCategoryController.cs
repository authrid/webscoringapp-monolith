using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebScoringApp.Data;
using WebScoringApp.Models;

namespace WebScoringApp.Controllers
{
    public class RiskCategoryController : Controller
    {
        private readonly AppDbContext _context;

        public RiskCategoryController(AppDbContext context)
        {
            _context = context;
        }

        // GET: RiskCategory
        public async Task<IActionResult> Index()
        {
            return View(await _context.RiskCategories.ToListAsync());
        }

        // GET: RiskCategory/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var riskCategory = await _context.RiskCategories
                .FirstOrDefaultAsync(m => m.Id == id);
            if (riskCategory == null)
            {
                return NotFound();
            }

            return View(riskCategory);
        }

        // GET: RiskCategory/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: RiskCategory/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,ScoreMin,ScoreMax")] RiskCategory riskCategory)
        {
            if (ModelState.IsValid)
            {
                _context.Add(riskCategory);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(riskCategory);
        }

        // GET: RiskCategory/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var riskCategory = await _context.RiskCategories.FindAsync(id);
            if (riskCategory == null)
            {
                return NotFound();
            }
            return View(riskCategory);
        }

        // POST: RiskCategory/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,ScoreMin,ScoreMax")] RiskCategory riskCategory)
        {
            if (id != riskCategory.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(riskCategory);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RiskCategoryExists(riskCategory.Id))
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
            return View(riskCategory);
        }

        // GET: RiskCategory/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var riskCategory = await _context.RiskCategories
                .FirstOrDefaultAsync(m => m.Id == id);
            if (riskCategory == null)
            {
                return NotFound();
            }

            return View(riskCategory);
        }

        // POST: RiskCategory/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var riskCategory = await _context.RiskCategories.FindAsync(id);
            if (riskCategory != null)
            {
                _context.RiskCategories.Remove(riskCategory);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RiskCategoryExists(int id)
        {
            return _context.RiskCategories.Any(e => e.Id == id);
        }
    }
}
