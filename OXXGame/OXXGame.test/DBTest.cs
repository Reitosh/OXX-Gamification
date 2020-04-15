using System;
using Xunit;
using OXXGame.Controllers;
using OXXGame.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Collections.Generic;
using System.Diagnostics;

namespace OXXGame.test
{
    public class DBTest
    {
        // --------------------------------------------------- Add metoder --------------------------------------------------- //

        [Fact]
        public void Add_User_test()
        {
            // dbName representerer navnet på gjeldende InMemoryDatabase (getOptions() setter navnet, det kan være hva som helst ...)
            string dbName = "Add_user_test";
            DbContextOptions<OXXGameDBContext> options = getOptions(dbName);

            // Setter testdata. Disse brukes i både test og verifisering. Skal verdiene endres, endres de her.
            // (For å endre bools må verdien endres i brukeropprettelsen og verifiseringsmetoden må byttes)
            int id = 1;
            byte[] hash = User.createHash("pwd");
            string fName = "Markus";
            string lName = "Reitan";
            string email = "markus@reitan.no";

            // Kjører test (legger til brukerdata) mot en instans av context
            using (OXXGameDBContext context = new OXXGameDBContext(options))
            {
                DB db = new DB(context);
                db.addUser(new User()
                {
                    userId = id,
                    pwdHash = hash,
                    firstname = fName,
                    lastname = lName,
                    email = email,
                    isAdmin = false
                });

                context.SaveChanges();
            }

            // Oppretter et nytt rent context-objekt og verifiserer testen
            using (OXXGameDBContext context = new OXXGameDBContext(options))
            {
                Assert.Equal(1, context.Users.Count());
                Assert.Equal(fName, context.Users.Single().Firstname);
                Assert.Equal(lName, context.Users.Single().Lastname);
                Assert.Equal(email, context.Users.Single().Email);
                Assert.Equal(hash, context.Users.Single().Password);
                Assert.False(context.Users.Single().IsAdmin);
            }
        }

        [Fact]
        public void Add_ResultPerCategory_test()
        {
            // Setter databasenavn og -options
            string dbName = "Add_ResultPerCategory_test";
            DbContextOptions<OXXGameDBContext> options = getOptions(dbName);

            // Setter testdata
            int id = 1;
            string category = "C#";
            int lvl = 1;
            int count = 0;

            List<ResultPerCategory> list = new List<ResultPerCategory>();
            list.Add(new ResultPerCategory() {
                userId = id,
                category = category,
                lvl = lvl,
                counter = count
            });

            // Kjører test
            using (OXXGameDBContext context = new OXXGameDBContext(options))
            {
                DB db = new DB(context);
                db.addResultPerCategory(list);
                context.SaveChanges();
            }

            // Verifiserer
            using (OXXGameDBContext context = new OXXGameDBContext(options))
            {
                Assert.Equal(1, context.ResultsPerCategory.Count());
                Assert.Equal(category, context.ResultsPerCategory.Single().Category);
                Assert.Equal(lvl, context.ResultsPerCategory.Single().Lvl);
                Assert.Equal(count, context.ResultsPerCategory.Single().Counter);
            }
        }

        [Fact]
        public void Add_Category_test()
        {
            // Setter databasenavn og -options
            string dbName = "add_category_test";
            DbContextOptions<OXXGameDBContext> options = getOptions(dbName);

            // Setter testdata
            string category = "C#";

            // Kjører test
            using (OXXGameDBContext context = new OXXGameDBContext(options))
            {
                DB db = new DB(context);
                db.addCategory(new Category() {
                    category = category
                });

                context.SaveChanges();
            }

            // Verifiserer
            using (OXXGameDBContext context = new OXXGameDBContext(options))
            {
                Assert.Equal(1, context.Categories.Count());
                Assert.Equal(category,context.Categories.Single().Category);
            }
        }

