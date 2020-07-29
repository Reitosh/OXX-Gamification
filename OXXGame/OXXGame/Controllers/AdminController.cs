using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using OXXGame.Models;
using Microsoft.AspNetCore.Http;
using System.Diagnostics;
using System.IO.Compression;

namespace OXXGame.Controllers
{
    public class AdminController : Controller
    {
        OXXGameDBContext dbContext;

        // Keys og verdier til session-variabler
        public readonly string IsAdmin = "admin_key";
        public readonly string LoggedIn = "login_key";
        public readonly int TRUE = 1;
        public readonly int FALSE = 0;

        // Constructor; mottar DBContext gjennom dependency injection
        public AdminController(OXXGameDBContext context)
        {
            dbContext = context;
        }

        // Entry point for adminportalen
        public ActionResult AdminPortal()
        {
            if (HttpContext.Session.GetInt32(LoggedIn) == TRUE)
            {
                HttpContext.Session.SetInt32(IsAdmin, TRUE);
                ViewData["email"] = HttpContext.Session.GetString("email");
                return View("AdminPortal");
            }
            else
            {
                return RedirectToAction("Index","Login");
            }
        }

        // Laster ned en spesifik fil fra server
        public FileResult DownloadFile(string path, string category, int userId, int testId)
        {
            return PhysicalFile(path, "text/plain", "u" + userId + "t" + testId + FileHandler.getFileExtension(category));
        }

        // Laster ned en gitt brukers besvarelser som zip
        public FileResult DownloadAllZip(int userId)
        {
            string directoryPath = "/home/submission_files/" + userId;
            string zipPath = "/home/submission_files/zips/" + userId + "_code.zip";

            if (System.IO.File.Exists(zipPath))
            {
                System.IO.File.Delete(zipPath);
            }

            ZipFile.CreateFromDirectory(directoryPath, zipPath);
            return PhysicalFile(zipPath, "application/zip", "User_" + userId + "_code.zip");
        }

        // Returnerer brukeradministrasjonssiden med nødvendig data
        public ActionResult UserAdmin()
        {
            if (AdminLoggedIn()) 
            {
                DB db = new DB(dbContext);
                List<User> users = db.allUsers();
                List<SingleTestResult> singleTestResults = db.allSingleTestResults();
                List<Models.Task> tasks = db.allTasks();

                Dictionary<int, string> category = new Dictionary<int, string>();
                Dictionary<int, int> difficulty = new Dictionary<int, int>();

                foreach (Models.Task task in tasks)
                {
                    category.Add(task.testId, task.category);
                    difficulty.Add(task.testId, task.difficulty);

                }

                ViewData["Users"] = users;
                ViewData["SingleTestResults"] = singleTestResults;
                ViewData["category"] = category;
                ViewData["difficulty"] = difficulty;

                return View("UserAdmin");
            }
            else
            {
                return RedirectToAction("Index", "Login");
            }
        }

        // Returnerer oppgaveadministrasjonssiden med nødvendig data
        public ActionResult TaskAdmin()
        {
            if (AdminLoggedIn())
            {
                DB db = new DB(dbContext);
                List<OXXGame.Models.Task> tasks = db.allTasks();

                ViewData["Tasks"] = tasks;

                return View("TaskAdmin");
            }
            else
            {
                return RedirectToAction("Index", "Login");
            }
        }

        // Sletter bruker og returnerer brukeradministrasjonsside
        public ActionResult DeleteUser(int userId)
        {
            var userDb = new DB(dbContext);
            bool OK = userDb.deleteUser(userId);
            if (OK)
            {
                Debug.WriteLine("User deleted");
            }
            return RedirectToAction("UserAdmin", "Admin");
        }

        // Sletter oppgave (Task) og returnerer oppgaveadministrasjonsside
        public ActionResult DeleteTask(int testId)
        {
            var taskDb = new DB(dbContext);
            bool  OK = taskDb.deleteTask(testId);
            if (OK)
            {
                Debug.WriteLine("Task deleted");
            }
            return RedirectToAction("TaskAdmin", "Admin");
        }

        // Returnerer side for oppgaveopprettelse
        public ActionResult CreateTask()
        {
            if (AdminLoggedIn())
            {
                DB db = new DB(dbContext);
                ViewData["Categories"] = db.allCategories();
                return View("CreateTask");
            }
            else
            {
                return RedirectToAction("Index", "Login");
            }
        }

        // Legger ny oppgave i database og returnerer side for oppgaveadministrasjon
        // Går tilbake til oppgave-editor om databaselagring feiler
        [HttpPost]
        public ActionResult CreateTask(Models.Task task)
        {
            DB db = new DB(dbContext);

            bool insertOK = db.addTask(task);
            if (insertOK)
            {
                return RedirectToAction("TaskAdmin");
            }
            return View(task);
        }

        // Henter valgt oppgave fra database og returnerer oppgave-editor
        public ActionResult EditTask(int testId)
        {
            var db = new DB(dbContext);
            ViewData["Categories"] = db.allCategories();
            Models.Task aTask = db.getSingleTask(testId);
            return View(aTask);   
        }

        // Lagrer endret oppgave til database og returnerer side for oppgaveadministrasjon
        // Skulle lagringen feile, returneres oppgave-editor
        [HttpPost]
        public ActionResult EditTask(int testId, Models.Task editTask)
        {
            if (AdminLoggedIn())
            {
                var db = new DB(dbContext);
                ViewData["Categories"] = db.allCategories();
                bool editOK = db.editTask(testId, editTask);
                if (editOK)
                {
                    return RedirectToAction("TaskAdmin");
                }
            }
            return View(editTask);
        }

        // Metode som sjekker om bruker er innlogget, og om bruker har administratorrettigheter
        // ActionResult-metoder i denne klassen vil kalle på denne metoden, og omdirigere til
        // login hvis false returneres
        private bool AdminLoggedIn()
        {
            if (HttpContext.Session.GetInt32(LoggedIn) == TRUE &&
                HttpContext.Session.GetInt32(IsAdmin) == TRUE)
            {
                return true;
            }
            else
            {
                Debug.WriteLine("LoggedIn = " + HttpContext.Session.GetInt32(LoggedIn));
                Debug.WriteLine("isAdmin = " + HttpContext.Session.GetInt32(IsAdmin));

                return false;
            }
        }
    }
}