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
    /// <summary>
    /// Controller for handling operations related to the Employee Dashboard.
    /// </summary>
    [Authorize]
    public class EmployeeDashboardController : Controller
    {
        private readonly ApplicationDbContext _context;

        public EmployeeDashboardController(ApplicationDbContext context)
        {
            _context = context;
        }
        /// <summary>
        /// Returns the EmployeeDashboard view using the username of the 
        /// employee who is currently logged in
        /// </summary>
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

        /// <summary>
        /// Returns the AddFarmer view.
        /// </summary>
        [HttpGet]
        public IActionResult AddFarmer()
        {
            return View();
        }

        /// <summary>
        /// Handles the POST request for AddFarmer and adds a new farmer to the database.
        /// </summary>
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

        /// <summary>
        /// Returns the FarmerProfiles view with a list of all farmers that
        /// currently logged in employee added to the database
        /// </summary>
        public async Task<IActionResult> FarmerProfiles()
        {
            var farmers = await _context.Farmers
                                    .Include(f => f.Employees)
                                    .Where(f => f.EmployeeID != null) // Filter out farmers without associated employees
                                    .ToListAsync();
            return View(farmers);
        }

        /// <summary>
        /// Returns the FarmerDetails view with the details of a selected farmer.
        /// </summary>
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

        /// <summary>
        /// Returns the EditFarmer view with the details of a selected farmer for editing.
        /// </summary>
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

        /// <summary>
        /// Handles the POST request for EditFarmer and updates the details of 
        /// the selected farmer after they have been edited.
        /// </summary>
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

        /// <summary>
        /// Handles the POST request for DeleteFarmer and deletes a selected farmer from the database.
        /// </summary>
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

        /// <summary>
        /// Returns the FilterProductsByDate view.
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> FilterProductsByDate()
        {
            return View();
        }

        /// <summary>
        /// Handles the POST request for FilterProductsByDate and returns a list of products filtered by production date.
        /// </summary>
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

