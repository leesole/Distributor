namespace Distributor.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _21st : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AvailableListings", "ItemCategory", c => c.Int(nullable: false));
            AddColumn("dbo.RequirementListings", "ItemCategory", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.RequirementListings", "ItemCategory");
            DropColumn("dbo.AvailableListings", "ItemCategory");
        }
    }
}
