namespace ourwebpage.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCoworkingSchema : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.BookingAmenities",
                c => new
                    {
                        booking_id = c.Int(nullable: false),
                        amenity_id = c.Int(nullable: false),
                        Booking_BookingId = c.Int(),
                    })
                .PrimaryKey(t => new { t.booking_id, t.amenity_id })
                .ForeignKey("dbo.Booking", t => t.Booking_BookingId)
                .ForeignKey("dbo.FacilityAmenity", t => t.amenity_id)
                .ForeignKey("dbo.Booking", t => t.booking_id)
                .Index(t => t.booking_id)
                .Index(t => t.amenity_id)
                .Index(t => t.Booking_BookingId);
            
            CreateTable(
                "dbo.FacilityAmenity",
                c => new
                    {
                        AmenityId = c.Int(nullable: false, identity: true),
                        AmenityName = c.String(nullable: false, maxLength: 100),
                        extra_price = c.Decimal(nullable: false, precision: 10, scale: 2),
                        Currency = c.String(nullable: false, maxLength: 10),
                    })
                .PrimaryKey(t => t.AmenityId);
            
            CreateTable(
                "dbo.Booking",
                c => new
                    {
                        booking_id = c.Int(nullable: false, identity: true),
                        role_id = c.Int(nullable: false),
                        facility_id = c.Int(nullable: false),
                        payment_id = c.Int(nullable: false),
                        booking_day = c.DateTime(nullable: false),
                        start_time = c.Time(nullable: false, precision: 7),
                        end_time = c.Time(nullable: false, precision: 7),
                        status = c.String(nullable: false, maxLength: 50),
                        Facility_FacilityId = c.Int(),
                        Role_RoleId = c.Int(),
                        FacilityAmenity_AmenityId = c.Int(),
                    })
                .PrimaryKey(t => t.booking_id)
                .ForeignKey("dbo.Facility", t => t.Facility_FacilityId)
                .ForeignKey("dbo.Role", t => t.Role_RoleId)
                .ForeignKey("dbo.Facility", t => t.facility_id)
                .ForeignKey("dbo.Payment", t => t.payment_id)
                .ForeignKey("dbo.Role", t => t.role_id)
                .ForeignKey("dbo.FacilityAmenity", t => t.FacilityAmenity_AmenityId)
                .Index(t => t.role_id)
                .Index(t => t.facility_id)
                .Index(t => t.payment_id)
                .Index(t => t.Facility_FacilityId)
                .Index(t => t.Role_RoleId)
                .Index(t => t.FacilityAmenity_AmenityId);
            
            CreateTable(
                "dbo.Facility",
                c => new
                    {
                        FacilityId = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 100),
                        Type = c.String(nullable: false, maxLength: 50),
                        Capacity = c.Int(nullable: false),
                        price = c.Decimal(nullable: false, precision: 10, scale: 2),
                        Currency = c.String(nullable: false, maxLength: 10),
                        Status = c.String(nullable: false, maxLength: 50),
                    })
                .PrimaryKey(t => t.FacilityId);
            
            CreateTable(
                "dbo.Cart_Items",
                c => new
                    {
                        cart_item_id = c.Int(nullable: false, identity: true),
                        cart_id = c.Int(nullable: false),
                        facility_id = c.Int(nullable: false),
                        booking_date = c.DateTime(nullable: false),
                        start_time = c.Time(nullable: false, precision: 7),
                        end_time = c.Time(nullable: false, precision: 7),
                        Facility_FacilityId = c.Int(),
                        FacilityAmenity_AmenityId = c.Int(),
                    })
                .PrimaryKey(t => t.cart_item_id)
                .ForeignKey("dbo.Cart", t => t.cart_id)
                .ForeignKey("dbo.Facility", t => t.facility_id)
                .ForeignKey("dbo.Facility", t => t.Facility_FacilityId)
                .ForeignKey("dbo.FacilityAmenity", t => t.FacilityAmenity_AmenityId)
                .Index(t => t.cart_id)
                .Index(t => t.facility_id)
                .Index(t => t.Facility_FacilityId)
                .Index(t => t.FacilityAmenity_AmenityId);
            
            CreateTable(
                "dbo.Cart",
                c => new
                    {
                        CartId = c.Int(nullable: false, identity: true),
                        RoleId = c.Int(nullable: false),
                        Role_RoleId = c.Int(),
                    })
                .PrimaryKey(t => t.CartId)
                .ForeignKey("dbo.Role", t => t.Role_RoleId)
                .ForeignKey("dbo.Role", t => t.RoleId)
                .Index(t => t.RoleId)
                .Index(t => t.Role_RoleId);
            
            CreateTable(
                "dbo.Role",
                c => new
                    {
                        RoleId = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 100),
                        Email = c.String(nullable: false, maxLength: 150),
                        Password = c.String(nullable: false, maxLength: 255),
                        RoleName = c.String(nullable: false, maxLength: 50),
                    })
                .PrimaryKey(t => t.RoleId);
            
            CreateTable(
                "dbo.Notification",
                c => new
                    {
                        NotificationId = c.Int(nullable: false, identity: true),
                        RoleId = c.Int(nullable: false),
                        Title = c.String(nullable: false, maxLength: 150),
                        Message = c.String(nullable: false),
                        Date = c.DateTime(nullable: false),
                        Time = c.Time(nullable: false, precision: 7),
                        Status = c.String(nullable: false, maxLength: 50),
                        Role_RoleId = c.Int(),
                    })
                .PrimaryKey(t => t.NotificationId)
                .ForeignKey("dbo.Role", t => t.RoleId)
                .ForeignKey("dbo.Role", t => t.Role_RoleId)
                .Index(t => t.RoleId)
                .Index(t => t.Role_RoleId);
            
            CreateTable(
                "dbo.Payment",
                c => new
                    {
                        PaymentId = c.Int(nullable: false, identity: true),
                        RoleId = c.Int(nullable: false),
                        amount = c.Decimal(nullable: false, precision: 10, scale: 2),
                        Currency = c.String(nullable: false, maxLength: 10),
                        PaymentMethod = c.String(nullable: false, maxLength: 50),
                        Status = c.String(nullable: false, maxLength: 50),
                        CreatedAt = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.PaymentId)
                .ForeignKey("dbo.Role", t => t.RoleId, cascadeDelete: true)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.CartItemAmenities",
                c => new
                    {
                        cart_item_id = c.Int(nullable: false),
                        amenity_id = c.Int(nullable: false),
                        CartItem_CartItemId = c.Int(),
                    })
                .PrimaryKey(t => new { t.cart_item_id, t.amenity_id })
                .ForeignKey("dbo.FacilityAmenity", t => t.amenity_id)
                .ForeignKey("dbo.Cart_Items", t => t.cart_item_id)
                .ForeignKey("dbo.Cart_Items", t => t.CartItem_CartItemId)
                .Index(t => t.cart_item_id)
                .Index(t => t.amenity_id)
                .Index(t => t.CartItem_CartItemId);
            
            CreateTable(
                "dbo.FacilityImage",
                c => new
                    {
                        ImageId = c.Int(nullable: false, identity: true),
                        FacilityId = c.Int(nullable: false),
                        ImageUrl = c.String(nullable: false, maxLength: 500),
                    })
                .PrimaryKey(t => t.ImageId)
                .ForeignKey("dbo.Facility", t => t.FacilityId, cascadeDelete: true)
                .Index(t => t.FacilityId);
            
            CreateTable(
                "dbo.OperatingHour",
                c => new
                    {
                        HourId = c.Int(nullable: false, identity: true),
                        FacilityId = c.Int(nullable: false),
                        DayOfWeek = c.String(nullable: false, maxLength: 20),
                        OpenTime = c.Time(precision: 7),
                        CloseTime = c.Time(precision: 7),
                        IsClosed = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.HourId)
                .ForeignKey("dbo.Facility", t => t.FacilityId, cascadeDelete: true)
                .Index(t => t.FacilityId);
            
            CreateTable(
                "dbo.FacilityFacilityAmenity",
                c => new
                    {
                        Facility_FacilityId = c.Int(nullable: false),
                        FacilityAmenity_AmenityId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Facility_FacilityId, t.FacilityAmenity_AmenityId })
                .ForeignKey("dbo.Facility", t => t.Facility_FacilityId, cascadeDelete: true)
                .ForeignKey("dbo.FacilityAmenity", t => t.FacilityAmenity_AmenityId, cascadeDelete: true)
                .Index(t => t.Facility_FacilityId)
                .Index(t => t.FacilityAmenity_AmenityId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.BookingAmenities", "booking_id", "dbo.Booking");
            DropForeignKey("dbo.BookingAmenities", "amenity_id", "dbo.FacilityAmenity");
            DropForeignKey("dbo.Cart_Items", "FacilityAmenity_AmenityId", "dbo.FacilityAmenity");
            DropForeignKey("dbo.Booking", "FacilityAmenity_AmenityId", "dbo.FacilityAmenity");
            DropForeignKey("dbo.Booking", "role_id", "dbo.Role");
            DropForeignKey("dbo.Booking", "payment_id", "dbo.Payment");
            DropForeignKey("dbo.Booking", "facility_id", "dbo.Facility");
            DropForeignKey("dbo.OperatingHour", "FacilityId", "dbo.Facility");
            DropForeignKey("dbo.FacilityImage", "FacilityId", "dbo.Facility");
            DropForeignKey("dbo.Cart_Items", "Facility_FacilityId", "dbo.Facility");
            DropForeignKey("dbo.Cart_Items", "facility_id", "dbo.Facility");
            DropForeignKey("dbo.CartItemAmenities", "CartItem_CartItemId", "dbo.Cart_Items");
            DropForeignKey("dbo.CartItemAmenities", "cart_item_id", "dbo.Cart_Items");
            DropForeignKey("dbo.CartItemAmenities", "amenity_id", "dbo.FacilityAmenity");
            DropForeignKey("dbo.Cart_Items", "cart_id", "dbo.Cart");
            DropForeignKey("dbo.Cart", "RoleId", "dbo.Role");
            DropForeignKey("dbo.Payment", "RoleId", "dbo.Role");
            DropForeignKey("dbo.Notification", "Role_RoleId", "dbo.Role");
            DropForeignKey("dbo.Notification", "RoleId", "dbo.Role");
            DropForeignKey("dbo.Cart", "Role_RoleId", "dbo.Role");
            DropForeignKey("dbo.Booking", "Role_RoleId", "dbo.Role");
            DropForeignKey("dbo.Booking", "Facility_FacilityId", "dbo.Facility");
            DropForeignKey("dbo.FacilityFacilityAmenity", "FacilityAmenity_AmenityId", "dbo.FacilityAmenity");
            DropForeignKey("dbo.FacilityFacilityAmenity", "Facility_FacilityId", "dbo.Facility");
            DropForeignKey("dbo.BookingAmenities", "Booking_BookingId", "dbo.Booking");
            DropIndex("dbo.FacilityFacilityAmenity", new[] { "FacilityAmenity_AmenityId" });
            DropIndex("dbo.FacilityFacilityAmenity", new[] { "Facility_FacilityId" });
            DropIndex("dbo.OperatingHour", new[] { "FacilityId" });
            DropIndex("dbo.FacilityImage", new[] { "FacilityId" });
            DropIndex("dbo.CartItemAmenities", new[] { "CartItem_CartItemId" });
            DropIndex("dbo.CartItemAmenities", new[] { "amenity_id" });
            DropIndex("dbo.CartItemAmenities", new[] { "cart_item_id" });
            DropIndex("dbo.Payment", new[] { "RoleId" });
            DropIndex("dbo.Notification", new[] { "Role_RoleId" });
            DropIndex("dbo.Notification", new[] { "RoleId" });
            DropIndex("dbo.Cart", new[] { "Role_RoleId" });
            DropIndex("dbo.Cart", new[] { "RoleId" });
            DropIndex("dbo.Cart_Items", new[] { "FacilityAmenity_AmenityId" });
            DropIndex("dbo.Cart_Items", new[] { "Facility_FacilityId" });
            DropIndex("dbo.Cart_Items", new[] { "facility_id" });
            DropIndex("dbo.Cart_Items", new[] { "cart_id" });
            DropIndex("dbo.Booking", new[] { "FacilityAmenity_AmenityId" });
            DropIndex("dbo.Booking", new[] { "Role_RoleId" });
            DropIndex("dbo.Booking", new[] { "Facility_FacilityId" });
            DropIndex("dbo.Booking", new[] { "payment_id" });
            DropIndex("dbo.Booking", new[] { "facility_id" });
            DropIndex("dbo.Booking", new[] { "role_id" });
            DropIndex("dbo.BookingAmenities", new[] { "Booking_BookingId" });
            DropIndex("dbo.BookingAmenities", new[] { "amenity_id" });
            DropIndex("dbo.BookingAmenities", new[] { "booking_id" });
            DropTable("dbo.FacilityFacilityAmenity");
            DropTable("dbo.OperatingHour");
            DropTable("dbo.FacilityImage");
            DropTable("dbo.CartItemAmenities");
            DropTable("dbo.Payment");
            DropTable("dbo.Notification");
            DropTable("dbo.Role");
            DropTable("dbo.Cart");
            DropTable("dbo.Cart_Items");
            DropTable("dbo.Facility");
            DropTable("dbo.Booking");
            DropTable("dbo.FacilityAmenity");
            DropTable("dbo.BookingAmenities");
        }
    }
}
