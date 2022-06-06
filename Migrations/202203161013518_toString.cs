namespace ARM_Hotel.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class toString : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Apartments", "Number", c => c.String());
            AlterColumn("dbo.Clients", "SeriaPas", c => c.String());
            AlterColumn("dbo.Clients", "NumberPas", c => c.String());
            AlterColumn("dbo.Clients", "TelNumber", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Clients", "TelNumber", c => c.Long(nullable: false));
            AlterColumn("dbo.Clients", "NumberPas", c => c.Int(nullable: false));
            AlterColumn("dbo.Clients", "SeriaPas", c => c.Int(nullable: false));
            AlterColumn("dbo.Apartments", "Number", c => c.Int(nullable: false));
        }
    }
}
