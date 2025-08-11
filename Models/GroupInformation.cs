using System.Dynamic;
using System.ComponentModel.DataAnnotations;

namespace WebScoringApp.Models
{
    public class GroupInformation
    {
        public int Id { get; set; }
        [Display(Name = "Group Name")]
        public string Name { get; set; } = string.Empty;
        [Display(Name = "Bobot %")]
        public decimal BobotB { get; set; }

        public ICollection<GroupItem> GroupItems { get; set; } = new List<GroupItem>();

    }
}