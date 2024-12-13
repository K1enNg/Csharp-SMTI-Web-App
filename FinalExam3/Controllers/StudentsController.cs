using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FinalExam3.Models;

namespace FinalExam3.Controllers
{
    public class StudentsController : Controller
    {
        private readonly FinalExam3Context _context;

        public StudentsController(FinalExam3Context context)
        {
            _context = context;
        }

        public IActionResult Dashboard()
        {
            var userId = Request.Cookies["UserId"];
            if (userId == null)
            {
                return RedirectToAction("Login", "Accounts");
            }

            // Get the logged-in student's details
            int studentNumber = int.Parse(userId);
            var student = _context.Students
                .Include(s => s.Registerations)
                .ThenInclude(r => r.CourseNumberNavigation) // Load related course details
                .FirstOrDefault(s => s.StudentNumber == studentNumber);

            if (student == null)
            {
                return RedirectToAction("Login", "Accounts");
            }

            return View(student);
        }

        public IActionResult CancelRegistration(string courseNumber)
        {
            var userId = Request.Cookies["UserId"];
            if (userId == null)
            {
                return RedirectToAction("Login", "Accounts");
            }

            int studentNumber = int.Parse(userId);

            // Find the registration to cancel
            var registration = _context.Registerations
                .FirstOrDefault(r => r.StudentsNumber == studentNumber && r.CourseNumber == courseNumber);

            if (registration != null)
            {
                _context.Registerations.Remove(registration);
                _context.SaveChanges();
            }

            return RedirectToAction("Dashboard");
        }

        public IActionResult RegisterCourse()
        {
            var userId = Request.Cookies["UserId"];
            if (userId == null)
            {
                return RedirectToAction("Login", "Accounts");
            }

            int studentNumber = int.Parse(userId);

            // Get courses that the student hasn't registered for
            var registeredCourseNumbers = _context.Registerations
                .Where(r => r.StudentsNumber == studentNumber)
                .Select(r => r.CourseNumber)
                .ToList();

            var availableCourses = _context.Courses
                .Where(c => !registeredCourseNumbers.Contains(c.CourseNumber))
                .ToList();

            return View(availableCourses);
        }

        [HttpPost]
        public IActionResult RegisterCourse(string courseNumber)
        {
            var userId = Request.Cookies["UserId"];
            if (userId == null)
            {
                return RedirectToAction("Login", "Accounts");
            }

            int studentNumber = int.Parse(userId);

            // Check if the student is already registered for 4 courses
            var currentRegistrations = _context.Registerations
                .Count(r => r.StudentsNumber == studentNumber);

            if (currentRegistrations >= 4)
            {
                ViewBag.ErrorMessage = "You cannot register for more than 4 courses.";
                return RedirectToAction("Dashboard");
            }

            // Add new registration
            var registration = new Registeration
            {
                StudentsNumber = studentNumber,
                CourseNumber = courseNumber
            };

            _context.Registerations.Add(registration);
            _context.SaveChanges();

            return RedirectToAction("Dashboard");
        }

        public IActionResult AvailableCourses()
        {
            var userId = Request.Cookies["UserId"];
            if (userId == null)
            {
                return RedirectToAction("Login", "Accounts");
            }

            int studentNumber = int.Parse(userId);

            // Get courses the student has already registered for
            var registeredCourseNumbers = _context.Registerations
                .Where(r => r.StudentsNumber == studentNumber)
                .Select(r => r.CourseNumber)
                .ToList();

            // Get courses the student has not registered for
            var availableCourses = _context.Courses
                .Where(c => !registeredCourseNumbers.Contains(c.CourseNumber))
                .ToList();

            return View(availableCourses);
        }

        [HttpPost]
        public IActionResult RegisterForCourse(string courseNumber)
        {
            var userId = Request.Cookies["UserId"];
            if (userId == null)
            {
                return RedirectToAction("Login", "Accounts");
            }

            int studentNumber = int.Parse(userId);

            // Check if the student has reached the maximum course limit
            var registeredCount = _context.Registerations.Count(r => r.StudentsNumber == studentNumber);
            if (registeredCount >= 4)
            {
                TempData["ErrorMessage"] = "You cannot register for more than 4 courses.";
                return RedirectToAction("AvailableCourses");
            }

            // Save the new course registration
            var registration = new Registeration
            {
                StudentsNumber = studentNumber,
                CourseNumber = courseNumber
            };

            _context.Registerations.Add(registration);
            _context.SaveChanges();

            TempData["SuccessMessage"] = "Course registered successfully!";
            return RedirectToAction("Dashboard");
        }
    }
}
