namespace Distributor.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _3rd : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AppUsers", "PrivacyLevel", c => c.Int(nullable: false));
            AddColumn("dbo.Branches", "PrivacyLevel", c => c.Int(nullable: false));
            AddColumn("dbo.Companies", "PrivacyLevel", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Companies", "PrivacyLevel");
            DropColumn("dbo.Branches", "PrivacyLevel");
            DropColumn("dbo.AppUsers", "PrivacyLevel");
        }
    }
}
