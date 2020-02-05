using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OXXGame.Models;

namespace OXXGame.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private OXXGameDBContext dbContext; //DbContext-objektet som brukes til database-aksess

        public HomeController(ILogger<HomeController> logger, OXXGameDBContext context)
        {
            _logger = logger;
            dbContext = context;
        }


        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Register()
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
                    Debug.WriteLine("Successful login!");
                    return View("TestInfo");
                }
            }

            Debug.WriteLine("Login failed..");
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
                user = null;
                return View("Index");
            }

            return RedirectToAction("Register");
        }
    }
}