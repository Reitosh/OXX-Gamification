using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using OXXGame.Controllers;
using OXXGame.Models;
using Microsoft.AspNetCore.Http;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Web;

namespace OXXGame.Controllers
{
    public class AdminController : Controller
    {
        OXXGameDBContext dbContext;

        public readonly string IsAdmin = "admin_key";
        public readonly string LoggedIn = "login_key";
        public readonly int TRUE = 1;
        public readonly int FALSE = 0;

        public AdminController(OXXGameDBContext context)
        {
            dbContext = context;
        }

        public ActionResult AdminPortal()
        {
            if (HttpContext.Session.GetInt32(LoggedIn) == TRUE)
            {
                HttpContext.Session.SetInt32(IsAdmin, TRUE);
                return View("AdminPortal");
            }
            else
            {
                return RedirectToAction("Index","Login");
            }
        }

        public ActionResult DownloadFile(string path)
        {
            //path = @"C:\Users\siver\Desktop\test.txt";
            return PhysicalFile(path, "text/plain", path);
        }

        public ActionResult UserAdmin()
        {
            if (AdminLoggedIn()) 
            {
                DB db = new DB(dbContext);
                List<User> users = db.allUsers();
                List<Result> results = db.allResults();
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
                ViewData["Results"] = results;
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

        [HttpPost]
        public ActionResult CreateTask(Models.Task task)
        {
            DB db = new DB(dbContext);

            bool insertOK = db.addTask(task);
            if (insertOK)
            {
                return RedirectToAction("TaskAdmin");
            }
            return View();
        }

        public ActionResult EditTask(int testId)
        {
            var db = new DB(dbContext);
            ViewData["Categories"] = db.allCategories();
            Models.Task aTask = db.getSingleTask(testId);
            return View(aTask);   
        }

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
            return View();
        }
    }
}