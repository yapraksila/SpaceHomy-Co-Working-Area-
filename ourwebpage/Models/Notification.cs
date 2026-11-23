using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ourwebpage.Models
{
    public class Notification
    {
        [Key]
        public int NotificationId { get; set; }

        [Required]
        [ForeignKey("RoleUser")]
        public int RoleId { get; set; }
        public virtual Role RoleUser { get; set; }

        [Required]
        [MaxLength(150)]
        public string Title { get; set; }

        [Required]
        public string Message { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [Required]
        public TimeSpan Time { get; set; }

        [Required]
        [MaxLength(50)]
        [RegularExpression("Unread|Read")]
        public string Status { get; set; }

        public Notification()
        {
            Status = "Unread";
        }
    }
}