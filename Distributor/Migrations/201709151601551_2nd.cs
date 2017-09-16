namespace Distributor.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _2nd : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.GroupMembers", "Type", c => c.Int(nullable: false));
            AddColumn("dbo.GroupMembers", "ReferenceId", c => c.Guid(nullable: false));
            AddColumn("dbo.GroupMembers", "AddedBy", c => c.Guid(nullable: false));
            AddColumn("dbo.GroupMembers", "AddedDateTime", c => c.DateTime(nullable: false));
            DropColumn("dbo.GroupMembers", "AppUserId");
            DropColumn("dbo.GroupMembers", "BranchId");
            DropColumn("dbo.GroupMembers", "CompanyId");
            DropColumn("dbo.GroupMembers", "UserAddedDateTime");
        }
        
        public override void Down()
        {
            AddColumn("dbo.GroupMembers", "UserAddedDateTime", c => c.DateTime(nullable: false));
            AddColumn("dbo.GroupMembers", "CompanyId", c => c.Guid(nullable: false));
            AddColumn("dbo.GroupMembers", "BranchId", c => c.Guid(nullable: false));
            AddColumn("dbo.GroupMembers", "AppUserId", c => c.Guid(nullable: false));
            DropColumn("dbo.GroupMembers", "AddedDateTime");
            DropColumn("dbo.GroupMembers", "AddedBy");
            DropColumn("dbo.GroupMembers", "ReferenceId");
            DropColumn("dbo.GroupMembers", "Type");
        }
    }
}
