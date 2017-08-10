namespace Distributor.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _9th : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.UserTasks",
                c => new
                    {
                        TaskId = c.Guid(nullable: false),
                        AppUserId = c.Guid(nullable: false),
                        TaskType = c.Int(nullable: false),
                        TaskDescription = c.String(),
                        ReferenceKey = c.Guid(nullable: false),
                        CreatedOn = c.DateTime(nullable: false),
                        CreatedBy = c.Guid(nullable: false),
                        EntityStatus = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.TaskId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.UserTasks");
        }
    }
}
