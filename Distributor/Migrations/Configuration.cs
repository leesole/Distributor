namespace Distributor.Migrations
{
    using Distributor.Models;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using static Distributor.Enums.EntityEnums;

    internal sealed class Configuration : DbMigrationsConfiguration<Distributor.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(Distributor.Models.ApplicationDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //

            //context.Branches.AddOrUpdate(
            //    p => p.BranchId,
            //    new Branch() { BranchId = Guid.NewGuid(), BranchName = "Company A (Head Office)", AddressLine1 = "Address Line 1", AddressTownCity = "Address Town 1", AddressPostcode = "PostCode1", TelephoneNumber = "111111111", Email = "1@1.com", ContactName = "Contact 1A", EntityStatus = EntityStatus.Active },
            //    new Branch() { BranchId = Guid.NewGuid(), BranchName = "Branch A2", AddressLine1 = "Address Line 1A2", AddressTownCity = "Address Town 1A2", AddressPostcode = "PostCode1A2", TelephoneNumber = "111111A22", Email = "1A2@1.com", ContactName = "Contact 1A2", EntityStatus = EntityStatus.Active },
            //    new Branch() { BranchId = Guid.NewGuid(), BranchName = "Company B (Head Office)", AddressLine1 = "Address Line 2", AddressTownCity = "Address Town 2", AddressPostcode = "PostCode2", TelephoneNumber = "22222222", Email = "2@2.com", ContactName = "Contact 2B", EntityStatus = EntityStatus.Active }
            //    );

            //List<Branch> branches = context.Branches.ToList();

            //context.Companies.AddOrUpdate(
            //    p => p.CompanyId,
            //    new Company() { CompanyId = Guid.NewGuid(), CompanyName = "Company A", EntityStatus = EntityStatus.Active, CompanyRegistrationDetails = "11111111", HeadOfficeBranchId = Guid.Parse("5d03e169-4aa6-4e4a-a30b-4cebfdecbece") },
            //    new Company() { CompanyId = Guid.NewGuid(), CompanyName = "Company B", EntityStatus = EntityStatus.Active, CompanyRegistrationDetails = "22222222", HeadOfficeBranchId = Guid.Parse("8444e5d9-3677-47d1-be60-81f77a3a65ee") }
            //    );
        }
    }
}
