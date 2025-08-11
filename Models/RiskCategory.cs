using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations;

namespace WebScoringApp.Models
{
    public class RiskCategory
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;  // "HighRisk", "MediumRisk", "LowRisk"
        public int ScoreMin { get; set; }  
        public int ScoreMax { get; set; }
    }

}