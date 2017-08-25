namespace Distributor.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _19th : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Campaigns", "CampaignStartDateTime", c => c.DateTime());
            AlterColumn("dbo.Campaigns", "CampaignEndDateTime", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Campaigns", "CampaignEndDateTime", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Campaigns", "CampaignStartDateTime", c => c.DateTime(nullable: false));
        }
    }
}
