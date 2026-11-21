using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ourwebpage.Models
{
    public class FacilityImage
    {
        [Key]
        public int ImageId { get; set; }

        [Required]
        [ForeignKey("Facility")]
        public int FacilityId { get; set; }
        public virtual Facility Facility { get; set; }

        [Required]
        [MaxLength(500)]
        public string ImageUrl { get; set; }
    }
}