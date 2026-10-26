using CourseRegistration.Console.Models;
using CourseRegistration.Console.Repositories;
using CourseRegistration.Console.Services;
using Microsoft.Extensions.DependencyInjection;
using System.ComponentModel.DataAnnotations;

var learners = new List<Learners>
{
    new Learners { Id = 1, Name = "Alice" , Email= "Alice@gmail.com", MeetsPrerequisites = true},
    new Learners { Id = 2, Name = "Bob" , Email = "Bob@gmail.com", MeetsPrerequisites = true },
    new Learners { Id = 3, Name = "Charlie", Email = "Charlie@gmail.com", MeetsPrerequisites = false},
    new Learners { Id = 4, Name = "Arjun" , Email="arjun@gmail.com", MeetsPrerequisites=true}
};

var courseOffering = new CourseOffering
{
    Id = 1,
    Name = "Introduction to Programming",
    Capacity = 2
};


var services = new ServiceCollection();
services.AddSingleton<ICourseRepository>(_=> new InMemoryCourseRepository(learners, courseOffering));
services.AddTransient<CourseEnrollmentService>();

var serviceProvider = services.BuildServiceProvider();

var service = serviceProvider.GetRequiredService<CourseEnrollmentService>();

var repository = serviceProvider.GetRequiredService<ICourseRepository>();

displaylearners(learners);
while (true)
{
    Console.WriteLine("\n=== Course Registration ===");
    Console.WriteLine("1. ENROLL a learner");
    Console.WriteLine("2. DROP Enrollment");
    Console.WriteLine("3. VIEW Enrollments");
    Console.WriteLine("4. VIEW Waitlist");
    Console.WriteLine("5. EXIT");
    Console.Write("Select an option (1-5): ");

    var choice = Console.ReadLine();

    switch (choice)
    {
        case "1":
            Console.Write("Enter Learner ID to enroll: ");
            if (int.TryParse(Console.ReadLine(), out int enrollId))
            {
                var result = service.Enroll(enrollId);
                Console.WriteLine($"Enrollment Result: {result}");
            }
            else
            {
                Console.WriteLine("Invalid Learner ID.");
            }
            break;
        case "2":
            Console.Write("Enter Learner ID to drop: ");
            if (int.TryParse(Console.ReadLine(), out int dropId))
            {
                var result = service.DropEnrollment(dropId);
                Console.WriteLine($"Drop Result: {result}");
            }
            else
            {
                Console.WriteLine("Invalid Learner ID.");
            }
            break;
        case "3":
            var enrollments = repository.GetEnrollments();
            Console.WriteLine("\nCurrent Enrollments:");
            foreach (var enrollment in enrollments)
            {
                var learner = repository.GetLearnerById(enrollment.LearnerId);
                Console.WriteLine($"Learner ID: {enrollment.LearnerId}, Name: {learner?.Name}, Enrolled At: {enrollment.EnrolledAt}");
            }
            break;
        case "4":
            var waitlistEntries = repository.GetWaitlistEntries();
            Console.WriteLine("\nCurrent Waitlist:");
            foreach (var waitlist in waitlistEntries)
            {
                var learner = repository.GetLearnerById(waitlist.LearnerId);
                Console.WriteLine($"Learner ID: {waitlist.LearnerId}, Name: {learner?.Name}, Sequence: {waitlist.Sequence}");
            }
            break;
        case "5":
            Console.WriteLine("Exiting...");
            return;
        default:
            Console.WriteLine("Invalid option. Please select a number between 1 and 5.");
            break;
    }

}

static void displaylearners(IEnumerable<Learners> learners)
{
    Console.WriteLine("\n=== Learners List ===");
    Console.WriteLine("{0,-10} {1,-15} {2,-30} {3}", "ID", "Name", "Email", "Prerequisites Met");
    foreach (var learner in learners)
    {
        Console.WriteLine("{0,-10} {1,-15} {2,-30} {3}", learner.Id, learner.Name, learner.Email, learner.MeetsPrerequisites);
    }
}
