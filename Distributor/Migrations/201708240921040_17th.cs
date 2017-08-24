namespace Distributor.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _17th : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Campaigns",
                c => new
                    {
                        CampaignId = c.Guid(nullable: false),
                        Name = c.String(),
                        StrapLine = c.String(),
                        Description = c.String(),
                        Image = c.Binary(),
                        ImageLocation = c.String(),
                        Website = c.String(),
                        CampaignStartDateTime = c.DateTime(nullable: false),
                        CampaignEndDateTime = c.DateTime(nullable: false),
                        LocationName = c.String(),
                        LocationAddressLine1 = c.String(),
                        LocationAddressLine2 = c.String(),
                        LocationAddressLine3 = c.String(),
                        LocationAddressTownCity = c.String(),
                        LocationAddressCounty = c.String(),
                        LocationAddressPostcode = c.String(),
                        LocationTelephoneNumber = c.String(),
                        LocationEmail = c.String(),
                        LocationContactName = c.String(),
                        EntityStatus = c.Int(nullable: false),
                        CampaignOriginatorAppUserId = c.Guid(nullable: false),
                        CampaignOriginatorBranchId = c.Guid(nullable: false),
                        CampaignOriginatorCompanyId = c.Guid(nullable: false),
                        CampaignOriginatorDateTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.CampaignId);
            
            CreateTable(
                "dbo.RequirementListings",
                c => new
                    {
                        ListingId = c.Guid(nullable: false),
                        ItemDescription = c.String(nullable: false),
                        ItemType = c.Int(nullable: false),
                        QuantityRequired = c.String(),
                        QuantityFulfilled = c.String(),
                        QuantityOutstanding = c.String(),
                        UoM = c.String(),
                        RequiredFrom = c.DateTime(nullable: false),
                        RequiredTo = c.DateTime(nullable: false),
                        AcceptDamagedItems = c.Boolean(nullable: false),
                        DeliveryAvailable = c.Boolean(nullable: false),
                        ListingStatus = c.Int(nullable: false),
                        ListingOriginatorAppUserId = c.Guid(nullable: false),
                        ListingOriginatorBranchId = c.Guid(nullable: false),
                        ListingOriginatorCompanyId = c.Guid(nullable: false),
                        ListingOriginatorDateTime = c.DateTime(nullable: false),
                        CampaignId = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.ListingId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.RequirementListings");
            DropTable("dbo.Campaigns");
        }
    }
}
