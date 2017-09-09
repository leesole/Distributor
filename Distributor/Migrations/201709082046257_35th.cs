namespace Distributor.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _35th : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AppUserSettings", "RequiredListingDashboardMaxDistance", c => c.Int());
            AddColumn("dbo.AppUserSettings", "RequiredListingDashboardMaxAge", c => c.Double());
            AddColumn("dbo.AppUserSettings", "AvailableListingDashboardMaxDistance", c => c.Int());
            AddColumn("dbo.AppUserSettings", "AvailableListingDashboardMaxAge", c => c.Double());
            AddColumn("dbo.AppUserSettings", "RequiredListingDashboardExternalSelectionLevel", c => c.Int(nullable: false));
            AddColumn("dbo.AppUserSettings", "AvailableListingDashboardExternalSelectionLevel", c => c.Int(nullable: false));
            DropColumn("dbo.AppUserSettings", "GlobalMaxDistance");
            DropColumn("dbo.AppUserSettings", "GlobalMaxAge");
            DropColumn("dbo.AppUserSettings", "GlobalInternalSelectionLevel");
            DropColumn("dbo.AppUserSettings", "GlobalExternalSelectionLevel");
            DropColumn("dbo.AppUserSettings", "AvailableListingRecentMaxDistance");
            DropColumn("dbo.AppUserSettings", "AvailableListingRecentMaxAge");
            DropColumn("dbo.AppUserSettings", "RequiredListingRecentMaxDistance");
            DropColumn("dbo.AppUserSettings", "RequiredListingRecentMaxAge");
            DropColumn("dbo.AppUserSettings", "AvailableListingRecentExternalSelectionLevel");
            DropColumn("dbo.AppUserSettings", "RequiredListingRecentExternalSelectionLevel");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AppUserSettings", "RequiredListingRecentExternalSelectionLevel", c => c.Int(nullable: false));
            AddColumn("dbo.AppUserSettings", "AvailableListingRecentExternalSelectionLevel", c => c.Int(nullable: false));
            AddColumn("dbo.AppUserSettings", "RequiredListingRecentMaxAge", c => c.Double());
            AddColumn("dbo.AppUserSettings", "RequiredListingRecentMaxDistance", c => c.Int());
            AddColumn("dbo.AppUserSettings", "AvailableListingRecentMaxAge", c => c.Double());
            AddColumn("dbo.AppUserSettings", "AvailableListingRecentMaxDistance", c => c.Int());
            AddColumn("dbo.AppUserSettings", "GlobalExternalSelectionLevel", c => c.Int(nullable: false));
            AddColumn("dbo.AppUserSettings", "GlobalInternalSelectionLevel", c => c.Int(nullable: false));
            AddColumn("dbo.AppUserSettings", "GlobalMaxAge", c => c.Double());
            AddColumn("dbo.AppUserSettings", "GlobalMaxDistance", c => c.Int());
            DropColumn("dbo.AppUserSettings", "AvailableListingDashboardExternalSelectionLevel");
            DropColumn("dbo.AppUserSettings", "RequiredListingDashboardExternalSelectionLevel");
            DropColumn("dbo.AppUserSettings", "AvailableListingDashboardMaxAge");
            DropColumn("dbo.AppUserSettings", "AvailableListingDashboardMaxDistance");
            DropColumn("dbo.AppUserSettings", "RequiredListingDashboardMaxAge");
            DropColumn("dbo.AppUserSettings", "RequiredListingDashboardMaxDistance");
        }
    }
}
