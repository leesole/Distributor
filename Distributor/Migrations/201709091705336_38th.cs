namespace Distributor.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _38th : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.GroupMembers", "BranchId", c => c.Guid(nullable: false));
            AddColumn("dbo.GroupMembers", "CompanyId", c => c.Guid(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.GroupMembers", "CompanyId");
            DropColumn("dbo.GroupMembers", "BranchId");
        }
    }
}
