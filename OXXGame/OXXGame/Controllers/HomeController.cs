using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OXXGame.Models;
using Microsoft.AspNetCore.Http;

namespace OXXGame.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private OXXGameDBContext dbContext; //DbContext-objektet som brukes til database-aksess

        public readonly string LoggedIn = "login_key";
        public readonly int TRUE = 1;
        public readonly int FALSE = 0;

        public HomeController(ILogger<HomeController> logger, OXXGameDBContext context)
        {
            _logger = logger;
            dbContext = context;
        }


        public ActionResult Index()
        {
            return View();
        }


        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpPost]
        public ActionResult Login(User inUser)
        {
            DB db = new DB(dbContext);

            User user = db.getUser(inUser.email);

            if (user != null)
            {
                if (Enumerable.SequenceEqual(inUser.pwdHash,user.pwdHash))
                {
                    loggedIn(true);
                    return View("TestInfo");
                }
            }


            return RedirectToAction("Index");
        }

        public ActionResult Register()
        {
            return View("RegisterUser");
        }

        [HttpPost]
        public ActionResult RegisterUser(User user)
        {
            DB db = new DB(dbContext);
            if (db.addUser(user))
            {
                ModelState.Clear();
                return View("Index");
            }

            return RedirectToAction("Register");
        }
        
        public ActionResult StartTest()
        {
            if (loggedIn(true))
            {
                return View("TestView");
            }
            else
            {
                return View("Index");
            }
        }

        public ActionResult Avbryt()
        {
            loggedIn(false);
            return RedirectToAction("Index");
        }


        public bool loggedIn(bool loggetInn)
        {
            
            if (HttpContext.Session.Get(LoggedIn) != null)
            {
                if (HttpContext.Session.GetInt32(LoggedIn) == TRUE)
                {
                    return true;
                }
            }
            else
            {
                HttpContext.Session.SetInt32(LoggedIn, FALSE);
                return false;
            }

            return loggetInn;
        }

    }
}