using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace test0000001.Models
{
    public class Home_Insurance
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Address { get; set; }
        public string? Gmail { get; set; }
        public string? YearBuilt { get; set; }
        public string? Structure { get; set; }
        public string? NumberOfBasement { get; set; }
        public int Area { get; set; }
        public string? LinkDriver { get; set; }
        public string? Status { get; set; } = "Not approved yet";
        //[ForeignKey(nameof(Policy))]
        //public int PolicyId { get; set; } // Foreign Key
        public virtual Policy? Policy { get; set; }
        [ForeignKey(nameof(Duration))]
        public int DurationId { get; set; } // Foreign Key
        public virtual Duration? Duration { get; set; } 
        //public ICollection<Photos>? Photos { get; set; }


    }
}
