namespace Distributor.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _32nd : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AvailableListings", "DisplayUntilDate", c => c.DateTime());
            AddColumn("dbo.AvailableListings", "SellByDate", c => c.DateTime());
            AddColumn("dbo.AvailableListings", "UseByDate", c => c.DateTime());
            AddColumn("dbo.RequirementListings", "AcceptOutOfDateItems", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.RequirementListings", "AcceptOutOfDateItems");
            DropColumn("dbo.AvailableListings", "UseByDate");
            DropColumn("dbo.AvailableListings", "SellByDate");
            DropColumn("dbo.AvailableListings", "DisplayUntilDate");
        }
    }
}
