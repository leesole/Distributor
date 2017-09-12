using Distributor.Helpers;
using Distributor.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;

namespace Distributor.Models
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

        #endregion

        #region Create



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
    }
}