namespace ARM_Hotel.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class newTables : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AdditionalServices",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Price = c.Decimal(nullable: false, precision: 18, scale: 2),
                        LivingId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Livings", t => t.LivingId)
                .Index(t => t.LivingId);
            
            CreateTable(
                "dbo.Livings",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Settling = c.DateTime(nullable: false),
                        Eviction = c.DateTime(nullable: false),
                        Number = c.String(),
                        MaxGuests = c.Int(nullable: false),
                        ClientId = c.Int(),
                        GuestId = c.Int(),
                        Price = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Type = c.String(),
                        ApartmentId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Clients", t => t.ClientId)
                .ForeignKey("dbo.Apartments", t => t.ApartmentId)
                .ForeignKey("dbo.Guests", t => t.GuestId)
                .Index(t => t.ClientId)
                .Index(t => t.GuestId)
                .Index(t => t.ApartmentId);
            
            CreateTable(
                "dbo.Bookings",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Settling = c.DateTime(nullable: false),
                        Eviction = c.DateTime(nullable: false),
                        Number = c.String(),
                        MaxGuests = c.Int(nullable: false),
                        ClientId = c.Int(),
                        GuestId = c.Int(),
                        Price = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Type = c.String(),
                        ApartmentId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Apartments", t => t.ApartmentId)
                .ForeignKey("dbo.Clients", t => t.ClientId)
                .ForeignKey("dbo.Guests", t => t.GuestId)
                .Index(t => t.ClientId)
                .Index(t => t.GuestId)
                .Index(t => t.ApartmentId);
            
            CreateTable(
                "dbo.Guests",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FirstName = c.String(),
                        LastName = c.String(),
                        Patronymic = c.String(),
                        SeriaPas = c.String(),
                        NumberPas = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Services", "AdditionalServiceId", c => c.Int());
            CreateIndex("dbo.Services", "AdditionalServiceId");
            AddForeignKey("dbo.Services", "AdditionalServiceId", "dbo.AdditionalServices", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Livings", "GuestId", "dbo.Guests");
            DropForeignKey("dbo.Bookings", "GuestId", "dbo.Guests");
            DropForeignKey("dbo.Services", "AdditionalServiceId", "dbo.AdditionalServices");
            DropForeignKey("dbo.Livings", "ApartmentId", "dbo.Apartments");
            DropForeignKey("dbo.Livings", "ClientId", "dbo.Clients");
            DropForeignKey("dbo.Bookings", "ClientId", "dbo.Clients");
            DropForeignKey("dbo.Bookings", "ApartmentId", "dbo.Apartments");
            DropForeignKey("dbo.AdditionalServices", "LivingId", "dbo.Livings");
            DropIndex("dbo.Services", new[] { "AdditionalServiceId" });
            DropIndex("dbo.Bookings", new[] { "ApartmentId" });
            DropIndex("dbo.Bookings", new[] { "GuestId" });
            DropIndex("dbo.Bookings", new[] { "ClientId" });
            DropIndex("dbo.Livings", new[] { "ApartmentId" });
            DropIndex("dbo.Livings", new[] { "GuestId" });
            DropIndex("dbo.Livings", new[] { "ClientId" });
            DropIndex("dbo.AdditionalServices", new[] { "LivingId" });
            DropColumn("dbo.Services", "AdditionalServiceId");
            DropTable("dbo.Guests");
            DropTable("dbo.Bookings");
            DropTable("dbo.Livings");
            DropTable("dbo.AdditionalServices");
        }
    }
}
