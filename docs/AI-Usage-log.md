Tool: GitHub Copilot Chat in Visual Studio 2022

Objective: Implement the in-memory repository for the course registration assessment.

Prompt used:

«Implement InMemoryCOurseRepository based on my existing ICourseRepository and Models -        
•	Store the learners in dictionary by id and enrollments and waitlist entries in lists.
•	Accept sampe learners and one course offering through constrcutor. -Implement all interface methods. -Compare emails case-insensitivity and reject duplicate learner Ids and emails
•	return copies of enrollment and waitlist collections, wit the waitlist ordered by sequence.
•	prevent duplicate enrollments, duplicate waitlist entries and a learner being both enrolled and waitlisted.
Dont modify unrelated files.»

AI contribution:

GitHub Copilot generated the implementation of "InMemoryCourseRepository" based on the existing repository interface and models.

Decision: Accepted with minor edits.

My review and changes:

- Reviewed the generated implementation.
- Removed unused "using" statements.
- Verified the null checks.
- Removed unnecessary conditions after reviewing the repository's responsibilities and the service-layer design.
- Reviewed the separation between data access in the repository and enrollment business rules in the service layer.

Files affected:
- "Repositories/InMemoryCourseRepository.cs"
