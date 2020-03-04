using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OXXGame.Models;
using System.Diagnostics;

namespace OXXGame
{
    public class DB
    {
        private OXXGameDBContext db;

        public DB(OXXGameDBContext db)
        {
            this.db = db;
        }

        /* ------------------------- Add metoder ------------------------- */
        public User addUser(User user)
        {
            var userRow = new Users()
            {
                Password = user.pwdHash,
                LoginCounter = 1,
                Firstname = user.firstname,
                Lastname = user.lastname,
                Email = user.email,
                IsAdmin = false,
                KnowHtml = user.knowHtml,
                KnowCss = user.knowCss,
                KnowJavascript = user.knowJavascript,
                KnowCsharp = user.knowCsharp,
                KnowMvc = user.knowMvc,
                KnowNetframework = user.knowNetframework,
                KnowTypescript = user.knowTypescript,
                KnowVue = user.knowVue,
                KnowReact = user.knowReact,
                KnowAngular = user.knowAngular
            };
            
            try
            {
                db.Add(userRow);
                db.SaveChanges();
                Debug.WriteLine("Try'en kjørte..");
                return user;
            } 
            catch (Exception e)
            {
                Debug.WriteLine(e.StackTrace);
                Debug.WriteLine(e.Message);
                Debug.WriteLine("Denne gikk til helvete..");
                return null;
            }
        }

        public bool addTask(Models.Task task)
        {
            var taskRow = new Tasks()
            {
                id = task.testId,
                Test = task.test,
                Difficulty = task.difficulty,
                Category = task.category
            };
            
            try
            {
                db.Add(taskRow);
                db.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.StackTrace);
                Debug.WriteLine(e.Message);
                return false;
            }
        }

