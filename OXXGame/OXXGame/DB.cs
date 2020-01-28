using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OXXGame.Models;

namespace OXXGame
{
    public class DB
    {
        OXXGameDBContext db;

        public DB(OXXGameDBContext db)
        {
            this.db = db;
        }

        public bool addUser(User user)
        {
            using (db)
            {
                try
                {
                    db.Add(user);
                    return true;
                } 
                
                catch (Exception e)
                {
                    return false;
                }
            }
        }

        public bool addTask(Models.Task task)
        {
            using (db)
            {
                try
                {
                    db.Add(task);
                    return true;
                }

                catch (Exception e)
                {
                    return false;
                }
            }
        }

        // Metode for å hente navn/epost/id for alle brukere
        public List<User> allUsers()
        {
            using(db)
            {
                List<User> users = db.Users.ToList();

                /*List<User> users = db.Users.Select(user => new User
                {
                    userId = user.userId,
                    uname = user.uname,
                    password = user.password,
                    loginCounter = user.loginCounter,
                    firstname = user.firstname,
                    lastname = user.lastname,
                    address = user.address,
                    zipCode = user.zipCode,
                    city = user.city,
                    email = user.email,
                    isAdmin = user.isAdmin,
                    knowHtml = user.knowHtml,
                    knowCss = user.knowCss,
                    knowJavascript = user.knowJavascript,
                    knowCsharp = user.knowCsharp,
                    knowMvc = user.knowMvc,
                    knowNetframework = user.knowNetframework,
                    knowTypescript = user.knowTypescript,
                    knowVue = user.knowVue,
                    knowReact = user.knowReact,
                    knowAngular = user.knowAngular
                }).ToList();*/

                return users;
            }
        }

        public List<Models.Task> allTasks()
        {
            using (db)
            {
                List<Models.Task> tasks = db.Tests.Select(task => new Models.Task
                {
                    testId = task.testId,
                    test = task.test,
                    difficulty = task.difficulty,
                    category = task.category
                }).ToList();

                return tasks;
            }
        }

        // Henter ut bruker ved brukernavn. Metoden returnerer null hvis man ikke finner nøyaktig 1 bruker.
        // Denne er i utgangspunktet ment til innloggingsvalidering. 
        public User getUser(string uname)
        {
            using (db)
            {
                try
                {
                    User user = db.Users.SingleOrDefault(u => u.uname == uname);
                    return user;
                }

                catch (InvalidOperationException e)
                {
                    return null;
                }
                
            }
        }

        // Henter totalresultat ved bruker-id. Har ikke brukeren noen oppføring i resultattabellen (bruker har ikke utført test) returneres null
        public Result resultTot(int uid)
        {
            using (db)
            {
                try
                {
                    Result result = db.Results.SingleOrDefault(r => r.userId == uid);
                    return result;
                }

                catch (InvalidOperationException e)
                {
                    return null;
                }
            }
        }

        // Henter resultater fra alle enkelt-tasks brukeren har utført ved bruker-id.
        // Har ikke brukeren noen oppføring i singelresultat-tabellen (bruker har ikke utført noen tasks) returneres en tom liste
        public List<SingleTestResult> resultPerTask(int uId)
        {
            using (db)
            {
                List<SingleTestResult> results = db.SingleTestResult.Where(r => r.userId == uId).ToList();
            }
        }
    }
}
