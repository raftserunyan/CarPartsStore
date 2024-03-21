using System.ComponentModel.DataAnnotations;

namespace CarPartsStore.Models
{
    public class TeamMember
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }

        public string ImageUrl { get; set; }
        [Required]
        public string Position { get; set; }

        public string FacebookLink { get; set; }

        public string TwitterLink { get; set; }

        public string LinkedinLink{ get; set; }
    }
}
