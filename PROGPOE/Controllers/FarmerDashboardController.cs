using Microsoft.AspNetCore.Mvc;
using PROGPOE.Data;
using PROGPOE.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PROGPOE.Data;
using PROGPOE.Models;
namespace PROGPOE.Controllers
{
    public class FarmerDashboardController : Controller
    {
        private readonly ApplicationDbContext _context;

        public FarmerDashboardController(ApplicationDbContext context)
        {
            _context = context;
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
        public async Task<IActionResult> AddProduct(string name, string description, string categoryName, float price, int quantity, DateTime productionDate)
        {
            var farmer = await _context.Farmers.FirstOrDefaultAsync(f => f.UserName == User.Identity.Name);
            if (farmer == null)
            {
                TempData["ErrorMessage"] = "Farmer not found.";
                return RedirectToAction("FarmerDashboard");
            }

            // Check if the category already exists, otherwise add it
            var category = await _context.Categories.FirstOrDefaultAsync(c => c.Name == categoryName);
            if (category == null)
            {
                category = new Categories { Name = categoryName };
                _context.Categories.Add(category);
                await _context.SaveChangesAsync();
            }

            var product = new Products
            {
                Name = name,
                Description = description,
                CategoryID = category.CategoryID,
                Price = price,
                Quantity = quantity,
                ProductionDate = DateOnly.FromDateTime(productionDate),
                FarmerID = farmer.FarmerID
            };

            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            return RedirectToAction("FarmerDashboard");
        }
    }
}