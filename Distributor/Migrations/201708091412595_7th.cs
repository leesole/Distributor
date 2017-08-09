namespace Distributor.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _7th : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Branches", "BusinessType", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Branches", "BusinessType");
        }
    }
}
