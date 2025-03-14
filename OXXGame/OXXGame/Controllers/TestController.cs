﻿using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Microsoft.AspNetCore.Http;
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

        // Constructor; mottar DBContext gjennom dependency injection
        public TestController(OXXGameDBContext context)
        {
            dbContext = context;
        }

        //-------------------------------------------------- ActionResults --------------------------------------------------//

        // Entry point for TestController; returnerer TestInfo-siden
        public ActionResult TestInfo(object p)
        {
            if (loggedIn())
            {
                // ViewData som er tildelt verdien av session-variabelen fra LoginController 
                ViewData["username"] = HttpContext.Session.GetString("username");
                return View();
            }
            else
            {
                Debug.WriteLine("Ikke logget inn");
                return RedirectToAction("Index", "Login");
            }
        }

        // Starter testen; henter populert TestModel og omdirigerer til ActionResult som velger view (basert på kategori)
        public ActionResult StartTest()
        {
            if (loggedIn())
            {
                if (setStartTestValues())
                {
                    TestModel model = getModel();

                    if (model == null)
                    {
                        return View("TestEndView");
                    }

                    return DecideView(model);
                }
            }

            return RedirectToAction("Index", "Login");
        }

        // Mottar TestModel inkludert brukerinput, kjører/tester inputkode og returnerer view.
        // runCode og submitCode representerer submit-knappene i test-skjema
        // Knappen som trykkes på vil passe sin verdi, mens den andre er null
        // Returns:
        //      runCode != null:    Forrige TestView returneres med output fra VM
        //      submitCode != null: Besvarelse lagres og brukeren sendes videre til neste oppgave
        //      default:            Omdirigeres til login. Dette vil ikke være et valg for brukeren, og burde ikke forekomme.
        [HttpPost]
        public ActionResult SubmitCode(TestModel testModel, string runCode, string submitCode)
        {
            RunCode(testModel);
            ModelState.Clear();

            if (!string.IsNullOrEmpty(runCode))
            {
                return DecideView(testModel);
            }
            else if (!string.IsNullOrEmpty(submitCode))
            {
                return Neste(testModel);
            }
            else
            {
                return RedirectToAction("Index", "Login");
            }
        }

        // Lagrer besvarelse og laster inn ny oppgave/avslutter testen.
        // Returns:
        //      getModel() == null -> getTask() == null -> ingen oppgaver igjen: returnerer TestEndView
        //      Submit(testModel) == true && getTask() != null:                  returnerer neste TestView
        //      Submit(testModel) == false -> kunne ikke lagre besvarelse:       returnerer forrige TestView
        //      loggedIn() == false:                                             omdirigerer til login
        [HttpGet]
        public ActionResult Neste(TestModel testModel)
        {
            if (loggedIn())
            {
                if (Submit(testModel))
                {
                    ModelState.Clear();
                    ViewData["Output"] = null;
                    TestModel newModel = getModel();

                    // Dette burde tilsi at testen er ferdig
                    // Kan også forårsakes av database-uthentingsfeil (aldri forekommet)
                    if (newModel == null)
                    {
                        return View("TestEndView");
                    }

                    return DecideView(newModel);
                }

                return DecideView(testModel);
            }

            return RedirectToAction("Index", "Login");
        }

        // Returnerer riktig TestView basert på oppgavekategori
        [HttpGet]
        public ActionResult DecideView(TestModel dModel) 
        {
            if (dModel.task.category == "HTML" || dModel.task.category == "CSS" || dModel.task.category == "JavaScript" || 
                dModel.task.category == "Vue.js" || dModel.task.category == "React")
                return View("TestViewHTMLCSS", dModel);
            else if (dModel.task.category == "TypeScript")
                return View("TypeScriptView", dModel);
            else
                return View("TestView", dModel);
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
                            Models.Task task = tasks[getRandomNum(tasks.Count - 1)];
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
                return null; // Dette vil avslutte testen, burde ikke forekomme, men kan med fordel løses på en annen måte
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

                model.code = model.task.template;
                model.startTime = DateTime.Now;
                return model;
            }

            return null; // Skulle det være problemer med uthenting av data fra database vil testen avsluttes 
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

        // Metode som sjekker om en bruker er logget inn
        // Metoden kalles i ActionResults for å sørge for at kun innloggede brukere kan aksessere testen
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

        // Oppretter SSH-tilkobling til VM, sender gjennom testbesvarelsen til kompilering/testing og henter output
        // Besvarelse vil vurderes fra output fra VM:
        // Besvarelse underkjennes ved feilmelding fra (en av følgende): C#-kompilator, C#-test, TS-kompilator
        // C#-oppgaver vil godkjennes dersom testen kjører feilfritt
        public void RunCode(TestModel testModel)
        {
            if (testModel.code == null)
            {
                testModel.singleTestResult.passed = SingleTestResult.NOT_PASSED;
                return;
            }
            
            SSHConnect ssh = new SSHConnect("Markus", "Plainsmuchj0urney", "51.140.218.174");
            string output = ssh.RunCode(testModel);

            testModel.singleTestResult.tries++;
            if (output.Contains("Compilation failed:") || output.Contains("error TS") || output.Contains("Not passed"))
            {
                testModel.singleTestResult.passed = SingleTestResult.NOT_PASSED;
            }
            else if (!output.Contains("Not passed") && testModel.task.category.Equals("C#"))
            {
                testModel.singleTestResult.passed = SingleTestResult.PASSED;
            }
            else 
            {
                testModel.singleTestResult.passed = SingleTestResult.UNDEFINED;
            }

            ViewData["Output"] = output;
        }

        // Oppdaterer progresjonsdata og utfører all nødvendig lagring av testdata:
        //      -Lagrer besvarelsesfil til server
        //      -Lagrer SingleTestResult til database
        // Metoden vil også ta timestamp for innleveringstidspunkt og beregne tid brukt på oppgaven, samt
        // opprette filnavn basert på bruker- og test-id
        private bool Submit(TestModel testModel)
        {
            if (updateTestValues(testModel.task.category,
                    !testModel.singleTestResult.passed.Equals(SingleTestResult.NOT_PASSED)))
            {
                DB db = new DB(dbContext);

                testModel.endTime = DateTime.Now;
                testModel.singleTestResult.timeSpent = (testModel.endTime - testModel.startTime).ToString(@"hh\:mm\:ss");


                FileHandler fileHandler = new FileHandler();

                string relativePath = string.Format("/{0}", HttpContext.Session.GetInt32(userId));
                string fileName;

                fileName = string.Format(
                "{0}_Ex{1}",
                testModel.task.category,
                testModel.task.testId
                );

                if (fileName.StartsWith(".")) { fileName = fileName.Replace(".", "dot"); }

                List<string> code = FileHandler.stringToList("/*" + testModel.task.test + "*/\n" + testModel.code);

                testModel.singleTestResult.codeLink = fileHandler.saveFile(
                    relativePath, 
                    fileName, 
                    code,
                    FileHandler.getFileExtension(testModel.task.category));

                if (db.addSingleResult(testModel.singleTestResult))
                {
                    saveResultsPerCategory();
                }

                return true;
            }

            return false;
        }
    }
}