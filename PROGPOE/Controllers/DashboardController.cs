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
    public class DashboardController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DashboardController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> NormalUserDashboard()
        {
            var products = await _context.Products.Include(p => p.Farmer).Include(p => p.Category).ToListAsync();
            return View(products);
        }

        public async Task<IActionResult> FarmerDashboard()
        {
            var farmer = await _context.Farmers.FirstOrDefaultAsync(f => f.UserName == User.Identity.Name);

            if (farmer == null)
            {
                // Handle the case where the farmer is not found
                TempData["ErrorMessage"] = "Farmer not found.";
                return RedirectToAction("Index", "Account"); // Redirect to login page or appropriate error page
            }

            var products = await _context.Products
                .Where(p => p.FarmerID == farmer.FarmerID)
                .Include(p => p.Category)
                .ToListAsync();

            ViewBag.Categories = await _context.Categories.ToListAsync();
            return View(products);
        }


        [HttpPost]
        public async Task<IActionResult> AddProduct(string name, string description, int categoryID, float price, int quantity, DateTime productionDate)
        {
            var farmer = await _context.Farmers.FirstOrDefaultAsync(f => f.UserName == User.Identity.Name);
            if (farmer == null)
            {
                TempData["ErrorMessage"] = "Farmer not found.";
                return RedirectToAction("FarmerDashboard");
            }

            var product = new Products
            {
                Name = name,
                Description = description,
                CategoryID = categoryID,
                Price = price,
                Quantity = quantity,
                ProductionDate = DateOnly.FromDateTime(productionDate),
                FarmerID = farmer.FarmerID // Assign the FarmerID of the logged-in farmer
            };
            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            return RedirectToAction("FarmerDashboard");
        }

        public async Task<IActionResult> EmployeeDashboard()
        {
            var farmer = await _context.Farmers.FirstOrDefaultAsync(f => f.UserName == User.Identity.Name);

            /*  var viewModel = new EmployeeDashboardViewModel
              {
                  FilteredProducts = await _context.Products.Include(p => p.Farmer).Include(p => p.Category).ToListAsync()
              };*/
              return View(farmer);
        }


        [HttpPost]
        public async Task<IActionResult> AddFarmer(string username, string password, string role, string name, string surname, string email, string contact, string address, string postcode)
        {
            var currentEmployee = await _context.Employees.FirstOrDefaultAsync(e => e.UserName == User.Identity.Name);

            var farmer = new Farmers
            {
                UserName = username,
                Name = name,
                Surname = surname,
                Email = email,
                Contact = contact,
                Address = address,
                Password = password,
                EmployeeID = 3

            };
            _context.Farmers.Add(farmer);


            await _context.SaveChangesAsync();
            return RedirectToAction("EmployeeDashboard");

        }


        [HttpPost]
        public async Task<IActionResult> FilterProducts(DateTime? startDate, DateTime? endDate, string category)
        {
            var query = _context.Products.Include(p => p.Farmer).Include(p => p.Category).AsQueryable();

            if (startDate.HasValue)
            {
                query = query.Where(p => p.ProductionDate >= DateOnly.FromDateTime(startDate.Value));
            }

            if (endDate.HasValue)
            {
                query = query.Where(p => p.ProductionDate <= DateOnly.FromDateTime(endDate.Value));
            }

            if (!string.IsNullOrEmpty(category))
            {
                query = query.Where(p => p.Category.Name.Contains(category));
            }

            var products = await query.ToListAsync();

            var viewModel = new EmployeeDashboardViewModel
            {
                FilteredProducts = products
            };

            return View("EmployeeDashboard", viewModel);
        }
    }
}

