using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using WebScoringApp.Data;
using WebScoringApp.Models;

namespace WebScoringApp.Services
{
    public class ScoreService
    {
        private readonly AppDbContext _context;

        public ScoreService(AppDbContext context)
        {
            _context = context;
        }

        // Tetap ada untuk Create/Edit yang mau hitung dari DB
        public (decimal Score, string Risk) CalculateFinalScore(int applicationId)
        {
            var app = _context.Applications
                .Include(a => a.ApplicationSelections)
                    .ThenInclude(s => s.ItemOption)
                        .ThenInclude(io => io.GroupItem)
                            .ThenInclude(gi => gi.GroupInformation)
                .FirstOrDefault(a => a.Id == applicationId);

            return CalculateFromAppInstance(app);
        }

        // Overload untuk hitung langsung dari object Application yang sudah ada di memory
        public (decimal Score, string Risk) CalculateFromAppInstance(Application app)
        {
            var riskCategories = _context.RiskCategories.ToList();

            if (app == null || app.ApplicationSelections == null)
                return (0, "Unknown");

            var groupedScores = app.ApplicationSelections
                .Where(s => s.ItemOption?.GroupItem?.GroupInformation != null)
                .GroupBy(s => s.ItemOption.GroupItem.GroupInformation)
                .Select(g =>
                {
                    var sumBobot = g.Sum(s => s.Bobot);
                    var bobotB = g.Key.BobotB;
                    return sumBobot * (bobotB / 100);
                });

            var totalScore = groupedScores.Sum();
            bool hasHighRiskItem = app.ApplicationSelections.Any(s => s.HighRisk);

            string riskLevel = hasHighRiskItem
                ? "High Risk"
                : riskCategories.FirstOrDefault(rc => totalScore >= rc.ScoreMin && totalScore <= rc.ScoreMax)?.Name ?? "Unknown";

            return (totalScore, riskLevel);
        }
    }

}
