namespace Distributor.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _18th : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.RequirementListings", "ListingBranchPostcode", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.RequirementListings", "ListingBranchPostcode");
        }
    }
}
