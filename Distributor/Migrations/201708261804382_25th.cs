namespace Distributor.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _25th : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Orders", "OrderCreationDateTime", c => c.DateTime());
            AlterColumn("dbo.Orders", "OrderDistributionDateTime", c => c.DateTime());
            AlterColumn("dbo.Orders", "OrderDeliveredDateTime", c => c.DateTime());
            AlterColumn("dbo.Orders", "OrderCollectedDateTime", c => c.DateTime());
            AlterColumn("dbo.Orders", "OrderClosedDateTime", c => c.DateTime());
            AlterColumn("dbo.Orders", "OrderOriginatorAppUserId", c => c.Guid());
            AlterColumn("dbo.Orders", "OrderOriginatorBranchId", c => c.Guid());
            AlterColumn("dbo.Orders", "OrderOriginatorCompanyId", c => c.Guid());
            AlterColumn("dbo.Orders", "OrderOriginatorDateTime", c => c.DateTime());
            AlterColumn("dbo.Orders", "OfferId", c => c.Guid());
            AlterColumn("dbo.Orders", "OfferOriginatorAppUserId", c => c.Guid());
            AlterColumn("dbo.Orders", "OfferOriginatorBranchId", c => c.Guid());
            AlterColumn("dbo.Orders", "OfferOriginatorCompanyId", c => c.Guid());
            AlterColumn("dbo.Orders", "ListingId", c => c.Guid());
            AlterColumn("dbo.Orders", "ListingOriginatorAppUserId", c => c.Guid());
            AlterColumn("dbo.Orders", "ListingOriginatorBranchId", c => c.Guid());
            AlterColumn("dbo.Orders", "ListingOriginatorCompanyId", c => c.Guid());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Orders", "ListingOriginatorCompanyId", c => c.Guid(nullable: false));
            AlterColumn("dbo.Orders", "ListingOriginatorBranchId", c => c.Guid(nullable: false));
            AlterColumn("dbo.Orders", "ListingOriginatorAppUserId", c => c.Guid(nullable: false));
            AlterColumn("dbo.Orders", "ListingId", c => c.Guid(nullable: false));
            AlterColumn("dbo.Orders", "OfferOriginatorCompanyId", c => c.Guid(nullable: false));
            AlterColumn("dbo.Orders", "OfferOriginatorBranchId", c => c.Guid(nullable: false));
            AlterColumn("dbo.Orders", "OfferOriginatorAppUserId", c => c.Guid(nullable: false));
            AlterColumn("dbo.Orders", "OfferId", c => c.Guid(nullable: false));
            AlterColumn("dbo.Orders", "OrderOriginatorDateTime", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Orders", "OrderOriginatorCompanyId", c => c.Guid(nullable: false));
            AlterColumn("dbo.Orders", "OrderOriginatorBranchId", c => c.Guid(nullable: false));
            AlterColumn("dbo.Orders", "OrderOriginatorAppUserId", c => c.Guid(nullable: false));
            AlterColumn("dbo.Orders", "OrderClosedDateTime", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Orders", "OrderCollectedDateTime", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Orders", "OrderDeliveredDateTime", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Orders", "OrderDistributionDateTime", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Orders", "OrderCreationDateTime", c => c.DateTime(nullable: false));
        }
    }
}
