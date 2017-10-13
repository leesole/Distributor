namespace Distributor.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _14th : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Offers", "LastCounterOfferOriginatorAppUserId", c => c.Guid());
            AddColumn("dbo.Offers", "LastCounterOfferOriginatorBranchId", c => c.Guid());
            AddColumn("dbo.Offers", "LastCounterOfferOriginatorCompanyId", c => c.Guid());
            AddColumn("dbo.Offers", "LastCounterOfferOriginatorDateTime", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Offers", "LastCounterOfferOriginatorDateTime");
            DropColumn("dbo.Offers", "LastCounterOfferOriginatorCompanyId");
            DropColumn("dbo.Offers", "LastCounterOfferOriginatorBranchId");
            DropColumn("dbo.Offers", "LastCounterOfferOriginatorAppUserId");
        }
    }
}
