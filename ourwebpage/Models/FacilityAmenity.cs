using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ourwebpage.Models
{
    public class FacilityAmenity
    {
        [Key]
        public int AmenityId { get; set; }

        [Required]
        [MaxLength(100)]
        public string AmenityName { get; set; }

        [Required]
        [Column("extra_price")]
        public decimal ExtraPrice { get; set; }

        [Required]
        [MaxLength(10)]
        public string Currency { get; set; }

        public virtual ICollection<Facility> Facilities { get; set; }
        public virtual ICollection<CartItem> CartItems { get; set; }
        public virtual ICollection<Booking> Bookings { get; set; }

        public FacilityAmenity()
        {
            ExtraPrice = 0.00m;
            Currency = "TRY";
            Facilities = new HashSet<Facility>();
            CartItems = new HashSet<CartItem>();
            Bookings = new HashSet<Booking>();
        }
    }
}