using PROGPOE.Models;
using System.Collections.Generic;

namespace PROGPOE.ViewModels
{
    public class EmployeeDashboardViewModel
    {
        public IEnumerable<Products> FilteredProducts { get; set; }
    }
}
