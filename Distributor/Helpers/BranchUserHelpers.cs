﻿using Distributor.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
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

        public static BranchUser GetBranchUser(Guid appUserId, Guid branchId, Guid companyId)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            return GetBranchUser(db, appUserId, branchId, companyId);
        }

        public static BranchUser GetBranchUser(ApplicationDbContext db, Guid appUserId, Guid branchId, Guid companyId)
        {
            BranchUser branchUser = (from b in db.BranchUsers
                                     where (b.BranchId == branchId && b.UserId == appUserId && b.CompanyId == companyId)
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

        #region Update

        public static BranchUser UpdateBranchUserStatus(Guid branchUserId, EntityStatusEnum newEntityStatus, Guid appUserId)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            return UpdateBranchUserStatus(db, branchUserId, newEntityStatus, appUserId);
        }

        public static BranchUser UpdateBranchUserStatus(ApplicationDbContext db, Guid branchUserId, EntityStatusEnum newEntityStatus, Guid appUserId)
        {
            BranchUser branchUser = GetBranchUser(db, branchUserId);
            branchUser.PreviousEntityStatus = branchUser.EntityStatus;
            branchUser.EntityStatus = newEntityStatus;
            branchUser.EntityStatusChangeBy = appUserId;
            branchUser.EntityStatusChangeDate = DateTime.Now;

            db.Entry(branchUser).State = EntityState.Modified;
            db.SaveChanges();

            return branchUser;
        }

        public static BranchUser UpdateBranchUserStatus(BranchUser branchUser, EntityStatusEnum newEntityStatus, Guid appUserId)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            return UpdateBranchUserStatus(db, branchUser, newEntityStatus, appUserId);
        }

        public static BranchUser UpdateBranchUserStatus(ApplicationDbContext db, BranchUser branchUser, EntityStatusEnum newEntityStatus, Guid appUserId)
        {
            branchUser.PreviousEntityStatus = branchUser.EntityStatus;
            branchUser.EntityStatus = newEntityStatus;
            branchUser.EntityStatusChangeBy = appUserId;
            branchUser.EntityStatusChangeDate = DateTime.Now;

            db.Entry(branchUser).State = EntityState.Modified;
            db.SaveChanges();

            return branchUser;
        }

        #endregion
    }
}