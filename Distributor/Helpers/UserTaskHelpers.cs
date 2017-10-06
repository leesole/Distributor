using Distributor.Models;
using Distributor.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using static Distributor.Enums.EntityEnums;
using static Distributor.Enums.UserEnums;
using static Distributor.Enums.UserTaskEnums;

namespace Distributor.Helpers
{
    public static class UserTaskHelpers
    {
        #region Get

        public static UserTask GetUserTask(Guid taskId)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            UserTask userTask = GetUserTask(db, taskId);
            db.Dispose();
            return userTask;
        }

        public static UserTask GetUserTask(ApplicationDbContext db, Guid taskId)
        {
            return db.UserTasks.Find(taskId);
        }

        public static List<UserTask> GetUserTasksForUser(Guid appUserId)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            List<UserTask> list = GetUserTasksForUser(db, appUserId);
            db.Dispose();
            return list;
        }

        public static List<UserTask> GetUserTasksForUser(ApplicationDbContext db, Guid appUserId)
        {
            List<UserTask> userTasksForUser = new List<UserTask>();
            
            userTasksForUser = (from t in db.UserTasks
                                join a in db.UserTaskAssignments on t.UserTaskId equals a.UserTaskId
                                where (a.AppUserId == appUserId && t.EntityStatus == EntityStatusEnum.Active)
                                orderby t.CreatedOn ascending
                                select t).ToList();

            return userTasksForUser;
        }

        public static List<UserTask> GetUserTasksForBranch(Guid branchId)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            List<UserTask> list = GetUserTasksForBranch(db, branchId);
            db.Dispose();
            return list;
        }

        public static List<UserTask> GetUserTasksForBranch(ApplicationDbContext db, Guid branchId)
        {
            List<UserTask> userTasksForBranch = new List<UserTask>();

            userTasksForBranch = (from t in db.UserTasks
                                join a in db.UserTaskAssignments on t.UserTaskId equals a.UserTaskId
                                where (t.ReferenceKey == branchId && t.EntityStatus == EntityStatusEnum.Active)
                                orderby t.CreatedOn ascending
                                select t).ToList();

            return userTasksForBranch;
        }

        public static List<UserTask> GetTasksByReference(Guid referenceId)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            List<UserTask> list = GetTasksByReference(db, referenceId);
            db.Dispose();
            return list;
        }

        public static List<UserTask> GetTasksByReference(ApplicationDbContext db, Guid referenceId)
        {
            List<UserTask> tasks = (from t in db.UserTasks
                                    where (t.ReferenceKey == referenceId && t.EntityStatus == EntityStatusEnum.Active)
                                    select t).ToList();

            return tasks;
        }

        #endregion

        #region Create

        public static UserTask CreateUserTask(TaskTypeEnum taskType, string taskDescription, Guid referenceKey, Guid createdBy, EntityStatusEnum entityStatus)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            UserTask userTask = CreateUserTask(db, taskType, taskDescription, referenceKey, createdBy, entityStatus);
            db.Dispose();
            return userTask;
        }

        public static UserTask CreateUserTask(ApplicationDbContext db, TaskTypeEnum taskType, string taskDescription, Guid referenceKey, Guid createdBy, EntityStatusEnum entityStatus)
        {
            UserTask userTask = new UserTask()
            {
                UserTaskId = Guid.NewGuid(),
                TaskType = taskType,
                TaskDescription = taskDescription,
                ReferenceKey = referenceKey,
                CreatedOn = DateTime.Now,
                CreatedBy = createdBy,
                EntityStatus = entityStatus
            };
            db.UserTasks.Add(userTask);

            CreateAdminListForTask(db, userTask);
            CreateManagerListForTask(db, userTask);

            db.SaveChanges();

            return userTask;
        }

        public static List<UserTaskAssignment> CreateAdminListForTask(ApplicationDbContext db, UserTask userTask)
        {
            List<UserTaskAssignment> userTaskAdminList = new List<UserTaskAssignment>();
            Guid companyId = Guid.Empty;

            //Find the company related to this Task depending on Task Type
            switch (userTask.TaskType)
            {
                case TaskTypeEnum.UserOnHold:
                    companyId = (from b in db.BranchUsers
                                 where b.UserId == userTask.ReferenceKey
                                 select b.CompanyId).FirstOrDefault();
                    break;
                case TaskTypeEnum.BranchOnHold:
                    companyId = (from b in db.BranchUsers
                                 where b.BranchId == userTask.ReferenceKey
                                 select b.CompanyId).FirstOrDefault();
                    break;
            }

            //Find all Admin users for this company
            List<AppUser> usersForCompany = (from b in db.BranchUsers
                                             join a in db.AppUsers on b.UserId equals a.AppUserId
                                             where (b.CompanyId == companyId && b.UserRole == UserRoleEnum.Admin)
                                             select a).ToList();

            var usersForCompanyDistict = usersForCompany.Distinct();

            //Add new User Task Admin records for this task
            foreach (AppUser user in usersForCompanyDistict)
            {
                UserTaskAssignment userTaskAdmin = new UserTaskAssignment()
                {
                    UserTaskAssignmentId = Guid.NewGuid(),
                    UserTaskId = userTask.UserTaskId,
                    AppUserId = user.AppUserId,
                    UserRole = UserRoleEnum.Admin
                };

                db.UserTaskAssignments.Add(userTaskAdmin);
                userTaskAdminList.Add(userTaskAdmin);
            }


            return userTaskAdminList;
        }

        public static List<UserTaskAssignment> CreateManagerListForTask(ApplicationDbContext db, UserTask userTask)
        {
            List<UserTaskAssignment> userTaskManagerList = new List<UserTaskAssignment>();
            Guid branchId = Guid.Empty;

            //Find the company related to this Task depending on Task Type
            switch (userTask.TaskType)
            {
                case TaskTypeEnum.UserOnHold:
                    branchId = (from b in db.BranchUsers
                                where b.UserId == userTask.ReferenceKey
                                select b.BranchId).FirstOrDefault();
                    break;
                case TaskTypeEnum.BranchOnHold:
                    branchId = (from b in db.BranchUsers
                                where b.BranchId == userTask.ReferenceKey
                                select b.BranchId).FirstOrDefault();
                    break;
            }

            //Find all Admin users for this branch
            List<AppUser> usersForBranch = (from b in db.BranchUsers
                                             join a in db.AppUsers on b.UserId equals a.AppUserId
                                             where (b.BranchId == branchId && b.UserRole == UserRoleEnum.Manager)
                                             select a).ToList();

            var usersForBranchDistict = usersForBranch.Distinct();

            //Add new User Task Admin records for this task
            foreach (AppUser user in usersForBranchDistict)
            {
                UserTaskAssignment userTaskManager = new UserTaskAssignment()
                {
                    UserTaskAssignmentId = Guid.NewGuid(),
                    UserTaskId = userTask.UserTaskId,
                    AppUserId = user.AppUserId,
                    UserRole = UserRoleEnum.Manager
                };

                db.UserTaskAssignments.Add(userTaskManager);
                userTaskManagerList.Add(userTaskManager);
            }

            return userTaskManagerList;
        }

        #endregion

        #region Update

        public static UserTask UpdateEntityStatus(Guid userTaskId, EntityStatusEnum entityStatus)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            UserTask userTask = UpdateEntityStatus(db, userTaskId, entityStatus);
            db.Dispose();
            return userTask;
        }

        public static UserTask UpdateEntityStatus(ApplicationDbContext db, Guid userTaskId, EntityStatusEnum entityStatus)
        {
            UserTask userTask = GetUserTask(db, userTaskId);
            userTask.EntityStatus = entityStatus;
            db.Entry(userTask).State = EntityState.Modified;
            db.SaveChanges();

            return userTask;
        }

        public static void CloseAllTasksForUserChangingStatusFromOnHold(Guid appUserId)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            CloseAllTasksForUserChangingStatusFromOnHold(db, appUserId);
            db.Dispose();
        }

        public static void CloseAllTasksForUserChangingStatusFromOnHold(ApplicationDbContext db, Guid appUserId)
        {
            List<UserTask> activeTasksForThisUser = UserTaskHelpers.GetTasksByReference(db, appUserId);

            foreach (UserTask activeTaskForThisUser in activeTasksForThisUser)
                UserTaskHelpers.UpdateEntityStatus(activeTaskForThisUser.UserTaskId, EntityStatusEnum.Closed);
        }

        #endregion
    }
    public static class UserTaskAssignmentHelpers
    {
        #region Update

        public static void ReassignAllTasksForUserChangingRoleFromAdmin(Guid appUserId)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            ReassignAllTasksForUserChangingRoleFromAdmin(db, appUserId);
            db.Dispose();
        }

        public static void ReassignAllTasksForUserChangingRoleFromAdmin(ApplicationDbContext db, Guid appUserId)
        {
            List<UserTaskAssignment> list = (from uta in db.UserTaskAssignments
                                             where uta.AppUserId == appUserId
                                             select uta).ToList();

            List<Guid> taskGuidsFromList = (from l in list
                                            select l.UserTaskId).Distinct().ToList();

            //Find current admins for the company
            List<AppUser> admins = AppUserHelpers.GetAdminAppUsersForCompany(db, BranchHelpers.GetBranch(db, AppUserHelpers.GetAppUser(db, appUserId).CurrentBranchId).CompanyId);

            //now match up admins to tasks and if any are missing add them
            foreach (Guid task in taskGuidsFromList)
            {
                foreach (AppUser admin in admins)
                {
                    List<Guid> assignedToTask = (from l in list
                                                 where l.UserTaskId == task
                                                 select l.AppUserId).ToList();

                    Guid result = assignedToTask.Find(x => x == admin.AppUserId);

                    if (result == Guid.Empty)
                        CopyTaskFromUserAToUserB(db, task, appUserId, admin.AppUserId);
                }
            }
        }
        
        public static UserTaskAssignment CopyTaskFromUserAToUserB(Guid userTaskId, Guid appUserIdA, Guid appUserIdB)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            UserTaskAssignment task = CopyTaskFromUserAToUserB(db, userTaskId, appUserIdA, appUserIdB);
            db.Dispose();
            return task;
        }

        public static UserTaskAssignment CopyTaskFromUserAToUserB(ApplicationDbContext db, Guid userTaskId, Guid appUserIdA, Guid appUserIdB)
        {
            UserTaskAssignment taskAssigment = (from ut in db.UserTaskAssignments
                                                where (ut.UserTaskId == userTaskId && ut.AppUserId == appUserIdA)
                                                select ut).FirstOrDefault();

            UserTaskAssignment taskAssignmentCopy = new UserTaskAssignment()
            {
                UserTaskAssignmentId = Guid.NewGuid(),
                UserTaskId = userTaskId,
                AppUserId = appUserIdB,
                UserRole = UserRoleEnum.Admin
            };

            db.UserTaskAssignments.Add(taskAssignmentCopy);
            db.SaveChanges();

            return taskAssignmentCopy;
        }

        #endregion
    }

    public static class UserTaskViewHelpers
    {
        #region Get

        public static List<UserTaskView> GetUserTasksForUserView(Guid appUserId)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            List<UserTaskView> list = GetUserTasksForUserView(db, appUserId);
            db.Dispose();
            return list;
        }
        public static List<UserTaskView> GetUserTasksForUserView(ApplicationDbContext db, Guid appUserId)
        {
            List<UserTaskView> userTasksForUserView = new List<UserTaskView>();

            List<UserTask> userTasksForUser = UserTaskHelpers.GetUserTasksForUser(db, appUserId);

            foreach (UserTask userTaskForUser in userTasksForUser)
            {
                AppUser appUser = null;
                Branch branch = null;

                switch (userTaskForUser.TaskType)
                {
                    case TaskTypeEnum.UserOnHold:
                        appUser = AppUserHelpers.GetAppUser(db, userTaskForUser.ReferenceKey);
                        break;
                    case TaskTypeEnum.BranchOnHold:
                        branch = BranchHelpers.GetBranch(db, userTaskForUser.ReferenceKey);
                        break;
                }

                UserTaskView userTaskForUserView = new UserTaskView()
                {
                    UserTaskId = userTaskForUser.UserTaskId,
                    TaskType = userTaskForUser.TaskType,
                    TaskDescription = userTaskForUser.TaskDescription,
                    AppUserReference = appUser,
                    BranchReference = branch,
                    CreatedOn = userTaskForUser.CreatedOn,
                    CreatedBy = AppUserHelpers.GetAppUser(db, userTaskForUser.CreatedBy),
                    EntityStatus = userTaskForUser.EntityStatus
                };

                userTasksForUserView.Add(userTaskForUserView);
            }

            return userTasksForUserView;
        }

        public static UserTaskView GetUserTaskForUserView(Guid appUserId, Guid userTaskId)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            UserTaskView view = GetUserTaskForUserView(db, appUserId, userTaskId);
            db.Dispose();
            return view;
        }

        public static UserTaskView GetUserTaskForUserView(ApplicationDbContext db, Guid appUserId, Guid userTaskId)
        {
            List<UserTaskView> userTasksForUserView = UserTaskViewHelpers.GetUserTasksForUserView(db, appUserId);

            UserTaskView userTaskForUserView = (from u in userTasksForUserView
                                                where u.UserTaskId == userTaskId
                                                select u).FirstOrDefault();

            return userTaskForUserView;
        }

        #endregion
    }
}