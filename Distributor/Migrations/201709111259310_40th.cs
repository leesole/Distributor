namespace Distributor.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _40th : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AppUserSettings", "OffersAcceptedAuthorisationManageViewLevel", c => c.Int(nullable: false));
            AddColumn("dbo.AppUserSettings", "OffersRejectedAuthorisationManageViewLevel", c => c.Int(nullable: false));
            AddColumn("dbo.AppUserSettings", "OffersReturnedAuthorisationManageViewLevel", c => c.Int(nullable: false));
            AddColumn("dbo.AppUserSettings", "OrdersDespatchedAuthorisationManageViewLevel", c => c.Int(nullable: false));
            AddColumn("dbo.AppUserSettings", "OrdersDeliveredAuthorisationManageViewLevel", c => c.Int(nullable: false));
            AddColumn("dbo.AppUserSettings", "OrdersCollectedAuthorisationManageViewLevel", c => c.Int(nullable: false));
            AddColumn("dbo.AppUserSettings", "OrdersClosedAuthorisationManageViewLevel", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AppUserSettings", "OrdersClosedAuthorisationManageViewLevel");
            DropColumn("dbo.AppUserSettings", "OrdersCollectedAuthorisationManageViewLevel");
            DropColumn("dbo.AppUserSettings", "OrdersDeliveredAuthorisationManageViewLevel");
            DropColumn("dbo.AppUserSettings", "OrdersDespatchedAuthorisationManageViewLevel");
            DropColumn("dbo.AppUserSettings", "OffersReturnedAuthorisationManageViewLevel");
            DropColumn("dbo.AppUserSettings", "OffersRejectedAuthorisationManageViewLevel");
            DropColumn("dbo.AppUserSettings", "OffersAcceptedAuthorisationManageViewLevel");
        }
    }
}
