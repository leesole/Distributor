namespace Distributor.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _45th : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.UserActionAssignments",
                c => new
                    {
                        UserActionAssignmentId = c.Guid(nullable: false),
                        UserActionId = c.Guid(nullable: false),
                        AppUserId = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.UserActionAssignmentId);
            
            CreateTable(
                "dbo.UserActions",
                c => new
                    {
                        UserActionId = c.Guid(nullable: false),
                        ActionType = c.Int(nullable: false),
                        ActionDescription = c.String(),
                        ReferenceKey = c.Guid(nullable: false),
                        CreatedOn = c.DateTime(nullable: false),
                        CreatedBy = c.Guid(nullable: false),
                        EntityStatus = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.UserActionId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.UserActions");
            DropTable("dbo.UserActionAssignments");
        }
    }
}
