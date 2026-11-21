using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace ourwebpage.Models
{
    public class Role
    {
        [Key]
        public int RoleId { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        [Required]
        [MaxLength(150)]
        public string Email { get; set; }

        [Required]
        [MaxLength(255)]
        public string Password { get; set; }

        [Required]
        [MaxLength(50)]
        public string RoleName { get; set; }

        public virtual ICollection<Notification> Notifications { get; set; }
        public virtual ICollection<Payment> Payments { get; set; }
        public virtual ICollection<Cart> Carts { get; set; }
        public virtual ICollection<Booking> Bookings { get; set; }

        public Role()
        {
            RoleName = "user";
            Notifications = new HashSet<Notification>();
            Payments = new HashSet<Payment>();
            Carts = new HashSet<Cart>();
            Bookings = new HashSet<Booking>();
        }
    }
}