using System;
using System.Collections.Generic;
using System.Linq;
using OXXGame.Models;
using System.Diagnostics;
using Microsoft.EntityFrameworkCore.ChangeTracking;

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
      
        public Users addUser(User user)
        {
            var userRow = new Users()
            {
                Password = user.pwdHash,
                LoginCounter = 1,
                Firstname = user.firstname,
                Lastname = user.lastname,
                Email = user.email,
                Tlf = user.tlf,
                IsAdmin = false,
            };
            
            try
            {
                EntityEntry entity = db.Add(userRow);
                Users newUser = (Users) entity.Entity;
                db.SaveChanges();
                return newUser;
            } 
            catch (Exception e)
            {
                Debug.WriteLine(e.StackTrace);
                Debug.WriteLine(e.Message);
                return null;
            }
        }

        public bool addResultPerCategory(List<ResultPerCategory> resPerCategory)
        {
            try
            {
                foreach (ResultPerCategory res in resPerCategory)
                {
                    ResultsPerCategory resultPerCategoryRow = new ResultsPerCategory()
                    {
                        UserId = res.userId,
                        Category = res.category,
                        Lvl = res.lvl,
                        Counter = 0
                    };

                    db.Add(resultPerCategoryRow);
                }

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

        public bool addCategory(Category category)
        {
            var categoryRow = new Categories()
            {
                Category = category.category
            };

            try
            {
                db.Add(categoryRow);
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

        public bool addTask(Models.Task task)
        {
            var taskRow = new Tasks()
            {
                id = task.testId,
                Test = task.test,
                Difficulty = task.difficulty,
                Category = task.category,
                Template = task.template
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
                CodeLink = result.codeLink
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

        public List<ResultPerCategory> allResultsPerCategory(int uId)
        {
            List<ResultsPerCategory> resPerCategory = db.ResultsPerCategory.Where(res => res.UserId == uId).ToList();
            List<ResultPerCategory> results = new List<ResultPerCategory>();

            foreach (ResultsPerCategory res in resPerCategory)
            {
                results.Add(new ResultPerCategory()
                {
                    userId = res.UserId,
                    category = res.Category,
                    lvl = res.Lvl,
                    counter = res.Counter
                });
            }

            return results;
        }

        public List<Category> allCategories()
        {
            List<Category> categories = db.Categories.Select(cat => new Category
            {
                category = cat.Category
            }).ToList();

            return categories;
        }

        public List<Models.Task> allTasks()
        {
            List<Models.Task> tasks = db.Tasks.Select(task => new Models.Task
            {
                testId = task.id,
                test = task.Test,
                difficulty = task.Difficulty,
                category = task.Category,
                template = task.Template
            }).ToList();

            return tasks;
        }

        public List<Models.Task> getTasks(string category, int difficulty)
        {
            List<Tasks> tasksPerCategory = db.Tasks.Where(
                task => task.Category == category && task.Difficulty == difficulty).ToList();

            List<Models.Task> tasks = new List<Models.Task>();

            foreach (Tasks task in tasksPerCategory)
            {
                tasks.Add(new Models.Task()
                {
                    testId = task.id,
                    test = task.Test,
                    difficulty = task.Difficulty,
                    category = task.Category,
                    template = task.Template
                });
            }

            return tasks;
        }

        public Models.Task getSingleTask(int testId)
        {
            var aTask = db.Tasks.Find(testId);

            if (aTask == null)
            {
                return null;
            }
            else
            {
                var outTask = new Models.Task()
                {
                    testId = aTask.id,
                    test = aTask.Test,
                    difficulty = aTask.Difficulty,
                    category = aTask.Category,
                    template = aTask.Template
                };
                return outTask;
            }
        }

        public List<Result> allResults()
        {
            List<Result> results = db.Results.Select(result => getResultData(result)).ToList();
            return results;
        }

        // Henter ut bruker ved brukernavn. Metoden returnerer null hvis man ikke finner nøyaktig 1 bruker.
        // Denne er i utgangspunktet ment til innloggingsvalidering. 
        public User getUser(string email)
        {
            try
            {
                Users user = db.Users.SingleOrDefault(u => u.Email == email);
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

        public User getUser(int uId)
        {
            try
            {
                Users user = db.Users.SingleOrDefault(u => u.id == uId);
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
            // Metode i TestController sender inn uid=-1 hvis ikke uid av en eller annen grunn ikke skulle finnes.
            // Dette tilfellet bør behandles her(?) på et eller annet vis.
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
        public List<SingleTestResult> getSingleTestResults(int uId)
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
        /*
        public bool updateSingleTestResult2(int userId, int taskId, SingleTestResult inSingleTestResult)
        {
            try
            {
                SingleTestResults result = db.SingleTestResults.Find(userId, taskId);

            }
            catch (Exception e)
            {
                Debug.WriteLine(e.StackTrace);
                Debug.WriteLine(e.Message);
                return false;
            }
        }
        */
        public bool updateSingleTestResult(int uId, int tId, SingleTestResult uSingleResult)
        {
            var singleResult = db.SingleTestResults.SingleOrDefault(t => (t.UserId == uId && t.TestId == tId));

            if (singleResult != null)
            {
                singleResult.Passed = uSingleResult.passed;
                singleResult.Attempts = uSingleResult.tries;
                singleResult.TimeUsed = uSingleResult.timeSpent;
                //singleResult.Submitted = uSingleResult.submitted;
            }
            else
            {
                return false;
            }
            
            try
            {
                    //db.SingleTestResults.Update(singleResult);
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

        public bool updateResultsPerCategory(List<ResultPerCategory> resPerCat)
        {
            try
            {
                foreach (ResultPerCategory updateResult in resPerCat)
                {
                    ResultsPerCategory result = db.ResultsPerCategory.Find(updateResult.userId, updateResult.category);
                    result.Lvl = updateResult.lvl;
                    result.Counter = updateResult.counter;

                    /*
                    db.Update(new ResultsPerCategory()
                    {
                        UserId = result.userId,
                        Category = result.category,
                        Lvl = result.lvl,
                        Counter = result.counter
                    });
                    */
                }

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

        public bool editTask(int testId, Models.Task inTask)
        {
            try
            {
                var editTsk = db.Tasks.Find(testId);
                Debug.WriteLine("Test id er funnet og vi endrer de andre verdiene");
                editTsk.Test = inTask.test;
                editTsk.Difficulty = inTask.difficulty;
                editTsk.Category = inTask.category;
                editTsk.Template = inTask.template;

                db.SaveChanges();
                return true;
            }
            catch
            {
                Debug.WriteLine("Edit task failed.");
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
                tlf = user.Tlf,
                isAdmin = user.IsAdmin,
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
                codeLink = sResult.CodeLink
            };
        }

         /*----------------------------------------------------- Slett metoder -------------------------------------------------------------*/

        public bool deleteUser(int deleteId)
        {
            try
            {
                Users deleteUsr = db.Users.Find(deleteId);
                deleteSingleTestResult(deleteId);
                deleteResPerCategory(deleteId);
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

        public bool deleteTask(int deleteId)
        {
            try
            {
                Tasks deleteTsk = db.Tasks.Find(deleteId);
                db.Tasks.Remove(deleteTsk);
                db.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.StackTrace);
                Debug.WriteLine("Detta gikk dårlig");
                Debug.WriteLine(e.Message);
                return false;
            }
        }

        public bool deleteResPerCategory(int deleteId)
        {
            try
            {
                List<ResultsPerCategory> resPerCat = db.ResultsPerCategory.Where(res => res.UserId == deleteId).ToList();
                db.ResultsPerCategory.RemoveRange(resPerCat);
                return true;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.StackTrace);
                Debug.WriteLine(e.Message);
                return false;
            }
        }

        public bool deleteSingleTestResult(int deleteId)
        {
            try
            {
                List<SingleTestResults> results = db.SingleTestResults.Where(r => r.UserId == deleteId).ToList();
                db.SingleTestResults.RemoveRange(results);
                return true;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.StackTrace);
                Debug.WriteLine(e.Message);
                return false;
            }
        }
    }
}