        public bool addSingleResult(SingleTestResult result)
        {
            var singleResultRow = new SingleTestResults()
            {
                Passed = result.passed,
                Attempts = result.tries,
                TimeUsed = result.timeSpent,
                UserId = result.userId,
                TestId = result.testId,
                Submitted = result.submitted
            };
            
            try
            {
                db.Add(singleResultRow);
                db.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.StackTrace);
                Debug.WriteLine(e.Message);
                return false;
            }
        }

        public bool addTotResult(Result result)
        {
            var resultRow = new Results()
            {
                UserId = result.userId,
                TimeUsed = result.timeUsed,
                TestsPassed = result.testsPassed,
                TestsFailed = result.testsFailed,
                Tests = result.tests
            };
            
            try
            {
                db.Add(resultRow);
                db.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.StackTrace);
                Debug.WriteLine(e.Message);
                return false;
            }
            
        }

        /* ------------------------- List metoder ------------------------- */
        public List<User> allUsers()
        {
            List<User> users = db.Users.Select(user => getUserData(user)).ToList();
            return users;
        }

        public List<Models.Task> allTasks()
        {
            List<Models.Task> tasks = db.Tasks.Select(task => new Models.Task
            {
                testId = task.id,
                test = task.Test,
                difficulty = task.Difficulty,
                category = task.Category
            }).ToList();

                return tasks;
        }

        public List<Result> allResults()
        {
            List<Result> results = db.Results.Select(result => getResultData(result)).ToList();
            return results;
        }

        // Henter ut bruker ved brukernavn. Metoden returnerer null hvis man ikke finner nøyaktig 1 bruker.
        // Denne er i utgangspunktet ment til innloggingsvalidering. 
        public User getUser(string uname)
        {
            try
            {
                Users user = db.Users.SingleOrDefault(u => u.Email == uname);
                User validUser = getUserData(user);
                
                return validUser;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.StackTrace);
                Debug.WriteLine(e.Message);
                return null;
            }
        }

        // Henter totalresultat ved bruker-id. Har ikke brukeren noen oppføring i resultattabellen (bruker har ikke utført test) returneres null
        public Result resultTot(int uid)
        {
            try
            {
                Results resultRow = db.Results.SingleOrDefault(r => r.UserId == uid);
                Result result = getResultData(resultRow);

                return result;
            }
            catch (InvalidOperationException e)
            {
                Debug.WriteLine(e.StackTrace);
                Debug.WriteLine(e.Message);
                return null;
            }
        }

        // Henter resultater fra alle enkelt-tasks brukeren har utført ved bruker-id.
        // Har ikke brukeren noen oppføring i singelresultat-tabellen (bruker har ikke utført noen tasks) returneres en tom liste
        public List<SingleTestResult> allSingleTestResults(int uId)
        {
            List<SingleTestResults> results = db.SingleTestResults.Where(r => r.UserId == uId).ToList();
            List<SingleTestResult> result = new List<SingleTestResult>();
            foreach (SingleTestResults res in results)
            {
                result.Add(getSResultData(res));
            }
            
            return result;
        }

        public List<SingleTestResult> allSingleTestResults()
        {
            List<SingleTestResult> results = db.SingleTestResults.Select(r => getSResultData(r)).ToList();
            return results;
        }

        /* ------------------------- Update metoder ------------------------- */
        public bool updateTask(int taskId, Models.Task uTask)
        {
            var task = db.Tasks.SingleOrDefault(t => t.id == taskId);
            
            if (task != null)
            {
                task.Test = uTask.test;
                task.Difficulty = uTask.difficulty;
                task.Category = uTask.category;
            } 
                
            else
            {
                return false;
            }
                
            try
            {
                db.Tasks.Update(task);
                db.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.StackTrace);
                Debug.WriteLine(e.Message);
                return false;
            }
        }

        public bool updateSingleTestResult(int uId, int tId, SingleTestResult uSingleResult)
        {
            var singleResult = db.SingleTestResults.SingleOrDefault(t => (t.UserId == uId && t.TestId == tId));

            if (singleResult != null)
            {
                singleResult.Passed = uSingleResult.passed;
                singleResult.Attempts = uSingleResult.tries;
                singleResult.TimeUsed += uSingleResult.timeSpent;
                singleResult.Submitted = uSingleResult.submitted;
            }
            else
            {
                return false;
            }
            
            try
            {
                    db.SingleTestResults.Update(singleResult);
                    db.SaveChanges();
                    return true;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.StackTrace);
                Debug.WriteLine(e.Message);
                return false;
            }
        }

        /* -------------------------------------------------------------------------------------------------------------------------------- */
        // Hjelpemetode for opprettelse av ny brukermodellobjekt basert på tabellrad (Users)
        private static User getUserData(Users user)
        {
            return new User()
            {
                userId = user.id,
                pwdHash = user.Password,
                loginCounter = user.LoginCounter,
                firstname = user.Firstname,
                lastname = user.Lastname,
                email = user.Email,
                isAdmin = user.IsAdmin,
                knowHtml = user.KnowHtml,
                knowCss = user.KnowCss,
                knowJavascript = user.KnowJavascript,
                knowCsharp = user.KnowCsharp,
                knowMvc = user.KnowMvc,
                knowNetframework = user.KnowNetframework,
                knowTypescript = user.KnowTypescript,
                knowVue = user.KnowVue,
                knowReact = user.KnowReact,
                knowAngular = user.KnowAngular
            };
        }

        private static Result getResultData(Results result)
        {
            return new Result()
            {
                userId = result.UserId,
                timeUsed = result.TimeUsed,
                testsPassed = result.TestsPassed,
                testsFailed = result.TestsFailed,
                tests = result.Tests
            };
        }

        private static SingleTestResult getSResultData(SingleTestResults sResult)
        {
            return new SingleTestResult()
            {
                passed = sResult.Passed,
                tries = sResult.Attempts,
                timeSpent = sResult.TimeUsed,
                userId = sResult.UserId,
                testId = sResult.TestId,
                submitted = sResult.Submitted
            };
        }

         /*----------------------------------------------------- Slett metoder -------------------------------------------------------------*/

        public bool deleteUser(int userId)
        {
            try
            {
                var deleteUsr = db.Users.Find(userId);
                db.Users.Remove(deleteUsr);
                db.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.StackTrace);
                Debug.WriteLine("Detta gikk dårlig");
                return false; 
            }
        }



        // Metode til bruk for testing (Andreas).
        public bool checkIfExist(string uname)
        {

            bool exist;

            try
            {
                Users user = db.Users.SingleOrDefault(u => u.Email == uname);
                exist = true;
                return exist;
            }
            catch (Exception e)
            {
                exist = false;
                return exist;
            }
        }
    }
}