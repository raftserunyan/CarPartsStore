using System;
using System.ComponentModel.DataAnnotations;

namespace CarPartsStore.Models
{
    public class TimelineModel
    {
        [Key]
        public int Id { get; set; }

        public string ImageUrl { get; set; }
        [Required]
        [UIHint("Date")]
        public DateTime Date { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Description { get; set; }

        public string? Side { get; set; }
    }
}
