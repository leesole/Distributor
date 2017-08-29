namespace Distributor.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _29th : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Offers", "RejectedOn", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Offers", "RejectedOn", c => c.DateTime(nullable: false));
        }
    }
}
