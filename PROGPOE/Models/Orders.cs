using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PROGPOE.Models
{
    public class Orders
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int OrderDetailsID { get; set; }
        public string OrderNo { get; set; }
        public int ProductID { get; set; }
        public int Quantity { get; set; }
        public int UserID { get; set; }
        public string Status { get; set; }
        public int PaymentID { get; set; }
        public DateOnly OrderDate { get; set; }
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
        [ForeignKey("PaymentID")]
        public virtual Payment Payment
        {
            get; set;
        }
    }
}
