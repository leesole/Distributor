namespace Distributor.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _43rd : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Companies", "AllowBranchTrading", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Companies", "AllowBranchTrading");
        }
    }
}
