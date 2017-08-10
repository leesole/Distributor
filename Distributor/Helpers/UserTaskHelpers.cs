using Distributor.Models;
using System;
using System.Collections.Generic;
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
            return GetUserTask(db, taskId);
        }

        public static UserTask GetUserTask(ApplicationDbContext db, Guid taskId)
        {
            return db.UserTasks.Find(taskId);
        }

        public static List<UserTask> GetUserTasksForUser(Guid appUserId)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            return GetUserTasksForUser(db, appUserId);
        }

        public static List<UserTask> GetUserTasksForUser(ApplicationDbContext db, Guid appUserId)
        {
            List<UserTask> userTasksForUser = (from t in db.UserTasks
                                       //where (t.AdminAppUserId == appUserId || t.ManagerAppUserId == appUserId)
                                       //orderby t.CreatedOn ascending
                                       select t).ToList();

            return userTasksForUser;
        }

        #endregion

        #region Create

        public static UserTask CreateUserTask(TaskTypeEnum taskType, string taskDescription, Guid referenceKey, Guid createdBy, EntityStatusEnum entityStatus)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            return CreateUserTask(db, taskType, taskDescription, referenceKey, createdBy, entityStatus);
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

        public static List<UserTaskAdmin> CreateAdminListForTask(ApplicationDbContext db, UserTask userTask)
        {
            List<UserTaskAdmin> userTaskAdminList = new List<UserTaskAdmin>();
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

            //Add new User Task Admin records for this task
            foreach (AppUser user in usersForCompany)
            {
                UserTaskAdmin userTaskAdmin = new UserTaskAdmin()
                {
                    UserTaskAdminId = Guid.NewGuid(),
                    UserTaskId = userTask.UserTaskId,
                    AppUserId = user.AppUserId
                };

                db.UserTaskAdmins.Add(userTaskAdmin);
                userTaskAdminList.Add(userTaskAdmin);
            }


            return userTaskAdminList;
        }

        public static List<UserTaskManager> CreateManagerListForTask(ApplicationDbContext db, UserTask userTask)
        {
            List<UserTaskManager> userTaskManagerList = new List<UserTaskManager>();
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

            //Add new User Task Admin records for this task
            foreach (AppUser user in usersForBranch)
            {
                UserTaskManager userTaskManager = new UserTaskManager()
                {
                    UserTaskManagerId = Guid.NewGuid(),
                    UserTaskId = userTask.UserTaskId,
                    AppUserId = user.AppUserId
                };

                db.UserTaskManagers.Add(userTaskManager);
                userTaskManagerList.Add(userTaskManager);
            }


            return userTaskManagerList;
        }

        #endregion
    }
}