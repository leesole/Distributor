namespace Distributor.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _19th : DbMigration
    {
        public override void Up()
        {
            DropTable("dbo.SuperUserSettings");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.SuperUserSettings",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CampaignsActivated = c.Boolean(nullable: false),
                        BlocksActivated = c.Boolean(nullable: false),
                        FriendsActivated = c.Boolean(nullable: false),
                        GroupsActivated = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
    }
}
