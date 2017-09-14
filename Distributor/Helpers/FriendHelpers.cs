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

        //This will bring back if this is a friend ORis currently requested OR Rejected
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