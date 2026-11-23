using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ourwebpage.Models
{
    [Table("BookingAmenities")]
    public class BookingAmenity
    {
        [Key]
        [Column("booking_id", Order = 0)]
        [ForeignKey("Booking")]
        public int BookingId { get; set; }
        public virtual Booking Booking { get; set; }

        [Key]
        [Column("amenity_id", Order = 1)]
        [ForeignKey("Amenity")]
        public int AmenityId { get; set; }
        public virtual FacilityAmenity Amenity { get; set; }
    }
}