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
            loggedIn();
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
                    HttpContext.Session.SetInt32("uId", user.userId);

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


        public ActionResult UserRegistration()
        {
            return View("UserRegistration");
        }

        [HttpPost]
        public ActionResult UserRegistration(User user)
        {

            DB db = new DB(dbContext);
            List<User> users = db.allUsers();

            foreach (User existingUser in users)
            {
                if (user.email == existingUser.email)
                {
                    ViewData["EmailErrorMessage"] = "Denne epost-addressen er allerede registrert";
                    return View("UserRegistration");
                }
            }

            if (db.addUser(user) != null)
            {
                ModelState.Clear();
                HttpContext.Session.SetInt32(LoggedIn, TRUE);
                return RedirectToAction("TestInfo","Test");
            }

            ViewData["DBErrorMessage"] = "Det oppsto en feil, prøv igjen.";
            return View("UserRegistration");
        }
                    
       

/*
        public ActionResult Avbryt()
        {
            HttpContext.Session.SetInt32(LoggedIn, FALSE);
            Debug.WriteLine("Logger ut...");
            return RedirectToAction("Index");
        }



*/

        //metode som sjekker om en bruker er Administrator (til bruk for testing)
        public bool isAdmin(User inUser)
        {
            bool isAdmin = false;

            DB db = new DB(dbContext);

            User user = db.getUser(inUser.email);

            if (user != null)
            {
                if (user.isAdmin)
                {
                    isAdmin = true;
                    Debug.WriteLine("User is admin");
                }
                else
                {
                    isAdmin = false;
                    Debug.WriteLine("User is not admin");
                }
            }

            return isAdmin;
        }

        // Metode som skal sjekke om en bruker er logget inn med session. 
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