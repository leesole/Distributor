namespace Distributor.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _4th : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Branches",
                c => new
                    {
                        BranchId = c.Guid(nullable: false),
                        CompanyId = c.Guid(nullable: false),
                        BranchName = c.String(nullable: false),
                        AddressLine1 = c.String(nullable: false),
                        AddressLine2 = c.String(),
                        AddressLine3 = c.String(),
                        AddressTownCity = c.String(nullable: false),
                        AddressCounty = c.String(),
                        AddressPostcode = c.String(nullable: false),
                        TelephoneNumber = c.String(nullable: false),
                        Email = c.String(nullable: false),
                        ContactName = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.BranchId);
            
            CreateTable(
                "dbo.BranchUsers",
                c => new
                    {
                        BranchUserId = c.Guid(nullable: false),
                        UserId = c.Guid(nullable: false),
                        BranchId = c.Guid(nullable: false),
                        CompanyId = c.Guid(nullable: false),
                        UserRole = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.BranchUserId);
            
            CreateTable(
                "dbo.Companies",
                c => new
                    {
                        ComapnyId = c.Guid(nullable: false),
                        HeadOfficeBranchId = c.Guid(nullable: false),
                        CompanyName = c.String(nullable: false),
                        CompanyRegistrationDetails = c.String(),
                        CharityRegistrationDetails = c.String(),
                        VATRegistrationDetails = c.String(),
                    })
                .PrimaryKey(t => t.ComapnyId);
            
            AddColumn("dbo.AppUsers", "EntityStatus", c => c.Int(nullable: false));
            DropColumn("dbo.AppUsers", "Role");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AppUsers", "Role", c => c.Int(nullable: false));
            DropColumn("dbo.AppUsers", "EntityStatus");
            DropTable("dbo.Companies");
            DropTable("dbo.BranchUsers");
            DropTable("dbo.Branches");
        }
    }
}
