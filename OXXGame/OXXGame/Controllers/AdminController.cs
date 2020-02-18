﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using OXXGame.Controllers;
using OXXGame.Models;
using Microsoft.AspNetCore.Http;
using System.Diagnostics;

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
                return RedirectToAction("Index","Home");
            }
        }

        public ActionResult UserAdmin()
        {
            if (AdminLoggedIn()) 
            {
                DB db = new DB(dbContext);
                List<User> users = db.allUsers();
                List<Result> results = db.allResults();

                ViewData["Users"] = users;
                ViewData["Results"] = results;

                return View("UserAdmin");
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        public ActionResult TaskAdmin()
        {
            if (AdminLoggedIn())
            {
                return View("TaskAdmin");
            }
            else
            {
                return RedirectToAction("Index", "Home");
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
    }
}