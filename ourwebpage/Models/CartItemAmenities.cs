using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ourwebpage.Models
{
    [Table("CartItemAmenities")]
    public class CartItemAmenity
    {
        [Key]
        [Column("cart_item_id", Order = 0)]
        [ForeignKey("CartItem")]
        public int CartItemId { get; set; }
        public virtual CartItem CartItem { get; set; }

        [Key]
        [Column("amenity_id", Order = 1)]
        [ForeignKey("Amenity")]
        public int AmenityId { get; set; }
        public virtual FacilityAmenity Amenity { get; set; }
    }
}