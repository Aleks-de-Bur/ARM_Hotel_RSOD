namespace ARM_Hotel.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class mnogie : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.GuestsInBookings",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        BookingId = c.Int(nullable: false),
                        GuestId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.GuestsInLivings",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        LivingId = c.Int(nullable: false),
                        GuestId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.GuestsInLivings");
            DropTable("dbo.GuestsInBookings");
        }
    }
}
