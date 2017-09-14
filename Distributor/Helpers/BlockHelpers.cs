using Distributor.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using static Distributor.Enums.GeneralEnums;

namespace Distributor.Helpers
{
    public static class BlockHelpers
    {
        #region Get

        public static Block GetBlockForReferenceIdAndType(LevelEnum referenceLevel, Guid ofReferenceId, Guid byReferenceId)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            Block block = GetBlockForReferenceIdAndType(db, referenceLevel, ofReferenceId, byReferenceId);
            db.Dispose();
            return block;
        }

        public static Block GetBlockForReferenceIdAndType(ApplicationDbContext db, LevelEnum referenceLevel, Guid ofReferenceId, Guid byReferenceId)
        {
            Block block = (from b in db.Blocked
                           where (b.Type == referenceLevel && b.BlockedOfId == ofReferenceId && b.BlockedById == byReferenceId)
                           select b).FirstOrDefault();

            return block;
        }

        #endregion

        #region Create

        public static Block CreateBlock(LevelEnum level, Guid ofReferenceId, Guid byReferenceId, Guid byAppUserId)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            Block block = CreateBlock(db, level, ofReferenceId, byReferenceId, byAppUserId);
            db.Dispose();
            return block;
        }

        public static Block CreateBlock(ApplicationDbContext db, LevelEnum level, Guid ofReferenceId, Guid byReferenceId, Guid byAppUserId)
        {
            Block block = new Block()
            {
                BlockId = Guid.NewGuid(),
                Type = level,
                BlockedOfId = ofReferenceId,
                BlockedById = byReferenceId,
                BlockedByUserId = byAppUserId,
                BlockedOn = DateTime.Now
            };

            return block;
        }

        #endregion

        #region Processing

        public static bool IsReferenceBlocked(LevelEnum referenceLevel, Guid ofReferenceId, Guid byReferenceId)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            bool blocked = IsReferenceBlocked(db, referenceLevel, ofReferenceId, byReferenceId);
            db.Dispose();
            return blocked;
        }

        public static bool IsReferenceBlocked(ApplicationDbContext db, LevelEnum referenceLevel, Guid ofReferenceId, Guid byReferenceId)
        {
            bool blocked = false;
            Block block = null;

            switch (referenceLevel)
            {
                case LevelEnum.Company:
                    block = GetBlockForReferenceIdAndType(db, referenceLevel, ofReferenceId, byReferenceId);
                    break;
                case LevelEnum.Branch:
                    block = GetBlockForReferenceIdAndType(db, referenceLevel, ofReferenceId, byReferenceId);
                    break;
                case LevelEnum.User:
                    block = GetBlockForReferenceIdAndType(db, referenceLevel, ofReferenceId, byReferenceId);
                    break;
            }

            if (block != null)
                blocked = true;

            return blocked;
        }

        #endregion
    }
}