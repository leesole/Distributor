namespace Distributor.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _46th : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Blocks", "BlockedByUserId", c => c.Guid(nullable: false));
            AddColumn("dbo.Friends", "RequestedByUserId", c => c.Guid(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Friends", "RequestedByUserId");
            DropColumn("dbo.Blocks", "BlockedByUserId");
        }
    }
}
