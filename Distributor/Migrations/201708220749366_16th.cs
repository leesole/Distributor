namespace Distributor.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _16th : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.BranchUsers", "PreviousEntityStatus", c => c.Int());
            AlterColumn("dbo.BranchUsers", "EntityStatusChangeBy", c => c.Guid());
            AlterColumn("dbo.BranchUsers", "EntityStatusChangeDate", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.BranchUsers", "EntityStatusChangeDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.BranchUsers", "EntityStatusChangeBy", c => c.Guid(nullable: false));
            AlterColumn("dbo.BranchUsers", "PreviousEntityStatus", c => c.Int(nullable: false));
        }
    }
}
