namespace Distributor.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _11th : DbMigration
    {
        public override void Up()
        {
            DropPrimaryKey("dbo.UserTasks");
            CreateTable(
                "dbo.UserTaskAdmins",
                c => new
                    {
                        UserTaskAdminId = c.Guid(nullable: false),
                        UserTaskId = c.Guid(nullable: false),
                        AppUserId = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.UserTaskAdminId);
            
            CreateTable(
                "dbo.UserTaskManagers",
                c => new
                    {
                        UserTaskManagerId = c.Guid(nullable: false),
                        UserTaskId = c.Guid(nullable: false),
                        AppUserId = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.UserTaskManagerId);
            
            AddColumn("dbo.UserTasks", "UserTaskId", c => c.Guid(nullable: false));
            AddPrimaryKey("dbo.UserTasks", "UserTaskId");
            DropColumn("dbo.UserTasks", "TaskId");
            DropColumn("dbo.UserTasks", "AdminAppUserId");
            DropColumn("dbo.UserTasks", "ManagerAppUserId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.UserTasks", "ManagerAppUserId", c => c.Guid(nullable: false));
            AddColumn("dbo.UserTasks", "AdminAppUserId", c => c.Guid(nullable: false));
            AddColumn("dbo.UserTasks", "TaskId", c => c.Guid(nullable: false));
            DropPrimaryKey("dbo.UserTasks");
            DropColumn("dbo.UserTasks", "UserTaskId");
            DropTable("dbo.UserTaskManagers");
            DropTable("dbo.UserTaskAdmins");
            AddPrimaryKey("dbo.UserTasks", "TaskId");
        }
    }
}
