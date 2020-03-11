using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using Microsoft.AspNetCore.Http;
using OXXGame.Controllers;
using OXXGame.Models;

namespace OXXGame.Controllers
{
    public class TestController : Controller
    {
        OXXGameDBContext dbContext;

        public TestController(OXXGameDBContext context)
        {
            dbContext = context;
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
                return RedirectToAction("Index", "Login");
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
                return RedirectToAction("Index", "Login");
            }
        }

        public ActionResult StartTest()
        {
            if (loggedIn())
            {
                Submission submission = new Submission()
                {
                    Code = @"using System;

namespace CSharp 
{
	class Solution 
	{
		static void Main(string[] args) 
		{
			
		}
	}
}"

                };
                return View("TestView", submission);
            }
            else
            {
                return RedirectToAction("Index", "Login");
            }
        }

        public ActionResult RunTypeScript(Submission submission)
        {
            SSHConnect TypeScript = new SSHConnect("Markus", "Plainsmuchj0urney", "51.140.218.174", dbContext);
            ViewData["TypeScriptOutput"] = TypeScript.RunCode(submission.Code, HttpContext.Session.GetInt32("uId"));
            return View("TypeScriptView", submission);
            
        }

        public ActionResult RunCSharp(Submission submission)
        {
            SSHConnect CSharp = new SSHConnect("Markus", "Plainsmuchj0urney", "51.140.218.174", dbContext);
            ViewData["CSharpOutput"] = CSharp.RunCode(submission.Code, HttpContext.Session.GetInt32("uId"));
            return View("TestView", submission);
        }
        public ActionResult Neste()
        {
            return View("TestView");
        }

        public ActionResult HTMLCSS()
        {
            return View("TestViewHTMLCSS");
        }

        public ActionResult TypeScript()
        {
            return View("TypeScriptView");
        }

        public ActionResult Avbryt()
        {
            HttpContext.Session.SetInt32(LoggedIn, FALSE);
            Debug.WriteLine("Logger ut...");
            return RedirectToAction("Index", "Login");
        }

        public readonly string LoggedIn = "login_key";
        public readonly int TRUE = 1;
        public readonly int FALSE = 0;

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