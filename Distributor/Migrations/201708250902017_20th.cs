namespace Distributor.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _20th : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.RequirementListings", "QuantityRequired", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.RequirementListings", "QuantityFulfilled", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.RequirementListings", "QuantityOutstanding", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.RequirementListings", "RequiredFrom", c => c.DateTime());
            AlterColumn("dbo.RequirementListings", "RequiredTo", c => c.DateTime());
            AlterColumn("dbo.RequirementListings", "CampaignId", c => c.Guid());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.RequirementListings", "CampaignId", c => c.Guid(nullable: false));
            AlterColumn("dbo.RequirementListings", "RequiredTo", c => c.DateTime(nullable: false));
            AlterColumn("dbo.RequirementListings", "RequiredFrom", c => c.DateTime(nullable: false));
            AlterColumn("dbo.RequirementListings", "QuantityOutstanding", c => c.String());
            AlterColumn("dbo.RequirementListings", "QuantityFulfilled", c => c.String());
            AlterColumn("dbo.RequirementListings", "QuantityRequired", c => c.String());
        }
    }
}
