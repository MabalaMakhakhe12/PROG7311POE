using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PROGPOE.Data;
using PROGPOE.Models;
namespace PROGPOE.Controllers
{
    /// <summary>
    /// Controller for handling operations related to the Normal User Dashboard.
    /// </summary>
    public class NormalUserDashboardController : Controller
    {
        private readonly ApplicationDbContext _context;

        public NormalUserDashboardController(ApplicationDbContext context)
        {
            _context = context;
        }
        /// <summary>
        /// Returns the NormalUserDashboard view with a list of all products
        /// </summary>
        public async Task<IActionResult> NormalUserDashboard()
        {
            var products = await _context.Products.Include(p => p.Farmer).Include(p => p.Category).ToListAsync();
            return View(products);
        }

    }
}
