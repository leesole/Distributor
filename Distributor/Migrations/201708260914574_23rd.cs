namespace Distributor.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _23rd : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Offers", "ListingType", c => c.Int(nullable: false));
            DropColumn("dbo.Offers", "OfferType");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Offers", "OfferType", c => c.Int(nullable: false));
            DropColumn("dbo.Offers", "ListingType");
        }
    }
}
