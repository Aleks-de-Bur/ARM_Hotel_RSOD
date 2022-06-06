namespace ARM_Hotel.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class boo : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.GuestBookings", "Guest_Id", "dbo.Guests");
            DropForeignKey("dbo.GuestBookings", "Booking_Id", "dbo.Bookings");
            DropForeignKey("dbo.GuestLivings", "Guest_Id", "dbo.Guests");
            DropForeignKey("dbo.GuestLivings", "Living_Id", "dbo.Livings");
            DropIndex("dbo.GuestBookings", new[] { "Guest_Id" });
            DropIndex("dbo.GuestBookings", new[] { "Booking_Id" });
            DropIndex("dbo.GuestLivings", new[] { "Guest_Id" });
            DropIndex("dbo.GuestLivings", new[] { "Living_Id" });
            AddColumn("dbo.Guests", "LivingId", c => c.Int());
            AddColumn("dbo.Guests", "BookingId", c => c.Int());
            CreateIndex("dbo.Guests", "LivingId");
            CreateIndex("dbo.Guests", "BookingId");
            AddForeignKey("dbo.Guests", "BookingId", "dbo.Bookings", "Id");
            AddForeignKey("dbo.Guests", "LivingId", "dbo.Livings", "Id");
            DropTable("dbo.GuestBookings");
            DropTable("dbo.GuestLivings");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.GuestLivings",
                c => new
                    {
                        Guest_Id = c.Int(nullable: false),
                        Living_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Guest_Id, t.Living_Id });
            
            CreateTable(
                "dbo.GuestBookings",
                c => new
                    {
                        Guest_Id = c.Int(nullable: false),
                        Booking_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Guest_Id, t.Booking_Id });
            
            DropForeignKey("dbo.Guests", "LivingId", "dbo.Livings");
            DropForeignKey("dbo.Guests", "BookingId", "dbo.Bookings");
            DropIndex("dbo.Guests", new[] { "BookingId" });
            DropIndex("dbo.Guests", new[] { "LivingId" });
            DropColumn("dbo.Guests", "BookingId");
            DropColumn("dbo.Guests", "LivingId");
            CreateIndex("dbo.GuestLivings", "Living_Id");
            CreateIndex("dbo.GuestLivings", "Guest_Id");
            CreateIndex("dbo.GuestBookings", "Booking_Id");
            CreateIndex("dbo.GuestBookings", "Guest_Id");
            AddForeignKey("dbo.GuestLivings", "Living_Id", "dbo.Livings", "Id", cascadeDelete: true);
            AddForeignKey("dbo.GuestLivings", "Guest_Id", "dbo.Guests", "Id", cascadeDelete: true);
            AddForeignKey("dbo.GuestBookings", "Booking_Id", "dbo.Bookings", "Id", cascadeDelete: true);
            AddForeignKey("dbo.GuestBookings", "Guest_Id", "dbo.Guests", "Id", cascadeDelete: true);
        }
    }
}
