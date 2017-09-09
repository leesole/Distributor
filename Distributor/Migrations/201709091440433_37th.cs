namespace Distributor.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _37th : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Campaigns", "LocationAddressPostcode", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Campaigns", "LocationAddressPostcode", c => c.String());
        }
    }
}
