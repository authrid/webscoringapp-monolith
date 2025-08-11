using System.ComponentModel.DataAnnotations;

namespace WebScoringApp.Models
{
    public class Application
    {
        public int Id { get; set; }

        [Display(Name = "No. Aplikasi")]
        public string AppNo { get; set; } = string.Empty;
        [Display(Name = "Nama")]
        public string Name { get; set; } = string.Empty;
        [Display(Name = "Tempat Lahir")]
        public string BirthPlace { get; set; } = string.Empty;
        [Display(Name = "Tgl. Lahir")]
        public DateOnly DateOfBirth { get; set; }
        [Display(Name = "Jenis Kelamin")]
        public string Gender { get; set; } = string.Empty;
        [Display(Name = "Kode Pos")]
        public int PortalCode { get; set; }
        [Display(Name = "Alamat")]
        public string Address { get; set; } = string.Empty;
        [Display(Name = "Score")]
        public decimal TotalScore { get; set; }
        [Display(Name = "Risk Category")]
        public string RiskCategoryName { get; set; } = string.Empty;

        public ICollection<ApplicationSelection> ApplicationSelections { get; set; } = new List<ApplicationSelection>();

    }
}