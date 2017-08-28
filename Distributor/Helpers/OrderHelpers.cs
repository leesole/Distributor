using Distributor.Models;
using Distributor.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using static Distributor.Enums.OrderEnums;

namespace Distributor.Helpers
{
    public class OrderHelpers
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

        public static List<Order> GetAllOrdersForUser(Guid appUserId)
        {
            ApplicationDbContext db = new ApplicationDbContext();

            List<Order> list = GetAllOrdersForUser(db, appUserId);
            db.Dispose();
            return list;
        }

        public static List<Order> GetAllOrdersForUser(ApplicationDbContext db, Guid appUserId)
        {
            List<Order> allOrdersForUser = (from o in db.Orders
                                            where (o.OrderOriginatorAppUserId == appUserId && o.OrderStatus == OrderStatusEnum.New)
                                            select o).ToList();

            return allOrdersForUser;
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
            List<Order> allOrdersForBranchUser = (from bu in db.BranchUsers
                                                  from o in db.Orders
                                                  where (bu.UserId == o.OrderOriginatorAppUserId && bu.BranchId == o.OrderOriginatorBranchId && bu.CompanyId == o.OrderOriginatorCompanyId
                                                        && o.OrderStatus == OrderStatusEnum.New)
                                                  select o).ToList();

            return allOrdersForBranchUser;
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
            List<Order> allOrdersForUser = OrderHelpers.GetAllOrdersForUser(db, appUser.AppUserId);

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

        public static List<OrderManageView> GetAllOrdersManageViewForUserBranch(IPrincipal user)
        {
            ApplicationDbContext db = new ApplicationDbContext();

            List<OrderManageView> list = GetAllOrdersManageViewForUserBranch(db, user);
            db.Dispose();
            return list;
        }

        public static List<OrderManageView> GetAllOrdersManageViewForUserBranch(ApplicationDbContext db, IPrincipal user)
        {
            List<OrderManageView> allOrdersManageView = new List<OrderManageView>();

            BranchUser branchUser = BranchUserHelpers.GetBranchUserCurrentForUser(db, user);
            List<Order> allOrdersForBranchUser = OrderHelpers.GetAllOrdersForBranchUser(db, branchUser);

            foreach (Order orderForBranchUser in allOrdersForBranchUser)
            {
                OrderManageView orderManageView = new OrderManageView()
                {
                    OrderDetails = orderForBranchUser
                };

                allOrdersManageView.Add(orderManageView);
            }

            return allOrdersManageView;
        }

        #endregion
    }
}