using System.ComponentModel.DataAnnotations;

namespace CarPartsStore.Models
{
    public class AdminCandidate
    {
        [Key]
        public int Id { get; set; }

        public string GuID { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }
    }
}
