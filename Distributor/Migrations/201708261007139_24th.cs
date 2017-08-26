namespace Distributor.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _24th : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Orders",
                c => new
                    {
                        OrderId = c.Guid(nullable: false),
                        OrderQuanity = c.Decimal(nullable: false, precision: 18, scale: 2),
                        OrderStatus = c.Int(nullable: false),
                        OrderCreationDateTime = c.DateTime(nullable: false),
                        OrderDistributionDateTime = c.DateTime(nullable: false),
                        OrderDeliveredDateTime = c.DateTime(nullable: false),
                        OrderCollectedDateTime = c.DateTime(nullable: false),
                        OrderClosedDateTime = c.DateTime(nullable: false),
                        OrderOriginatorAppUserId = c.Guid(nullable: false),
                        OrderOriginatorBranchId = c.Guid(nullable: false),
                        OrderOriginatorCompanyId = c.Guid(nullable: false),
                        OrderOriginatorDateTime = c.DateTime(nullable: false),
                        OfferId = c.Guid(nullable: false),
                        OfferOriginatorAppUserId = c.Guid(nullable: false),
                        OfferOriginatorBranchId = c.Guid(nullable: false),
                        OfferOriginatorCompanyId = c.Guid(nullable: false),
                        ListingId = c.Guid(nullable: false),
                        ListingOriginatorAppUserId = c.Guid(nullable: false),
                        ListingOriginatorBranchId = c.Guid(nullable: false),
                        ListingOriginatorCompanyId = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.OrderId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Orders");
        }
    }
}
