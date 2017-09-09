namespace Distributor.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _39th : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AppUserSettings", "OffersManageViewInternalSelectionLevel", c => c.Int(nullable: false));
            AddColumn("dbo.AppUserSettings", "OrdersManageViewInternalSelectionLevel", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AppUserSettings", "OrdersManageViewInternalSelectionLevel");
            DropColumn("dbo.AppUserSettings", "OffersManageViewInternalSelectionLevel");
        }
    }
}
