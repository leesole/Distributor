namespace Distributor.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _11th : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AppUsers", "AdminUser", c => c.Boolean(nullable: false));
            AddColumn("dbo.AppUsers", "SuperUser", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AppUsers", "SuperUser");
            DropColumn("dbo.AppUsers", "AdminUser");
        }
    }
}
