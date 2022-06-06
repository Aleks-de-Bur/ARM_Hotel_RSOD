namespace ARM_Hotel.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addBool : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Livings", "Active", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Livings", "Active");
        }
    }
}
