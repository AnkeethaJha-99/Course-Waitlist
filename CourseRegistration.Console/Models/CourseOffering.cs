using System.ComponentModel.DataAnnotations;

namespace CourseRegistration.Console.Models
{
    public class CourseOffering
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;

        public int Capacity { get; set; }
    }
}
