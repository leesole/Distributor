namespace Distributor.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _36th : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AppUserSettings", "CampaignDashboardMaxDistance", c => c.Int());
            AddColumn("dbo.AppUserSettings", "CampaignDashboardMaxAge", c => c.Double());
            AddColumn("dbo.AppUserSettings", "CampaignDashboardExternalSelectionLevel", c => c.Int(nullable: false));
            AddColumn("dbo.AppUserSettings", "CampaignGeneralInfoMaxDistance", c => c.Int());
            AddColumn("dbo.AppUserSettings", "CampaignGeneralInfoExternalSelectionLevel", c => c.Int(nullable: false));
            AddColumn("dbo.AppUserSettings", "CampaignManageViewInternalSelectionLevel", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AppUserSettings", "CampaignManageViewInternalSelectionLevel");
            DropColumn("dbo.AppUserSettings", "CampaignGeneralInfoExternalSelectionLevel");
            DropColumn("dbo.AppUserSettings", "CampaignGeneralInfoMaxDistance");
            DropColumn("dbo.AppUserSettings", "CampaignDashboardExternalSelectionLevel");
            DropColumn("dbo.AppUserSettings", "CampaignDashboardMaxAge");
            DropColumn("dbo.AppUserSettings", "CampaignDashboardMaxDistance");
        }
    }
}
