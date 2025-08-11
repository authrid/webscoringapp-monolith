using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations;

namespace WebScoringApp.Models
{
    public class ItemOption
    {
        public int Id { get; set; }
        [Display(Name = "Option Name")]
        public string Name { get; set; } = string.Empty; // e.g., 21 - 30 Tahun
        [Display(Name = "Bobot")]
        public int BobotF { get; set; } // e.g., 50
        [Display(Name = "Item")]
        public int GroupItemId { get; set; }

        [BindNever]
        [Display(Name = "Item")]
        public GroupItem? GroupItem { get; set; }

        public bool HighRisk { get; set; }
        public string HighRiskText => HighRisk ? "Ya" : "Tidak";
    }
}