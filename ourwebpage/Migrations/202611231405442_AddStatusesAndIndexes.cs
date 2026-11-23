namespace ourwebpage.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddStatusesAndIndexes : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Booking", new[] { "facility_id" });
            CreateIndex("dbo.Booking", new[] { "facility_id", "booking_day" }, name: "IX_Booking_Facility_Day");
            CreateIndex("dbo.Role", "Email", unique: true, name: "IX_Roles_Email_Unique");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Role", "IX_Roles_Email_Unique");
            DropIndex("dbo.Booking", "IX_Booking_Facility_Day");
            CreateIndex("dbo.Booking", "facility_id");
        }
    }
}
