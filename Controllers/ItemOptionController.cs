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
    public class ItemOptionController : Controller
    {
        private readonly AppDbContext _context;

        public ItemOptionController(AppDbContext context)
        {
            _context = context;
        }

        // GET: ItemOption
        public async Task<IActionResult> Index()
        {
            var groupInformations = await _context.GroupInformations
                .Include(gi => gi.GroupItems)
                    .ThenInclude(g => g.ItemOptions)
                .ToListAsync();

            return View(groupInformations);
        }

        [HttpGet]
        public async Task<IActionResult> GetGroupItemsByGroupInfo(int groupInfoId)
        {
            var items = await _context.GroupItems
                .Where(gi => gi.GroupInformationId == groupInfoId)
                .Select(gi => new { gi.Id, gi.Name })
                .ToListAsync();

            return Json(items);
        }

        // GET: ItemOption/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var itemOption = await _context.ItemOptions
                .Include(i => i.GroupItem)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (itemOption == null)
            {
                return NotFound();
            }

            return View(itemOption);
        }

        // GET: ItemOption/Create
        public IActionResult Create()
        {
            // ViewData["GroupItemId"] = new SelectList(_context.GroupItems, "Id", "Name");
            ViewBag.GroupInformations = _context.GroupInformations.ToList();
            return View();
        }

        // POST: ItemOption/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,BobotF,GroupItemId,HighRisk")] ItemOption itemOption)
        {
            if (ModelState.IsValid)
            {
                _context.Add(itemOption);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["GroupItemId"] = new SelectList(_context.GroupItems, "Id", "Name", itemOption.GroupItemId);
            return View(itemOption);
        }

        // GET: ItemOption/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var itemOption = await _context.ItemOptions
                .Include(io => io.GroupItem)
                .ThenInclude(gi => gi!.GroupInformation)
                .FirstOrDefaultAsync(io => io.Id == id);
            if (itemOption == null)
            {
                return NotFound();
            }

            // Ambil semua GroupInformation untuk dropdown pertama
            ViewBag.GroupInformations = await _context.GroupInformations.ToListAsync();
            // Simpan GroupInformationId dari GroupItem yang dipilih
            ViewBag.SelectedGroupInfoId = itemOption?.GroupItem?.GroupInformationId;

            // Ambil semua GroupItem dari GroupInformation terpilih
            ViewBag.GroupItems = await _context.GroupItems
                .Where(gi => gi.GroupInformationId == itemOption!.GroupItem!.GroupInformationId)
                .ToListAsync();
            ViewData["GroupItemId"] = new SelectList(_context.GroupItems, "Id", "Name", itemOption?.GroupItemId);
            return View(itemOption);
        }

        // POST: ItemOption/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,BobotF,GroupItemId,HighRisk")] ItemOption itemOption)
        {
            if (id != itemOption.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(itemOption);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ItemOptionExists(itemOption.Id))
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
            ViewData["GroupItemId"] = new SelectList(_context.GroupItems, "Id", "Name", itemOption.GroupItemId);
            return View(itemOption);
        }

        // GET: ItemOption/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var itemOption = await _context.ItemOptions
                .Include(i => i.GroupItem)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (itemOption == null)
            {
                return NotFound();
            }

            return View(itemOption);
        }

        // POST: ItemOption/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var itemOption = await _context.ItemOptions.FindAsync(id);
            if (itemOption != null)
            {
                _context.ItemOptions.Remove(itemOption);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ItemOptionExists(int id)
        {
            return _context.ItemOptions.Any(e => e.Id == id);
        }
    }
}
