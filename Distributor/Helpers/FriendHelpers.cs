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
                           where (f.Type == referenceLevel && f.RequestedOfId == ofReferenceId && f.RequestedById == byReferenceId)
                           select f).FirstOrDefault();

            return friend;
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

        #region Processing

        public static bool IsReferenceFriend(LevelEnum referenceLevel, Guid ofReferenceId, Guid byReferenceId)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            bool friend = IsReferenceFriend(db, referenceLevel, ofReferenceId, byReferenceId);
            db.Dispose();
            return friend;
        }

        public static bool IsReferenceFriend(ApplicationDbContext db, LevelEnum referenceLevel, Guid ofReferenceId, Guid byReferenceId)
        {
            bool friend = false;
            Friend friendDetails = null;

            switch (referenceLevel)
            {
                case LevelEnum.Company:
                    friendDetails = GetFriendForReferenceIdAndType(db, referenceLevel, ofReferenceId, byReferenceId);
                    break;
                case LevelEnum.Branch:
                    friendDetails = GetFriendForReferenceIdAndType(db, referenceLevel, ofReferenceId, byReferenceId);
                    break;
                case LevelEnum.User:
                    friendDetails = GetFriendForReferenceIdAndType(db, referenceLevel, ofReferenceId, byReferenceId);
                    break;
            }

            if (friendDetails != null)
                friend = true;

            return friend;
        }

        #endregion
    }
}