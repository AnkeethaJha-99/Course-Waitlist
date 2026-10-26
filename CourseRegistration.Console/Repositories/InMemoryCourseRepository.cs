using CourseRegistration.Console.Models;

namespace CourseRegistration.Console.Repositories
{
    public class InMemoryCourseRepository : ICourseRepository
    {
        private readonly Dictionary<int, Learners> _learnersById = new();
        private readonly Dictionary<string, int> _emailToId = new(StringComparer.OrdinalIgnoreCase);
        private readonly List<Enrollment> _enrollments = new();
        private readonly List<WaitlistEntry> _waitlist = new();
        private readonly CourseOffering _courseOffering;

        public InMemoryCourseRepository(IEnumerable<Learners> learners, CourseOffering courseOffering)
        {
            _courseOffering = courseOffering ?? throw new ArgumentNullException(nameof(courseOffering));

            foreach (var learner in learners)
            {
                if (_learnersById.ContainsKey(learner.Id))
                    throw new ArgumentException($"Duplicate learner Id: {learner.Id}");

                if (learner == null || string.IsNullOrWhiteSpace(learner.Email))
                    throw new ArgumentException("Learner or Email is null/empty");

                if (_emailToId.ContainsKey(learner.Email))
                    throw new ArgumentException($"Duplicate learner email: {learner.Email}");

                _learnersById[learner.Id] = learner;
                _emailToId[learner.Email] = learner.Id;
            }
        }

        public Learners? GetLearnerById(int id)
        {
            return _learnersById.TryGetValue(id, out var learner) ? learner : null;
        }

        public CourseOffering GetCourseOffering()
        {
            return _courseOffering;
        }

        public List<Enrollment> GetEnrollments()
        {
            // Return a copy to prevent external modification
            return _enrollments.Select(e => new Enrollment
            {
                LearnerId = e.LearnerId,
                EnrolledAt = e.EnrolledAt
            }).ToList();
        }

        public List<WaitlistEntry> GetWaitlistEntries()
        {
            // Return a copy, ordered by sequence
            return _waitlist
                .OrderBy(w => w.Sequence)
                .Select(w => new WaitlistEntry
                {
                    LearnerId = w.LearnerId,
                    Sequence = w.Sequence
                }).ToList();
        }

        public void AddEnrollment(Enrollment enrollment)
        {
            if (enrollment == null)
                throw new ArgumentNullException(nameof(enrollment));

            if (!_learnersById.ContainsKey(enrollment.LearnerId))
                throw new ArgumentException("Learner does not exist");

            if (_enrollments.Any(e => e.LearnerId == enrollment.LearnerId))
                throw new InvalidOperationException("Learner is already enrolled");

            if (_waitlist.Any(w => w.LearnerId == enrollment.LearnerId))
                throw new InvalidOperationException("Learner is on the waitlist");

            _enrollments.Add(new Enrollment
            {
                LearnerId = enrollment.LearnerId,
                EnrolledAt = enrollment.EnrolledAt
            });
        }

        public void RemoveEnrollment(int learnerId)
        {
            _enrollments.RemoveAll(e => e.LearnerId == learnerId);
        }

        public void AddToWaitlist(WaitlistEntry waitlist)
        {
            if (waitlist == null)
                throw new ArgumentNullException(nameof(waitlist));

            if (!_learnersById.ContainsKey(waitlist.LearnerId))
                throw new ArgumentException("Learner does not exist");

            if (_enrollments.Any(e => e.LearnerId == waitlist.LearnerId))
                throw new InvalidOperationException("Learner is already enrolled");

            if (_waitlist.Any(w => w.LearnerId == waitlist.LearnerId))
                throw new InvalidOperationException("Learner is already on the waitlist");

            int nextSequence = _waitlist.Count == 0 ? 1 : _waitlist.Max(w => w.Sequence) + 1;

            _waitlist.Add(new WaitlistEntry
            {
                LearnerId = waitlist.LearnerId,
                Sequence = nextSequence
            });
        }

        public void RemoveFromWaitlist(int learnerId)
        {
            _waitlist.RemoveAll(w => w.LearnerId == learnerId);

            // Re-sequence the waitlist to maintain order
            int seq = 1;
            foreach (var entry in _waitlist.OrderBy(w => w.Sequence))
            {
                entry.Sequence = seq++;
            }
        }
    }
}