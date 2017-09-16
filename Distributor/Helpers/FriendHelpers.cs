using Distributor.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using static Distributor.Enums.FriendEnums;
using static Distributor.Enums.GeneralEnums;

namespace Distributor.Helpers
{
    public static class FriendHelpers
    {
        #region Get

        public static Friend GetFriendForReferenceIdAndType(LevelEnum referenceLevel, Guid ofReferenceId, Guid byReferenceId)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            Friend friend = GetFriendForReferenceIdAndType(db, referenceLevel, ofReferenceId, byReferenceId);
            db.Dispose();
            return friend;
        }

        public static Friend GetFriendForReferenceIdAndType(ApplicationDbContext db, LevelEnum referenceLevel, Guid ofReferenceId, Guid byReferenceId)
        {
            Friend friend = (from f in db.Friends
                             where (f.Type == referenceLevel && f.RequestedOfId == ofReferenceId && f.RequestedById == byReferenceId && f.Status == FriendStatusEnum.Accepted)
                             select f).FirstOrDefault();

            return friend;
        }

        public static Friend GetFriendRequestForReferenceIdAndType(LevelEnum referenceLevel, Guid ofReferenceId, Guid byReferenceId)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            Friend friendRequest = GetFriendRequestForReferenceIdAndType(db, referenceLevel, ofReferenceId, byReferenceId);
            db.Dispose();
            return friendRequest;
        }

        //This will bring back if this is a friend is currently requested OR Rejected
        public static Friend GetFriendRequestForReferenceIdAndType(ApplicationDbContext db, LevelEnum referenceLevel, Guid ofReferenceId, Guid byReferenceId)
        {
            Friend friendRequest = (from f in db.Friends
                                    where (f.Type == referenceLevel && f.RequestedOfId == ofReferenceId && f.RequestedById == byReferenceId && f.Status != FriendStatusEnum.Closed)
                                    select f).FirstOrDefault();

            return friendRequest;
        }

        #endregion

        #region Create

        public static Friend CreateFriend(LevelEnum level, Guid ofReferenceId, Guid byReferenceId, Guid byAppUserId)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            Friend friend = CreateFriend(db, level, ofReferenceId, byReferenceId, byAppUserId);
            db.Dispose();
            return friend;
        }

        public static Friend CreateFriend(ApplicationDbContext db, LevelEnum level, Guid ofReferenceId, Guid byReferenceId, Guid byAppUserId)
        {
            Friend friend = new Friend()
            {
                FriendId = Guid.NewGuid(),
                Type = level,
                RequestedById = byReferenceId,
                RequestedOfId = ofReferenceId,
                RequestedByUserId = byAppUserId,
                Status = FriendStatusEnum.Requested,
                RequestedOn = DateTime.Now
            };

            db.Friends.Add(friend);
            db.SaveChanges();

            //Now add Action to the user or manager of branch or admin of company to change status
            UserActionHelpers.CreateActionForFriendRequestFromUser(db, level, ofReferenceId, byReferenceId);
            
            //LSLSLS PSPSPS change the searches to only look for those that are active not 'awaiting'......think abou tthat

            return friend;
        }

        #endregion

        #region Remove

        public static void RemoveFriend(LevelEnum level, Guid ofReferenceId, Guid byReferenceId)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            RemoveFriend(db, level, ofReferenceId, byReferenceId);
            db.Dispose();
        }

        public static void RemoveFriend(ApplicationDbContext db, LevelEnum level, Guid ofReferenceId, Guid byReferenceId)
        {
            List<Friend> list = (from f in db.Friends
                                 where (f.Type == level && f.RequestedOfId == ofReferenceId && f.RequestedById == byReferenceId)
                                 select f).ToList();

            db.Friends.RemoveRange(list);
            db.SaveChanges();
        }

        public static void RemoveFriendItemsDueToBlock(LevelEnum level, Guid ofReferenceId, Guid byReferenceId, Guid byAppUserId)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            RemoveFriendItemsDueToBlock(db, level, ofReferenceId, byReferenceId, byAppUserId);
            db.Dispose();
        }

        /// <summary>
        /// Remove from friend list if there (at the correct level)
        // - company - blats all company, branch and users
        // - branch - blats the branch and it's users (even if users on other branches)
        // - users - blats that user
        /// </summary>
        /// <param name="db"></param>
        /// <param name="level"></param>
        /// <param name="ofReferenceId"></param>
        /// <param name="byReferenceId"></param>
        /// <param name="byAppUserId"></param>
        public static void RemoveFriendItemsDueToBlock(ApplicationDbContext db, LevelEnum level, Guid ofReferenceId, Guid byReferenceId, Guid byAppUserId)
        {
            switch (level)
            {
                case LevelEnum.Company:
                    //Remove the company level
                    RemoveFriend(db, level, ofReferenceId, byReferenceId);
                    
                    //Remove the branches for that company from our company branches
                    List<Branch> ofBranches = BranchHelpers.GetBranchesForCompany(db, ofReferenceId);
                    List<Branch> byBranches = BranchHelpers.GetBranchesForCompany(db, byReferenceId);
                    foreach (Branch ofBranch in ofBranches)
                        foreach (Branch byBranch in byBranches)
                            RemoveFriend(db, LevelEnum.Branch, ofBranch.BranchId, byBranch.BranchId);

                    //Remove users of that company from our company users
                    List<AppUser> ofCompanyUsers = AppUserHelpers.GetAppUsersForCompany(db, ofReferenceId);
                    List<AppUser> byCompanyUsers = AppUserHelpers.GetAppUsersForCompany(db, byReferenceId);
                    foreach (AppUser ofUser in ofCompanyUsers)
                        foreach (AppUser byUser in byCompanyUsers)
                            RemoveFriend(db, LevelEnum.User, ofUser.AppUserId, byUser.AppUserId);
                    break;
                case LevelEnum.Branch:
                    //Remove the branch level
                    RemoveFriend(db, level, ofReferenceId, byReferenceId);

                    //Remove users of that branch from our branch users
                    List<AppUser> ofBranchUsers = AppUserHelpers.GetAppUsersForBranch(db, ofReferenceId);
                    List<AppUser> byBranchUsers = AppUserHelpers.GetAppUsersForBranch(db, byReferenceId);
                    foreach (AppUser ofUser in ofBranchUsers)
                        foreach (AppUser byUser in byBranchUsers)
                            RemoveFriend(db, LevelEnum.User, ofUser.AppUserId, byUser.AppUserId);
                    break;
                case LevelEnum.User:
                    RemoveFriend(db, level, ofReferenceId, byReferenceId);
                    break;
            }
        }

        #endregion

        #region Processing

        public static bool IsReferenceAnActiveFriendRequest(LevelEnum referenceLevel, Guid ofReferenceId, Guid byReferenceId)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            bool friend = IsReferenceAnActiveFriendRequest(db, referenceLevel, ofReferenceId, byReferenceId);
            db.Dispose();
            return friend;
        }

        public static bool IsReferenceAnActiveFriendRequest(ApplicationDbContext db, LevelEnum referenceLevel, Guid ofReferenceId, Guid byReferenceId)
        {
            bool friend = false;
            Friend friendDetails = null;

            friendDetails = GetFriendRequestForReferenceIdAndType(db, referenceLevel, ofReferenceId, byReferenceId);

            if (friendDetails != null)
                friend = true;

            return friend;
        }

        #endregion
    }
}