        [Fact]
        public void Add_Task_test()
        {
            // Setter databasenavn og -options
            string dbName = "add_task_test";
            DbContextOptions<OXXGameDBContext> options = getOptions(dbName);

            // Setter testdata
            int id = 1;
            string test = "test";
            int difficulty = 1;
            string category = "C#";

            // Kjører test
            using (OXXGameDBContext context = new OXXGameDBContext(options))
            {
                DB db = new DB(context);
                db.addTask(new Models.Task()
                {
                    testId = id,
                    test = test,
                    difficulty = difficulty,
                    category = category
                });

                context.SaveChanges();
            }

            // Verifiserer
            using (OXXGameDBContext context = new OXXGameDBContext(options))
            {
                Assert.Equal(1, context.Tasks.Count());
                Assert.Equal(id, context.Tasks.Single().id);
                Assert.Equal(test, context.Tasks.Single().Test);
                Assert.Equal(difficulty, context.Tasks.Single().Difficulty);
                Assert.Equal(category, context.Tasks.Single().Category);
            }
        }

        [Fact]
        public void Add_SingleTestResult_test()
        {
            // Setter databasenavn og -options
            string dbName = "add_singleTestResult_test";
            DbContextOptions<OXXGameDBContext> options = getOptions(dbName);

            // Setter testdata
            int uid = 1;
            int tid = 1;
            int tries = 1;
            string time = "10:40:23";

            // Kjører test
            using (OXXGameDBContext context = new OXXGameDBContext(options))
            {
                DB db = new DB(context);
                db.addSingleResult(new SingleTestResult
                {
                    userId = uid,
                    testId = tid,
                    passed = true,
                    tries = tries,
                    timeSpent = time
                });

                context.SaveChanges();
            }

            // Verifiserer
            using (OXXGameDBContext context = new OXXGameDBContext(options))
            {
                Assert.Equal(1, context.SingleTestResults.Count());
                Assert.Equal(uid, context.SingleTestResults.Single().UserId);
                Assert.Equal(tid, context.SingleTestResults.Single().TestId);
                Assert.True(context.SingleTestResults.Single().Passed);
                Assert.Equal(tries, context.SingleTestResults.Single().Attempts);
                Assert.Equal(time, context.SingleTestResults.Single().TimeUsed);
            }
        }

        [Fact]
        public void Add_Result_test()
        {
            // Setter databasenavn og -options
            string dbName = "add_result_test";
            DbContextOptions<OXXGameDBContext> options = getOptions(dbName);

            // Setter testdata
            int id = 1;
            string time = "20:23:34";
            int passed_failed = 10;
            int tests = passed_failed * 2;

            // Kjører test
            using (OXXGameDBContext context = new OXXGameDBContext(options))
            {
                DB db = new DB(context);
                db.addTotResult(new Result
                {
                    userId = id,
                    timeUsed = time,
                    testsPassed = passed_failed,
                    testsFailed = passed_failed,
                    tests = tests
                });

                context.SaveChanges();
            }

            // Verifiserer
            using (OXXGameDBContext context = new OXXGameDBContext(options))
            {
                Assert.Equal(1, context.Results.Count());
                Assert.Equal(id, context.Results.Single().UserId);
                Assert.Equal(time, context.Results.Single().TimeUsed);
                Assert.Equal(passed_failed, context.Results.Single().TestsPassed);
                Assert.Equal(passed_failed, context.Results.Single().TestsFailed);
                Assert.Equal(tests, context.Results.Single().Tests);
            }
        }

        // --------------------------------------------------- List/get metoder --------------------------------------------------- //

        [Fact]
        public void All_Users_test()
        {
            // Setter databasenavn og -options
            string dbName = "all_users_test";
            DbContextOptions<OXXGameDBContext> options = getOptions(dbName);

            // Setter antall innlegg i databasen. Denne brukes igjen i verifisering.
            int count = 10;

            // Legger testdata til database
            using (OXXGameDBContext context = new OXXGameDBContext(options))
            {
                DB db = new DB(context);

                for (int i = 1; i <= count; i++)
                {
                    db.addUser(new User()
                    {
                        userId = i
                    });
                }

                context.SaveChanges();
            }

            // Tester uthenting og verifiserer antallet
            using (OXXGameDBContext context = new OXXGameDBContext(options))
            {
                DB db = new DB(context);
                Assert.Equal(count, db.allUsers().Count());
            }
        }

