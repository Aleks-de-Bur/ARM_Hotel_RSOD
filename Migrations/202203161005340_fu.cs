namespace ARM_Hotel.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class fu : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Clients", "TelNumber", c => c.Long(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Clients", "TelNumber", c => c.Int(nullable: false));
        }
    }
}
