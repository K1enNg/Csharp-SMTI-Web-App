using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using FinalExam3.Models;

namespace FinalExam3.Controllers
{
    public class AccountsController : Controller
    {
        private readonly FinalExam3Context _context;

        public AccountsController(FinalExam3Context context)
        {
            _context = context;
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(int userCode, string password)
        {
            if (userCode <= 0 || string.IsNullOrEmpty(password))
            {
                ViewBag.ErrorMessage = "User Code and Password are required.";
                return View();
            }

            var user = _context.Users
                .Include(u => u.UserCodeNavigation)
                .FirstOrDefault(u => u.UserCode == userCode && u.Password == password);

            if (user == null)
            {
                ViewBag.ErrorMessage = "Invalid User Code or Password.";
                return View();
            }

            var student = user.UserCodeNavigation;
            if (student == null)
            {
                ViewBag.ErrorMessage = "No associated student found for this user.";
                return View();
            }

            SetCookies("UserId", user.UserCode.ToString());
            SetCookies("UserType", "Student");
            SetCookies("StudentName", $"{student.FirstName} {student.LastName}");

            return RedirectToAction("Dashboard", "Students");
        }


        private void SetCookies(string key, string value, int? expireTime = null)
        {
            CookieOptions options = new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict
            };

            options.Expires = expireTime.HasValue
                ? DateTime.Now.AddMinutes(expireTime.Value)
                : DateTime.Now.AddDays(7);

            Response.Cookies.Append(key, value, options);
        }
    }
}
