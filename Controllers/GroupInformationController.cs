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
    public class GroupInformationController : Controller
    {
        private readonly AppDbContext _context;

        public GroupInformationController(AppDbContext context)
        {
            _context = context;
        }

        // GET: GroupInformation
        public async Task<IActionResult> Index()
        {
            return View(await _context.GroupInformations.ToListAsync());
        }

        // GET: GroupInformation/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var groupInformation = await _context.GroupInformations
                .FirstOrDefaultAsync(m => m.Id == id);
            if (groupInformation == null)
            {
                return NotFound();
            }

            return View(groupInformation);
        }

        // GET: GroupInformation/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: GroupInformation/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,BobotB")] GroupInformation groupInformation)
        {
            if (ModelState.IsValid)
            {
                _context.Add(groupInformation);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(groupInformation);
        }

        // GET: GroupInformation/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var groupInformation = await _context.GroupInformations.FindAsync(id);
            if (groupInformation == null)
            {
                return NotFound();
            }
            return View(groupInformation);
        }

        // POST: GroupInformation/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,BobotB")] GroupInformation groupInformation)
        {
            if (id != groupInformation.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(groupInformation);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GroupInformationExists(groupInformation.Id))
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
            return View(groupInformation);
        }

        // GET: GroupInformation/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var groupInformation = await _context.GroupInformations
                .FirstOrDefaultAsync(m => m.Id == id);
            if (groupInformation == null)
            {
                return NotFound();
            }

            return View(groupInformation);
        }

        // POST: GroupInformation/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var groupInformation = await _context.GroupInformations.FindAsync(id);
            if (groupInformation != null)
            {
                _context.GroupInformations.Remove(groupInformation);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool GroupInformationExists(int id)
        {
            return _context.GroupInformations.Any(e => e.Id == id);
        }
    }
}
