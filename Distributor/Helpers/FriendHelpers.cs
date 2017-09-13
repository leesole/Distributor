using Distributor.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
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