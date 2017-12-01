namespace Distributor.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _17th : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.SuperUserSettings",
                c => new
                    {
                        BlockId = c.Guid(nullable: false),
                        CampaignsActivated = c.Boolean(nullable: false),
                        BlocksActivated = c.Boolean(nullable: false),
                        FriendsActivated = c.Boolean(nullable: false),
                        GroupsActivated = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.BlockId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.SuperUserSettings");
        }
    }
}
