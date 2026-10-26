using CourseRegistration.Console.Models;
using CourseRegistration.Console.Repositories;
using CourseRegistration.Console.Results;

namespace CourseRegistration.Console.Services
{
    public class CourseEnrollmentService
    {
        private readonly ICourseRepository _courseRepository;

        public CourseEnrollmentService(ICourseRepository courseRepository)
        {
            _courseRepository = courseRepository;
        }

        public EnrollmentResult Enroll(int learnerId)
        {
            try
            {
                //validate learner exists
                var learner = _courseRepository.GetLearnerById(learnerId);
                if (learner == null)
                {
                    return EnrollmentResult.LearnerNotFound; 
                }

                //check prerequisites
                if (learner.MeetsPrerequisites)
                {
                    //check if already enrolled
                    var enrollments = _courseRepository.GetEnrollments();
                    if (enrollments.Any(e => e.LearnerId == learnerId))
                    {
                        return EnrollmentResult.AlreadyEnrolled;
                    }

                    //check if already waitlisted
                    var waitlistEntries = _courseRepository.GetWaitlistEntries();
                    if (waitlistEntries.Any(w => w.LearnerId == learnerId))
                    {
                        return EnrollmentResult.AlreadyWaitlisted;
                    }

                    //check if course is full
                    var courseOffering = _courseRepository.GetCourseOffering();
                    if (enrollments.Count < courseOffering.Capacity)
                    {
                        //enroll the learner
                        var enrollment = new Models.Enrollment
                        {
                            LearnerId = learnerId,
                            EnrolledAt = DateTime.UtcNow
                        };
                        _courseRepository.AddEnrollment(enrollment);
                        return EnrollmentResult.Enrolled;
                    }
                    else
                    {
                        //add to waitlist
                        var waitlistEntry = new WaitlistEntry
                        {
                            LearnerId = learnerId                            
                        };
                        _courseRepository.AddToWaitlist(waitlistEntry);
                        return EnrollmentResult.Waitlisted;
                    }
                }
                else
                {
                    return EnrollmentResult.PrerequisiteNotMet;
                }

            }
            catch (Exception ex)
            {
                // Log the exception (not implemented here)
                return EnrollmentResult.NotEnrolled; // Return a generic failure result
            }
        }

        public EnrollmentResult DropEnrollment(int learnerId)
        {
            try
            {
                //validate learner exists
                var learner = _courseRepository.GetLearnerById(learnerId);
                if (learner == null)
                {
                    return EnrollmentResult.LearnerNotFound;
                }
                //check if enrolled
                var enrollments = _courseRepository.GetEnrollments();
                if (enrollments.Any(e => e.LearnerId == learnerId))
                {
                    //remove enrollment
                    _courseRepository.RemoveEnrollment(learnerId);
                    //check waitlist for next learner to enroll
                    var waitlistEntries = _courseRepository.GetWaitlistEntries();
                    
                    if (waitlistEntries.Any())
                    {
                        var nextInLine = waitlistEntries.OrderBy(w => w.Sequence);

                        //find the eligible learner from the waitlist who meets prerequisites
                        var eligibleLearner = nextInLine
                            .Select(w => _courseRepository.GetLearnerById(w.LearnerId))
                            .FirstOrDefault(l => l != null && l.MeetsPrerequisites);

                        //check if there are any eligible learners in the waitlist
                        if (eligibleLearner != null)
                        {
                            var enrollment = new Models.Enrollment
                            {
                                LearnerId = eligibleLearner.Id,
                                EnrolledAt = DateTime.UtcNow
                            };
                            _courseRepository.AddEnrollment(enrollment);
                            _courseRepository.RemoveFromWaitlist(eligibleLearner.Id);
                        }
                    }
                    return EnrollmentResult.Dropped; // Successfully dropped
                }
                else
                {
                    return EnrollmentResult.NotEnrolled; // Not enrolled in the course
                }
            }
            catch (Exception ex)
            {
                // Log the exception (not implemented here)
                return EnrollmentResult.NotEnrolled; // Return a generic failure result
            }
        }


    }
}

