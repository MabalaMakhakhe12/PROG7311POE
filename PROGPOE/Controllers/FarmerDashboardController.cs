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

          //  ViewBag.Categories = await _context.Categories.ToListAsync();
            return View(products);
        }
        [HttpGet]
        public IActionResult AddProduct()
        {
           return View();
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
        [HttpGet]
        public async Task<IActionResult> EditProduct(int id)
        {
            var product = await _context.Products
                .Include(p => p.Category)
                .FirstOrDefaultAsync(p => p.ProductID == id);

            if (product != null)
            {
                var viewModel = new EditProductsViewModel()
                {
                    ProductID = product.ProductID,
                    Name = product.Name,
                    Description = product.Description,
                    CategoryID = product.CategoryID,
                    Price = product.Price,
                    Quantity = product.Quantity,
                    ProductionDate = product.ProductionDate
                };

                ViewBag.Categories = await _context.Categories.ToListAsync();
                return View(viewModel);
            }

            TempData["ErrorMessage"] = "Product not found.";
            return RedirectToAction("FarmerDashboard");
        }


        [HttpPost]
        public async Task<IActionResult> EditProduct(EditProductsViewModel model)
        {
            var product = await _context.Products.FindAsync(model.ProductID);

            if (product != null)
            {
                product.Name = model.Name;
                product.Description = model.Description;
                product.CategoryID = model.CategoryID;
                product.Price = model.Price;
                product.Quantity = model.Quantity;
                product.ProductionDate = model.ProductionDate;

                await _context.SaveChangesAsync();

                return RedirectToAction("FarmerDashboard");
            }
            return RedirectToAction("FarmerDashboard"); 
        }

        [HttpGet]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var product = await _context.Products.FindAsync(id);

            if (product != null)
            {
                _context.Products.Remove(product);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("FarmerDashboard");
        }

        

        [HttpGet]
        public async Task<IActionResult> ProductList()
        {
            var farmer = await _context.Farmers.FirstOrDefaultAsync(f => f.UserName == User.Identity.Name);

            if (farmer == null)
            {
                TempData["ErrorMessage"] = "Farmer not found.";
                return RedirectToAction("Index", "Account");
            }

            var products = await _context.Products
                .Where(p => p.FarmerID == farmer.FarmerID)
                .Include(p => p.Category)
                .ToListAsync();

            return View(products);
        }
    }
    }
