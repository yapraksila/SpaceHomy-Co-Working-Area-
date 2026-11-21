using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace ourwebpage.Models
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext() : base("name=DefaultConnection")
        {
        }

        public DbSet<Role> Roles { get; set; }
        public DbSet<Facility> Facilities { get; set; }
        public DbSet<FacilityAmenity> FacilityAmenities { get; set; }
        public DbSet<OperatingHour> OperatingHours { get; set; }
        public DbSet<FacilityImage> FacilityImages { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<Booking> Bookings { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

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

            modelBuilder.Entity<Notification>()
                .HasRequired(n => n.RoleUser)
                .WithMany()
                .HasForeignKey(n => n.RoleId)
                .WillCascadeOnDelete(false);
        }
    }
}