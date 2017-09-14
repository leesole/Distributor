using Distributor.Helpers;
using Distributor.Models;
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
            List<UserAction> model = UserActionHelpers.GetActionsForUser(db, User);
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