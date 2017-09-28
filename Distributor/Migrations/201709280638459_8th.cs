namespace Distributor.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _8th : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.AppUserSettings", "CampaignDashboardDisplayBlockedListings");
            DropColumn("dbo.AppUserSettings", "RequiredListingDashboardDisplayBlockedListings");
            DropColumn("dbo.AppUserSettings", "AvailableListingDashboardDisplayBlockedListings");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AppUserSettings", "AvailableListingDashboardDisplayBlockedListings", c => c.Boolean(nullable: false));
            AddColumn("dbo.AppUserSettings", "RequiredListingDashboardDisplayBlockedListings", c => c.Boolean(nullable: false));
            AddColumn("dbo.AppUserSettings", "CampaignDashboardDisplayBlockedListings", c => c.Boolean(nullable: false));
        }
    }
}
