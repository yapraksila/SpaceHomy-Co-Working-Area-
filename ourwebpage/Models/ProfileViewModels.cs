using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ourwebpage.Models
{
    public class ProfileEditViewModel
    {
        public int RoleId { get; set; }

        [Display(Name = "Username")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Full name is required.")]
        [MaxLength(100)]
        [Display(Name = "Full Name")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Please enter a valid email address.")]
        [MaxLength(150)]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Display(Name = "Role")]
        public string RoleName { get; set; }
    }

    public class ProfileIndexViewModel
    {
        public string UserName { get; set; }
        public Role UserProfile { get; set; }
        public ProfileEditViewModel EditModel { get; set; }
        public List<Booking> ActiveBookings { get; set; }
        public List<Booking> PastBookings { get; set; }
        public int UnreadNotificationCount { get; set; }
    }
}