        [Fact]
        public void All_ResultsPerCategory_test()
        {
            // Setter databasenavn og -options
            string dbName = "all_ResultsPerCategory_test";
            DbContextOptions<OXXGameDBContext> options = getOptions(dbName);

            // Setter testdata
            int id = 1;
            int count = 10;
            List<ResultPerCategory> list = new List<ResultPerCategory>();

            for (int i = 0; i < count; i++)
            {
                list.Add(new ResultPerCategory()
                {
                    userId = id,
                    category = i.ToString()
                });
            }

            // Legger inn testdata
            using (OXXGameDBContext context = new OXXGameDBContext(options))
            {
                DB db = new DB(context);
                db.addResultPerCategory(list);
                context.SaveChanges();
            }

            // Tester og verifiserer
            using (OXXGameDBContext context = new OXXGameDBContext(options))
            {
                DB db = new DB(context);
                Assert.Equal(count, db.allResultsPerCategory(id).Count());
            }
        }

        [Fact]
        public void All_Categories_test()
        {
            // Setter databasenavn og -options
            string dbName = "all_Categories_test";
            DbContextOptions<OXXGameDBContext> options = getOptions(dbName);

            // Setter testdata
            List<Category> categories = new List<Category>();
            categories.Add(new Category() { category = "C#" });
            categories.Add(new Category() { category = "vue.js" });
            categories.Add(new Category() { category = "JavaScript" });
            categories.Add(new Category() { category = "HTML" });
            categories.Add(new Category() { category = "CSS" });

            // Legger inn testdata
            using (OXXGameDBContext context = new OXXGameDBContext(options))
            {
                DB db = new DB(context);
                foreach (Category category in categories)
                {
                    db.addCategory(category);
                }

                context.SaveChanges();
            }

            // Tester og verifiserer
            using (OXXGameDBContext context = new OXXGameDBContext(options))
            {
                DB db = new DB(context);
                Assert.Equal(5, db.allCategories().Count());
            }
        }

        [Fact]
        public void All_Tasks_test()
        {
            // Setter databasenavn og -options
            string dbName = "all_tasks_test";
            DbContextOptions<OXXGameDBContext> options = getOptions(dbName);

            // Setter og legger inn testdata
            int count = 10;

            using (OXXGameDBContext context = new OXXGameDBContext(options))
            {
                DB db = new DB(context);
                for (int i = 1; i <= count; i++)
                {
                    db.addTask(new Models.Task()
                    {
                        testId = i
                    });
                }

                context.SaveChanges();
            }

            // Tester og verifiserer
            using (OXXGameDBContext context = new OXXGameDBContext(options))
            {
                DB db = new DB(context);
                Assert.Equal(count, db.allTasks().Count());
            }
        }

        [Fact]
        public void Get_Tasks_test()
        {
            // Setter databasenavn og -options
            string dbName = "get_tasks_test";
            DbContextOptions<OXXGameDBContext> options = getOptions(dbName);

            // Setter og legger inn testdata
            // (Her vil antall db.addTask() metodekall per runde i for-loopen angi forventet verdi for testen,
            // og difficulty kan være 0, 1 eller 2)
            int count = 3;
            string category = "C#";
            int difficulty = 1;

            using (OXXGameDBContext context = new OXXGameDBContext(options))
            {
                DB db = new DB(context);
                for (int i = 1; i <= count; i++)
                {
                    db.addTask(new Models.Task()
                    {
                        testId = i,
                        difficulty = i - 1,
                        category = category
                    });

                    db.addTask(new Models.Task()
                    {
                        testId = i + 3,
                        difficulty = i - 1,
                        category = category
                    });
                }

                context.SaveChanges();
            }

            // Tester og verifiserer
            using (OXXGameDBContext context = new OXXGameDBContext(options))
            {
                DB db = new DB(context);
                Assert.Equal(2, db.getTasks(category,difficulty).Count());
            }
        }

