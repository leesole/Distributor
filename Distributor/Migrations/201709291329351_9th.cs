namespace Distributor.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _9th : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AppUserSettings", "CampaignGeneralInfoDisplayMyUserListings", c => c.Boolean(nullable: false));
            AddColumn("dbo.AppUserSettings", "CampaignGeneralInfoDisplayMyBranchListings", c => c.Boolean(nullable: false));
            AddColumn("dbo.AppUserSettings", "CampaignGeneralInfoDisplayMyCompanyListings", c => c.Boolean(nullable: false));
            AddColumn("dbo.AppUserSettings", "RequiredListingGeneralInfoDisplayMyUserListings", c => c.Boolean(nullable: false));
            AddColumn("dbo.AppUserSettings", "RequiredListingGeneralInfoDisplayMyBranchListings", c => c.Boolean(nullable: false));
            AddColumn("dbo.AppUserSettings", "RequiredListingGeneralInfoDisplayMyCompanyListings", c => c.Boolean(nullable: false));
            AddColumn("dbo.AppUserSettings", "AvailableListingGeneralInfoDisplayMyUserListings", c => c.Boolean(nullable: false));
            AddColumn("dbo.AppUserSettings", "AvailableListingGeneralInfoDisplayMyBranchListings", c => c.Boolean(nullable: false));
            AddColumn("dbo.AppUserSettings", "AvailableListingGeneralInfoDisplayMyCompanyListings", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AppUserSettings", "AvailableListingGeneralInfoDisplayMyCompanyListings");
            DropColumn("dbo.AppUserSettings", "AvailableListingGeneralInfoDisplayMyBranchListings");
            DropColumn("dbo.AppUserSettings", "AvailableListingGeneralInfoDisplayMyUserListings");
            DropColumn("dbo.AppUserSettings", "RequiredListingGeneralInfoDisplayMyCompanyListings");
            DropColumn("dbo.AppUserSettings", "RequiredListingGeneralInfoDisplayMyBranchListings");
            DropColumn("dbo.AppUserSettings", "RequiredListingGeneralInfoDisplayMyUserListings");
            DropColumn("dbo.AppUserSettings", "CampaignGeneralInfoDisplayMyCompanyListings");
            DropColumn("dbo.AppUserSettings", "CampaignGeneralInfoDisplayMyBranchListings");
            DropColumn("dbo.AppUserSettings", "CampaignGeneralInfoDisplayMyUserListings");
        }
    }
}
