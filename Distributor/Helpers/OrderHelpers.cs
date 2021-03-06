﻿using Distributor.Models;
using Distributor.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Principal;
using System.Web;
using static Distributor.Enums.GeneralEnums;
using static Distributor.Enums.OrderEnums;

namespace Distributor.Helpers
{
    public static class OrderHelpers
    {
        #region Get

        public static Order GetOrder(Guid orderId)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            Order order = GetOrder(db, orderId);
            db.Dispose();
            return order;
        }

        public static Order GetOrder(ApplicationDbContext db, Guid orderId)
        {
            return db.Orders.Find(orderId);
        }

        public static List<Order> GetAllOrders()
        {
            ApplicationDbContext db = new ApplicationDbContext();

            List<Order> list = GetAllOrders(db);
            db.Dispose();
            return list;
        }

        public static List<Order> GetAllOrders(ApplicationDbContext db)
        {
            return db.Orders.ToList();
        }

        public static List<Order> GetAllOrdersForUser(Guid appUserId, bool getHistory)
        {
            ApplicationDbContext db = new ApplicationDbContext();

            List<Order> list = GetAllOrdersForUser(db, appUserId, getHistory);
            db.Dispose();
            return list;
        }

        public static List<Order> GetAllOrdersForUser(ApplicationDbContext db, Guid appUserId, bool getHistory)
        {
            List<Order> allOrdersForUser = null;

            if (getHistory)
            {
                allOrdersForUser = (from o in db.Orders
                                    where ((o.OrderOriginatorAppUserId == appUserId && o.OrderStatus == OrderStatusEnum.Closed) || (o.OfferOriginatorAppUserId == appUserId && o.OrderStatus == OrderStatusEnum.Closed))
                                    select o).ToList();
            }
            else
            {
                allOrdersForUser = (from o in db.Orders
                                    where ((o.OrderOriginatorAppUserId == appUserId && o.OrderStatus != OrderStatusEnum.Closed) || (o.OfferOriginatorAppUserId == appUserId && o.OrderStatus != OrderStatusEnum.Closed))
                                    select o).ToList();
            }

            return allOrdersForUser;
        }

        public static List<Order> GetAllOrdersForBranch(Guid branchId, bool getHistory)
        {
            ApplicationDbContext db = new ApplicationDbContext();

            List<Order> list = GetAllOrdersForBranch(db, branchId, getHistory);
            db.Dispose();
            return list;
        }

        public static List<Order> GetAllOrdersForBranch(ApplicationDbContext db, Guid branchId, bool getHistory)
        {
            List<Order> list = null;

            if (getHistory)
            {
                list = (from o in db.Orders
                        where ((o.OrderOriginatorBranchId == branchId && o.OrderStatus == OrderStatusEnum.Closed) || (o.OfferOriginatorBranchId == branchId && o.OrderStatus == OrderStatusEnum.Closed))
                        select o).ToList();
            }
            else
            {
                list = (from o in db.Orders
                        where ((o.OrderOriginatorBranchId == branchId && o.OrderStatus != OrderStatusEnum.Closed) || (o.OfferOriginatorBranchId == branchId && o.OrderStatus != OrderStatusEnum.Closed))
                        select o).ToList();
            }

            return list;
        }

        public static List<Order> GetAllOrdersForCompany(Guid companyId, bool getHistory)
        {
            ApplicationDbContext db = new ApplicationDbContext();

            List<Order> list = GetAllOrdersForCompany(db, companyId, getHistory);
            db.Dispose();
            return list;
        }

        public static List<Order> GetAllOrdersForCompany(ApplicationDbContext db, Guid companyId, bool getHistory)
        {
            List<Order> list = null;

            if (getHistory)
            {
                list = (from o in db.Orders
                        where ((o.OrderOriginatorCompanyId == companyId && o.OrderStatus == OrderStatusEnum.Closed) || (o.OfferOriginatorCompanyId == companyId && o.OrderStatus == OrderStatusEnum.Closed))
                        select o).ToList();
            }
            else
            {
                list = (from o in db.Orders
                        where ((o.OrderOriginatorCompanyId == companyId && o.OrderStatus != OrderStatusEnum.Closed) || (o.OfferOriginatorCompanyId == companyId && o.OrderStatus != OrderStatusEnum.Closed))
                        select o).ToList();
            }

            return list;
        }

