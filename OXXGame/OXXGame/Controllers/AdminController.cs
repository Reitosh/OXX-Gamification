using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using OXXGame.Controllers;
using OXXGame.Models;

namespace OXXGame.Controllers
{
    public class AdminController : Controller
    {
        OXXGameDBContext dbContext;

        public AdminController(OXXGameDBContext context)
        {
            dbContext = context;
        }

        public ActionResult AdminPortal()
        {
            return View("AdminPortal");
        }

        public ActionResult UserAdmin()
        {
            DB db = new DB(dbContext);
            List<User> users = db.allUsers();
            List<Result> results = db.allResults();

            ViewData["Users"] = users;
            ViewData["Results"] = results;

            return View("UserAdmin");
        }

        public ActionResult TaskAdmin()
        {
            return View("TaskAdmin");
        }
    }
}