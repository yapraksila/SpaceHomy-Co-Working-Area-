using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.ModelConfiguration.Conventions;
using Microsoft.AspNet.Identity.EntityFramework;

namespace ourwebpage.Models
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext() : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        public new DbSet<Role> Roles { get; set; }
        public DbSet<Facility> Facilities { get; set; }
        public DbSet<FacilityAmenity> FacilityAmenities { get; set; }
        public DbSet<OperatingHour> OperatingHours { get; set; }
        public DbSet<FacilityImage> FacilityImages { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<CartItemAmenity> CartItemAmenities { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<BookingAmenity> BookingAmenities { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            modelBuilder.Entity<Role>()
                .Property(r => r.Email)
                .HasColumnAnnotation(
                    IndexAnnotation.AnnotationName,
                    new IndexAnnotation(new IndexAttribute("IX_Roles_Email_Unique") { IsUnique = true }));

            modelBuilder.Entity<Booking>()
                .Property(b => b.FacilityId)
                .HasColumnAnnotation(
                    IndexAnnotation.AnnotationName,
                    new IndexAnnotation(new IndexAttribute("IX_Booking_Facility_Day", 1)));

            modelBuilder.Entity<Booking>()
                .Property(b => b.BookingDay)
                .HasColumnAnnotation(
                    IndexAnnotation.AnnotationName,
                    new IndexAnnotation(new IndexAttribute("IX_Booking_Facility_Day", 2)));

            modelBuilder.Entity<Facility>()
                .Property(f => f.Price)
                .HasPrecision(10, 2);

            modelBuilder.Entity<FacilityAmenity>()
                .Property(a => a.ExtraPrice)
                .HasPrecision(10, 2);

            modelBuilder.Entity<Payment>()
                .Property(p => p.Amount)
                .HasPrecision(10, 2);

            modelBuilder.Entity<Booking>()
                .HasRequired(b => b.RoleUser)
                .WithMany()
                .HasForeignKey(b => b.RoleId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Booking>()
                .HasRequired(b => b.Facility)
                .WithMany()
                .HasForeignKey(b => b.FacilityId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Booking>()
                .HasRequired(b => b.Payment)
                .WithMany(p => p.Bookings)
                .HasForeignKey(b => b.PaymentId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<BookingAmenity>()
                .HasRequired(ba => ba.Booking)
                .WithMany()
                .HasForeignKey(ba => ba.BookingId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<BookingAmenity>()
                .HasRequired(ba => ba.Amenity)
                .WithMany()
                .HasForeignKey(ba => ba.AmenityId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Cart>()
                .HasRequired(c => c.RoleUser)
                .WithMany()
                .HasForeignKey(c => c.RoleId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<CartItem>()
                .HasRequired(ci => ci.Cart)
                .WithMany(c => c.CartItems)
                .HasForeignKey(ci => ci.CartId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<CartItem>()
                .HasRequired(ci => ci.Facility)
                .WithMany()
                .HasForeignKey(ci => ci.FacilityId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<CartItemAmenity>()
                .HasRequired(cia => cia.CartItem)
                .WithMany()
                .HasForeignKey(cia => cia.CartItemId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<CartItemAmenity>()
                .HasRequired(cia => cia.Amenity)
                .WithMany()
                .HasForeignKey(cia => cia.AmenityId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Notification>()
                .HasRequired(n => n.RoleUser)
                .WithMany()
                .HasForeignKey(n => n.RoleId)
                .WillCascadeOnDelete(false);
        }
    }
}