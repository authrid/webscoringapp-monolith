using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations;

namespace WebScoringApp.Models
{
    public class GroupItem
    {
        public int Id { get; set; }
        [Display(Name = "Item Name")]
        public string Name { get; set; } = string.Empty; // e.g., Umur Pemohon
        [Display(Name = "Bobot %")]
        public decimal BobotD { get; set; } // e.g., 0.3 (30%)
        [Display(Name = "Group")]
        public int GroupInformationId { get; set; }
        [BindNever][Display(Name = "Group")]
        public GroupInformation? GroupInformation { get; set; }

        public ICollection<ItemOption> ItemOptions { get; set; } = new List<ItemOption>();
    }
}