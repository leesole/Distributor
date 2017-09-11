namespace Distributor.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _41st : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Orders", "OrderReceivedDateTime", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Orders", "OrderReceivedDateTime");
        }
    }
}
