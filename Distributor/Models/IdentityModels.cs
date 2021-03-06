﻿using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using static Distributor.Enums.UserEnums;

namespace Distributor.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        //Additional fields
        public Guid AppUserId { get; set; }
        public string FullName { get; set; }
        public UserRoleEnum CurrentUserRole { get; set; }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            userIdentity.AddClaim(new Claim("AppUserId", this.AppUserId.ToString()));
            userIdentity.AddClaim(new Claim("FullName", this.FullName));
            userIdentity.AddClaim(new Claim("CurrentUserRole", this.CurrentUserRole.ToString()));

            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        //dbset list of tables
        public DbSet<AppUser> AppUsers { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<Branch> Branches { get; set; }
        public DbSet<BranchUser> BranchUsers { get; set; }
        public DbSet<UserTask> UserTasks { get; set; }
        public DbSet<UserTaskAssignment> UserTaskAssignments { get; set; }
        public DbSet<UserAction> UserActions { get; set; }
        public DbSet<UserActionAssignment> UserActionAssignments { get; set; }

        public DbSet<Campaign> Campaigns { get; set; }
        public DbSet<RequirementListing> RequirementListings { get; set; }
        public DbSet<AvailableListing> AvailableListings { get; set; }
        public DbSet<Offer> Offers { get; set; }
        public DbSet<Order> Orders { get; set; }

        public DbSet<AppUserSettings> AppUserSettings { get; set; }

        public DbSet<Group> Groups { get; set; }
        public DbSet<GroupMember> GroupMembers { get; set; }
        public DbSet<Friend> Friends { get; set; }
        public DbSet<Block> Blocks { get; set; }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }
}