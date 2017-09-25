using Distributor.Models;
using Distributor.ViewModels;
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
            Block block = (from b in db.Blocks
                           where (b.Type == referenceLevel && b.BlockedOfId == ofReferenceId && b.BlockedById == byReferenceId)
                           select b).FirstOrDefault();

            return block;
        }

        public static List<Block> GetBlocksCreatedByUser(Guid appUserId)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            List<Block> list = GetBlocksCreatedByUser(db, appUserId);
            db.Dispose();
            return list;
        }

        public static List<Block> GetBlocksCreatedByUser(ApplicationDbContext db, Guid appUserId)
        {
            List<Block> list = (from b in db.Blocks
                                where b.BlockedById == appUserId
                                select b).ToList();

            return list;
        }

        public static List<Block> GetBlocksCreatedByUserBranches(Guid appUserId)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            List<Block> list = GetBlocksCreatedByUserBranches(db, appUserId);
            db.Dispose();
            return list;
        }

        public static List<Block> GetBlocksCreatedByUserBranches(ApplicationDbContext db, Guid appUserId)
        {
            List<Branch> userBranches = BranchHelpers.GetBranchesForUser(db, appUserId);

            List<Block> list = (from ub in userBranches
                                join b in db.Blocks on ub.BranchId equals b.BlockedById
                                select b).Distinct().ToList();

            return list;
        }

        public static List<Block> GetBlocksCreatedByUserCompany(Guid appUserId)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            List<Block> list = GetBlocksCreatedByUserCompany(db, appUserId);
            db.Dispose();
            return list;
        }

        public static List<Block> GetBlocksCreatedByUserCompany(ApplicationDbContext db, Guid appUserId)
        {
            Company userCompany = CompanyHelpers.GetCompanyForUser(db, appUserId);

            List<Block> list = (from b in db.Blocks
                                where b.BlockedById == userCompany.CompanyId
                                select b).ToList();

            return list;
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
            //Remove Friends for those to be blocked
            FriendHelpers.RemoveFriendItemsDueToBlock(db, level, ofReferenceId, byReferenceId, byAppUserId);

            //LSLSLS Remove from Group if there (at the correct level)


            Block block = new Block()
            {
                BlockId = Guid.NewGuid(),
                Type = level,
                BlockedOfId = ofReferenceId,
                BlockedById = byReferenceId,
                BlockedByUserId = byAppUserId,
                BlockedOn = DateTime.Now
            };

            db.Blocks.Add(block);
            db.SaveChanges();

            return block;
        }

        #endregion

        #region Remove

        public static void RemoveBlock(Guid blockId)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            RemoveBlock(db, blockId);
            db.Dispose();
        }

        public static void RemoveBlock(ApplicationDbContext db, Guid blockId)
        {
            Block block = db.Blocks.Find(blockId);
            db.Blocks.Remove(block);
            db.SaveChanges();
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

            block = GetBlockForReferenceIdAndType(db, referenceLevel, ofReferenceId, byReferenceId);

            if (block != null)
                blocked = true;

            return blocked;
        }

        #endregion
    }

    public static class BlockViewHelpers
    {
        #region Get

        public static List<BlockView> GetBlockViewByType(Guid appUserId, LevelEnum type)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            List<BlockView> list = GetBlockViewByType(db, appUserId, type);
            db.Dispose();
            return list;
        }

        public static List<BlockView> GetBlockViewByType(ApplicationDbContext db, Guid appUserId, LevelEnum type)
        {
            List<BlockView> list = new List<BlockView>();

            List<Block> blockList = null;

            //Depending on type passed through will depend on what level of blocks we are collecting
            switch (type)
            {
                case LevelEnum.User:
                    blockList = BlockHelpers.GetBlocksCreatedByUser(db, appUserId);
                    break;
                case LevelEnum.Branch:
                    blockList = BlockHelpers.GetBlocksCreatedByUserBranches(db, appUserId);
                    break;
                case LevelEnum.Company:
                    blockList = BlockHelpers.GetBlocksCreatedByUserCompany(db, appUserId);
                    break;
            }

            foreach (Block block in blockList)
            {
                //get the user/branch/company names depending on the block type
                string nameBy = "";
                string nameOn = "";

                switch (block.Type)
                {
                    case LevelEnum.User:
                        nameBy = AppUserHelpers.GetAppUserName(db, block.BlockedById);
                        nameOn = AppUserHelpers.GetAppUserName(db, block.BlockedOfId);
                        break;
                    case LevelEnum.Branch:
                        nameBy = BranchHelpers.GetBranchNameTownPostCode(db, block.BlockedById);
                        nameOn = BranchHelpers.GetBranchNameTownPostCode(db, block.BlockedOfId);
                        break;
                    case LevelEnum.Company:
                        nameBy = CompanyHelpers.GetCompanyNameTownPostCode(db, block.BlockedById);
                        nameOn = CompanyHelpers.GetCompanyNameTownPostCode(db, block.BlockedOfId);
                        break;
                }

                string blockedByUserName = AppUserHelpers.GetAppUserName(db, block.BlockedByUserId);

                bool blockedByLoggedInUser = false;

                if (block.BlockedByUserId == appUserId)
                    blockedByLoggedInUser = true;

                BlockView view = new BlockView()
                {
                    BlockId = block.BlockId,
                    Type = block.Type,
                    BlockedByName = nameBy,
                    BlockedByUserName = blockedByUserName,
                    BlockedOfName = nameOn,
                    BlockedOn = block.BlockedOn,
                    BlockedByLoggedInUser = blockedByLoggedInUser
                };

                list.Add(view);
            }

            return list;
        }

        #endregion

        #region Create

        public static BlockViewList CreateBlockViewListFromAppUserEditView(ApplicationDbContext db, Guid appUserId, string url)
        {
            List<BlockView> userBlockListView = BlockViewHelpers.GetBlockViewByType(db, appUserId, LevelEnum.User);
            List<BlockView> userBranchBlockListView = BlockViewHelpers.GetBlockViewByType(db, appUserId, LevelEnum.Branch);
            List<BlockView> userCompanyBlockListView = BlockViewHelpers.GetBlockViewByType(db, appUserId, LevelEnum.Company);

            BlockViewList list = new BlockViewList()
            {
                UserBlockListView = userBlockListView,
                UserBranchBlockListView = userBranchBlockListView,
                UserCompanyBlockListView = userCompanyBlockListView,
                CallingUrl = url
            };

            return list;
        }

        #endregion

        
    }
}