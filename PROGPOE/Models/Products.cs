using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PROGPOE.Models
{
    public class Products
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ProductID { get; set; }

        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }

        [Required]
        public int CategoryID { get; set; }
        public int FarmerID { get; set; }

        [Required]
        public float Price { get; set; }
        [Required]
        public int Quantity { get; set; }
        [Required]
        public DateOnly ProductionDate { get; set; }
        [ForeignKey("CategoryID")]
        public virtual Categories Category
        {
            get; set;
        }
        [ForeignKey("FarmerID")]
        public virtual Farmers Farmer
        {
            get; set;
        }
        public virtual ICollection<Carts> Carts { get; set; }
        public virtual ICollection<Orders> Orders { get; set; }

    }
}
