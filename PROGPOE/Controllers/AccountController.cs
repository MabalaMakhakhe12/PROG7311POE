using Microsoft.AspNetCore.Mvc;
using PROGPOE.Data;
using PROGPOE.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;

namespace AgriEnergyConnect.Controllers
{/// <summary>
/// 
/// </summary>
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AccountController(ApplicationDbContext context)
        {
            _context = context;
        }
        /// <summary>
        /// Returns the Login view.
        /// </summary>
        public IActionResult Login()
        {
            return View();
        }
        /// <summary>
        /// Handles the POST request for Login and authenticates the user.
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> Login(string username, string password)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                TempData["ErrorMessage"] = "Username and password are required.";
                return RedirectToAction("Login");
            }

            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserName == username && u.Password == password);
            var farmer = await _context.Farmers.FirstOrDefaultAsync(f => f.UserName == username && f.Password == password);
            var employee = await _context.Employees.FirstOrDefaultAsync(e => e.UserName == username && e.Password == password);

            if (user != null)
            {
                await SignInUser(user.UserName);
                return RedirectToAction("NormalUserDashboard", "NormalUserDashboard");
            }
            else if (farmer != null)
            {
                await SignInUser(farmer.UserName);
                return RedirectToAction("FarmerDashboard", "FarmerDashboard");
            }
            else if (employee != null)
            {
                await SignInUser(employee.UserName);
                return RedirectToAction("EmployeeDashboard", "EmployeeDashboard");
            }

            TempData["ErrorMessage"] = "Invalid username or password.";
            return RedirectToAction("Login");
        }

        /// <summary>
        /// Signs in the user with the given username taken in as a parameter
        /// </summary>
        private async Task SignInUser(string username)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, username)
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            var authProperties = new AuthenticationProperties
            {
                IsPersistent = true
            };

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties);
        }

        /// <summary>
        /// Returns the Register view.
        /// </summary>
        public IActionResult Register()
        {
            return View();
        }
        /// <summary>
        /// Handles the POST request for Register and registers a new user.
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> Register(string username, string password, string confirmPassword, string role, string name, string surname, string email, string contact, string address, string postcode)
        {
            var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.UserName == username);
            var existingFarmer = await _context.Farmers.FirstOrDefaultAsync(f => f.UserName == username);
            var existingEmployee = await _context.Employees.FirstOrDefaultAsync(e => e.UserName == username);

            if (existingUser != null || existingFarmer != null || existingEmployee != null)
            {
                TempData["ErrorMessage"] = "Username already exists.";
                return RedirectToAction("Register");
            }

            if (role == "NormalUser")
            {
                var user = new Users
                {
                    UserName = username,
                    Password = password,
                    Name = name,
                    Surname = surname,
                    Email = email,
                    mobileNo = contact,
                    Address = address,
                    PostCode = postcode,
                    CreatedDate = "MoreGarbage", // Placeholder
                    ImageUrl = "Garbage" // Placeholder
                };
                _context.Users.Add(user);
            }
            else if (role == "Employee")
            {
                var employee = new Employees
                {
                    UserName = username,
                    Name = name,
                    Surname = surname,
                    Email = email,
                    mobile = contact,
                    Address = address,
                    Password = password,
                    ConfirmPassword = password
                };
                _context.Employees.Add(employee);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction("Login");
        }
    }
}

