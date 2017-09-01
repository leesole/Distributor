namespace Distributor.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _30th : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AvailableListings", "DeliveryAvailable", c => c.Boolean(nullable: false));
            AddColumn("dbo.RequirementListings", "CollectionAvailable", c => c.Boolean(nullable: false));
            DropColumn("dbo.AvailableListings", "CollectionAvailable");
            DropColumn("dbo.RequirementListings", "DeliveryAvailable");
        }
        
        public override void Down()
        {
            AddColumn("dbo.RequirementListings", "DeliveryAvailable", c => c.Boolean(nullable: false));
            AddColumn("dbo.AvailableListings", "CollectionAvailable", c => c.Boolean(nullable: false));
            DropColumn("dbo.RequirementListings", "CollectionAvailable");
            DropColumn("dbo.AvailableListings", "DeliveryAvailable");
        }
    }
}
