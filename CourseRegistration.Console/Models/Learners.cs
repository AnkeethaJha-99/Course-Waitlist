using System.ComponentModel.DataAnnotations;

namespace CourseRegistration.Console.Models
{
    public class Learners
    {
        [Required]
        [EmailAddress]
        [Key]
        public string Email { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public bool MeetsPrerequisites { get; set; }
    }
}
