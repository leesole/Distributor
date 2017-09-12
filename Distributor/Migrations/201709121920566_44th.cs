namespace Distributor.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _44th : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Blocks",
                c => new
                    {
                        BlockId = c.Guid(nullable: false),
                        Type = c.Int(nullable: false),
                        BlockedById = c.Guid(nullable: false),
                        BlockedOfId = c.Guid(nullable: false),
                        BlockedOn = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.BlockId);
            
            CreateTable(
                "dbo.Friends",
                c => new
                    {
                        FriendId = c.Guid(nullable: false),
                        Type = c.Int(nullable: false),
                        RequestedById = c.Guid(nullable: false),
                        RequestedOfId = c.Guid(nullable: false),
                        Status = c.Int(nullable: false),
                        RequestedOn = c.DateTime(),
                        AccceptedOn = c.DateTime(),
                        RejectedOn = c.DateTime(),
                        ClosedOn = c.DateTime(),
                        ClosedBy = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.FriendId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Friends");
            DropTable("dbo.Blocks");
        }
    }
}
