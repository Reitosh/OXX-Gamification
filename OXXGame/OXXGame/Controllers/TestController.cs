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

        public readonly string userId = "uId_key";
        public readonly string LoggedIn = "login_key";
        public readonly int TRUE = 1;
        public readonly int FALSE = 0;

        public readonly int MAX_LVL = 2;
        public readonly int MIN_LVL = 0;
        public readonly int MAX_TASK_COUNT = 1;

        public TestController(OXXGameDBContext context)
        {
            dbContext = context;
        }

        //-------------------------------------------------- ActionResults --------------------------------------------------//

        public ActionResult TestInfo(object p)
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
                if (setStartTestValues())
                {
                    TestModel model = getModel();

                    if (model == null)
                    {
                        return RedirectToAction("Index", "Login");
                    }

                    return View("TestView",model);
                }
            }

            return RedirectToAction("Index", "Login");
        }

        // Setter her passed-variabel i testModel.singleTestResult KUN basert på om koden kompilerer.
        // Hvis koden kompilerer forblir resultatet UNDEFINED
        public ActionResult KjorKode(TestModel testModel)
        {
            SSHConnect ssh = new SSHConnect("Markus", "Plainsmuchj0urney", "51.140.218.174");
            string output = ssh.RunCode(testModel);
            
            testModel.singleTestResult.tries++;
            if (output.Contains("Compilation failed:"))
            {
                testModel.singleTestResult.passed = SingleTestResult.NOT_PASSED;
            }

            ViewData["Output"] = output;
            return View("TestView",testModel);
        }

        public ActionResult Neste(TestModel inModel)
        {
            if (loggedIn())
            {
                if (updateTestValues(inModel.task.category, 
                    ! inModel.singleTestResult.passed.Equals(SingleTestResult.NOT_PASSED)))
                {
                    DB db = new DB(dbContext);

                    inModel.endTime = DateTime.Now;
                    inModel.singleTestResult.timeSpent = (inModel.endTime - inModel.startTime).ToString(@"hh\:mm\:ss");

                    if (db.addSingleResult(inModel.singleTestResult))
                    {
                        saveResultsPerCategory();
                        ModelState.Clear();
                    }

                    TestModel model = getModel();

                    if (model == null)
                    {
                        return RedirectToAction("Index", "Login");
                    }

                    return View("TestView", model);
                }

                return View("TestView", inModel);
            }

            return RedirectToAction("Index", "Login");
        }
        /*
        public ActionResult RunTypeScript(Submission submission)
        {

            SSHConnect TypeScript = new SSHConnect("Markus", "Plainsmuchj0urney", "51.140.218.174");
            ViewData["TypeScriptOutput"] = TypeScript.RunCode(submission.Code, HttpContext.Session.GetInt32("uId"));
            return View("TypeScriptView", submission);
            
        }

        public ActionResult RunCSharp(Submission submission)
        {
            SSHConnect CSharp = new SSHConnect("Markus", "Plainsmuchj0urney", "51.140.218.174");
            ViewData["CSharpOutput"] = CSharp.RunCode(submission.Code, HttpContext.Session.GetInt32("uId"));
            return View("TestView", submission);

        }
        */
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

        //-------------------------------------------------- Andre metoder --------------------------------------------------//

        // Metoden henter ut en "tilfeldig" oppgave fra listen basert på noen parametre:
        //    *Kategori
        //    *Nivå (per kategori)
        //    *Oppgaver allerede utført
        //    *Antall oppgaver utført (per kategori)
        //
        // Det kastes Exception dersom:
        //    *uid ikke er satt
        //    *kategorinivå eller oppgavetelleren ikke er satt
        //
        // Det returneres et Models.Task objekt dersom en passende oppgave finnes, hvis ikke returneres null.
        private Models.Task getTask()
        {
            DB db = new DB(dbContext);
            List<SingleTestResult> testResults;
            int? uid = HttpContext.Session.GetInt32(userId);

            if (uid != null)
            {
                testResults = db.getSingleTestResults((int)uid); // alle tidligere testresultater til sammenligning med nye oppgaver
            }
            else
            {
                throw new Exception("Session variable [" + userId + "] is not set.");
            }

            List<Category> categories = db.allCategories();
            foreach (Category category in categories)
            {
                int? taskCount = HttpContext.Session.GetInt32(category.category + "_count_key");
                int? categoryLvl = HttpContext.Session.GetInt32(category.category + "_lvl_key");

                if (taskCount != null && categoryLvl != null) // sjekker om session-variablene er satt (hvis ikke har du focket opp man)
                {
                    if (taskCount < MAX_TASK_COUNT) // sjekker om brukeren har igjen oppgaver i den gitte kategorien
                    {
                        List<Models.Task> tasks = db.getTasks(category.category, (int)categoryLvl);

                        while (tasks.Count > 0)
                        {
                            Models.Task task = tasks[getRandomNum(tasks.Count)];
                            if (isNewTask(task, testResults)) // sjekker om ny oppgave allerede er utført
                            {
                                return task;
                            }
                            else
                            {
                                tasks.Remove(task);
                            }
                        }
                    }
                }
                else
                {
                    throw new Exception("Session variable(s) [" + category.category + "_count_key" 
                        + "] and/or [" + category.category + "_lvl_key" + "] are/is not set.");
                }
            }

            // Hvis det skulle oppstå problemer med uthenting fra databasen vil denne linjen nås. 
            // Løs dette med en if der denne metoden kalles hvor antall oppgaver utført sjekkes. 
            // Er antall oppgaver == max oppgaver -> good, testen er ferdig.
            return null;
        }

        // Setter startverdier for sessionvariabler for nivå- og antall oppgaver per kategori.
        // Henter ut resultater fra databasen (har kandidaten ikke tatt noen tester ennå, vil verdiene
        // brukeren satt 
        private bool setStartTestValues()
        {
            DB db = new DB(dbContext);
            int? uid = HttpContext.Session.GetInt32(userId);

            if (uid != null)
            {
                List<ResultPerCategory> resPerCategory = db.allResultsPerCategory((int)uid);
                foreach (ResultPerCategory result in resPerCategory)
                {
                    HttpContext.Session.SetInt32(result.category + "_lvl_key", result.lvl);
                    HttpContext.Session.SetInt32(result.category + "_count_key", result.counter);
                }

                return true;
            }
            else
            {
                return false;
            }
        }

        // Metode som oppdaterer session-variabler for nivå- og antall oppgaver per kategori.
        // Metoden tar inn den aktuelle kategorien og en bool som representerer om kandidaten har 
        // bestått den aktuelle oppgaven, og oppdaterer variablene deretter.
        // Metoden returnerer false dersom variablene ikke er satt ennå.
        private bool updateTestValues(string category, bool passed)
        {
            int? lvl = HttpContext.Session.GetInt32(category + "_lvl_key");
            int? count = HttpContext.Session.GetInt32(category + "_count_key");

            if (lvl != null && count != null)
            {
                count++;

                if (passed && lvl < MAX_LVL)
                {
                    lvl++;
                }
                else if (!passed && lvl > MIN_LVL)
                {
                    lvl--;
                }

                HttpContext.Session.SetInt32(category + "_lvl_key", (int)lvl);
                HttpContext.Session.SetInt32(category + "_count_key", (int)count);

                return true;
            }

            return false;
        }

        // Metode som looper over kategorier og tar inn beregnede session-variabler og lagrer de til database.
        // Denne metoden bør også kunne brukes for å oppdatere gamle brukere etter eventuell opprettelse av ny kategori.
        private bool saveResultsPerCategory()
        {
            int? uid = HttpContext.Session.GetInt32(userId);

            if (uid != null)
            {
                DB db = new DB(dbContext);
                List<Category> categories = db.allCategories();
                List<ResultPerCategory> resultsPerCategory = db.allResultsPerCategory((int)uid);
                bool existingResults = resultsPerCategory.Count != 0;

                if (existingResults)
                {
                    resultsPerCategory = new List<ResultPerCategory>();
                }

                foreach (Category category in categories)
                {
                    resultsPerCategory.Add(new ResultPerCategory()
                    {
                        userId = (int)uid,
                        category = category.category,
                        lvl = HttpContext.Session.GetInt32(category.category + "_lvl_key") ?? 0,
                        counter = HttpContext.Session.GetInt32(category.category + "_count_key") ?? 0
                    });
                }

                if (existingResults)
                {
                    if (db.updateResultsPerCategory(resultsPerCategory))
                    {
                        return true;
                    }
                }
                else
                {
                    if (db.addResultPerCategory(resultsPerCategory))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        // Metode som returnerer et TestModel-objekt klargjort med startverdier og ny oppgave
        // Returnerer null dersom getTask() enten returner null eller kaster unntak
        private TestModel getModel()
        {
            TestModel model = new TestModel();

            try
            {
                model.task = getTask();
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                return null;
            }

            if (model.task != null)
            {
                model.singleTestResult = new SingleTestResult()
                {
                    userId = (int)HttpContext.Session.GetInt32(userId),
                    testId = model.task.testId,
                    tries = 0,
                    passed = SingleTestResult.UNDEFINED
                };

                model.startTime = DateTime.Now;
                return model;
            }

            return null;
        }

        // Hjelpemetode som genererer tilfeldig tall til oppgaveutvelgelse
        private int getRandomNum(int max)
        {
            Random randGen = new Random();
            return randGen.Next(0, max);
        }

        // Hjelpemetode som sjekker at oppgaven ikke allerede er løst
        private bool isNewTask(Models.Task task, List<SingleTestResult> list)
        {
            foreach (SingleTestResult doneTask in list)
            {
                if (task.testId == doneTask.testId)
                {
                    return false;
                }
            }
            return true;
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