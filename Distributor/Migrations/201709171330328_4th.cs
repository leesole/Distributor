namespace Distributor.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _4th : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Groups", "Type", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Groups", "Type");
        }
    }
}
