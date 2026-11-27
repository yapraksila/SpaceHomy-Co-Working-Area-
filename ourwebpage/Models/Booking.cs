using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ourwebpage.Models;

namespace ourwebpage.Models
{
    [Table("Booking")]
    public class Booking
    {
        [Key]
        [Column("booking_id")]
        public int BookingId { get; set; }

        [Required]
        [Column("role_id")]
        [ForeignKey("RoleUser")]
        public int RoleId { get; set; }
        public virtual Role RoleUser { get; set; }

        [Required]
        [Column("facility_id")]
        [ForeignKey("Facility")]
        public int FacilityId { get; set; }
        public virtual Facility Facility { get; set; }

        [Required]
        [Column("payment_id")]
        [ForeignKey("Payment")]
        public int PaymentId { get; set; }
        public virtual Payment Payment { get; set; }

        [Required]
        [Column("booking_day")]
        public DateTime BookingDay { get; set; }

        [Required]
        [Column("start_time")]
        public TimeSpan StartTime { get; set; }

        [Required]
        [Column("end_time")]
        public TimeSpan EndTime { get; set; }

        [Required]
        [MaxLength(50)]
        [RegularExpression("Pending|Confirmed|Cancelled|Completed")]
        [Column("status")]
        public string Status { get; set; }

        public virtual ICollection<BookingAmenity> BookingAmenities { get; set; }

        public Booking()
        {
            Status = "Pending";
            BookingAmenities = new HashSet<BookingAmenity>();
        }

        public DateTime? CheckInAt { get; set; }
        public DateTime? CheckOutAt { get; set; }
    }
}