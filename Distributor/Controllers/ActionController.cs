﻿using Distributor.Helpers;
using Distributor.Models;
using Distributor.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Distributor.Controllers
{
    [Authorize]
    public class ActionController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Action
        public ActionResult Index()
        {
            List<UserActionView> model = UserActionViewsHelpers.GetActionsForViewsForUser(db, User);
            return View(model);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}