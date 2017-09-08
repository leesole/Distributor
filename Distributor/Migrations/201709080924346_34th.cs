namespace Distributor.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _34th : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.AppUserSettings", "GlobalMaxDistance", c => c.Int());
            AlterColumn("dbo.AppUserSettings", "GlobalMaxAge", c => c.Double());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.AppUserSettings", "GlobalMaxAge", c => c.Double(nullable: false));
            AlterColumn("dbo.AppUserSettings", "GlobalMaxDistance", c => c.Int(nullable: false));
        }
    }
}
