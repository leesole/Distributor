namespace Distributor.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _5th : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Groups", "VisibilityLevel", c => c.Int(nullable: false));
            AddColumn("dbo.Groups", "InviteLevel", c => c.Int(nullable: false));
            AddColumn("dbo.Groups", "AcceptanceLevel", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Groups", "AcceptanceLevel");
            DropColumn("dbo.Groups", "InviteLevel");
            DropColumn("dbo.Groups", "VisibilityLevel");
        }
    }
}
