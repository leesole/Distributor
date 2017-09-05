namespace Distributor.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _31st : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AppUsers", "LoginEmail", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.AppUsers", "LoginEmail");
        }
    }
}
