namespace Distributor.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _26th : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Offers", "PreviousOfferQuantity", c => c.Decimal(precision: 18, scale: 2));
            AlterColumn("dbo.Offers", "CounterOfferQuantity", c => c.Decimal(precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Offers", "CounterOfferQuantity", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.Offers", "PreviousOfferQuantity", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
    }
}
