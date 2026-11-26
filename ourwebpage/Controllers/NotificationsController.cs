using System;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using ourwebpage.Models;

namespace ourwebpage.Controllers
{
    [Authorize]
    public class NotificationsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private ApplicationUserManager _userManager;

        public ApplicationUserManager UserManager
        {
            get => _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            private set => _userManager = value;
        }

        private SelectList GetRoleSelectList(int? selectedValue = null)
        {
            var roles = db.Roles
                .OrderBy(r => r.RoleName)
                .ThenBy(r => r.Name)
                .Select(r => new
                {
                    r.RoleId,
                    DisplayText = r.Name + " (" + r.RoleName + ") - " + r.Email
                })
                .ToList();

            return new SelectList(roles, "RoleId", "DisplayText", selectedValue);
        }

        // ADMIN: Bildirim oluşturma ekranı
        [Authorize(Roles = MyConstants.RoleAdmin)]
        public ActionResult Create()
        {
            ViewBag.RoleId = GetRoleSelectList();
            ViewBag.RecentNotifications = db.Notifications
                .Include(n => n.RoleUser)
                .OrderByDescending(n => n.Date)
                .ThenByDescending(n => n.Time)
                .Take(20)
                .ToList();

            return View(new Notification());
        }

        // ADMIN: Yeni bildirim kaydetme
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = MyConstants.RoleAdmin)]
        public ActionResult Create([Bind(Include = "RoleId,Title,Message")] Notification notification)
        {
            var now = DateTime.Now;
            notification.Date = now.Date;
            notification.Time = now.TimeOfDay;
            notification.Status = "Unread";

            ModelState.Remove("Date");
            ModelState.Remove("Time");
            ModelState.Remove("Status");

            if (ModelState.IsValid)
            {
                db.Notifications.Add(notification);
                db.SaveChanges();
                TempData["SuccessMessage"] = "Bildirim başarıyla gönderildi.";
                return RedirectToAction("Create");
            }

            ViewBag.RoleId = GetRoleSelectList(notification.RoleId);
            ViewBag.RecentNotifications = db.Notifications
                .Include(n => n.RoleUser)
                .OrderByDescending(n => n.Date)
                .ThenByDescending(n => n.Time)
                .Take(20)
                .ToList();

            return View(notification);
        }

        // USER: Giriş yapan kullanıcının bildirimleri
        public ActionResult Inbox()
        {
            var currentRoleUser = GetCurrentRoleUser();
            int activeRoleId = currentRoleUser != null ? currentRoleUser.RoleId : 0;

            var notifications = db.Notifications
                .Include(n => n.RoleUser)
                .Where(n => n.RoleId == activeRoleId)
                .OrderBy(n => n.Status == "Read")
                .ThenByDescending(n => n.Date)
                .ThenByDescending(n => n.Time)
                .ToList();

            ViewBag.CurrentUser = currentRoleUser;
            ViewBag.ActiveRoleId = activeRoleId;
            ViewBag.UnreadCount = notifications.Count(n => n.Status == "Unread");
            return View(notifications);
        }

        // USER: Tek bildirimi okundu işaretleme
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult MarkAsRead(int id)
        {
            var currentRoleUser = GetCurrentRoleUser();
            var notification = db.Notifications.Find(id);

            if (notification == null)
            {
                return HttpNotFound();
            }

            if (currentRoleUser != null && notification.RoleId != currentRoleUser.RoleId)
            {
                return new HttpUnauthorizedResult();
            }

            if (notification.Status != "Read")
            {
                notification.Status = "Read";
                db.Entry(notification).State = EntityState.Modified;
                db.SaveChanges();
            }

            return RedirectToAction("Inbox");
        }

        // USER: Tüm bildirimleri okundu yapma
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult MarkAllAsRead()
        {
            var currentRoleUser = GetCurrentRoleUser();
            if (currentRoleUser == null)
            {
                return RedirectToAction("Inbox");
            }

            var unreadNotifications = db.Notifications
                .Where(n => n.RoleId == currentRoleUser.RoleId && n.Status == "Unread")
                .ToList();

            foreach (var item in unreadNotifications)
            {
                item.Status = "Read";
            }

            db.SaveChanges();
            return RedirectToAction("Inbox");
        }

        private Role GetCurrentRoleUser()
        {
            var userId = User.Identity.GetUserId();
            if (string.IsNullOrEmpty(userId))
            {
                return null;
            }

            var identityUser = UserManager.FindById(userId);
            if (identityUser == null || string.IsNullOrEmpty(identityUser.Email))
            {
                return null;
            }

            return db.Roles.FirstOrDefault(r => r.Email == identityUser.Email);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}