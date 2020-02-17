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
    public class LoginController : Controller
    {
        private readonly ILogger<LoginController> _logger;
        private OXXGameDBContext dbContext; //DbContext-objektet som brukes til database-aksess

        public readonly string LoggedIn = "login_key";
        public readonly int TRUE = 1;
        public readonly int FALSE = 0;

        public LoginController(ILogger<LoginController> logger, OXXGameDBContext context)
        {
            _logger = logger;
            dbContext = context;
        }

        //  Index, kjøres når programmet starter. Sørger for at egen Session variabel blir FALSE
        //  slik at en bruker ikke er logget inn fra start. 

        public ActionResult Index()
        {
            HttpContext.Session.SetInt32(LoggedIn, FALSE);
            return View();
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
                    HttpContext.Session.SetInt32(LoggedIn, TRUE);

                    if (user.isAdmin)
                    {
                        return RedirectToAction("AdminPortal", "Admin");
                    }
                    else
                    {
                        return RedirectToAction("TestInfo", "Test");
                    }

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

            return View("RegisterUser");
        }

/*        public ActionResult Avbryt()

        {
            HttpContext.Session.SetInt32(LoggedIn, FALSE);
            Debug.WriteLine("Logger ut...");
            return RedirectToAction("Index");
        }

*/

        public bool loggedIn()
        {
            bool loggetInn;

            if (HttpContext.Session.GetInt32(LoggedIn) == TRUE)
            {
                loggetInn = true;
            }
            else
            {
                HttpContext.Session.SetInt32(LoggedIn, FALSE);
                loggetInn = false;
            }

            return loggetInn;
        }
    }
}