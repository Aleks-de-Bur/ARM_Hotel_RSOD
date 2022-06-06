namespace ARM_Hotel.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Services : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Services", "AdditionalServiceId", "dbo.AdditionalServices");
            DropIndex("dbo.Services", new[] { "AdditionalServiceId" });
            AddColumn("dbo.AdditionalServices", "ServiceId", c => c.Int());
            AddColumn("dbo.AdditionalServices", "Count", c => c.Int(nullable: false));
            CreateIndex("dbo.AdditionalServices", "ServiceId");
            AddForeignKey("dbo.AdditionalServices", "ServiceId", "dbo.Services", "Id");
            DropColumn("dbo.AdditionalServices", "Name");
            DropColumn("dbo.Services", "AdditionalServiceId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Services", "AdditionalServiceId", c => c.Int());
            AddColumn("dbo.AdditionalServices", "Name", c => c.String(nullable: false, maxLength: 50));
            DropForeignKey("dbo.AdditionalServices", "ServiceId", "dbo.Services");
            DropIndex("dbo.AdditionalServices", new[] { "ServiceId" });
            DropColumn("dbo.AdditionalServices", "Count");
            DropColumn("dbo.AdditionalServices", "ServiceId");
            CreateIndex("dbo.Services", "AdditionalServiceId");
            AddForeignKey("dbo.Services", "AdditionalServiceId", "dbo.AdditionalServices", "Id");
        }
    }
}
