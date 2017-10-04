using Distributor.Helpers;
using Distributor.Models;
using Distributor.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static Distributor.Enums.GeneralEnums;
using static Distributor.Enums.UserEnums;

namespace Distributor.Controllers
{
    [Authorize]
    public class DataController : Controller
    {
        // GET: Data
        [AllowAnonymous]
        [HttpPost]
        public ActionResult GetBranchByCompany(Guid companyId)
        {
            return Json(ControlHelpers.AllBranchesForCompany(companyId));
        }

        //public ActionResult ActivateUser(Guid appUserId)
        //{
        //    return Json(new { success = true });
        //}

        //get branch object for branchid
        [HttpPost]
        public ActionResult GetBranchAddressDetailsForBranch(Guid branchId)
        {
            if (branchId != null)
            {
                Branch branchDetails = BranchHelpers.GetBranch(branchId);

                if (branchDetails != null)
                {
                    string branchBusinessType = EnumHelpers.GetDescription(branchDetails.BusinessType);

                    return Json(new { branchDetails, branchBusinessType, success = true });
                }
                return Json(new { success = false });
            }
            return Json(new { success = false });
        }


        /// <summary>
        /// This will process the 'Block', 'Add Friend', 'Add to Group' button presses on the General Info screens.
        /// </summary>
        /// <param name="buttonName"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ProcessButton(string buttonName)
        {
            string[] keys = buttonName.Split(':');

            string buttonType = keys[0];
            string buttonLevel = keys[1];
            Guid ofReferenceId = Guid.Empty;
            Guid byAppUserId = Guid.Empty;
            Guid.TryParse(keys[2], out ofReferenceId);
            Guid.TryParse(keys[3], out byAppUserId);

            Guid byReferenceId = Guid.Empty;
            LevelEnum levelEnum = LevelEnum.User;

            //Set the byReference to be either the user, the user's branch or the user's company depending on level
            switch (buttonLevel)
            {
                case "company":
                    levelEnum = LevelEnum.Company;
                    byReferenceId = CompanyHelpers.GetCompanyForUser(byAppUserId).CompanyId;
                    break;
                case "branch":
                    levelEnum = LevelEnum.Branch;
                    byReferenceId = AppUserHelpers.GetAppUser(byAppUserId).CurrentBranchId;
                    break;
                case "user":
                    levelEnum = LevelEnum.User;
                    byReferenceId = byAppUserId;
                    break;
            }

            switch (buttonType)
            {
                case "block":
                    BlockHelpers.CreateBlock(levelEnum, ofReferenceId, byReferenceId, byAppUserId);
                    break;
                case "friend":
                    FriendHelpers.CreateFriend(levelEnum, ofReferenceId, byReferenceId, byAppUserId);
                    break;
                case "group":
                    //Need to go to Group screen to select a group or allow adding of a new group
                    return RedirectToAction("Create", "Groups", new { levelEnum = levelEnum, ofReferenceId = ofReferenceId, byReferenceId = byReferenceId, appUserid = byAppUserId });
            }

            return Json(new { success = true });
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult GetCampaignForHomeCampaignView(Guid campaignId)
        {
            if (campaignId != null)
            {
                Campaign campaignDetails = CampaignHelpers.GetCampaign(campaignId);

                if (campaignDetails != null)
                {
                    return Json(new { campaignDetails, success = true });
                }
                return Json(new { success = false });
            }
            return Json(new { success = false });
        }

        /// <summary>
        /// This will remove the 'Friend' from a given FriendId
        /// </summary>
        /// <param name="buttonName"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult RemoveFriend(Guid friendId)
        {
            FriendHelpers.RemoveFriend(friendId);
            return Json(new { success = true });
        }

        /// <summary>
        /// This will remove the 'Block' from a given blockId
        /// </summary>
        /// <param name="buttonName"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult RemoveBlock(Guid blockId)
        {
            BlockHelpers.RemoveBlock(blockId);
            return Json(new { success = true });
        }

        /// <summary>
        /// validate that an ADMIN user exists on the given branch if the current user (appUserId passed here) is changed from Admin
        /// </summary>
        /// <param name="branchId"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult OtherAdminUsersExistForCompany(Guid appUserId, Guid branchId)
        {
            List<BranchUser> branchUsers = BranchUserHelpers.GetAdminBranchUsersForBranchExcludingUser(branchId, appUserId);
            BranchUser branchUserForCallingUser = BranchUserHelpers.GetBranchUser(appUserId, branchId, BranchHelpers.GetBranch(branchId).CompanyId);

            string originalSelectedItem = ((int)branchUserForCallingUser.UserRole + 1).ToString();

            //Add 1 to the selected item as there is a blank option at the start
            if (branchUsers == null)
                return Json(new { success = false, originalRole = originalSelectedItem });
            else
                return Json(new { success = true, originalRole = originalSelectedItem });
        }


        [HttpPost]
        public ActionResult SetUserToNewRole(Guid appUserId, Guid branchId, string newRoleId)
        {
            int roleId = 0;
            int.TryParse(newRoleId, out roleId);

            BranchUserHelpers.UpdateBranchUserRoleForAllBranches(appUserId, (UserRoleEnum)roleId);

            return Json(new { success = true });
        }
    }
}