namespace ourwebpage.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddCheckInOutToBooking1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Booking", "CheckInAt", c => c.DateTime());
            AddColumn("dbo.Booking", "CheckOutAt", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Booking", "CheckOutAt");
            DropColumn("dbo.Booking", "CheckInAt");
        }
    }
}
