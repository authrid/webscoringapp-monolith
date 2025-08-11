using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebScoringApp.Data;
using WebScoringApp.Models;
using WebScoringApp.Services;

namespace WebScoringApp.Controllers
{
    public class ApplicationController : Controller
    {
        private readonly AppDbContext _context;
        private readonly ScoreService _scoreService;

        public ApplicationController(AppDbContext context, ScoreService scoreService)
        {
            _context = context;
            _scoreService = scoreService;
        }

        // GET: Application
        public async Task<IActionResult> Index()
        {
            var apps = await _context.Applications.ToListAsync();
            
            return View(apps);
        }

        // GET: Application/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var application = await _context.Applications
                .Include(a => a.ApplicationSelections)
                    .ThenInclude(s => s.ItemOption)
                    .ThenInclude(io => io.GroupItem)
                    .ThenInclude(gi => gi.GroupInformation)
                .FirstOrDefaultAsync(m => m.Id == id);
            
            if (application == null) return NotFound();

            var groups = await _context.GroupInformations
                .Include(g => g.GroupItems)
                    .ThenInclude(gi => gi.ItemOptions)
                .ToListAsync();

            ViewData["Groups"] = groups;
            ViewData["Selections"] = application.ApplicationSelections.ToDictionary(s => s.GroupItemId, s => s.ItemOptionId);

            return View(application);
        }

        // GET: Application/Create
        public IActionResult Create()
        {
            var groups = _context.GroupInformations
                .Include(g => g.GroupItems)
                .ThenInclude(gi => gi.ItemOptions)
                .ToList();

            ViewData["Groups"] = groups;
            return View();
        }

