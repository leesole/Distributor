namespace Distributor.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _22nd : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Offers",
                c => new
                    {
                        OfferId = c.Guid(nullable: false),
                        ListingId = c.Guid(nullable: false),
                        OfferType = c.Int(nullable: false),
                        OfferStatus = c.Int(nullable: false),
                        CurrentOfferQuantity = c.Decimal(nullable: false, precision: 18, scale: 2),
                        PreviousOfferQuantity = c.Decimal(nullable: false, precision: 18, scale: 2),
                        CounterOfferQuantity = c.Decimal(nullable: false, precision: 18, scale: 2),
                        OfferOriginatorAppUserId = c.Guid(nullable: false),
                        OfferOriginatorBranchId = c.Guid(nullable: false),
                        OfferOriginatorCompanyId = c.Guid(nullable: false),
                        OfferOriginatorDateTime = c.DateTime(nullable: false),
                        OrderId = c.Guid(),
                        OrderOriginatorAppUserId = c.Guid(),
                        OrderOriginatorBranchId = c.Guid(),
                        OrderOriginatorCompanyId = c.Guid(),
                        OrderOriginatorDateTime = c.DateTime(),
                    })
                .PrimaryKey(t => t.OfferId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Offers");
        }
    }
}
