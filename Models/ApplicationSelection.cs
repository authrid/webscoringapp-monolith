using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace WebScoringApp.Models
{
    public class ApplicationSelection
    {
        public int Id { get; set; }

        public int ApplicationId { get; set; }
        [BindNever]
        public Application? Application { get; set; }

        public int GroupItemId { get; set; }
        [BindNever]
        public GroupItem? GroupItem { get; set; }

        public int ItemOptionId { get; set; }
        [BindNever]
        public ItemOption? ItemOption { get; set; }

        public decimal Bobot { get; set; }
        public bool HighRisk { get; set; }
        public string HighRiskText => HighRisk ? "Ya" : "Tidak";
    }
}
