using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace PROGPOE.Models
{
    public class Employees
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int EmployeeID { get; set; }
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Surname { get; set; }
        [Required, EmailAddress]
        public string Email { get; set; }
        [Required]
       // public string mobile { get; set; }
        //[Required]
        public string Address { get; set; }
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [NotMapped]
        [Required(ErrorMessage = "Confirm Password is required")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        [Display(Name = "Confirm Password")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }
        public virtual ICollection<Farmers> Farmers { get; set; }

    }
}
