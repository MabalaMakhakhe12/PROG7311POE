using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PROGPOE.Data;
using PROGPOE.Models;
using PROGPOE.ViewModels;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace PROGPOE.Controllers
{
    public class EmployeeDashboardController : Controller
    {
        private readonly ApplicationDbContext _context;

        public EmployeeDashboardController(ApplicationDbContext context)
        {
            _context = context;
        }

        //[Authorize(Roles = "Employee")]
        public async Task<IActionResult> EmployeeDashboard()
        {
            var currentEmployee = await _context.Employees.FirstOrDefaultAsync(e => e.UserName == User.Identity.Name);

            if (currentEmployee == null)
            {
                //   TempData["ErrorMessage"] = "Employee not found.";
                return RedirectToAction("Login", "Account"); // Redirect to login page or appropriate error page
            }

            var farmer = new Farmers();
            return View(farmer);
        }

        [HttpGet]
        public IActionResult AddFarmer()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddFarmer(string username, string password, string confirmPassword, string role, string name, string surname, string email, string contact, string address, string postcode)
        {
            var currentEmployee = await _context.Employees.FirstOrDefaultAsync(e => e.UserName == User.Identity.Name);

            var existingFarmer = await _context.Farmers.FirstOrDefaultAsync(f => f.UserName == username);

            if (existingFarmer != null)
            {
                TempData["ErrorMessage"] = "Username already exists.";
                return View("AddFarmer");
            }
            if (password != confirmPassword)
            {
                ModelState.AddModelError("ConfirmPassword", "The password and confirmation password do not match.");
                return RedirectToAction("EmployeeDashboard");
            }
            // Ensure currentEmployee is found
            if (currentEmployee == null)
            {
                TempData["ErrorMessage"] = "Employee not found.";
                return RedirectToAction("Index", "Account"); // Redirect to login page or appropriate error page
            }

            var farmer = new Farmers
            {
                UserName = username,
                Name = name,
                Surname = surname,
                Email = email,
                Contact = contact,
                Address = address,
                Password = password,
                ConfirmPassword = password,
                EmployeeID = currentEmployee.EmployeeID // Assign EmployeeID of the current employee

            };
            _context.Farmers.Add(farmer);


            await _context.SaveChangesAsync();
            return RedirectToAction("EmployeeDashboard");

        }
        public async Task<IActionResult> FarmerProfiles()
        {
            var farmers = await _context.Farmers
                                    .Include(f => f.Employees)
                                    .Where(f => f.EmployeeID != null) // Filter out farmers without associated employees
                                    .ToListAsync();
            return View(farmers);
        }


        public async Task<IActionResult> FarmerDetails(int id)
        {
            var farmer = await _context.Farmers
                .Include(f => f.Products)
                .ThenInclude(p => p.Category)
                .FirstOrDefaultAsync(f => f.FarmerID == id);

            if (farmer == null)
            {
                TempData["ErrorMessage"] = "Farmer not found.";
                return RedirectToAction("FarmerProfiles");
            }

            return View(farmer);
        }
        [HttpGet]
        public async Task<IActionResult> EditFarmer(int id)
        {
            var farmer = await _context.Farmers.FirstOrDefaultAsync(f => f.FarmerID == id);

            if (farmer != null)
            {
                var viewModel = new EditFarmerViewModel()
                {
                    FarmerID = farmer.FarmerID,
                    UserName = farmer.UserName,
                    Name = farmer.Name,
                    Surname = farmer.Surname,
                    Email = farmer.Email,
                    Contact = farmer.Contact,
                    Address = farmer.Address,
                    Password = farmer.Password,
                    ConfirmPassword = farmer.Password
                };
                return View(viewModel);
            }
            TempData["ErrorMessage"] = "Farmer not found.";
            return RedirectToAction("FarmerProfiles");
        }
        [HttpPost]
        public async Task<IActionResult> EditFarmer(EditFarmerViewModel model)
        {
            var farmer = await _context.Farmers.FindAsync(model.FarmerID);

            if (farmer != null)
            {
                farmer.UserName = model.UserName;
                farmer.Name = model.Name;
                farmer.Surname = model.Surname;
                farmer.Email = model.Email;
                farmer.Contact = model.Contact;
                farmer.Address = model.Address;
                farmer.Password = model.Password;
                farmer.ConfirmPassword = model.Password;

                await _context.SaveChangesAsync();

                return RedirectToAction("FarmerProfiles");
            }
            return RedirectToAction("FarmerProfiles");
        }
        [HttpPost]
        public async Task<IActionResult> DeleteFarmer(EditFarmerViewModel model)
        {
            var farmer = await _context.Farmers.FindAsync(model.FarmerID);

            if (farmer != null)
            {
                _context.Farmers.Remove(farmer);
                await _context.SaveChangesAsync();
                return RedirectToAction("FarmerProfiles");

            }

            return RedirectToAction("FarmerProfiles");
        }

        [HttpGet]
        public async Task<IActionResult> FilterProductsByDate()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> FilterProductsByDate(DateTime? startDate, DateTime? endDate)
        {
            if (!startDate.HasValue || !endDate.HasValue)
            {
                TempData["ErrorMessage"] = "Please enter both start and end dates.";
                return RedirectToAction("EmployeeDashboard");
            }

            var products = await _context.Products
                .Where(p => p.ProductionDate >= DateOnly.FromDateTime(startDate.Value) &&
                            p.ProductionDate <= DateOnly.FromDateTime(endDate.Value))
                .Include(p => p.Farmer)
                .Include(p => p.Category)
                .ToListAsync();

            return View("FilteredProducts", products);
        }
    }
}

