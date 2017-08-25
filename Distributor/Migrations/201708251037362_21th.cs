namespace Distributor.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _21th : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AvailableListings",
                c => new
                    {
                        ListingId = c.Guid(nullable: false),
                        ItemDescription = c.String(nullable: false),
                        ItemType = c.Int(nullable: false),
                        QuantityRequired = c.Decimal(nullable: false, precision: 18, scale: 2),
                        QuantityFulfilled = c.Decimal(nullable: false, precision: 18, scale: 2),
                        QuantityOutstanding = c.Decimal(nullable: false, precision: 18, scale: 2),
                        UoM = c.String(),
                        AvailableFrom = c.DateTime(),
                        AvailableTo = c.DateTime(),
                        ItemCondition = c.Int(nullable: false),
                        CollectionAvailable = c.Boolean(nullable: false),
                        ListingStatus = c.Int(nullable: false),
                        ListingBranchPostcode = c.String(),
                        ListingOriginatorAppUserId = c.Guid(nullable: false),
                        ListingOriginatorBranchId = c.Guid(nullable: false),
                        ListingOriginatorCompanyId = c.Guid(nullable: false),
                        ListingOriginatorDateTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ListingId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.AvailableListings");
        }
    }
}
