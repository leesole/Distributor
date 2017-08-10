namespace Distributor.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _8th : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AppUsers", "CurrentBranchId", c => c.Guid(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AppUsers", "CurrentBranchId");
        }
    }
}
