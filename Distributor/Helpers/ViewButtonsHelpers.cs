﻿using Distributor.Models;
using Distributor.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using static Distributor.Enums.GeneralEnums;
using static Distributor.Enums.UserEnums;

namespace Distributor.Helpers
{
    public static class ViewButtonsHelpers
    {
        #region Get 

        public static ViewButtons GetAvailableButtonsForSingleView(AppUser appUser, Branch branch, Company company, IPrincipal user)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            ViewButtons buttons = GetAvailableButtonsForSingleView(db, appUser, branch, company, user);
            db.Dispose();
            return buttons;
        }

        public static ViewButtons GetAvailableButtonsForSingleView(ApplicationDbContext db, AppUser appUser, Branch branch, Company company, IPrincipal user)
        {
            //Build initial list
            ViewButtons buttons = new ViewButtons()
            {
                CompanyBlockButton = false,
                CompanyAddFriendButton = false,
                CompanyAddToGroupButton = false,
                BranchBlockButton = false,
                BranchAddFriendButton = false,
                BranchAddToGroupButton = false,
                UserBlockButton = false,
                UserAddFriendButton = false,
                UserAddToGroupButton = false
            };

            //First: check this is not our listing
            AppUser currentUser = AppUserHelpers.GetAppUser(db, user);
            if (currentUser.AppUserId == appUser.AppUserId)
                return buttons;

            //Second: check the branch is not our branch, else return current button settings
            if (currentUser.CurrentBranchId == branch.BranchId)
                return buttons;

            //Third: check if the Company allows interbranch dealing, if not and company level are the same return current button settings
            Branch currentUserBranch = BranchHelpers.GetBranch(db, currentUser.CurrentBranchId);
            if (!company.AllowBranchTrading && currentUserBranch.CompanyId == company.CompanyId)
                return buttons;

            //Now validate for button status depending on type of user for this branch user combo
            BranchUser branchUser = BranchUserHelpers.GetBranchUser(db, appUser.AppUserId, branch.BranchId, company.CompanyId);
            switch (branchUser.UserRole)
            {
                case UserRoleEnum.SuperUser:
                case UserRoleEnum.Admin:
                    buttons = SetCompanyButtons(db, buttons, company.CompanyId, currentUserBranch.CompanyId);
                    buttons = SetBranchButtons(db, buttons, branch.BranchId, currentUser.CurrentBranchId);
                    buttons = SetUserButtons(db, buttons, appUser.AppUserId, currentUser.AppUserId);
                    break;
                case UserRoleEnum.Manager:
                    buttons = SetBranchButtons(db, buttons, branch.BranchId, currentUser.CurrentBranchId);
                    buttons = SetUserButtons(db, buttons, appUser.AppUserId, currentUser.AppUserId);
                    break;
                case UserRoleEnum.User:
                    buttons = SetUserButtons(db, buttons, appUser.AppUserId, currentUser.AppUserId);
                    break;
            }

            return buttons;
        }

        #endregion

        #region Processing

        public static ViewButtons SetCompanyButtons(ApplicationDbContext db, ViewButtons buttons, Guid viewItemcompanyId, Guid currentCompanyId)
        {
            buttons.CompanyBlockButton = !BlockHelpers.IsReferenceBlocked(db, LevelEnum.Company, viewItemcompanyId, currentCompanyId);
            buttons.CompanyAddFriendButton = !FriendHelpers.IsReferenceFriend(db, LevelEnum.Company, viewItemcompanyId, currentCompanyId);
            buttons.CompanyAddToGroupButton = !GroupHelpers.IsReferenceInGroup(db, LevelEnum.Company, viewItemcompanyId, currentCompanyId);
            return buttons;
        }

        public static ViewButtons SetBranchButtons(ApplicationDbContext db, ViewButtons buttons, Guid viewItemBranchId, Guid currentBranchId)
        {
            buttons.BranchBlockButton = !BlockHelpers.IsReferenceBlocked(db, LevelEnum.Branch, viewItemBranchId, currentBranchId);
            buttons.BranchAddFriendButton = !FriendHelpers.IsReferenceFriend(db, LevelEnum.Branch, viewItemBranchId, currentBranchId);
            buttons.BranchAddToGroupButton = !GroupHelpers.IsReferenceInGroup(db, LevelEnum.Branch, viewItemBranchId, currentBranchId);
            return buttons;
        }

        public static ViewButtons SetUserButtons(ApplicationDbContext db, ViewButtons buttons, Guid viewItemUserId, Guid currentUserId)
        {
            buttons.UserBlockButton = !BlockHelpers.IsReferenceBlocked(db, LevelEnum.User, viewItemUserId, currentUserId);
            buttons.UserAddFriendButton = !FriendHelpers.IsReferenceFriend(db, LevelEnum.User, viewItemUserId, currentUserId);
            buttons.UserAddToGroupButton = !GroupHelpers.IsReferenceInGroup(db, LevelEnum.User, viewItemUserId, currentUserId);
            return buttons;
        }

        #endregion
    }
}