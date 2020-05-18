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

        public readonly string userId = "uId_key";
        public readonly string LoggedIn = "login_key";
        public readonly int TRUE = 1;
        public readonly int FALSE = 0;

        public LoginController(ILogger<LoginController> logger, OXXGameDBContext context)
        {
            _logger = logger;
            dbContext = context;
        }

        // Index, kjøres når programmet starter. Sørger for at egen Session variabel blir FALSE
        // slik at en bruker ikke er logget inn fra start. 
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
                    HttpContext.Session.SetInt32(userId, user.userId);
                    

                    if (user.isAdmin)
                    {
                        HttpContext.Session.SetString("email", user.email);
                        return RedirectToAction("AdminPortal", "Admin");
                    }
                    else
                    {
                        // Setter en String session variabel med navnet til innlogget bruker som verdi
                        HttpContext.Session.SetString("username", user.firstname);
                        return RedirectToAction("TestInfo", "Test");
                    }
                }
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult UserRegistration()
        {
            DB db = new DB(dbContext);
            List<Category> categories = db.allCategories();
            User model = new User()
            {
                categoryLvls = new List<User.CategoryLvl>()
            };

            foreach (Category category in categories)
            {
                model.categoryLvls.Add(new User.CategoryLvl()
                {
                    category = category.category,
                    lvl = false
                });
            }

            return View("UserRegistration",model);
        }

        [HttpPost]
        public ActionResult UserRegistration(User user)
        {
            if (!ModelState.IsValid)
            {
                return View("UserRegistration",user);
            }

            DB db = new DB(dbContext);
            List<User> users = db.allUsers();
            
            List<Category> categories = db.allCategories();
            List<ResultPerCategory> resPerCategory = new List<ResultPerCategory>();

            foreach (User existingUser in users)
            {
                if (user.email == existingUser.email)
                {
                    ViewData["EmailErrorMessage"] = "Denne epost-addressen er allerede registrert";
                    return View("UserRegistration",user);
                }
            }

            Users newUser = db.addUser(user);
            if (newUser != null)
            {
                for (int i = 0; i < categories.Count(); i++)
                {
                    int inLvl = 0;
                    if (user.categoryLvls[i].lvl)
                    {
                        inLvl = 1;
                    }

                    resPerCategory.Add(new ResultPerCategory()
                    {
                        userId = newUser.id,
                        category = categories[i].category,
                        lvl = inLvl
                    });
                }

                if (db.addResultPerCategory(resPerCategory))
                {
                    ModelState.Clear();
                    HttpContext.Session.SetInt32(LoggedIn, TRUE);
                    HttpContext.Session.SetInt32(userId, newUser.id);
                    HttpContext.Session.SetString("username", user.firstname);
                    return RedirectToAction("TestInfo", "Test");
                }
            }

            ViewData["DBErrorMessage"] = "Det oppsto en feil, prøv igjen.";
            return View("UserRegistration",user);
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