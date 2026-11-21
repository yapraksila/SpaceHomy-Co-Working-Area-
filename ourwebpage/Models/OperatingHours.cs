using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ourwebpage.Models
{
    public class OperatingHour
    {
        [Key]
        public int HourId { get; set; }

        [Required]
        [ForeignKey("Facility")]
        public int FacilityId { get; set; }
        public virtual Facility Facility { get; set; }

        [Required]
        [MaxLength(20)]
        public string DayOfWeek { get; set; }

        public TimeSpan? OpenTime { get; set; }

        public TimeSpan? CloseTime { get; set; }

        [Required]
        public bool IsClosed { get; set; }
    }
}