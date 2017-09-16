using Distributor.Helpers;
using Distributor.Models;
using Distributor.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static Distributor.Enums.GeneralEnums;

namespace Distributor.Controllers
{
    [Authorize]
    public class DataController : Controller
    {
        // GET: Data
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
                    ///LSLSLS need to go to Group screen to select a group or allow adding of a new group
                    return RedirectToAction("Create", "Groups", new { levelEnum = levelEnum, ofReferenceId = ofReferenceId, byReferenceId = byReferenceId, appUserid = byAppUserId });
                    break;

            }

            return Json(new { success = true });
        }
    }
}