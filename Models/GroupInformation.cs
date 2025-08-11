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

        // public decimal GetAverageBobotB()
        // {
        //     var itemOptions = GroupItems
        //     .Where(gi => gi != null && gi.ItemOptions != null)
        //     .SelectMany(gi => gi.ItemOptions)
        //     .Where(io => io.GroupItem is not null);

        //     if (!itemOptions.Any()) return 0;

        //     decimal totalFD = itemOptions
        //     .Sum(io => io.BobotF * io.GroupItem!.BobotD);
        //     decimal averageFD = totalFD / itemOptions.Count();

        //     return BobotB * averageFD;
        // }
    }
}