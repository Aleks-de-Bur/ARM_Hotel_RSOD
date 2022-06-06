namespace ARM_Hotel.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class bag : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Bookings", "GuestId", "dbo.Guests");
            DropForeignKey("dbo.Livings", "GuestId", "dbo.Guests");
            DropIndex("dbo.Livings", new[] { "GuestId" });
            DropIndex("dbo.Bookings", new[] { "GuestId" });
            CreateTable(
                "dbo.GuestBookings",
                c => new
                    {
                        Guest_Id = c.Int(nullable: false),
                        Booking_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Guest_Id, t.Booking_Id })
                .ForeignKey("dbo.Guests", t => t.Guest_Id, cascadeDelete: true)
                .ForeignKey("dbo.Bookings", t => t.Booking_Id, cascadeDelete: true)
                .Index(t => t.Guest_Id)
                .Index(t => t.Booking_Id);
            
            CreateTable(
                "dbo.GuestLivings",
                c => new
                    {
                        Guest_Id = c.Int(nullable: false),
                        Living_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Guest_Id, t.Living_Id })
                .ForeignKey("dbo.Guests", t => t.Guest_Id, cascadeDelete: true)
                .ForeignKey("dbo.Livings", t => t.Living_Id, cascadeDelete: true)
                .Index(t => t.Guest_Id)
                .Index(t => t.Living_Id);
            
            DropColumn("dbo.Livings", "GuestId");
            DropColumn("dbo.Bookings", "GuestId");
            DropTable("dbo.GuestsInBookings");
            DropTable("dbo.GuestsInLivings");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.GuestsInLivings",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        LivingId = c.Int(nullable: false),
                        GuestId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.GuestsInBookings",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        BookingId = c.Int(nullable: false),
                        GuestId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Bookings", "GuestId", c => c.Int());
            AddColumn("dbo.Livings", "GuestId", c => c.Int());
            DropForeignKey("dbo.GuestLivings", "Living_Id", "dbo.Livings");
            DropForeignKey("dbo.GuestLivings", "Guest_Id", "dbo.Guests");
            DropForeignKey("dbo.GuestBookings", "Booking_Id", "dbo.Bookings");
            DropForeignKey("dbo.GuestBookings", "Guest_Id", "dbo.Guests");
            DropIndex("dbo.GuestLivings", new[] { "Living_Id" });
            DropIndex("dbo.GuestLivings", new[] { "Guest_Id" });
            DropIndex("dbo.GuestBookings", new[] { "Booking_Id" });
            DropIndex("dbo.GuestBookings", new[] { "Guest_Id" });
            DropTable("dbo.GuestLivings");
            DropTable("dbo.GuestBookings");
            CreateIndex("dbo.Bookings", "GuestId");
            CreateIndex("dbo.Livings", "GuestId");
            AddForeignKey("dbo.Livings", "GuestId", "dbo.Guests", "Id");
            AddForeignKey("dbo.Bookings", "GuestId", "dbo.Guests", "Id");
        }
    }
}
