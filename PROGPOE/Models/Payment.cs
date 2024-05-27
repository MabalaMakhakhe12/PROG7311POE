using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace PROGPOE.Models
{
    public class Payment
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PaymentID { get; set; }
        public string Name { get; set; }
        public string CardNo { get; set; }
        public string ExpiryDate { get; set; }
        public int Cvv { get; set; }
        public string Address { get; set; }
        public string PaymentMode { get; set; }
        public virtual ICollection<Orders> Orders { get; set; }

    }
}
