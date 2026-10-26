using CourseRegistration.Console.Models;

namespace CourseRegistration.Console.Repositories
{
    public interface ICourseRepository
    {
        Learners? GetLearnerById(int Id);
        CourseOffering GetCourseOffering();
        List<Enrollment> GetEnrollments();
        List<WaitlistEntry> GetWaitlistEntries();
        void AddEnrollment(Enrollment enrollment);
        void RemoveEnrollment(int learnerId);
        void AddToWaitlist(WaitlistEntry waitlist);
        void RemoveFromWaitlist(int learnerId);
    }
}
