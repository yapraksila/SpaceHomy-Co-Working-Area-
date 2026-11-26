namespace ourwebpage.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using ourwebpage.Models;

    internal sealed class Configuration : DbMigrationsConfiguration<ourwebpage.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(ourwebpage.Models.ApplicationDbContext context)
        {
            // 1. Role kaydı (Email unique index'e göre eşleştirilir)
            context.Roles.AddOrUpdate(
                r => r.Email,
                new Role
                {
                    Name = "Test User",
                    Email = "testuser@spacehomy.com",
                    Password = "SeedPassword123!",
                    RoleName = "User"
                }
            );
            context.SaveChanges();

            var roleId = context.Roles
                .Where(r => r.Email == "testuser@spacehomy.com")
                .Select(r => r.RoleId)
                .FirstOrDefault();

            // 2. Facility kaydı
            context.Facilities.AddOrUpdate(
                f => f.Name,
                new Facility
                {
                    Name = "Room 204",
                    Type = "Meeting Room",
                    Capacity = 6,
                    Price = 150.00m,
                    Currency = "USD",
                    Status = "Available"
                }
            );
            context.SaveChanges();

            var facilityId = context.Facilities
                .Where(f => f.Name == "Room 204")
                .Select(f => f.FacilityId)
                .FirstOrDefault();

            // 3. Payment kaydı
            context.Payments.AddOrUpdate(
                p => new { p.RoleId, p.CreatedAt },
                new Payment
                {
                    RoleId = roleId,
                    Amount = 150.00m,
                    Currency = "USD",
                    PaymentMethod = "Credit Card",
                    Status = "Paid",
                    CreatedAt = new DateTime(2026, 11, 20, 10, 0, 0)
                }
            );
            context.SaveChanges();

            var paymentId = context.Payments
                .Where(p => p.RoleId == roleId)
                .OrderByDescending(p => p.PaymentId)
                .Select(p => p.PaymentId)
                .FirstOrDefault();

            // 4. Booking kayıtları (Active + Old)
            context.Bookings.AddOrUpdate(
                b => new { b.RoleId, b.FacilityId, b.BookingDay, b.StartTime },
                new Booking
                {
                    RoleId = roleId,
                    FacilityId = facilityId,
                    PaymentId = paymentId,
                    BookingDay = new DateTime(2026, 12, 10),
                    StartTime = new TimeSpan(9, 0, 0),
                    EndTime = new TimeSpan(11, 0, 0),
                    Status = "Confirmed"
                },
                new Booking
                {
                    RoleId = roleId,
                    FacilityId = facilityId,
                    PaymentId = paymentId,
                    BookingDay = new DateTime(2026, 10, 15),
                    StartTime = new TimeSpan(14, 0, 0),
                    EndTime = new TimeSpan(16, 0, 0),
                    Status = "Completed"
                }
            );
            context.SaveChanges();
        }
    }
}