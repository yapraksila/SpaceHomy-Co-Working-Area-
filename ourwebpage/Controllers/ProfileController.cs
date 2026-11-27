using System;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using ourwebpage.Models;

namespace ourwebpage.Controllers
{
    [Authorize]
    public class ProfileController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private ApplicationUserManager _userManager;

        public ApplicationUserManager UserManager
        {
            get => _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            private set => _userManager = value;
        }

        private async Task<ApplicationUser> GetCurrentIdentityUserAsync()
        {
            var userId = User.Identity.GetUserId();
            if (string.IsNullOrEmpty(userId))
            {
                return null;
            }
            return await UserManager.FindByIdAsync(userId);
        }

        private async Task<Role> GetCurrentRoleUserAsync(ApplicationUser identityUser)
        {
            if (identityUser == null || string.IsNullOrEmpty(identityUser.Email))
            {
                return null;
            }
            return await db.Roles.FirstOrDefaultAsync(r => r.Email == identityUser.Email);
        }

        // GET: Profile/Index
        public async Task<ActionResult> Index()
        {
            var identityUser = await GetCurrentIdentityUserAsync();
            var roleUser = await GetCurrentRoleUserAsync(identityUser);

            if (roleUser == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var today = DateTime.Today;

            var userBookings = await db.Bookings
                .Include(b => b.Facility)
                .Where(b => b.RoleId == roleUser.RoleId)
                .OrderBy(b => b.BookingDay)
                .ThenBy(b => b.StartTime)
                .ToListAsync();

            var activeBookings = userBookings
                .Where(b => b.BookingDay >= today && b.Status != "Cancelled" && b.Status != "Completed")
                .ToList();

            var pastBookings = userBookings
                .Where(b => b.BookingDay < today || b.Status == "Cancelled" || b.Status == "Completed")
                .OrderByDescending(b => b.BookingDay)
                .ThenByDescending(b => b.StartTime)
                .ToList();

            var unreadCount = await db.Notifications
                .CountAsync(n => n.RoleId == roleUser.RoleId && n.Status == "Unread");

            var vm = new ProfileIndexViewModel
            {
                UserName = identityUser.UserName,
                UserProfile = roleUser,
                EditModel = new ProfileEditViewModel
                {
                    RoleId = roleUser.RoleId,
                    UserName = identityUser.UserName,
                    Name = roleUser.Name,
                    Email = roleUser.Email,
                    RoleName = roleUser.RoleName
                },
                ActiveBookings = activeBookings,
                PastBookings = pastBookings,
                UnreadNotificationCount = unreadCount
            };

            return View(vm);
        }

        // POST: Profile/CancelBooking
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CancelBooking(int id)
        {
            var identityUser = await GetCurrentIdentityUserAsync();
            var roleUser = await GetCurrentRoleUserAsync(identityUser);

            if (roleUser == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var booking = await db.Bookings.FindAsync(id);
            if (booking == null)
            {
                return HttpNotFound();
            }

            if (booking.RoleId != roleUser.RoleId)
            {
                return new HttpUnauthorizedResult();
            }

            booking.Status = "Cancelled";
            db.Entry(booking).State = EntityState.Modified;
            await db.SaveChangesAsync();

            TempData["SuccessMessage"] = "Reservation has been cancelled.";
            return RedirectToAction("Index");
        }

        // GET: Profile/Edit
        public async Task<ActionResult> Edit()
        {
            var identityUser = await GetCurrentIdentityUserAsync();
            var roleUser = await GetCurrentRoleUserAsync(identityUser);

            if (roleUser == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var model = new ProfileEditViewModel
            {
                RoleId = roleUser.RoleId,
                UserName = identityUser.UserName,
                Name = roleUser.Name,
                Email = roleUser.Email,
                RoleName = roleUser.RoleName
            };

            return View(model);
        }

        // POST: Profile/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(ProfileEditViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var identityUser = await GetCurrentIdentityUserAsync();
            var roleUser = await GetCurrentRoleUserAsync(identityUser);

            if (roleUser == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var oldEmail = roleUser.Email;
            roleUser.Name = model.Name;
            roleUser.Email = model.Email;
            db.Entry(roleUser).State = EntityState.Modified;
            await db.SaveChangesAsync();

            if (identityUser != null)
            {
                identityUser.FullName = model.Name;
                identityUser.Email = model.Email;
                await UserManager.UpdateAsync(identityUser);
            }

            TempData["SuccessMessage"] = "Profile updated successfully.";
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CheckIn(int id)
        {
            var booking = db.Bookings.Find(id);
            if (booking == null)
                return HttpNotFound();

            if (booking.Status == "Cancelled" || booking.Status == "Completed" || booking.CheckInAt.HasValue)
            {
                TempData["ErrorMessage"] = "This reservation cannot be checked in.";
                return RedirectToAction("Index");
            }

            DateTime scheduledStart = booking.BookingDay.Date + booking.StartTime;
            DateTime earliestCheckIn = scheduledStart.AddMinutes(-5);
            DateTime now = DateTime.Now;

            if (now < earliestCheckIn)
            {
                TempData["ErrorMessage"] = $"Check-in opens at {earliestCheckIn:dd MMM yyyy HH:mm} (5 minutes before start time).";
                return RedirectToAction("Index");
            }

            booking.CheckInAt = now;
            booking.Status = "CheckedIn";
            db.SaveChanges();

            TempData["SuccessMessage"] = "Check-in time recorded.";
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CheckOut(int id)
        {
            var booking = db.Bookings.Find(id);
            if (booking == null)
                return HttpNotFound();

            if (!booking.CheckInAt.HasValue && booking.Status != "CheckedIn")
            {
                TempData["ErrorMessage"] = "You must check in before checking out.";
                return RedirectToAction("Index");
            }

            if (booking.CheckOutAt.HasValue || booking.Status == "Completed")
            {
                TempData["ErrorMessage"] = "Check-out has already been completed.";
                return RedirectToAction("Index");
            }

            booking.CheckOutAt = DateTime.Now;
            booking.Status = "Completed";
            db.SaveChanges();

            TempData["SuccessMessage"] = "Check-out time recorded.";
            return RedirectToAction("Index");
        }


    }
}