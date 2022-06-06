namespace ARM_Hotel.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class anot : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Clients", "FirstName", c => c.String(nullable: false, maxLength: 50));
            AlterColumn("dbo.Clients", "LastName", c => c.String(nullable: false, maxLength: 50));
            AlterColumn("dbo.Clients", "Patronymic", c => c.String(nullable: false, maxLength: 50));
            AlterColumn("dbo.Clients", "SeriaPas", c => c.String(nullable: false, maxLength: 4));
            AlterColumn("dbo.Clients", "NumberPas", c => c.String(nullable: false, maxLength: 6));
            AlterColumn("dbo.Clients", "TelNumber", c => c.String(nullable: false, maxLength: 12));
            AlterColumn("dbo.Guests", "FirstName", c => c.String(nullable: false, maxLength: 50));
            AlterColumn("dbo.Guests", "LastName", c => c.String(nullable: false, maxLength: 50));
            AlterColumn("dbo.Guests", "Patronymic", c => c.String(nullable: false, maxLength: 50));
            AlterColumn("dbo.Guests", "SeriaPas", c => c.String(nullable: false, maxLength: 4));
            AlterColumn("dbo.Guests", "NumberPas", c => c.String(nullable: false, maxLength: 6));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Guests", "NumberPas", c => c.String());
            AlterColumn("dbo.Guests", "SeriaPas", c => c.String());
            AlterColumn("dbo.Guests", "Patronymic", c => c.String());
            AlterColumn("dbo.Guests", "LastName", c => c.String());
            AlterColumn("dbo.Guests", "FirstName", c => c.String());
            AlterColumn("dbo.Clients", "TelNumber", c => c.String());
            AlterColumn("dbo.Clients", "NumberPas", c => c.String());
            AlterColumn("dbo.Clients", "SeriaPas", c => c.String());
            AlterColumn("dbo.Clients", "Patronymic", c => c.String());
            AlterColumn("dbo.Clients", "LastName", c => c.String());
            AlterColumn("dbo.Clients", "FirstName", c => c.String());
        }
    }
}
