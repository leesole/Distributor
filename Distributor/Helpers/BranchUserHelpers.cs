using Distributor.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using static Distributor.Enums.EntityEnums;
using static Distributor.Enums.UserEnums;

namespace Distributor.Helpers
{
    public static class BranchUserHelpers
    {
        #region Get

        public static BranchUser GetBranchUser(Guid branchUserId)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            return GetBranchUser(db, branchUserId);
        }

        public static BranchUser GetBranchUser(ApplicationDbContext db, Guid branchUserId)
        {
            return db.BranchUsers.Find(branchUserId);
        }

        public static BranchUser GetBranchUser(Guid appUserId, Guid branchId)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            return GetBranchUser(db, appUserId, branchId);
        }

        public static BranchUser GetBranchUser(ApplicationDbContext db, Guid appUserId, Guid branchId)
        {
            BranchUser branchUser = (from b in db.BranchUsers
                                     where (b.BranchId == branchId && b.UserId == appUserId)
                                     select b).FirstOrDefault();

            return branchUser;
        }

        #endregion

        #region Create

        public static BranchUser CreateBranchUser(Guid userId, Guid branchId, Guid companyId, UserRoleEnum userRole, EntityStatusEnum entityStatus)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            return CreateBranchUser(db, userId, branchId, companyId, userRole, entityStatus);
        }

        public static BranchUser CreateBranchUser(ApplicationDbContext db, Guid userId, Guid branchId, Guid companyId, UserRoleEnum userRole, EntityStatusEnum entityStatus)
        {
            BranchUser branchUser = new BranchUser()
            {
                BranchUserId = Guid.NewGuid(),
                UserId = userId,
                BranchId = branchId,
                CompanyId = companyId,
                UserRole = userRole,
                EntityStatus = entityStatus
            };
            db.BranchUsers.Add(branchUser);
            db.SaveChanges();

            return branchUser;
        }

        #endregion
    }
}