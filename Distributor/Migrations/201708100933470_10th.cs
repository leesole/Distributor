namespace Distributor.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _10th : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.UserTasks", "AdminAppUserId", c => c.Guid(nullable: false));
            AddColumn("dbo.UserTasks", "ManagerAppUserId", c => c.Guid(nullable: false));
            DropColumn("dbo.UserTasks", "AppUserId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.UserTasks", "AppUserId", c => c.Guid(nullable: false));
            DropColumn("dbo.UserTasks", "ManagerAppUserId");
            DropColumn("dbo.UserTasks", "AdminAppUserId");
        }
    }
}
