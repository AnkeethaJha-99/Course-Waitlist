using System.ComponentModel.DataAnnotations.Schema;

namespace CourseRegistration.Console.Models
{
    public class Enrollment
    {
        [ForeignKey("Learners")]
        public int LearnerId { get; set; }
        public DateTime EnrolledAt { get; set; }
    }
}
