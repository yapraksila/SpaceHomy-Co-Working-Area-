using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ourwebpage.Models
{
    [Table("Cart_Items")]
    public class CartItem
    {
        [Key]
        [Column("cart_item_id")]
        public int CartItemId { get; set; }

        [Required]
        [Column("cart_id")]
        [ForeignKey("Cart")]
        public int CartId { get; set; }
        public virtual Cart Cart { get; set; }

        [Required]
        [Column("facility_id")]
        [ForeignKey("Facility")]
        public int FacilityId { get; set; }
        public virtual Facility Facility { get; set; }

        [Required]
        [Column("booking_date")]
        public DateTime BookingDate { get; set; }

        [Required]
        [Column("start_time")]
        public TimeSpan StartTime { get; set; }

        [Required]
        [Column("end_time")]
        public TimeSpan EndTime { get; set; }

        public virtual ICollection<CartItemAmenity> CartItemAmenities { get; set; }

        public CartItem()
        {
            CartItemAmenities = new HashSet<CartItemAmenity>();
        }
    }
}