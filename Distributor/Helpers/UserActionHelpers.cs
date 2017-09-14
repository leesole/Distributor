using Distributor.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using static Distributor.Enums.EntityEnums;
using static Distributor.Enums.GeneralEnums;
using static Distributor.Enums.UserActionEnums;

namespace Distributor.Helpers
{
    public static class UserActionHelpers
    {
        #region Get

        public static List<UserAction> GetActionsForUser(IPrincipal user)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            List<UserAction> list = GetActionsForUser(db, user);
            db.Dispose();
            return list;
        }

        public static List<UserAction> GetActionsForUser(ApplicationDbContext db, IPrincipal user)
        {
            List<UserAction> list = GetActionsForUser(db, AppUserHelpers.GetAppUserIdFromUser(user));
            return list;
        }

        public static List<UserAction> GetActionsForUser(Guid appUserId)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            List<UserAction> list = GetActionsForUser(db, appUserId);
            db.Dispose();
            return list;
        }

        public static List<UserAction> GetActionsForUser(ApplicationDbContext db, Guid appUserId)
        {
            List<UserAction> list = (from uaa in db.UserActionAssignments
                                     join ua in db.UserActions on uaa.UserActionId equals ua.UserActionId
                                     where (uaa.AppUserId == appUserId && ua.EntityStatus == EntityStatusEnum.Active)
                                     select ua).Distinct().ToList();

            return list;
        }

        public static UserActionAssignment GetActionAssignmentForActionAndUser(Guid userActionId, Guid appUserId)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            UserActionAssignment assignment = GetActionAssignmentForActionAndUser(db, userActionId, appUserId);
            db.Dispose();
            return assignment;
        }

        public static UserActionAssignment GetActionAssignmentForActionAndUser(ApplicationDbContext db, Guid userActionId, Guid appUserId)
        {
            UserActionAssignment assignment = (from uaa in db.UserActionAssignments
                                               where (uaa.UserActionId == userActionId && uaa.AppUserId == appUserId)
                                               select uaa).FirstOrDefault();

            return assignment;
        }

        #endregion

        #region Create

        public static UserAction CreateAction(ActionTypeEnum actionType, string actionDescription, Guid referenceKey, Guid createdBy, DateTime createdOn, EntityStatusEnum status)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            UserAction action = CreateAction(db, actionType, actionDescription, referenceKey,  createdBy, createdOn, status);
            db.Dispose();
            return action;
        }

        public static UserAction CreateAction(ApplicationDbContext db, ActionTypeEnum actionType, string actionDescription, Guid referenceKey, Guid createdBy, DateTime createdOn, EntityStatusEnum status)
        {
            UserAction action = new UserAction()
            {
                UserActionId = Guid.NewGuid(),
                ActionType = actionType,
                ActionDescription = actionDescription,
                ReferenceKey = referenceKey,
                CreatedBy = createdBy,
                CreatedOn = createdOn,
                EntityStatus = status
            };

            db.UserActions.Add(action);
            db.SaveChanges();

            return action;
        }

        public static UserActionAssignment CreateActionAssignment(Guid userActionId, Guid appUserId)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            UserActionAssignment assignment = CreateActionAssignment(db, userActionId, appUserId);
            db.Dispose();
            return assignment;
        }

        public static UserActionAssignment CreateActionAssignment(ApplicationDbContext db, Guid userActionId, Guid appUserId)
        {
            UserActionAssignment assignment = new UserActionAssignment()
            {
                UserActionAssignmentId = Guid.NewGuid(),
                UserActionId = userActionId,
                AppUserId = appUserId
            };

            db.UserActionAssignments.Add(assignment);
            db.SaveChanges();

            return assignment;
        }

        public static UserAction CreateActionForFriendRequestFromUser(LevelEnum level, Guid ofReferenceId, Guid byReferenceId)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            UserAction action = CreateActionForFriendRequestFromUser(db, level, ofReferenceId, byReferenceId);
            db.Dispose();
            return action;
        }

        /// <summary>
        /// Create a Friend Request UserAction.  This will be created for a user at User level, the manager(s) of branch at Branch level, the admin(s) of company at Company level
        /// </summary>
        /// <param name="db"></param>
        /// <param name="level">level of request, i.e. User, Branch, Company, Group</param>
        /// <param name="ofReferenceId">The 'level' reference id</param>
        /// <param name="byReferenceId">The userid making the request</param>
        /// <returns></returns>
        public static UserAction CreateActionForFriendRequestFromUser(ApplicationDbContext db, LevelEnum level, Guid ofReferenceId, Guid byReferenceId)
        {
            UserAction action = CreateAction(ActionTypeEnum.AwaitFriendRequest, EnumHelpers.GetDescription(level) + " level request.", byReferenceId, byReferenceId, DateTime.Now, EntityStatusEnum.Active);

            //Create assignment list for this UserAction
            switch (level)
            {
                case LevelEnum.Company:
                    CreateUserAssignmentForAction(db, action.UserActionId, level, ofReferenceId, byReferenceId);
                    break;
                case LevelEnum.Branch:
                    CreateUserAssignmentForAction(db, action.UserActionId, level, ofReferenceId, byReferenceId);
                    break;
                case LevelEnum.User:
                    CreateUserAssignmentForAction(db, action.UserActionId, level, ofReferenceId, byReferenceId);
                    break;
            }

            return action;
        }

        public static List<UserActionAssignment> CreateUserAssignmentForAction(Guid userActionId, LevelEnum level, Guid ofReferenceId, Guid byReferenceId)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            List<UserActionAssignment> list = CreateUserAssignmentForAction(db, userActionId, level, ofReferenceId, byReferenceId);
            db.Dispose();
            return list;
        }

        /// <summary>
        /// Build the list of assignments for this Action.  At user level there is the one, at Branch level there is the 'Manager' of the branch and the 'Admin' of the company, and at 
        ///     Admin level there is the 'Admin' of the company.  There could be multiple for Branch and Admin levels
        /// </summary>
        /// <param name="db"></param>
        /// <param name="level">level of request, User, Branch, Company</param>
        /// <param name="ofReferenceId">id based on level, so AppUserId, BranchId, CompanyId</param>
        /// <param name="byReferenceId">id based on level, so AppUserId, BranchId, CompanyId</param>
        /// <returns></returns>
        public static List<UserActionAssignment> CreateUserAssignmentForAction(ApplicationDbContext db, Guid userActionId, LevelEnum level, Guid ofReferenceId, Guid byReferenceId)
        {
            List<UserActionAssignment> list = new List<UserActionAssignment>();

            switch (level)
            {
                case LevelEnum.Company:
                    List<AppUser> adminUsers = AppUserHelpers.GetAdminAppUsersForCompany(db, ofReferenceId);
                    foreach (AppUser adminUser in adminUsers)
                    {
                        UserActionAssignment adminUserAssignment = CreateActionAssignment(db, userActionId, adminUser.AppUserId);
                        list.Add(adminUserAssignment);
                    }
                    break;
                case LevelEnum.Branch:
                    List<AppUser> branchUsers = AppUserHelpers.GetManagerAppUsersForBranch(db, ofReferenceId);
                    foreach (AppUser branchUser in branchUsers)
                    {
                        UserActionAssignment branchUserAssignment = CreateActionAssignment(db, userActionId, branchUser.AppUserId);
                        list.Add(branchUserAssignment);
                    }
                    //Note, Admin users can also approve these requests so list them in the assignment list also
                    List<AppUser> adminUsersForBranchApproval = AppUserHelpers.GetAdminAppUsersForCompany(db, BranchHelpers.GetBranch(ofReferenceId).CompanyId);
                    foreach (AppUser adminUser in adminUsersForBranchApproval)
                    {
                        //Don't duplicate user entries
                        if (GetActionAssignmentForActionAndUser(db, userActionId, adminUser.AppUserId) == null)
                        {
                            UserActionAssignment adminUserAssignment = CreateActionAssignment(db, userActionId, adminUser.AppUserId);
                            list.Add(adminUserAssignment);
                        }
                    }
                    break;
                case LevelEnum.User:
                    UserActionAssignment assignment = CreateActionAssignment(db, userActionId, ofReferenceId);
                    list.Add(assignment);
                    break;
            }

            return list;
        }

        #endregion
    }
}