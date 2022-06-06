namespace ARM_Hotel.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class photo : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Photos", "Description", c => c.String());
            AddColumn("dbo.Photos", "Filename", c => c.String());
            DropColumn("dbo.Photos", "Photography");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Photos", "Photography", c => c.Binary());
            DropColumn("dbo.Photos", "Filename");
            DropColumn("dbo.Photos", "Description");
        }
    }
}
