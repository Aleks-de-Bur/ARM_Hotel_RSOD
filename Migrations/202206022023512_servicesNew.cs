namespace ARM_Hotel.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class servicesNew : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.AdditionalServices", "Count");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AdditionalServices", "Count", c => c.Int(nullable: false));
        }
    }
}
