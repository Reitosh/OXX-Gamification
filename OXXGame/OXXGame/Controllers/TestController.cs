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
        public readonly int MAX_TASK_COUNT = 3;

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

        public ActionResult TestView(SingleTestResult singleTest)
        {
            if (loggedIn())
            {
                DB db = new DB(dbContext);
                string category = getTestCategory(singleTest.testId,db);

                if (category != null)
                {
                    if (updateTestValues(category, singleTest.passed))
                    {
                        db.addSingleResult(singleTest);
                        return View();
                    }
                    else
                    {
                        Debug.WriteLine("Error: could not update session variables; Session variables were not defined.");
                    }
                }
                else
                {
                    Debug.WriteLine("Error: Could not identify category.");
                }
            }
            else
            {
                Debug.WriteLine("Ikke logget inn");
            }

            return RedirectToAction("Index", "Login");
        }

        private string getTestCategory(int testId, DB database)
        {
            List<Models.Task> tasks = database.allTasks();

            foreach (Models.Task task in tasks)
            {
                if (testId == task.testId)
                {
                    return task.category;
                }
            }

            // Dette tilfellet bør ikke forekomme, ettersom databasen vil sørge for at kategorier som ikke
            // ligger i Categories-tabellen heller ikke vil kunne brukes som kategori til en oppgave.
            return null;
        }

        public ActionResult StartTest()
        {
            if (loggedIn())
            {
                if (setStartTestValues())
                {
                    try
                    {
                        ViewData["Task"] = getTask();
                    }
                    catch (Exception e)
                    {
                        Debug.WriteLine(e.Message);
                        return RedirectToAction("Index", "Login");
                    }

                    return View("TestView");
                }
            }

            return RedirectToAction("Index", "Login");
        }

        public ActionResult KjorKode(SingleTestResult singleTest)
        {
            SSHConnect ssh = new SSHConnect("Markus", "Plainsmuchj0urney", "51.140.218.174");
            ssh.ConnectToVM(singleTest.Code);

            return RedirectToAction("TestView");
        }

        public ActionResult Avbryt()
        {
            HttpContext.Session.SetInt32(LoggedIn, FALSE);
            Debug.WriteLine("Logger ut...");
            return RedirectToAction("Index", "Login");
        }

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
                testResults = db.allSingleTestResults((int)uid); // alle tidligere testresultater til sammenligning med nye oppgaver
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

        // Hjelpemetode som genererer tilfeldig tall til oppgaveutvelgelse
        private int getRandomNum(int max)
        {
            Random randGen = new Random();
            return randGen.Next(0, max);
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
        private bool updateTestValues(string category,bool passed)
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
    }
}