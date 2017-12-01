namespace Distributor.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _18th : DbMigration
    {
        public override void Up()
        {
            DropPrimaryKey("dbo.SuperUserSettings");
            AddColumn("dbo.SuperUserSettings", "Id", c => c.Int(nullable: false, identity: true));
            AddPrimaryKey("dbo.SuperUserSettings", "Id");
            DropColumn("dbo.SuperUserSettings", "BlockId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.SuperUserSettings", "BlockId", c => c.Guid(nullable: false));
            DropPrimaryKey("dbo.SuperUserSettings");
            DropColumn("dbo.SuperUserSettings", "Id");
            AddPrimaryKey("dbo.SuperUserSettings", "BlockId");
        }
    }
}
