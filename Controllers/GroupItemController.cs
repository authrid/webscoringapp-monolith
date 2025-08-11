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
    public class GroupItemController : Controller
    {
        private readonly AppDbContext _context;

        public GroupItemController(AppDbContext context)
        {
            _context = context;
        }

        // GET: GroupItem
        public async Task<IActionResult> Index()
        {
            var groupItems = await _context.GroupItems
                .Include(g => g.GroupInformation)
                .ToListAsync();

            var groups = await _context.GroupInformations.ToListAsync();
            ViewData["Groups"] = groups;

            return View(groupItems);
        }

        // GET: GroupItem/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var groupItem = await _context.GroupItems
                .Include(g => g.GroupInformation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (groupItem == null)
            {
                return NotFound();
            }

            return View(groupItem);
        }

        // GET: GroupItem/Create
        public IActionResult Create()
        {
            ViewData["GroupInformationId"] = new SelectList(_context.GroupInformations, "Id", "Name");
            return View();
        }

        // POST: GroupItem/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,BobotD,GroupInformationId")] GroupItem groupItem)
        {
            if (!ModelState.IsValid)
    {
                foreach (var error in ModelState)
                {
                    Console.WriteLine($"Key: {error.Key}");
                    foreach (var subError in error.Value.Errors)
                    {
                        Console.WriteLine($"   Error: {subError.ErrorMessage}");
                    }
                }
            }

            if (ModelState.IsValid)
            {
                _context.Add(groupItem);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["GroupInformationId"] = new SelectList(_context.GroupInformations, "Id", "Name", groupItem.GroupInformationId);
            return View(groupItem);
        }

        // GET: GroupItem/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var groupItem = await _context.GroupItems.FindAsync(id);
            if (groupItem == null)
            {
                return NotFound();
            }
            ViewData["GroupInformationId"] = new SelectList(_context.GroupInformations, "Id", "Name", groupItem.GroupInformationId);
            return View(groupItem);
        }

        // POST: GroupItem/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,BobotD,GroupInformationId")] GroupItem groupItem)
        {
            if (id != groupItem.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(groupItem);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GroupItemExists(groupItem.Id))
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
            ViewData["GroupInformationId"] = new SelectList(_context.GroupInformations, "Id", "Name", groupItem.GroupInformationId);
            return View(groupItem);
        }

        // GET: GroupItem/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var groupItem = await _context.GroupItems
                .Include(g => g.GroupInformation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (groupItem == null)
            {
                return NotFound();
            }

            return View(groupItem);
        }

        // POST: GroupItem/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var groupItem = await _context.GroupItems.FindAsync(id);
            if (groupItem != null)
            {
                _context.GroupItems.Remove(groupItem);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool GroupItemExists(int id)
        {
            return _context.GroupItems.Any(e => e.Id == id);
        }
    }
}
