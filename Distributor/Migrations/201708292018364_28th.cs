namespace Distributor.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _28th : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Offers", "RejectedBy", c => c.Guid());
            AddColumn("dbo.Offers", "RejectedOn", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Offers", "RejectedOn");
            DropColumn("dbo.Offers", "RejectedBy");
        }
    }
}
