namespace Distributor.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _7th : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AppUserSettings", "CampaignDashboardDisplayBlockedListings", c => c.Boolean(nullable: false));
            AddColumn("dbo.AppUserSettings", "RequiredListingDashboardDisplayBlockedListings", c => c.Boolean(nullable: false));
            AddColumn("dbo.AppUserSettings", "AvailableListingDashboardDisplayBlockedListings", c => c.Boolean(nullable: false));
            AddColumn("dbo.AppUserSettings", "CampaignGeneralInfoDisplayBlockedListings", c => c.Boolean(nullable: false));
            AddColumn("dbo.AppUserSettings", "RequiredListingGeneralInfoDisplayBlockedListings", c => c.Boolean(nullable: false));
            AddColumn("dbo.AppUserSettings", "AvailableListingGeneralInfoDisplayBlockedListings", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AppUserSettings", "AvailableListingGeneralInfoDisplayBlockedListings");
            DropColumn("dbo.AppUserSettings", "RequiredListingGeneralInfoDisplayBlockedListings");
            DropColumn("dbo.AppUserSettings", "CampaignGeneralInfoDisplayBlockedListings");
            DropColumn("dbo.AppUserSettings", "AvailableListingDashboardDisplayBlockedListings");
            DropColumn("dbo.AppUserSettings", "RequiredListingDashboardDisplayBlockedListings");
            DropColumn("dbo.AppUserSettings", "CampaignDashboardDisplayBlockedListings");
        }
    }
}
