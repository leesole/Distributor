namespace Distributor.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _15th : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Orders", "ListingType", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Orders", "ListingType");
        }
    }
}
