using System.ComponentModel.DataAnnotations;

namespace CourseRegistration.Console.Models
{
    public class Learners
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public bool MeetsPrerequisites { get; set; }
    }
}
