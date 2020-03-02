using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Renci.SshNet;
using System.Diagnostics;
using Microsoft.Extensions.Logging;

namespace OXXGame.Models
{

    public class SSHConnect
    {
        private string user = "Markus";
        private string password = "Plainsmuchj0urney";
        private string host = "51.140.218.174";
        private OXXGameDBContext db;

        public SSHConnect(string user, string password, string host, OXXGameDBContext db)
        {
            this.user = user;
            this.password = password;
            this.host = host;
            this.db = db;
        }
        public string RunCode(string Code, int? userId)
        {
            using (var client = new SshClient(new ConnectionInfo(
                host, user, new PasswordAuthenticationMethod(user, password)
                )))
            {
                DB category = new DB(db);
                List<Task> tasks = category.allTasks();


                client.Connect();

                foreach (Task task in tasks)
                {
                    switch (task.category)
                    {
                        case "CSharp":
                            var CSharpCommand = client.RunCommand("sudo sh /home/Markus/Scripts/CSharp.sh '" + Code + "' "
                        + "'" + userId + "'");
                            string CSharpOutput = CSharpCommand.Result;
                            Debug.WriteLine("ja her valgte CSharp da");
                            Debug.WriteLine(CSharpOutput);
                            client.Disconnect();
                            return CSharpOutput;
                        
                        case "JavaScript":
                            var JavaScriptCommand = client.RunCommand("sudo sh /home/Markus/Scripts/JavaScript.sh '" + Code + "' " + "'"
                                + userId + "'");
                            string JavaScriptOutput = JavaScriptCommand.Result;
                            Debug.WriteLine("ja her valgte javascript da");
                            Debug.WriteLine(JavaScriptOutput);
                            client.Disconnect();
                            return JavaScriptOutput;

                        case "vue.js":
                            Debug.WriteLine("ja her valgte vue.js da");
                            return "ikke laget";

                        case "MVC":
                            return "ikke laget";

                        case "TypeScript":
                            return "ikke laget";

                        case "DotNetFramework":
                            return "ikke laget";

                        case "React":
                            return "ikke laget";

                        case "Angular":
                            return "ikke laget";
                        

                    }
                }
                return "Empty";
            }
        }
    }
}