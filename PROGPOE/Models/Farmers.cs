using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace PROGPOE.Models
{
    public class Farmers
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int FarmerID { get; set; }
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Surname { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Contact { get; set; }
        [Required]
        public string Address { get; set; }
        public int EmployeeID { get; set; }
        public virtual ICollection<Products> Products { get; set; }
        [ForeignKey("EmployeeID")]
        public virtual Employees Employees
        {
            get; set;
        }
    }
}
