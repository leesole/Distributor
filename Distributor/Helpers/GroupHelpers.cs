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

        public static List<Group> GetGroupsCreatedByUser(Guid appuserId)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            List<Group> list = GetGroupsCreatedByUser(db, appuserId);
            db.Dispose();
            return list;
        }

        public static List<Group> GetGroupsCreatedByUser(ApplicationDbContext db, Guid appuserId)
        {
            List<Group> list = (from g in db.Groups
                                where g.GroupOriginatorAppUserId == appuserId
                                select g).ToList();

            return list;
        }

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
                                where gm.ReferenceId == appUserId
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

            list = (from gm in db.GroupMembers
                    join g in db.Groups on gm.GroupId equals g.GroupId
                    where (gm.ReferenceId == referenceId && gm.Type == referenceLevel)
                    select g).ToList();

            return list;
        }

        #endregion

        #region Create

        public static Group CreateGroupFromGroupAddView(GroupAddView view, IPrincipal user)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            Group group = CreateGroupFromGroupAddView(db, view, user);
            db.Dispose();
            return group;
        }

        public static Group CreateGroupFromGroupAddView(ApplicationDbContext db, GroupAddView view, IPrincipal user)
        {
            Group group = new Group()
            {
                GroupId = Guid.NewGuid(),
                Name = view.Name,
                Type = view.Type,
                VisibilityLevel = view.VisibilityLevel,
                InviteLevel = view.InviteLevel,
                AcceptanceLevel = view.AcceptanceLevel,
                GroupOriginatorAppUserId = AppUserHelpers.GetAppUserIdFromUser(user),
                GroupOriginatorDateTime = DateTime.Now
            };

            db.Groups.Add(group);
            db.SaveChanges();

            GroupViewHelpers.CreateGroupMembersFromGroupAddView(db, view, group);

            return group;
        }

        #endregion

        #region Remove



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

        public static List<GroupListView> GetGroupListViewsCreatedByUser(IPrincipal user)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            List<GroupListView> view = GetGroupListViewsCreatedByUser(db, user);
            db.Dispose();
            return view;
        }

        public static List<GroupListView> GetGroupListViewsCreatedByUser(ApplicationDbContext db, IPrincipal user)
        {
            return GetGroupListViewsCreatedByUser(db, AppUserHelpers.GetAppUser(db, user).AppUserId);
        }

        public static List<GroupListView> GetGroupListViewsCreatedByUser(Guid appUserId)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            List<GroupListView> view = GetGroupListViewsCreatedByUser(db, appUserId);
            db.Dispose();
            return view;
        }

        public static List<GroupListView> GetGroupListViewsCreatedByUser(ApplicationDbContext db, Guid appUserId)
        {
            List<GroupListView> list = new List<GroupListView>();

            List<Group> allGroupsCreatedByUser = GroupHelpers.GetGroupsCreatedByUser(db, appUserId);

            foreach (Group group in allGroupsCreatedByUser)
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

        public static List<GroupListView> GetGroupListViewsRelevantToUser(IPrincipal user)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            List<GroupListView> view = GetGroupListViewsRelevantToUser(db, user);
            db.Dispose();
            return view;
        }

        public static List<GroupListView> GetGroupListViewsRelevantToUser(ApplicationDbContext db, IPrincipal user)
        {
            return GetGroupListViewsRelevantToUser(db, AppUserHelpers.GetAppUser(db, user).AppUserId);
        }

        public static List<GroupListView> GetGroupListViewsRelevantToUser(Guid appUserId)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            List<GroupListView> view = GetGroupListViewsRelevantToUser(db, appUserId);
            db.Dispose();
            return view;
        }

        public static List<GroupListView> GetGroupListViewsRelevantToUser(ApplicationDbContext db, Guid appUserId)
        {
            List<GroupListView> list = new List<GroupListView>();

            List<Group> allGroupsRelevantUser = new List<Group>();

            //Get user groups containing this user
            allGroupsRelevantUser = GroupHelpers.GetGroupsForUser(db, appUserId);
            //Get branch groups containing this users branches
            List<Branch> usersBranches = BranchHelpers.GetBranchesForUser(db, appUserId);
            foreach (Branch branch in usersBranches)
            {
                List<Group> groups = GroupHelpers.GetGroupsForTypeAndReferenceId(db, LevelEnum.Branch, branch.BranchId);
                foreach (Group group in groups)
                    allGroupsRelevantUser.Add(group);
            }
            //Get company groups containing this users company
            List<Group> companyGroups = GroupHelpers.GetGroupsForTypeAndReferenceId(db, LevelEnum.Company, CompanyHelpers.GetCompanyForUser(db, appUserId).CompanyId);
            foreach (Group group in companyGroups)
                allGroupsRelevantUser.Add(group);


            foreach (Group group in allGroupsRelevantUser)
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

        public static GroupEditView GetGroupEditViewForForUserOnly(Guid appUserId)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            GroupEditView view = GetGroupEditViewForForUserOnly(db, appUserId);
            db.Dispose();
            return view;
        }

        public static GroupEditView GetGroupEditViewForForUserOnly(ApplicationDbContext db, Guid appUserId)
        {
            List<GroupListView> groupListViewsCreatedByUser = GroupViewHelpers.GetGroupListViewsCreatedByUser(db, appUserId);

            GroupEditView view = new GroupEditView()
            {
                GroupListViewsCreatedByUser = groupListViewsCreatedByUser,
                GroupListViewsRelevantToUser = null
            };

            return view;
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
            List<GroupListView> groupListViewsCreatedByUser = GroupViewHelpers.GetGroupListViewsCreatedByUser(db, appUserId);
            List<GroupListView> GroupListViewsRelevantToUser = GroupViewHelpers.GetGroupListViewsRelevantToUser(db, appUserId);

            GroupEditView view = new GroupEditView()
            {
                GroupListViewsCreatedByUser = groupListViewsCreatedByUser,
                GroupListViewsRelevantToUser = GroupListViewsRelevantToUser
            };

            return view;
        }

        public static GroupAddView GetGroupAddView(LevelEnum? level, Guid? ofReferenceId, Guid? byReferenceId, Guid? byAppUserId, Guid appUserId)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            GroupAddView view = GetGroupAddView(db, level, ofReferenceId, byReferenceId, byAppUserId, appUserId);
            db.Dispose();
            return view;
        }

        public static GroupAddView GetGroupAddView(ApplicationDbContext db, LevelEnum? level, Guid? ofReferenceId, Guid? byReferenceId, Guid? byAppUserId, Guid appUserId)
        {
            //Build a list of companies/branches/users from level if set
            List<GroupAddMemberView> members = new List<GroupAddMemberView>();
            GroupAddView view = new GroupAddView();

            if (level != null)  //Build a list of users for this view
            {
                switch (level.Value)
                {
                    case LevelEnum.Company:
                        List<Company> companies = CompanyHelpers.GetAllCompaniesForGroupForUser(db, appUserId);
                        foreach (Company company in companies)
                            members.Add(GroupMemberViewHelpers.CreateGroupAddMemberViewMember(db, false, company.CompanyId, company.CompanyName));
                        break;
                    case LevelEnum.Branch:
                        List<Branch> branches = BranchHelpers.GetAllBranchesForGroupForUser(db, appUserId);
                        foreach (Branch branch in branches)
                            members.Add(GroupMemberViewHelpers.CreateGroupAddMemberViewMember(db, false, branch.BranchId, branch.BranchName + ", " + branch.AddressTownCity));
                        break;
                    case LevelEnum.User:
                        List<AppUser> users = AppUserHelpers.GetAllAppUsersForGroupForUser(db, appUserId);
                        foreach (AppUser user in users)
                            members.Add(GroupMemberViewHelpers.CreateGroupAddMemberViewMember(db, false, user.AppUserId, user.FirstName + " " + user.LastName));
                        break;
                }  

                view.Type = level.Value;
                view.scratchEntry = false;
                view.Members = members;
            }
            else //return blank view with blank users as this is new from scratch
            {
                view.scratchEntry = true;  //this  will be used in view to stop the changing fo the 'type' field.

                //build members as User as this is the default
                List<AppUser> users = AppUserHelpers.GetAllAppUsersForGroupForUser(db, appUserId);
                foreach (AppUser user in users)
                    members.Add(GroupMemberViewHelpers.CreateGroupAddMemberViewMember(db, false, user.AppUserId, user.FirstName + " " + user.LastName));
                
                view.Members = members;
            }

            return view;
        }

        public static GroupAddView GetGroupAddView(string groupName, string level, IPrincipal user)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            GroupAddView view = GetGroupAddView(db, groupName, level, user);
            db.Dispose();
            return view;
        }

        public static GroupAddView GetGroupAddView(ApplicationDbContext db, string groupName, string level, IPrincipal user)
        {
            List<GroupAddMemberView> members = new List<GroupAddMemberView>();
            GroupAddView view = new GroupAddView();
            LevelEnum levelEnum = LevelEnum.User;

            switch (level)
            {
                case "Company":
                    levelEnum = LevelEnum.Company;
                    List<Company> companies = CompanyHelpers.GetAllCompaniesForGroupForUser(db, AppUserHelpers.GetAppUserIdFromUser(user));
                    foreach (Company company in companies)
                        members.Add(GroupMemberViewHelpers.CreateGroupAddMemberViewMember(db, false, company.CompanyId, company.CompanyName));
                    break;
                case "Branch":
                    levelEnum = LevelEnum.Branch;
                    List<Branch> branches = BranchHelpers.GetAllBranchesForGroupForUser(db, AppUserHelpers.GetAppUserIdFromUser(user));
                    foreach (Branch branch in branches)
                        members.Add(GroupMemberViewHelpers.CreateGroupAddMemberViewMember(db, false, branch.BranchId, branch.BranchName + ", " + branch.AddressTownCity));
                    break;
                case "User":
                    levelEnum = LevelEnum.User;
                    List<AppUser> users = AppUserHelpers.GetAllAppUsersForGroupForUser(db, AppUserHelpers.GetAppUserIdFromUser(user));
                    foreach (AppUser appUser in users)
                        members.Add(GroupMemberViewHelpers.CreateGroupAddMemberViewMember(db, false, appUser.AppUserId, appUser.FirstName + " " + appUser.LastName));
                    break;
            }

            view.Name = groupName;
            view.Type = levelEnum;
            view.scratchEntry = true; //this only comes from the scratch entry values
            view.Members = members;

            return view;
        }

        #endregion

        #region Create

        public static GroupMember CreateGroupMember(Guid groupId, LevelEnum type, Guid referenceId, Guid addedBy)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            GroupMember member = CreateGroupMember(db, groupId, type, referenceId, addedBy);
            db.Dispose();
            return member;
        }

        public static GroupMember CreateGroupMember(ApplicationDbContext db, Guid groupId, LevelEnum type, Guid referenceId, Guid addedBy)
        {
            GroupMember member = new GroupMember()
            {
                GroupMemberId = Guid.NewGuid(),
                GroupId = groupId,
                Type = type,
                ReferenceId = referenceId,
                AddedBy = addedBy,
                AddedDateTime = DateTime.Now
            };

            db.GroupMembers.Add(member);
            db.SaveChanges();

            return member;
        }

        public static List<GroupMember> CreateGroupMembersFromGroupAddView(GroupAddView view, Group group)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            List<GroupMember> list = CreateGroupMembersFromGroupAddView(db, view, group);
            db.Dispose();
            return list;
        }

        public static List<GroupMember> CreateGroupMembersFromGroupAddView(ApplicationDbContext db, GroupAddView view, Group group)
        {
            List<GroupMember> members = new List<GroupMember>();

            foreach (GroupAddMemberView member in view.Members)
                if (member.SelectedUser)
                    members.Add(CreateGroupMember(db, group.GroupId, view.Type, member.ReferenceId, group.GroupOriginatorAppUserId));

            return members;
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

            member = (from gm in db.GroupMembers
                      where (gm.GroupId == groupId && gm.ReferenceId == referenceId && gm.Type == referenceLevel)
                      select gm).FirstOrDefault();

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

    public static class GroupMemberViewHelpers
    {
        #region Create

        public static GroupAddMemberView CreateGroupAddMemberViewMember(bool selected, Guid referenceId, string referenceName)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            GroupAddMemberView view = CreateGroupAddMemberViewMember(db, selected, referenceId, referenceName);
            db.Dispose();
            return view;
        }

        public static GroupAddMemberView CreateGroupAddMemberViewMember(ApplicationDbContext db, bool selected, Guid referenceId, string referenceName)
        {
            GroupAddMemberView member = new GroupAddMemberView()
            {
                SelectedUser = selected,
                ReferenceId = referenceId,
                ReferenceName = referenceName
            };

            return member;
        }

        #endregion
    }
}