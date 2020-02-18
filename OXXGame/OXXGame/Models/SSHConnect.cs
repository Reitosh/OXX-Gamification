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
        private string user;
        private string password;
        private string host;
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
                "51.140.218.174", "Markus", new PasswordAuthenticationMethod("Markus", "Plainsmuchj0urney"))))
            {
                DB category = new DB(db);
                List<Models.Task> tasks = category.allTasks();

                client.Connect();

                foreach (Models.Task task in tasks)
                {
                    switch (task.category)
                    {
                        case "ZSharp":
                            var CSharpCommand = client.RunCommand("sudo sh /home/Markus/Scripts/CSharp.sh '" + Code + "' "
                        + "'" + userId + "'");
                            string CSharpOutput = CSharpCommand.Result;
                            Debug.WriteLine("ja her valgte zxsharp da");
                            Debug.WriteLine(CSharpOutput);
                            client.Disconnect();
                            return CSharpOutput;
                        
                        case "JavaScript":
                            var JavaScriptCommand = client.RunCommand("sudo sh /home/Markus/Scripts/JavaScript.sh '" + Code + "' " + "'"
                                + userId + "'");
                            string JavaScriptOutput = JavaScriptCommand.Result;
                            Debug.WriteLine(JavaScriptOutput);
                            client.Disconnect();
                            return JavaScriptOutput;

                        case "Vue.js":
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