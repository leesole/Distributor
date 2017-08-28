namespace Distributor.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _27th : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Offers", "ListingOriginatorAppUserId", c => c.Guid());
            AddColumn("dbo.Offers", "ListingOriginatorBranchId", c => c.Guid());
            AddColumn("dbo.Offers", "ListingOriginatorCompanyId", c => c.Guid());
            AddColumn("dbo.Offers", "ListingOriginatorDateTime", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Offers", "ListingOriginatorDateTime");
            DropColumn("dbo.Offers", "ListingOriginatorCompanyId");
            DropColumn("dbo.Offers", "ListingOriginatorBranchId");
            DropColumn("dbo.Offers", "ListingOriginatorAppUserId");
        }
    }
}
