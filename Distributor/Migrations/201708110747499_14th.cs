namespace Distributor.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _14th : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.UserTaskAssignments",
                c => new
                    {
                        UserTaskAssignmentId = c.Guid(nullable: false),
                        UserTaskId = c.Guid(nullable: false),
                        AppUserId = c.Guid(nullable: false),
                        UserRole = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.UserTaskAssignmentId);
            
            DropTable("dbo.UserTaskAdmins");
            DropTable("dbo.UserTaskManagers");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.UserTaskManagers",
                c => new
                    {
                        UserTaskManagerId = c.Guid(nullable: false),
                        UserTaskId = c.Guid(nullable: false),
                        AppUserId = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.UserTaskManagerId);
            
            CreateTable(
                "dbo.UserTaskAdmins",
                c => new
                    {
                        UserTaskAdminId = c.Guid(nullable: false),
                        UserTaskId = c.Guid(nullable: false),
                        AppUserId = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.UserTaskAdminId);
            
            DropTable("dbo.UserTaskAssignments");
        }
    }
}
