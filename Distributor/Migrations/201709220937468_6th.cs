namespace Distributor.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _6th : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.GroupMembers", "Status", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.GroupMembers", "Status");
        }
    }
}
