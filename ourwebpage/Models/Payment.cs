using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ourwebpage.Models
{
    public class Payment
    {
        [Key]
        public int PaymentId { get; set; }

        [Required]
        [ForeignKey("RoleUser")]
        public int RoleId { get; set; }
        public virtual Role RoleUser { get; set; }

        [Required]
        [Column("amount")]
        public decimal Amount { get; set; }

        [Required]
        [MaxLength(10)]
        public string Currency { get; set; }

        [Required]
        [MaxLength(50)]
        public string PaymentMethod { get; set; }

        [Required]
        [MaxLength(50)]
        [RegularExpression("Paid|Failed|Refunded")]
        public string Status { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; }

        public virtual ICollection<Booking> Bookings { get; set; }

        public Payment()
        {
            Currency = "TRY";
            Status = "Paid";
            CreatedAt = DateTime.Now;
            Bookings = new HashSet<Booking>();
        }
    }
}