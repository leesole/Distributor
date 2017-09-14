using Distributor.Helpers;
using Distributor.Models;
using Distributor.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using static Distributor.Enums.GeneralEnums;

namespace Distributor.Helpers
{
    public static class GroupHelpers
    {
        #region Get

        public static List<Group> GetGroupsForUser(Guid appUserId)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            List<Group> list = GetGroupsForUser(db, appUserId);
            db.Dispose();
            return list;
        }

        public static List<Group> GetGroupsForUser(ApplicationDbContext db, Guid appUserId)
        {
            List<Group> list = (from gm in db.GroupMembers
                                join g in db.Groups on gm.GroupId equals g.GroupId
                                where gm.AppUserId == appUserId
                                select g).ToList();

            return list;
        }

        public static List<Group> GetGroupsForTypeAndReferenceId(LevelEnum referenceLevel, Guid referenceId)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            List<Group> list = GetGroupsForTypeAndReferenceId(db, referenceLevel, referenceId);
            db.Dispose();
            return list;
        }

        public static List<Group> GetGroupsForTypeAndReferenceId(ApplicationDbContext db, LevelEnum referenceLevel, Guid referenceId)
        {
            List<Group> list = null;

            switch (referenceLevel)
            {
                case LevelEnum.Company:
                    list = (from gm in db.GroupMembers
                            join g in db.Groups on gm.GroupId equals g.GroupId
                            where gm.CompanyId == referenceId
                            select g).ToList();
                    break;
                case LevelEnum.Branch:
                    list = (from gm in db.GroupMembers
                            join g in db.Groups on gm.GroupId equals g.GroupId
                            where gm.BranchId == referenceId
                            select g).ToList();
                    break;
                case LevelEnum.User:
                    list = (from gm in db.GroupMembers
                            join g in db.Groups on gm.GroupId equals g.GroupId
                            where gm.AppUserId == referenceId
                            select g).ToList();
                    break;
            }

            return list;
        }

        #endregion

        #region Create

        #endregion

        #region Processing

        public static bool IsReferenceInGroup(LevelEnum referenceLevel, Guid ofReferenceId, Guid byReferenceId)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            bool inGroup = IsReferenceInGroup(db, referenceLevel, ofReferenceId, byReferenceId);
            db.Dispose();
            return inGroup;
        }

        public static bool IsReferenceInGroup(ApplicationDbContext db, LevelEnum referenceLevel, Guid ofReferenceId, Guid byReferenceId)
        {
            bool inGroup = false;

            //Find groups for byReference
            List<Group> byReferenceGroups = GetGroupsForTypeAndReferenceId(db, referenceLevel, byReferenceId);

            //For each group the, check that the ofReference is not there, else set to inGroup to "Y"
            foreach (Group group in byReferenceGroups)
            {
                //if we find 1 group in which this reference is there then reutrn true and jump out.
                if (GroupMemberHelpers.IsReferenceInGroupForType(db, group.GroupId, referenceLevel, ofReferenceId))
                    return true; 
            }

            return inGroup;
        }

        #endregion
    }

    public static class GroupViewHelpers
    {
        #region Get

        public static List<GroupListView> GetGroupListViewForUser(IPrincipal user)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            List<GroupListView> view = GetGroupListViewForUser(db, user);
            db.Dispose();
            return view;
        }

        public static List<GroupListView> GetGroupListViewForUser(ApplicationDbContext db, IPrincipal user)
        {
            return GetGroupListViewForUser(db, AppUserHelpers.GetAppUser(db, user).AppUserId);
        }

        public static List<GroupListView> GetGroupListViewForUser(Guid appUserId)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            List<GroupListView> view = GetGroupListViewForUser(db, appUserId);
            db.Dispose();
            return view;
        }

        public static List<GroupListView> GetGroupListViewForUser(ApplicationDbContext db, Guid appUserId)
        {
            List<GroupListView> list = new List<GroupListView>();

            //Get all groups this user is part of
            List<Group> UserGroups = GroupHelpers.GetGroupsForUser(db, appUserId);

            foreach (Group group in UserGroups)
            {
                //Get members of the group
                List<GroupMember> groupMembers = GroupMemberHelpers.GetMembersForGroup(db, group.GroupId);

                //create view record to add to list of view records
                GroupListView view = new GroupListView();
                view.Group = group;
                view.Members = groupMembers;

                list.Add(view);
            }

            return list;
        }

        public static GroupEditView GetGroupEditViewForUser(Guid appUserId)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            GroupEditView view = GetGroupEditViewForUser(db, appUserId);
            db.Dispose();
            return view;
        }

        public static GroupEditView GetGroupEditViewForUser(ApplicationDbContext db, Guid appUserId)
        {
            List<GroupListView> groupListView = GroupViewHelpers.GetGroupListViewForUser(db, appUserId);

            GroupEditView view = new GroupEditView()
            {
                GroupListViewsForUser = groupListView
            };

            return view;
        }

        #endregion
    }

    public static class GroupMemberHelpers
    {
        #region Get

        public static GroupMember GetGroupMemberForGroupAndReferenceByType(Guid groupId, LevelEnum referenceLevel, Guid referenceId)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            GroupMember member = GetGroupMemberForGroupAndReferenceByType(db, groupId, referenceLevel, referenceId);
            db.Dispose();
            return member;
        }

        public static GroupMember GetGroupMemberForGroupAndReferenceByType(ApplicationDbContext db, Guid groupId, LevelEnum referenceLevel, Guid referenceId)
        {
            GroupMember member = null;

            switch (referenceLevel)
            {
                case LevelEnum.Company:
                    member = (from gm in db.GroupMembers
                              where (gm.GroupId == groupId && gm.CompanyId == referenceId)
                              select gm).FirstOrDefault();
                    break;
                case LevelEnum.Branch:
                    member = (from gm in db.GroupMembers
                              where (gm.GroupId == groupId && gm.BranchId == referenceId)
                              select gm).FirstOrDefault();
                    break;
                case LevelEnum.User:
                    member = (from gm in db.GroupMembers
                              where (gm.GroupId == groupId && gm.AppUserId == referenceId)
                              select gm).FirstOrDefault();
                    break;
            }

            return member;
        }

        public static List<GroupMember> GetMembersForGroup(Guid groupId)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            List<GroupMember> list = GetMembersForGroup(db, groupId);
            db.Dispose();
            return list;
        }

        public static List<GroupMember> GetMembersForGroup(ApplicationDbContext db, Guid groupId)
        {
            List<GroupMember> list = (from gm in db.GroupMembers
                                      where gm.GroupId == groupId
                                      select gm).ToList();

            return list;
        }

        #endregion

        #region Create
        #endregion

        #region Processing

        public static bool IsReferenceInGroupForType(Guid groupId, LevelEnum referenceLevel, Guid referenceId)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            bool inGroup = IsReferenceInGroupForType(db, groupId, referenceLevel, referenceId);
            db.Dispose();
            return inGroup;
        }

        public static bool IsReferenceInGroupForType(ApplicationDbContext db, Guid groupId, LevelEnum referenceLevel, Guid referenceId)
        {
            bool inGroup = false;

            if (GetGroupMemberForGroupAndReferenceByType(db, groupId, referenceLevel, referenceId) != null)
                inGroup = true;

            return inGroup;
        }

        #endregion
    }
}