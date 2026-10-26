#  Course Registration Console Application

A .NET console application for managing course enrollment, enrollment drops, and **FIFO (First-In, First-Out) waitlist promotion**. The application demonstrates course capacity management, prerequisite validation, and automated waitlist handling using an in-memory repository.

---

## Table of Contents

1. [Overview](#-overview)
2. [Features](#-features)
3. [Prerequisites](#-prerequisites)
4. [Locate the Solution or Project Files](#-locate-the-solution-or-project-files)
5. [Restore and Build](#-restore-and-build)
6. [Run the Console Application](#-run-the-console-application)
7. [Run the Automated Tests](#-run-the-automated-tests)
8. [Assumptions and Limitations](#-assumptions-and-limitations)
9. [Verification Results](#-verification-results)

---

##  Overview

The application manages enrollment for a **single course offering** through a console-based interface.

Learner and course data are initialized at startup and stored in memory. When a seat becomes available, the application attempts to promote the earliest eligible learner from the waitlist.

##  Features

- **Course Enrollment:** Enroll eligible learners when seats are available.
- **Waitlist Management:** Add eligible learners to the waitlist when the course reaches capacity.
- **Duplicate Prevention:** Prevent duplicate enrollment and waitlist entries.
- **Enrollment Drop:** Allow learners to drop an existing enrollment.
- **FIFO Promotion:** Promote the earliest eligible waitlisted learner when a seat becomes available.
- **Prerequisite Validation:** Skip ineligible learners during promotion while retaining them on the waitlist.
- **Error Handling:** Return clear results for invalid requests, missing learners, and repeated drop attempts.

---

##  Prerequisites

Ensure the following are available before running the application:

- [Visual Studio 2022](https://visualstudio.microsoft.com/) or a compatible .NET development environment.
- The .NET SDK version targeted by the project.
- The console application project and its associated xUnit test project.

To verify your installed .NET SDK, run:

```bash
dotnet --version
```

---

##  Locate the Solution or Project Files

Open the **Developer Command Prompt for Visual Studio** and navigate to your repository directory:

```bat
cd /d C:\Users\user\source\repos\Course-Waitlist
```

### Find the solution file

```bat
dir /s /b *.sln
```

If a solution file is found, use its full path for the build and test commands.

### Find the project files

If no solution file exists, search for individual project files:

```bat
dir /s /b *.csproj
```

Identify the paths for:

- The **console application project**.
- The **xUnit test project**.

Replace the placeholder paths in the following commands with the actual paths discovered above.

---

##  Restore and Build

If a solution file exists, restore dependencies and build the solution:

```bash
dotnet restore "FULL_PATH_TO_SOLUTION.sln"
dotnet build "FULL_PATH_TO_SOLUTION.sln"
```

If no solution file exists, run the commands against the appropriate project files instead:

```bash
dotnet restore "FULL_PATH_TO_CONSOLE_PROJECT.csproj"
dotnet build "FULL_PATH_TO_CONSOLE_PROJECT.csproj"
```

**Expected result:** The build should complete successfully without errors before you proceed to run the application.

---

## ▶ Run the Console Application

Start the application using the console project's path:

```bash
dotnet run --project "FULL_PATH_TO_CONSOLE_PROJECT.csproj"
```

On startup, the application initializes the sample learners and course offering and displays the available learners with their IDs.

Use the displayed learner IDs to perform enrollment, drop, and other operations supported by the console interface.

---

##  Run the Automated Tests

Run the xUnit test suite using the solution file, if available:

```bash
dotnet test "FULL_PATH_TO_SOLUTION.sln"
```

Alternatively, execute the test project directly:

```bash
dotnet test "FULL_PATH_TO_TEST_PROJECT.csproj"
```

Make sure the specified project is the **xUnit test project**, not the console application project.

### Test Coverage

The automated tests should cover the following scenarios:

- Enrollment when seats are available.
- Waitlist placement when the course is full.
- Duplicate enrollment and waitlist requests.
- Missing learners and prerequisite failures.
- Successful, invalid, and repeated enrollment drops.
- FIFO waitlist promotion when a seat becomes available.
- Skipping ineligible learners while retaining them on the waitlist.
- No eligible learner available for promotion.

---

##  Assumptions and Limitations

- **Single Course:** Only one course offering is supported.
- **In-Memory Storage:** Learner, course, and enrollment data are reset when the application restarts.
- **Prerequisite Eligibility:** Eligibility is represented by a Boolean property.
- **Waitlist Retention:** Ineligible learners remain on the waitlist but are skipped during promotion.
- **No Persistence:** Database storage and long-term data persistence are not implemented.
- **No Concurrency Support:** Concurrent enrollment requests and synchronization are outside the scope of this prototype.

---

##  Verification Results

Record the actual results after executing the build, test, and manual verification steps.

| Verification | Actual Result |
|---|---|
| `dotnet restore` | Restore complete |
| `dotnet build` | build succeeded |
| `dotnet test` | Test summary: total: 11, failed: 0, succeeded: 11, skipped: 0 |
| `dotnet run` | Opened the console app with learners list and operation options |

---

##  License


---

*Built with .NET and C#, with xUnit for automated testing.*
