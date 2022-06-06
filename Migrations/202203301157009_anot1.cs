namespace ARM_Hotel.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class anot1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Livings", "ValueOfGuests", c => c.Int(nullable: false));
            AddColumn("dbo.Bookings", "ValueOfGuests", c => c.Int(nullable: false));
            AlterColumn("dbo.AdditionalServices", "Name", c => c.String(nullable: false, maxLength: 50));
            AlterColumn("dbo.Livings", "Number", c => c.String(nullable: false, maxLength: 4));
            AlterColumn("dbo.Livings", "Type", c => c.String(nullable: false, maxLength: 50));
            AlterColumn("dbo.Apartments", "Number", c => c.String(nullable: false, maxLength: 4));
            AlterColumn("dbo.Apartments", "Type", c => c.String(nullable: false, maxLength: 50));
            AlterColumn("dbo.Bookings", "Number", c => c.String(nullable: false, maxLength: 4));
            AlterColumn("dbo.Bookings", "Type", c => c.String(nullable: false, maxLength: 50));
            AlterColumn("dbo.Services", "ServiceName", c => c.String(nullable: false, maxLength: 50));
            DropColumn("dbo.Livings", "MaxGuests");
            DropColumn("dbo.Bookings", "MaxGuests");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Bookings", "MaxGuests", c => c.Int(nullable: false));
            AddColumn("dbo.Livings", "MaxGuests", c => c.Int(nullable: false));
            AlterColumn("dbo.Services", "ServiceName", c => c.String());
            AlterColumn("dbo.Bookings", "Type", c => c.String());
            AlterColumn("dbo.Bookings", "Number", c => c.String());
            AlterColumn("dbo.Apartments", "Type", c => c.String());
            AlterColumn("dbo.Apartments", "Number", c => c.String());
            AlterColumn("dbo.Livings", "Type", c => c.String());
            AlterColumn("dbo.Livings", "Number", c => c.String());
            AlterColumn("dbo.AdditionalServices", "Name", c => c.String());
            DropColumn("dbo.Bookings", "ValueOfGuests");
            DropColumn("dbo.Livings", "ValueOfGuests");
        }
    }
}
