# AI Usage Log

**Tool:** GitHub Copilot Chat in Visual Studio 2022

---

## Objective

Implement the in-memory repository for the course registration assessment.

---

## Prompts & Responses

### 1. InMemoryCourseRepository Implementation

**Prompt Used:**
> Implement InMemoryCourseRepository based on my existing ICourseRepository and Models -  
> • Store the learners in dictionary by id and enrollments and waitlist entries in lists.  
> • Accept sample learners and one course offering through constructor.  
> • Implement all interface methods.  
> • Compare emails case-insensitivity and reject duplicate learner Ids and emails  
> • Return copies of enrollment and waitlist collections, with the waitlist ordered by sequence.  
> • Prevent duplicate enrollments, duplicate waitlist entries and a learner being both enrolled and waitlisted.  
> Dont modify unrelated files.

**AI Contribution:**
- GitHub Copilot generated the implementation of `InMemoryCourseRepository` based on the existing repository interface and models.

**Decision:**  
Accepted with minor edits.

**My Review and Changes:**
- Reviewed the generated implementation.
- Removed unused `using` statements.
- Verified the null checks.
- Removed unnecessary conditions after reviewing the repository's responsibilities and the service-layer design.
- Reviewed the separation between data access in the repository and enrollment business rules in the service layer.

**Files Affected:**
- `Repositories/InMemoryCourseRepository.cs`

---

### 2. CourseEnrollmentService Review

**Prompt Used:**
> #file:'CourseEnrollmentService.cs' Review my service. Validate both methods - Enroll and DropEnrollment() for correctness, edge cases, duplicate requests, prerequisite checks, capacity limits, FIFO waitlist promotion and invalid or repeated drops.  
> Check for null references, incorrect return results and inconsistent repository states. Verify the ineligible waitlisted learners are skipped and earliest eligible learner is promoted.  
> DO NOT REWRITE MY CODE OR MODIFY ANY FILES. Report the issues found, explain why each is an issue and suggest minimal fixes.

**Response:**
- **General Observations:**
  - Null Reference Risks: Several repository methods are assumed to never return null. Defensive programming would check for nulls.
  - Thread Safety: Not addressed, but acceptable for in-memory/single-threaded use.
  - Error Logging: Exceptions are caught but not logged. Consider logging for diagnostics.
  - Return Results: Generally correct and consistent with method outcomes.

**My Review and Changes:**
- Updated the capacity logic.
- Verified the waitlist sequence generation.

---

### 3. xUnit Unit Tests for CourseEnrollmentService

**Prompt Used:**
> Write xUnit unit tests for my existing #file:'CourseEnrollmentService.cs' using my current models, repository, and result enum.  
> Cover:  
> - Enrollment when seats are available and when the course is full.  
> - Duplicate enrollment and duplicate waitlist requests.  
> - Missing learner and prerequisite failure.  
> - Successful drop, invalid drop, and repeated drop.  
> - FIFO waitlist promotion, skipping ineligible learners while retaining them on the waitlist.  
> - No eligible learner available for promotion.  
> - Verify both returned results and repository state after each operation.  
> Use a fresh in-memory repository for each test. Follow my existing project conventions and use clear test names. Do not modify production code or unrelated files. Ensure the tests compile and run.

**Response Summary:**
- Provided 11 test cases.
- Each test uses a fresh `InMemoryCourseRepository` and `CourseEnrollmentService`.
- Tests cover all requested scenarios, including edge cases and repository state checks.
- Test names are descriptive and follow your conventions.
- The tests assume your `InMemoryCourseRepository` and models behave as specified in your requirements.

**My Review and Changes:**
- Checked and updated correct assertions.
- Ran the test cases and updated the methods for failed test cases.

---
