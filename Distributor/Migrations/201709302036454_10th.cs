namespace Distributor.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _10th : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.UserActions", "ActionLevel", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.UserActions", "ActionLevel");
        }
    }
}