        public static List<Order> GetAllOrdersForBranchUser(BranchUser branchUser)
        {
            ApplicationDbContext db = new ApplicationDbContext();

            List<Order> list = GetAllOrdersForBranchUser(db, branchUser);
            db.Dispose();
            return list;
        }

        public static List<Order> GetAllOrdersForBranchUser(ApplicationDbContext db, BranchUser branchUser)
        {
            List<Order> allOrdersForBranchUser = (from o in db.Orders
                                                  where (o.OrderOriginatorAppUserId == branchUser.UserId && o.OrderStatus != OrderStatusEnum.Closed)
                                                  select o).ToList();

            return allOrdersForBranchUser;
        }

        public static List<Order> GetAllManageListingFilteredOrders(Guid appUserId, bool getHistory)
        {
            ApplicationDbContext db = new ApplicationDbContext();

            List<Order> list = GetAllManageListingFilteredOrders(db, appUserId, getHistory);
            db.Dispose();
            return list;
        }

        public static List<Order> GetAllManageListingFilteredOrders(ApplicationDbContext db, Guid appUserId, bool getHistory)
        {
            AppUser appUser = AppUserHelpers.GetAppUser(db, appUserId);
            Branch branch = BranchHelpers.GetBranch(db, appUser.CurrentBranchId);
            AppUserSettings settings = AppUserSettingsHelpers.GetAppUserSettingsForUser(db, appUserId);

            //Create list 
            List<Order> list = new List<Order>();

            //Now bring in the Selection Level sort
            switch (settings.OrdersManageViewInternalSelectionLevel)
            {
                case InternalSearchLevelEnum.User:
                    list = GetAllOrdersForUser(db, appUserId, getHistory);
                    break;
                case InternalSearchLevelEnum.Branch: //user's current branch to filter
                    list = GetAllOrdersForBranch(db, branch.BranchId, getHistory);
                    break;
                case InternalSearchLevelEnum.Company: //user's current company to filter
                    list = GetAllOrdersForCompany(db, branch.CompanyId, getHistory);
                    break;
                case InternalSearchLevelEnum.Group: //user's built group sets to filter ***TO BE DONE***
                    break;
            }

            return list;
        }

        #endregion

        #region Create

