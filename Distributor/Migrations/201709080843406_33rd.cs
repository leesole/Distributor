namespace Distributor.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _33rd : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AppUserSettings",
                c => new
                    {
                        AppUserSettingsId = c.Guid(nullable: false),
                        AppUserId = c.Guid(nullable: false),
                        GlobalMaxDistance = c.Int(nullable: false),
                        GlobalMaxAge = c.Double(nullable: false),
                        GlobalInternalSelectionLevel = c.Int(nullable: false),
                        GlobalExternalSelectionLevel = c.Int(nullable: false),
                        AvailableListingGeneralInfoMaxDistance = c.Int(),
                        AvailableListingRecentMaxDistance = c.Int(),
                        AvailableListingRecentMaxAge = c.Double(),
                        RequiredListingGeneralInfoMaxDistance = c.Int(),
                        RequiredListingRecentMaxDistance = c.Int(),
                        RequiredListingRecentMaxAge = c.Double(),
                        AvailableListingManageViewInternalSelectionLevel = c.Int(nullable: false),
                        RequiredListingManageViewInternalSelectionLevel = c.Int(nullable: false),
                        AvailableListingGeneralInfoExternalSelectionLevel = c.Int(nullable: false),
                        RequiredListingGeneralInfoExternalSelectionLevel = c.Int(nullable: false),
                        AvailableListingRecentExternalSelectionLevel = c.Int(nullable: false),
                        RequiredListingRecentExternalSelectionLevel = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.AppUserSettingsId);
            
            CreateTable(
                "dbo.GroupMembers",
                c => new
                    {
                        GroupMemberId = c.Guid(nullable: false),
                        GroupId = c.Guid(nullable: false),
                        AppUserId = c.Guid(nullable: false),
                        UserAddedDateTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.GroupMemberId);
            
            CreateTable(
                "dbo.Groups",
                c => new
                    {
                        GroupId = c.Guid(nullable: false),
                        Name = c.String(nullable: false),
                        GroupOriginatorAppUserId = c.Guid(nullable: false),
                        GroupOriginatorDateTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.GroupId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Groups");
            DropTable("dbo.GroupMembers");
            DropTable("dbo.AppUserSettings");
        }
    }
}
