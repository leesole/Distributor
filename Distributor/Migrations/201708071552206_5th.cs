namespace Distributor.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _5th : DbMigration
    {
        public override void Up()
        {
            DropPrimaryKey("dbo.Companies");
            AddColumn("dbo.Branches", "EntityStatus", c => c.Int(nullable: false));
            AddColumn("dbo.BranchUsers", "EntityStatus", c => c.Int(nullable: false));
            AddColumn("dbo.Companies", "CompanyId", c => c.Guid(nullable: false));
            AddColumn("dbo.Companies", "EntityStatus", c => c.Int(nullable: false));
            AddPrimaryKey("dbo.Companies", "CompanyId");
            DropColumn("dbo.Companies", "ComapnyId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Companies", "ComapnyId", c => c.Guid(nullable: false));
            DropPrimaryKey("dbo.Companies");
            DropColumn("dbo.Companies", "EntityStatus");
            DropColumn("dbo.Companies", "CompanyId");
            DropColumn("dbo.BranchUsers", "EntityStatus");
            DropColumn("dbo.Branches", "EntityStatus");
            AddPrimaryKey("dbo.Companies", "ComapnyId");
        }
    }
}
