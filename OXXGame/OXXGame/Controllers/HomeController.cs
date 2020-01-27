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

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        // Lager ActionResult-metoder her knyttet til sidebytte. Data hentes ut fra/legges til i databasen her.
        // Metodenavnet skal samsvare med navnet til viewet (.cshtml filen). Eks:
        /*
            public ActionResult Eksempel()
            {
                Entitet e = new Entitet();
                e.tall = 5;
                e.ord = "pest";
                dbContext.Add(hallo);
                dbContext.SaveChanges();
                return View();
            }
        */
    }
}