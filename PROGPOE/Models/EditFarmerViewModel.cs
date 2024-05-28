using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace PROGPOE.Models
{
    public class EditFarmerViewModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int FarmerID { get; set; }
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }
        [NotMapped]
        [Required(ErrorMessage = "Confirm Password is required")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        [Display(Name = "Confirm Password")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Surname { get; set; }
        [Required, EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "Contact number is required")]
        [RegularExpression(@"^\d{10}$", ErrorMessage = "Please enter a valid 10-digit telephone number")]
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
