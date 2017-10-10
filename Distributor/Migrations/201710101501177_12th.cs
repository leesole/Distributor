namespace Distributor.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _12th : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AppUserSettings", "OrdersReceivedAuthorisationManageViewLevel", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AppUserSettings", "OrdersReceivedAuthorisationManageViewLevel");
        }
    }
}