        public static Order CreateOrder(IPrincipal user, Offer offer)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            Order order = CreateOrder(db, user, offer);
            db.Dispose();
            return order;
        }

        public static Order CreateOrder(ApplicationDbContext db, IPrincipal user, Offer offer)
        {
            BranchUser branchUser = BranchUserHelpers.GetBranchUserCurrentForUser(db, user);

            //depending on 'branch trading' check to see if last counter is related to this user, if not then use the last counter details (if counter details exist)
            if (offer.LastCounterOfferOriginatorAppUserId != null)
            {
                Company company = CompanyHelpers.GetCompany(db, branchUser.CompanyId);

                if (company.AllowBranchTrading)
                {
                    if (branchUser.BranchId != offer.LastCounterOfferOriginatorBranchId)
                        branchUser = BranchUserHelpers.GetBranchUser(db, offer.LastCounterOfferOriginatorAppUserId.Value, offer.LastCounterOfferOriginatorBranchId.Value, offer.LastCounterOfferOriginatorCompanyId.Value);
                }
                else
                {
                    if (branchUser.CompanyId != offer.LastCounterOfferOriginatorCompanyId)
                        branchUser = BranchUserHelpers.GetBranchUser(db, offer.LastCounterOfferOriginatorAppUserId.Value, offer.LastCounterOfferOriginatorBranchId.Value, offer.LastCounterOfferOriginatorCompanyId.Value);
                }
            }

            decimal orderQty = offer.CurrentOfferQuantity;
            if (offer.CurrentOfferQuantity == 0 && offer.CounterOfferQuantity != 0)
                orderQty = offer.CounterOfferQuantity.Value;

            Order order = new Order()
            {
                OrderId = Guid.NewGuid(),
                ListingType = offer.ListingType,
                OrderQuanity = orderQty,
                OrderStatus = OrderStatusEnum.New,
                OrderCreationDateTime = DateTime.Now,
                OrderOriginatorAppUserId = branchUser.UserId,
                OrderOriginatorBranchId = branchUser.BranchId,
                OrderOriginatorCompanyId = branchUser.CompanyId,
                OrderOriginatorDateTime = DateTime.Now,
                OfferId = offer.OfferId,
                OfferOriginatorAppUserId = offer.OfferOriginatorAppUserId,
                OfferOriginatorBranchId = offer.OfferOriginatorBranchId,
                OfferOriginatorCompanyId = offer.OfferOriginatorCompanyId,
                ListingId = offer.ListingId,
                ListingOriginatorAppUserId = offer.ListingOriginatorAppUserId,
                ListingOriginatorBranchId = offer.ListingOriginatorBranchId,
                ListingOriginatorCompanyId = offer.ListingOriginatorCompanyId
            };

            db.Orders.Add(order);

            //Update the quantities on listing
            
            switch (offer.ListingType)
            {
                case ListingTypeEnum.Available:
                    AvailableListingHelpers.UpdateQuantitiesFromOrder(db, offer);
                    break;
                case ListingTypeEnum.Requirement:
                    RequirementListingHelpers.UpdateQuantitiesFromOrder(db, offer);
                    break;
            }

            db.SaveChanges();

            return order;
        }

        #endregion

        #region Update

        public static Order UpdateOrderFromOrderEditView(OrderEditView view)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            Order orderDetails = UpdateOrderFromOrderEditView(db, view);
            db.Dispose();
            return orderDetails;
        }

        public static Order UpdateOrderFromOrderEditView(ApplicationDbContext db, OrderEditView view)
        {
            Order orderDetails = GetOrder(db, view.OrderId);
            orderDetails.OrderQuanity = view.OrderQuanity;
            orderDetails.OrderStatus = view.OrderStatus;
            orderDetails.OrderCreationDateTime = view.OrderCreationDateTime;
            orderDetails.OrderDistributionDateTime = view.OrderDistributionDateTime;
            orderDetails.OrderDeliveredDateTime = view.OrderDeliveredDateTime;
            orderDetails.OrderCollectedDateTime = view.OrderCollectedDateTime;
            orderDetails.OrderClosedDateTime = view.OrderClosedDateTime;

            db.Entry(orderDetails).State = EntityState.Modified;
            db.SaveChanges();

            return orderDetails;
        }

        public static Order ChangeOrderStatus(Guid orderId, OrderStatusEnum newStatus, IPrincipal user)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            Order orderDetails = ChangeOrderStatus(db, orderId, newStatus, user);
            db.Dispose();
            return orderDetails;
        }

        public static Order ChangeOrderStatus(ApplicationDbContext db, Guid orderId, OrderStatusEnum newStatus, IPrincipal user)
        {
            AppUser appUser = AppUserHelpers.GetAppUser(db, user);
            Order orderDetails = GetOrder(db, orderId);
            orderDetails.OrderStatus = newStatus;

            switch (newStatus)
            {
                case OrderStatusEnum.Despatched:
                    orderDetails.OrderDistributionDateTime = DateTime.Now;
                    orderDetails.OrderDistributedBy = appUser.AppUserId;
                    break;
                case OrderStatusEnum.Collected:
                    orderDetails.OrderCollectedDateTime = DateTime.Now;
                    orderDetails.OrderCollectedBy = appUser.AppUserId;
                    break;
                case OrderStatusEnum.Delivered:
                    orderDetails.OrderDeliveredDateTime = DateTime.Now;
                    orderDetails.OrderDeliveredBy = appUser.AppUserId;
                    break;
                case OrderStatusEnum.Received:
                    orderDetails.OrderReceivedDateTime = DateTime.Now;
                    orderDetails.OrderReceivedBy = appUser.AppUserId;
                    break;
                case OrderStatusEnum.Closed:
                    orderDetails.OrderClosedDateTime = DateTime.Now;
                    orderDetails.OrderClosedBy = appUser.AppUserId;
                    break;
            }

            db.Entry(orderDetails).State = EntityState.Modified;
            db.SaveChanges();

            return orderDetails;
        }

        #endregion
    }

    public static class OrderManageHelpers
    {
        #region Get

        public static List<OrderManageView> GetAllOrdersManageViewForUser(IPrincipal user)
        {
            ApplicationDbContext db = new ApplicationDbContext();

            List<OrderManageView> list = GetAllOrdersManageViewForUser(db, user);
            db.Dispose();
            return list;
        }

        public static List<OrderManageView> GetAllOrdersManageViewForUser(ApplicationDbContext db, IPrincipal user)
        {
            List<OrderManageView> allOrdersManageView = new List<OrderManageView>();

            AppUser appUser = AppUserHelpers.GetAppUser(db, user);
            List<Order> allOrdersForUser = OrderHelpers.GetAllOrdersForUser(db, appUser.AppUserId, false);

            foreach (Order orderForUser in allOrdersForUser)
            {
                OrderManageView orderManageView = new OrderManageView()
                {
                    OrderDetails = orderForUser
                };

                allOrdersManageView.Add(orderManageView);
            }

            return allOrdersManageView;
        }

        public static List<OrderManageView> GetAllOrdersManageView(IPrincipal user)
        {
            ApplicationDbContext db = new ApplicationDbContext();

            List<OrderManageView> list = GetAllOrdersManageView(db, user, false);
            db.Dispose();
            return list;
        }

        public static List<OrderManageView> GetAllOrdersManageView(IPrincipal user, bool getHistory)
        {
            ApplicationDbContext db = new ApplicationDbContext();

            List<OrderManageView> list = GetAllOrdersManageView(db, user, getHistory);
            db.Dispose();
            return list;
        }

        public static List<OrderManageView> GetAllOrdersManageView(ApplicationDbContext db, IPrincipal user, bool getHistory)
        {
            List<OrderManageView> allOrdersManageView = new List<OrderManageView>();

            AppUser appUser = AppUserHelpers.GetAppUser(db, user);
            List<Order> allOrdersForUser = OrderHelpers.GetAllManageListingFilteredOrders(db, appUser.AppUserId, getHistory);

            //If we allow branch trading then differentiate between branches for in/out trading, otherwise it is at company level
            Company company = CompanyHelpers.GetCompanyForUser(db, user);

            foreach (Order orderForBranchUser in allOrdersForUser)
            {
                OrderManageView orderManageView = new OrderManageView();
                orderManageView.OrderDetails = orderForBranchUser;

                //set Inhouse flag
                orderManageView.InhouseOrder = OrderProcessHelpers.SetInhouseFlag(orderForBranchUser, appUser, company);

                //Set OrderOut flag
                orderManageView.OrderOut = OrderProcessHelpers.SetOrderOutFlag(orderForBranchUser, orderManageView.InhouseOrder);

                //set buttons
                bool? displayDespatchButton = null;
                bool? displayDeliveredButton = null;
                bool? displayReceivedButton = null;
                bool? displayCollectedButton = null;
                bool? displayClosedButton = null;

                OrderProcessHelpers.SetOrderButtons(db, user, orderForBranchUser, orderManageView.OrderOut, out displayDespatchButton, out displayDeliveredButton, out displayReceivedButton, out displayCollectedButton, out displayClosedButton);

                orderManageView.DisplayDespatchButton = displayDespatchButton;
                orderManageView.DisplayDeliveredButton = displayDeliveredButton;
                orderManageView.DisplayReceivedButton = displayReceivedButton;
                orderManageView.DisplayCollectedButton = displayCollectedButton;
                orderManageView.DisplayClosedButton = displayClosedButton;

                allOrdersManageView.Add(orderManageView);
            }

            return allOrdersManageView;
        }

        #endregion
    }

    public static class OrderEditHelpers
    {
        #region Get

        public static OrderEditView GetOrderEditView(Guid orderId, IPrincipal user)
        {
            ApplicationDbContext db = new ApplicationDbContext();

            OrderEditView view = GetOrderEditView(db, orderId, user);
            db.Dispose();
            return view;
        }

        public static OrderEditView GetOrderEditView(ApplicationDbContext db, Guid orderId, IPrincipal user)
        {
            Order orderDetails = OrderHelpers.GetOrder(db, orderId);
            AppUser orderAppUser = null;
            Branch orderBranch = null;
            AppUser offerAppUser = null;
            Branch offerBranch = null;
            AppUser listingAppUser = null;
            Branch listingBranch = null;
            Offer offerDetails = null;
            AvailableListing availableListing = null;
            RequirementListing requirementListing = null;
            
            if (orderDetails.OrderOriginatorAppUserId != null)
                if (orderDetails.OrderOriginatorAppUserId.Value != Guid.Empty)
                    orderAppUser = AppUserHelpers.GetAppUser(db, orderDetails.OrderOriginatorAppUserId.Value);

            if (orderDetails.OrderOriginatorBranchId != null)
                if (orderDetails.OrderOriginatorBranchId.Value != Guid.Empty)
                    orderBranch = BranchHelpers.GetBranch(db, orderDetails.ListingOriginatorBranchId.Value);

            if (orderDetails.OfferOriginatorAppUserId != null)
                if (orderDetails.OfferOriginatorAppUserId.Value != Guid.Empty)
                    offerAppUser = AppUserHelpers.GetAppUser(db, orderDetails.OfferOriginatorAppUserId.Value);

            if (orderDetails.OfferOriginatorBranchId != null)
                if (orderDetails.OfferOriginatorBranchId.Value != Guid.Empty)
                    offerBranch = BranchHelpers.GetBranch(db, orderDetails.OfferOriginatorBranchId.Value);

            if (orderDetails.ListingOriginatorAppUserId != null)
                if (orderDetails.ListingOriginatorAppUserId.Value != Guid.Empty)
                    listingAppUser = AppUserHelpers.GetAppUser(db, orderDetails.ListingOriginatorAppUserId.Value);

            if (orderDetails.ListingOriginatorBranchId != null)
                if (orderDetails.ListingOriginatorBranchId.Value != Guid.Empty)
                    listingBranch = BranchHelpers.GetBranch(db, orderDetails.ListingOriginatorBranchId.Value);

            if (orderDetails.OfferId != null)
            {
                if (orderDetails.OfferId.Value != Guid.Empty)
                {
                    offerDetails = OfferHelpers.GetOffer(db, orderDetails.OfferId.Value);
                    if (orderDetails.ListingId != null)
                    {
                        if (orderDetails.ListingId.Value != Guid.Empty)
                        {
                            switch (offerDetails.ListingType)
                            {
                                case ListingTypeEnum.Available:
                                    availableListing = AvailableListingHelpers.GetAvailableListing(db, orderDetails.ListingId.Value);
                                    break;
                                case ListingTypeEnum.Requirement:
                                    requirementListing = RequirementListingHelpers.GetRequirementListing(db, orderDetails.ListingId.Value);
                                    break;
                            }
                        }
                    }
                }
            }
            
            OrderEditView view = new OrderEditView()
            {
                OrderId = orderDetails.OrderId,
                ListingType = orderDetails.ListingType,
                OrderQuanity = orderDetails.OrderQuanity,
                OrderStatus = orderDetails.OrderStatus,
                OrderCreationDateTime = orderDetails.OrderCreationDateTime,
                OrderDistributionDateTime = orderDetails.OrderDistributionDateTime,
                OrderDeliveredDateTime = orderDetails.OrderDeliveredDateTime,
                OrderCollectedDateTime = orderDetails.OrderCollectedDateTime,
                OrderClosedDateTime = orderDetails.OrderClosedDateTime,
                OrderAppUser = orderAppUser,
                OrderBranchDetails = orderBranch,
                OfferId = orderDetails.OfferId.GetValueOrDefault(),
                OfferAppUser = offerAppUser,
                OfferBranchDetails = offerBranch,
                OfferDetails = offerDetails,
                ListingId = orderDetails.ListingId.GetValueOrDefault(),
                ListingAppUser = listingAppUser,
                ListingBranchDetails = listingBranch,
                AvailableListingDetails = availableListing,
                RequirementListingDetails = requirementListing
            };

            AppUser thisAppUser = AppUserHelpers.GetAppUser(db, user);
            //If we allow branch trading then differentiate between branches for in/out trading, otherwise it is at company level
            Company thisCompany = CompanyHelpers.GetCompanyForUser(db, user);

            //set Inhouse flag
            view.InhouseOrder = OrderProcessHelpers.SetInhouseFlag(orderDetails, thisAppUser, thisCompany);

            //Set OrderOut flag
            view.OrderOut = OrderProcessHelpers.SetOrderOutFlag(orderDetails, view.InhouseOrder);

            //set buttons
            bool? displayDespatchButton = null;
            bool? displayDeliveredButton = null;
            bool? displayReceivedButton = null;
            bool? displayCollectedButton = null;
            bool? displayClosedButton = null;

            OrderProcessHelpers.SetOrderButtons(db, user, orderDetails, view.OrderOut, out displayDespatchButton, out displayDeliveredButton, out displayReceivedButton, out displayCollectedButton, out displayClosedButton);

            view.DisplayDespatchButton = displayDespatchButton;
            view.DisplayDeliveredButton = displayDeliveredButton;
            view.DisplayReceivedButton = displayReceivedButton;
            view.DisplayCollectedButton = displayCollectedButton;
            view.DisplayClosedButton = displayClosedButton;

            return view;
        }

        #endregion
    }

    public static class OrderProcessHelpers
    {
        public static bool SetInhouseFlag(Order order, AppUser appUser, Company company)
        {
            if (company.AllowBranchTrading)
            {
                if (order.OrderOriginatorBranchId == appUser.CurrentBranchId)
                    return true;
                else
                    return false;
            }
            else
            {
                if (order.OrderOriginatorCompanyId == company.CompanyId)
                    return true;
                else
                    return false;
            }
        }

        public static bool SetOrderOutFlag(Order order, bool inhouseOrder)
        {
            if ((inhouseOrder && order.ListingType == ListingTypeEnum.Available) || (!inhouseOrder && order.ListingType == ListingTypeEnum.Requirement))
                return true;
            else
                return false;
        }

        public static void SetOrderButtons(ApplicationDbContext db, IPrincipal user, Order order, bool orderOut, out bool? displayDespatchButton, out bool? displayDeliveredButton, out bool? displayReceivedButton, out bool? displayCollectedButton, out bool? displayClosedButton)
        {
            //Get settings for logged in user
            AppUserSettings settings = AppUserSettingsHelpers.GetAppUserSettingsForUser(db, user);

            //Set the authorisation levels and IDs for button activation on form
            InternalSearchLevelEnum despatchedAuthorisationLevel = settings.OrdersDespatchedAuthorisationManageViewLevel;
            Guid despatchedAuthorisationId = DataHelpers.GetAuthorisationId(settings.OrdersDespatchedAuthorisationManageViewLevel, user);
            InternalSearchLevelEnum deliveredAuthorisationLevel = settings.OrdersDeliveredAuthorisationManageViewLevel;
            Guid deliveredAuthorisationId = DataHelpers.GetAuthorisationId(settings.OrdersDeliveredAuthorisationManageViewLevel, user);
            InternalSearchLevelEnum receivedAuthorisationLevel = settings.OrdersReceivedAuthorisationManageViewLevel;
            Guid receivedAuthorisationId = DataHelpers.GetAuthorisationId(settings.OrdersReceivedAuthorisationManageViewLevel, user);
            InternalSearchLevelEnum collectedAuthorisationLevel = settings.OrdersCollectedAuthorisationManageViewLevel;
            Guid collectedAuthorisationId = DataHelpers.GetAuthorisationId(settings.OrdersCollectedAuthorisationManageViewLevel, user);
            InternalSearchLevelEnum closedAuthorisationLevel = settings.OrdersClosedAuthorisationManageViewLevel;
            Guid closedAuthorisationId = DataHelpers.GetAuthorisationId(settings.OrdersClosedAuthorisationManageViewLevel, user);

            //set buttons
            if (orderOut)
            {
                displayDespatchButton = true;
                displayDeliveredButton = true;
                displayReceivedButton = null;
                displayCollectedButton = null;
                displayClosedButton = true;

                switch (despatchedAuthorisationLevel)
                {
                    case InternalSearchLevelEnum.Company:
                        if (order.OrderOriginatorCompanyId != despatchedAuthorisationId && order.OfferOriginatorCompanyId != despatchedAuthorisationId)
                            displayDespatchButton = false;
                        break;
                    case InternalSearchLevelEnum.Branch:
                        if (order.OrderOriginatorBranchId != despatchedAuthorisationId && order.OfferOriginatorBranchId != despatchedAuthorisationId)
                            displayDespatchButton = false;
                        break;
                    case InternalSearchLevelEnum.User:
                        if (order.OrderOriginatorAppUserId != despatchedAuthorisationId && order.OfferOriginatorAppUserId != despatchedAuthorisationId)
                            displayDespatchButton = false;
                        break;
                    case InternalSearchLevelEnum.Group:   //LSLSLS  TO BE DONE WHEN GROUPS ADDED
                        break;
                }

                switch (deliveredAuthorisationLevel)
                {
                    case InternalSearchLevelEnum.Company:
                        if (order.OrderOriginatorCompanyId != deliveredAuthorisationId && order.OfferOriginatorCompanyId != deliveredAuthorisationId)
                            displayDeliveredButton = false;
                        break;
                    case InternalSearchLevelEnum.Branch:
                        if (order.OrderOriginatorBranchId != deliveredAuthorisationId && order.OfferOriginatorBranchId != deliveredAuthorisationId)
                            displayDespatchButton = false;
                        break;
                    case InternalSearchLevelEnum.User:
                        if (order.OrderOriginatorAppUserId != deliveredAuthorisationId && order.OfferOriginatorAppUserId != deliveredAuthorisationId)
                            displayDespatchButton = false;
                        break;
                    case InternalSearchLevelEnum.Group:   //LSLSLS  TO BE DONE WHEN GROUPS ADDED
                        break;
                }

                switch (closedAuthorisationLevel)
                {
                    case InternalSearchLevelEnum.Company:
                        if (order.OrderOriginatorCompanyId != closedAuthorisationId && order.OfferOriginatorCompanyId != closedAuthorisationId)
                            displayClosedButton = false;
                        break;
                    case InternalSearchLevelEnum.Branch:
                        if (order.OrderOriginatorBranchId != closedAuthorisationId && order.OfferOriginatorBranchId != closedAuthorisationId)
                            displayClosedButton = false;
                        break;
                    case InternalSearchLevelEnum.User:
                        if (order.OrderOriginatorAppUserId != closedAuthorisationId && order.OfferOriginatorAppUserId != closedAuthorisationId)
                            displayClosedButton = false;
                        break;
                    case InternalSearchLevelEnum.Group:   //LSLSLS  TO BE DONE WHEN GROUPS ADDED
                        break;
                }
            }
            else
            {
                displayDespatchButton = null;
                displayDeliveredButton = null;
                displayReceivedButton = true;
                displayCollectedButton = true;
                displayClosedButton = true;

                switch (collectedAuthorisationLevel)
                {
                    case InternalSearchLevelEnum.Company:
                        if (order.OrderOriginatorCompanyId != collectedAuthorisationId && order.OfferOriginatorCompanyId != collectedAuthorisationId)
                            displayCollectedButton = false;
                        break;
                    case InternalSearchLevelEnum.Branch:
                        if (order.OrderOriginatorBranchId != collectedAuthorisationId && order.OfferOriginatorBranchId != collectedAuthorisationId)
                            displayCollectedButton = false;
                        break;
                    case InternalSearchLevelEnum.User:
                        if (order.OrderOriginatorAppUserId != collectedAuthorisationId && order.OfferOriginatorAppUserId != collectedAuthorisationId)
                            displayCollectedButton = false;
                        break;
                    case InternalSearchLevelEnum.Group:   //LSLSLS  TO BE DONE WHEN GROUPS ADDED
                        break;
                }

                switch (receivedAuthorisationLevel)
                {
                    case InternalSearchLevelEnum.Company:
                        if (order.OrderOriginatorCompanyId != receivedAuthorisationId && order.OfferOriginatorCompanyId != receivedAuthorisationId)
                            displayReceivedButton = false;
                        break;
                    case InternalSearchLevelEnum.Branch:
                        if (order.OrderOriginatorBranchId != receivedAuthorisationId && order.OfferOriginatorBranchId != receivedAuthorisationId)
                            displayReceivedButton = false;
                        break;
                    case InternalSearchLevelEnum.User:
                        if (order.OrderOriginatorAppUserId != receivedAuthorisationId && order.OfferOriginatorAppUserId != receivedAuthorisationId)
                            displayReceivedButton = false;
                        break;
                    case InternalSearchLevelEnum.Group:   //LSLSLS  TO BE DONE WHEN GROUPS ADDED
                        break;
                }

                switch (closedAuthorisationLevel)
                {
                    case InternalSearchLevelEnum.Company:
                        if (order.OrderOriginatorCompanyId != closedAuthorisationId && order.OfferOriginatorCompanyId != closedAuthorisationId)
                            displayClosedButton = false;
                        break;
                    case InternalSearchLevelEnum.Branch:
                        if (order.OrderOriginatorBranchId != closedAuthorisationId && order.OfferOriginatorBranchId != closedAuthorisationId)
                            displayClosedButton = false;
                        break;
                    case InternalSearchLevelEnum.User:
                        if (order.OrderOriginatorAppUserId != closedAuthorisationId && order.OfferOriginatorAppUserId != closedAuthorisationId)
                            displayClosedButton = false;
                        break;
                    case InternalSearchLevelEnum.Group:   //LSLSLS  TO BE DONE WHEN GROUPS ADDED
                        break;
                }
            }
        }
    }
}