        [Fact]
        public void All_Results_test()
        {
            // Setter databasenavn og -options
            string dbName = "all_results_test";
            DbContextOptions<OXXGameDBContext> options = getOptions(dbName);

            // Setter og legger inn testdata
            int count = 10;

            using (OXXGameDBContext context = new OXXGameDBContext(options))
            {
                DB db = new DB(context);
                for (int i = 1; i <= count; i++)
                {
                    db.addTotResult(new Result()
                    {
                        userId = i
                    }); 
                }
            }

            // Tester og verifiserer
            using (OXXGameDBContext context = new OXXGameDBContext(options))
            {
                DB db = new DB(context);
                Assert.Equal(count, db.allResults().Count());
            }
        }

        [Fact]
        public void Get_User_test()
        {
            // Setter databasenavn og -options
            string dbName = "get_user_test";
            DbContextOptions<OXXGameDBContext> options = getOptions(dbName);

            // Setter og legger inn testdata
            int id = 1;
            string email = "markus@reitan.no";
            string fName = "Markus";
            string lName = "Reitan";
            bool isAdmin = false;

            using (OXXGameDBContext context = new OXXGameDBContext(options))
            {
                DB db = new DB(context);
                db.addUser(new User()
                {
                    userId = id,
                    email = email,
                    firstname = fName,
                    lastname = lName,
                    isAdmin = isAdmin
                });
            }

            using (OXXGameDBContext context = new OXXGameDBContext(options))
            {
                DB db = new DB(context);
                User user = db.getUser(email);

                Assert.Equal(email, user.email);
                Assert.Equal(fName, user.firstname);
                Assert.Equal(lName, user.lastname);
                Assert.False(user.isAdmin);
            }
        }

        [Fact]
        public void Get_SingleTestResults_test()
        {
            // Setter databasenavn og -options
            string dbName = "get_singleTestResults_test";
            DbContextOptions<OXXGameDBContext> options = getOptions(dbName);

            // Setter og legger inn testdata
            int id = 1;
            int count = 3;

            using (OXXGameDBContext context = new OXXGameDBContext(options))
            {
                DB db = new DB(context);
                for (int i = 1; i <= count; i++)
                {
                    db.addSingleResult(new SingleTestResult()
                    {
                        userId = id,
                        testId = i
                    });
                }

                context.SaveChanges();
            }

            // Tester og verifiserer
            using (OXXGameDBContext context = new OXXGameDBContext(options))
            {
                DB db = new DB(context);
                Assert.Equal(count, db.getSingleTestResults(id).Count());
            }
        }

        [Fact]
        public void All_SingleTestResults_test()
        {
            // Setter databasenavn og -options
            string dbName = "all_singleTestResults_test";
            DbContextOptions<OXXGameDBContext> options = getOptions(dbName);

            // Setter og legger inn testdata
            int count = 10;

            using (OXXGameDBContext context = new OXXGameDBContext(options))
            {
                DB db = new DB(context);
                for (int i = 1; i <= count; i++)
                {
                    db.addSingleResult(new SingleTestResult()
                    {
                        userId = i,
                        testId = i
                    });
                }

                context.SaveChanges();
            }

            // Tester og verifiserer
            using (OXXGameDBContext context = new OXXGameDBContext(options))
            {
                DB db = new DB(context);
                Assert.Equal(count, db.allSingleTestResults().Count());
            }
        }

        // --------------------------------------------------- Update metoder --------------------------------------------------- //

        [Fact]
        public void Update_Task_test()
        {
            // Setter databasenavn og -options
            string dbName = "update_tesk_test";
            DbContextOptions<OXXGameDBContext> options = getOptions(dbName);

            // Setter og legger inn testdataa
            int id = 1;
            string test_original = "original test";
            int difficulty_original = 0;
            string category_original = "C#";

            string test_update = "updated test";
            int difficulty_update = 1;
            string category_update = "TypeScript";

            using (OXXGameDBContext context = new OXXGameDBContext(options))
            {
                DB db = new DB(context);
                db.addTask(new Task()
                {
                    testId = id,
                    test = test_original,
                    difficulty = difficulty_original,
                    category = category_original
                });

                context.SaveChanges();
            }

            // Kjører test og verifiserer
            using (OXXGameDBContext context = new OXXGameDBContext(options))
            {
                DB db = new DB(context);
                
                // Test
                db.editTask(id, new Models.Task()
                {
                    test = test_update,
                    difficulty = difficulty_update,
                    category = category_update
                });

                context.SaveChanges();

                // Verifisring
                Assert.Equal(test_update, context.Tasks.Find(id).Test);
                Assert.Equal(difficulty_update, context.Tasks.Find(id).Difficulty);
                Assert.Equal(category_update, context.Tasks.Find(id).Category);
            }
        }

