using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PROGPOE.Models
{
    public class Carts
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CartID { get; set; }
        public int ProductID { get; set; }
        public int UserID { get; set; }
        public DateTime CreatedDate { get; set; }
        [ForeignKey("ProductID")]
        public virtual Products Product
        {
            get; set;
        }
        [ForeignKey("UserID")]
        public virtual Users User
        {
            get; set;
        }
    }
}
