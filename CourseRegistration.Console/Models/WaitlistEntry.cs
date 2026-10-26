using System.ComponentModel.DataAnnotations.Schema;

namespace CourseRegistration.Console.Models
{
    public class WaitlistEntry
    {
        [ForeignKey("Learners")]
        public int LearnerId { get; set; }
        public int Sequence { get; set; }
    }
}
