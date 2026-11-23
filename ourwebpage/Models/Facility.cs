using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ourwebpage.Models
{
    public class Facility
    {
        [Key]
        public int FacilityId { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        [Required]
        [MaxLength(50)]
        public string Type { get; set; }

        [Required]
        public int Capacity { get; set; }

        [Required]
        [Column("price")]
        public decimal Price { get; set; }
      

        [Required]
        [MaxLength(10)]
        public string Currency { get; set; }

        [Required]
        [MaxLength(50)]
        public string Status { get; set; }

        public virtual ICollection<OperatingHour> OperatingHours { get; set; }
        public virtual ICollection<FacilityImage> FacilityImages { get; set; }
        public virtual ICollection<FacilityAmenity> Amenities { get; set; }
        public virtual ICollection<CartItem> CartItems { get; set; }
        public virtual ICollection<Booking> Bookings { get; set; }

        public Facility()
        {
            Currency = "TRY";
            Status = "available";
            OperatingHours = new HashSet<OperatingHour>();
            FacilityImages = new HashSet<FacilityImage>();
            Amenities = new HashSet<FacilityAmenity>();
            CartItems = new HashSet<CartItem>();
            Bookings = new HashSet<Booking>();
        }
    }
}