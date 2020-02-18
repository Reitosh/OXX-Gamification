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
        private OXXGameDBContext dbContext; // DbContext-objektet som brukes til database-aksess

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
            HttpContext.Session.SetInt32(LoggedIn, FALSE);
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
                    HttpContext.Session.SetInt32(LoggedIn, TRUE);

                    if (user.isAdmin)
                    {
                        return RedirectToAction("AdminPortal", "Admin");
                    }
                    else
                    {
                        return View("TestInfo");
                    }

                }
            }
            return RedirectToAction("Index");
        }

        public ActionResult TestInfo()
        {
            if (loggedIn())
            {
                return View();
            }
            else
            {
                Debug.WriteLine("Ikke logget inn");
                return RedirectToAction("UserRegistration");
            }
        }

        public ActionResult TestView()
        {
            if (loggedIn())
            {
                return View();
            }
            else
            {
                Debug.WriteLine("Ikke logget inn");
                return RedirectToAction("UserRegistration");
            }
        }

        public ActionResult UserRegistration()
        {
            return View("UserRegistration");
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

            return View("UserRegistration");
        }
        
        public ActionResult StartTest()
        {
            if (loggedIn())
            {
                return View("TestView");
            }
            else
            {
                return View("Index");
            }
        }

        public ActionResult KjorKode(Submission Submission)
        {
            SSHConnect ssh = new SSHConnect("Markus", "Plainsmuchj0urney", "51.140.218.174");
            ssh.ConnectToVM(Submission.Code);
            
            return RedirectToAction("TestView");
        }

        public ActionResult Avbryt()
        {
            HttpContext.Session.SetInt32(LoggedIn, FALSE);
            Debug.WriteLine("Logger ut...");
            return RedirectToAction("Index");
        }


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