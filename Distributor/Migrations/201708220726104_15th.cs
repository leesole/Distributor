namespace Distributor.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _15th : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.BranchUsers", "PreviousEntityStatus", c => c.Int(nullable: false));
            AddColumn("dbo.BranchUsers", "EntityStatusChangeBy", c => c.Guid(nullable: false));
            AddColumn("dbo.BranchUsers", "EntityStatusChangeDate", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.BranchUsers", "EntityStatusChangeDate");
            DropColumn("dbo.BranchUsers", "EntityStatusChangeBy");
            DropColumn("dbo.BranchUsers", "PreviousEntityStatus");
        }
    }
}