        // POST: Application/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            [Bind("Id,Name,BirthPlace,DateOfBirth,Gender,PortalCode,Address")] Application application,
            Dictionary<int, int> selections)
        {
            // Console.WriteLine("=== Create Debug Start ===");
            // Console.WriteLine("ModelState.IsValid: " + ModelState.IsValid);
            // Console.WriteLine("Selections count: " + (selections?.Count ?? 0));

            // if (!ModelState.IsValid)
            // {
            //     var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
            //     Console.WriteLine("Errors: " + string.Join("\n", errors));
            // }
            // Console.WriteLine("=== Create Debug End ===");

            if (ModelState.IsValid)
            {
                // Generate nomor aplikasi 10 digit
                application.AppNo = GenerateAppNo();

                // 1. Simpan Application baru
                _context.Applications.Add(application);
                await _context.SaveChangesAsync();

                // 2. Loop selections untuk buat ApplicationSelection
                var appSelections = new List<ApplicationSelection>();

                foreach (var selection in selections)
                {
                    var groupItemId = selection.Key;
                    var itemOptionId = selection.Value;

                    if (itemOptionId == 0) continue; // skip jika tidak dipilih

                    var option = await _context.ItemOptions
                        .Include(o => o.GroupItem)
                        .FirstOrDefaultAsync(o => o.Id == itemOptionId);

                    if (option != null)
                    {
                        var bobotItem = option.BobotF * (option.GroupItem.BobotD / 100m);

                        appSelections.Add(new ApplicationSelection
                        {
                            ApplicationId = application.Id,
                            GroupItemId = groupItemId,
                            ItemOptionId = itemOptionId,
                            Bobot = bobotItem,
                            HighRisk = option.HighRisk
                        });
                    }
                }

                _context.ApplicationSelections.AddRange(appSelections);
                await _context.SaveChangesAsync();

                // Hitung score dan risk
                var (score, risk) = _scoreService.CalculateFinalScore(application.Id);
                application.TotalScore = score;
                application.RiskCategoryName = risk;

                _context.Update(application);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            // Kalau gagal, kirim data lagi ke view
            var groups = _context.GroupInformations
                .Include(g => g.GroupItems)
                .ThenInclude(gi => gi.ItemOptions)
                .ToList();
            ViewData["Groups"] = groups;
            
            return View(application);
        }

        private string GenerateAppNo()
        {
            // Gunakan timestamp + random biar unik
            return DateTime.UtcNow.ToString("yyMMdd") + new Random().Next(1000, 9999);
        }

        // GET: application/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var application = await _context.Applications
                .Include(a => a.ApplicationSelections)
                .ThenInclude(s => s.ItemOption)
                .ThenInclude(io => io.GroupItem)
                .FirstOrDefaultAsync(a => a.Id == id);

            if (application == null)
            {
                return NotFound();
            }

            var groups = await _context.GroupInformations
                .Include(g => g.GroupItems)
                .ThenInclude(gi => gi.ItemOptions)
                .ToListAsync();

            ViewData["Groups"] = groups;
            ViewData["Selections"] = application.ApplicationSelections.ToDictionary(s => s.GroupItemId, s => s.ItemOptionId);

            return View(application);
        }

        // POST: application/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(
            int id,
            [Bind("Id,Name,BirthPlace,DateOfBirth,Gender,PortalCode,Address")] Application application,
            Dictionary<int, int> selections)
        {
            if (id != application.Id)
                return NotFound();

            if (!ModelState.IsValid)
                return View(application);

            // 1. Ambil entity dari DB
            var appFromDb = await _context.Applications
                .Include(a => a.ApplicationSelections)
                .FirstOrDefaultAsync(a => a.Id == id);

            if (appFromDb == null)
                return NotFound();

            // 2. Update field basic
            appFromDb.Name = application.Name;
            appFromDb.BirthPlace = application.BirthPlace;
            appFromDb.DateOfBirth = application.DateOfBirth;
            appFromDb.Gender = application.Gender;
            appFromDb.PortalCode = application.PortalCode;
            appFromDb.Address = application.Address;

            // 3. Hapus selections lama
            _context.ApplicationSelections.RemoveRange(appFromDb.ApplicationSelections);

            // 4. Tambahkan selections baru
            var newSelections = new List<ApplicationSelection>();
            foreach (var sel in selections)
            {
                var groupItemId = sel.Key;
                var itemOptionId = sel.Value;
                if (itemOptionId == 0) continue;

                var option = await _context.ItemOptions
                    .Include(o => o.GroupItem)
                    .ThenInclude(gi => gi.GroupInformation)
                    .FirstOrDefaultAsync(o => o.Id == itemOptionId && o.GroupItemId == groupItemId);

                if (option != null)
                {
                    var bobotItem = option.BobotF * (option.GroupItem.BobotD / 100m);
                    newSelections.Add(new ApplicationSelection
                    {
                        ApplicationId = appFromDb.Id,
                        GroupItemId = groupItemId,
                        ItemOptionId = itemOptionId,
                        Bobot = bobotItem,
                        HighRisk = option.HighRisk,
                        ItemOption = option
                    });
                }
            }

            appFromDb.ApplicationSelections = newSelections;

            // 5. Hitung ulang skor & risk
            var (score, risk) = _scoreService.CalculateFromAppInstance(appFromDb);
            appFromDb.TotalScore = score;
            appFromDb.RiskCategoryName = risk;

            // 6. Simpan Update
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }


        // GET: application/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var application = await _context.Applications
                .Include(a => a.ApplicationSelections)
                    .ThenInclude(s => s.ItemOption)
                    .ThenInclude(io => io.GroupItem)
                    .ThenInclude(gi => gi.GroupInformation)
                .FirstOrDefaultAsync(m => m.Id == id);
            
            if (application == null) return NotFound();

            var groups = await _context.GroupInformations
                .Include(g => g.GroupItems)
                    .ThenInclude(gi => gi.ItemOptions)
                .ToListAsync();

            ViewData["Groups"] = groups;
            ViewData["Selections"] = application.ApplicationSelections.ToDictionary(s => s.GroupItemId, s => s.ItemOptionId);

            return View(application);
        }

        // POST: application/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var application = await _context.Applications
            .Include(a => a.ApplicationSelections)
            .FirstOrDefaultAsync(a => a.Id == id);

            if (application != null)
            {
                // Hapus semua ApplicationSelections terkait
                if (application.ApplicationSelections != null && application.ApplicationSelections.Any())
                {
                    _context.ApplicationSelections.RemoveRange(application.ApplicationSelections);
                }

                _context.Applications.Remove(application);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        private bool applicationExists(int id)
        {
            return _context.Applications.Any(e => e.Id == id);
        }
    }
}