        [Fact]
        public void Update_SingleTestResult_test()
        {
            // Setter databasenavn og -options
            string dbName = "update_singleTestResult_test";
            DbContextOptions<OXXGameDBContext> options = getOptions(dbName);

            // Setter og legger inn testdata
            int id = 1;
            int tries_original = 1;
            string timeSpent_original = "00:12:23";
            // Legge inn Code_original når Code property er på plass

            int tries_update = 2;
            string timeSpent_update = "00:23:11";
            // Legge inn Code_update når Code property er på plass

            using (OXXGameDBContext context = new OXXGameDBContext(options))
            {
                DB db = new DB(context);
                db.addSingleResult(new SingleTestResult()
                {
                    userId = id,
                    testId = id,
                    tries = tries_original,
                    timeSpent = timeSpent_original,
                    passed = false
                });

                context.SaveChanges();
            }

            // Tester og verifiserer
            using (OXXGameDBContext context = new OXXGameDBContext(options))
            {
                DB db = new DB(context);
                db.updateSingleTestResult(id, id, new SingleTestResult()
                {
                    tries = tries_update,
                    timeSpent = timeSpent_update,
                    passed = true
                });

                context.SaveChanges();

                Assert.Equal(tries_update, context.SingleTestResults.Find(id,id).Attempts);
                Assert.Equal(timeSpent_update, context.SingleTestResults.Find(id,id).TimeUsed);
                Assert.True(context.SingleTestResults.Find(id,id).Passed);
            }
        }

        [Fact]
        public void Update_ResultsPerCategory_test()
        {
            // Setter databasenavn og -options
            string dbName = "update_singleTestResult_test";
            DbContextOptions<OXXGameDBContext> options = getOptions(dbName);

            // Setter og legger inn testdata
            int id = 1;
            int count = 5;
            int lvl_original = 0;
            int taskCounter_original = 0;
            List<ResultPerCategory> list_original = new List<ResultPerCategory>();

            int lvl_update = 1;
            int taskCounter_update = 1;
            List<ResultPerCategory> list_update = new List<ResultPerCategory>();

            for (int i = 0; i < count; i++)
            {
                list_original.Add(new ResultPerCategory()
                {
                    userId = id,
                    category = i.ToString(),
                    lvl = lvl_original,
                    counter = taskCounter_original
                });

                list_update.Add(new ResultPerCategory()
                {
                    userId = id,
                    category = i.ToString(),
                    lvl = lvl_update,
                    counter = taskCounter_update
                });
            }

            using (OXXGameDBContext context = new OXXGameDBContext(options))
            {
                DB db = new DB(context);

                db.addResultPerCategory(list_original);
                context.SaveChanges();
            }

            // Tester og verifiserer
            using (OXXGameDBContext context = new OXXGameDBContext(options))
            {
                DB db = new DB(context);

                db.updateResultsPerCategory(list_update);
                context.SaveChanges();

                List<ResultPerCategory> list = db.allResultsPerCategory(id);

                foreach (ResultPerCategory resPerCat in list)
                {
                    Assert.Equal(lvl_update, resPerCat.lvl);
                    Assert.Equal(taskCounter_update, resPerCat.counter);
                    Assert.Equal(list_original.Count, list.Count());
                }
            }
        }

        // Gjengående metode for opprettelse av options for InMemoryDatabase
        private DbContextOptions<OXXGameDBContext> getOptions(string dbName)
        {
            return new DbContextOptionsBuilder<OXXGameDBContext>()
                .UseInMemoryDatabase(databaseName: dbName)
                .Options;
        }
    }
}
