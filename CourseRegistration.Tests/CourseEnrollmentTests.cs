using CourseRegistration.Console.Models;
using CourseRegistration.Console.Repositories;
using CourseRegistration.Console.Results;
using CourseRegistration.Console.Services;

namespace CourseRegistration.Tests
{
    public class CourseEnrollmentTests
    {
        private static (InMemoryCourseRepository repo, CourseEnrollmentService service, List<Learners> learners) SetupRepository(
            int capacity = 2,
            IEnumerable<Learners>? customLearners = null)
        {
            var learners = customLearners?.ToList() ?? new List<Learners>
            {
                new Learners { Id = 1, Email = "a@example.com", Name = "A", MeetsPrerequisites = true },
                new Learners { Id = 2, Email = "b@example.com", Name = "B", MeetsPrerequisites = true },
                new Learners { Id = 3, Email = "c@example.com", Name = "C", MeetsPrerequisites = true },
                new Learners { Id = 4, Email = "d@example.com", Name = "D", MeetsPrerequisites = false }
            };
            var course = new CourseOffering { Id = 1, Name = "Test Course", Capacity = capacity };
            var repo = new InMemoryCourseRepository(learners, course);
            var service = new CourseEnrollmentService(repo);
            return (repo, service, learners);
        }

        [Fact]
        public void Enroll_WhenSeatsAvailable_EnrollsLearner()
        {
            var (repo, service, learners) = SetupRepository();
            var result = service.Enroll(learners[0].Id);

            Assert.Equal(EnrollmentResult.Enrolled, result);
            Assert.Single(repo.GetEnrollments());
            Assert.Empty(repo.GetWaitlistEntries());
            Assert.Equal(learners[0].Id, repo.GetEnrollments().First().LearnerId);
        }

        [Fact]
        public void Enroll_WhenCourseFull_AddsToWaitlist()
        {
            var (repo, service, learners) = SetupRepository(capacity: 2);

            service.Enroll(learners[0].Id);
            service.Enroll(learners[1].Id);
            var result = service.Enroll(learners[2].Id);

            Assert.Equal(EnrollmentResult.Waitlisted, result);
            Assert.Equal(2, repo.GetEnrollments().Count);
            Assert.Single(repo.GetWaitlistEntries());
            Assert.Equal(learners[2].Id, repo.GetWaitlistEntries().First().LearnerId);
        }

        [Fact]
        public void Enroll_DuplicateEnrollment_ReturnsAlreadyEnrolled()
        {
            var (repo, service, learners) = SetupRepository();

            service.Enroll(learners[0].Id);
            var result = service.Enroll(learners[0].Id);

            Assert.Equal(EnrollmentResult.AlreadyEnrolled, result);
            Assert.Single(repo.GetEnrollments());
        }

        [Fact]
        public void Enroll_DuplicateWaitlist_ReturnsAlreadyWaitlisted()
        {
            var (repo, service, learners) = SetupRepository(capacity: 1);

            service.Enroll(learners[0].Id); // fills seat
            service.Enroll(learners[1].Id); // waitlisted
            var result = service.Enroll(learners[1].Id);

            Assert.Equal(EnrollmentResult.AlreadyWaitlisted, result);
            Assert.Single(repo.GetWaitlistEntries());
        }

        [Fact]
        public void Enroll_MissingLearner_ReturnsLearnerNotFound()
        {
            var (repo, service, _) = SetupRepository();
            var result = service.Enroll(999);

            Assert.Equal(EnrollmentResult.LearnerNotFound, result);
        }

        [Fact]
        public void Enroll_PrerequisiteFailure_ReturnsPrerequisiteNotMet()
        {
            var (repo, service, learners) = SetupRepository();
            var ineligible = learners.First(l => !l.MeetsPrerequisites);

            var result = service.Enroll(ineligible.Id);

            Assert.Equal(EnrollmentResult.PrerequisiteNotMet, result);
            Assert.Empty(repo.GetEnrollments());
            Assert.Single(repo.GetWaitlistEntries());
        }

        [Fact]
        public void DropEnrollment_SuccessfulDrop_PromotesNextEligibleWaitlisted()
        {
            var (repo, service, learners) = SetupRepository(capacity: 1);

            service.Enroll(learners[0].Id); // enrolled
            service.Enroll(learners[1].Id); // enrolled
            var dropResult = service.DropEnrollment(learners[0].Id);

            Assert.Equal(EnrollmentResult.Dropped, dropResult);
            Assert.Single(repo.GetEnrollments());
            Assert.Empty(repo.GetWaitlistEntries());
            Assert.Equal(learners[1].Id, repo.GetEnrollments().First().LearnerId);
        }

        [Fact]
        public void DropEnrollment_InvalidDrop_ReturnsNotEnrolled()
        {
            var (repo, service, learners) = SetupRepository();

            var result = service.DropEnrollment(learners[0].Id);

            Assert.Equal(EnrollmentResult.NotEnrolled, result);
        }

        [Fact]
        public void DropEnrollment_RepeatedDrop_ReturnsNotEnrolled()
        {
            var (repo, service, learners) = SetupRepository();

            service.Enroll(learners[0].Id);
            service.DropEnrollment(learners[0].Id);
            var result = service.DropEnrollment(learners[0].Id);

            Assert.Equal(EnrollmentResult.NotEnrolled, result);
        }

        [Fact]
        public void DropEnrollment_FifoWaitlistPromotion_SkipsIneligibleLearners()
        {
            var (repo, service, learners) = SetupRepository(capacity: 1);

            // Enroll one, fill course
            service.Enroll(learners[0].Id);

            // Add two to waitlist: first ineligible, second eligible
            service.Enroll(learners[3].Id); // ineligible
            service.Enroll(learners[1].Id); // eligible

            // Drop the enrolled learner, should promote eligible (learners[1])
            var dropResult = service.DropEnrollment(learners[0].Id);

            Assert.Equal(EnrollmentResult.Dropped, dropResult);
            Assert.Single(repo.GetEnrollments());
            Assert.Single(repo.GetWaitlistEntries()); // ineligible remains
            Assert.Equal(learners[1].Id, repo.GetEnrollments().First().LearnerId);
            Assert.Equal(learners[3].Id, repo.GetWaitlistEntries().First().LearnerId);
        }

        [Fact]
        public void DropEnrollment_NoEligibleWaitlistedLearner_NoPromotion()
        {
            var (repo, service, learners) = SetupRepository(capacity: 1);

            service.Enroll(learners[0].Id);
            service.Enroll(learners[3].Id); // ineligible waitlisted

            var dropResult = service.DropEnrollment(learners[0].Id);

            Assert.Equal(EnrollmentResult.Dropped, dropResult);
            Assert.Empty(repo.GetEnrollments());
            Assert.Single(repo.GetWaitlistEntries());
            Assert.Equal(learners[3].Id, repo.GetWaitlistEntries().First().LearnerId);
        }
    }
}