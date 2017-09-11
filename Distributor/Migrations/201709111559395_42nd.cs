namespace Distributor.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _42nd : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Orders", "OrderDistributedBy", c => c.Guid());
            AddColumn("dbo.Orders", "OrderDeliveredBy", c => c.Guid());
            AddColumn("dbo.Orders", "OrderCollectedBy", c => c.Guid());
            AddColumn("dbo.Orders", "OrderReceivedBy", c => c.Guid());
            AddColumn("dbo.Orders", "OrderClosedBy", c => c.Guid());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Orders", "OrderClosedBy");
            DropColumn("dbo.Orders", "OrderReceivedBy");
            DropColumn("dbo.Orders", "OrderCollectedBy");
            DropColumn("dbo.Orders", "OrderDeliveredBy");
            DropColumn("dbo.Orders", "OrderDistributedBy");
        }
    }
}
