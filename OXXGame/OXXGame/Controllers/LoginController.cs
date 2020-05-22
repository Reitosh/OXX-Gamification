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

        // Index view, kjøres når webapplikasjonen starter
        public ActionResult Index()
        {
           
            // Session variabel satt til FALSE slik at en bruker ikke er logget inn fra start 
            HttpContext.Session.SetInt32(LoggedIn, FALSE);
            loggedIn();
            return View();
        }

        [HttpPost]
        public ActionResult Login(User inUser)
        {
            // Oppretter tilkoblingen til databasen
            DB db = new DB(dbContext);
            User user = db.getUser(inUser.email);

            // Sjekker om gitt bruker er registrert i databasen  
            if (user != null)
            {
                // Hvis brukeren eksisterer i databasen, sjekkes det videre om angitt passord 
                // samsvarer med brukernes hashet passord som er lagret i databasen.
                if (Enumerable.SequenceEqual(inUser.pwdHash,user.pwdHash))
                {
                    // Hvis passordet samsvarer, setter vi session variabel til TRUE nå,
                    // som sier da at brukeren er logget in
                    HttpContext.Session.SetInt32(LoggedIn, TRUE);
                    HttpContext.Session.SetInt32(userId, user.userId);

                    // if-uttalelse som sjekker om innlogget bruker er admin eller ikke.
                    // Ut i fra resultatet blir brukeren videre dirigert til enten adminportalen,
                    // eller startsiden for testen (TestInfo.cshtml view).
                    if (user.isAdmin)
                    {
                        // Setter en string session variabel med e-post til innlogget bruker som verdi
                        HttpContext.Session.SetString("email", user.email);
                        return RedirectToAction("AdminPortal", "Admin");
                    }
                    else
                    {
                        // Setter en string session variabel med navnet til innlogget bruker som verdi
                        HttpContext.Session.SetString("username", user.firstname);
                        return RedirectToAction("TestInfo", "Test");
                    }
                }
            }

            // Hvis brukeren ikke er registrert med gitt e-postadresse eller passordet ikke samsvarer,
            // oppretter systemet en feilmelding ved bruk av ViewData, fjerner brukerens inputverdi 
            // fra inputfeltet for e-post og returnerer brukeren tilbake til Index/logginn view.
            ViewData["LoginError"] = "Feil e-postadresse eller passord";
            ModelState.Clear();
            return View("Index");
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
                // Sjekker om angitt e-postadresse er allerede registrert for en bruker.
                // HVis ja, oppretter feilmelding via ViewData og returnerer tilbake til 
                // registreringssiden.
                if (user.email == existingUser.email)
                {   
                    ViewData["EmailErrorMessage"] = "E-postadressen du har angitt er allerede registrert";
                    return View("UserRegistration", user);
                }
                // Sjekker om angitt telefonnummer er allerede registrert for en bruker.
                // HVis ja, oppretter feilmelding via ViewData og returnerer tilbake til 
                // registreringssiden.
                else if (user.tlf == existingUser.tlf)
                {
                    ViewData["TlfErrorMessage"] = "Telefonnummeret du har angitt er allerede registrert";
                    return View("UserRegistration", user);
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

        // Bestemmer om brukeren er logget inn basert på session-variabler som er satt
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