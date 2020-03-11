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
                host, user, new PasswordAuthenticationMethod(user, password))))
            {
                DB category = new DB(db);
                List<Task> tasks = category.getTasks("TypeScript", 1);


                client.Connect();

                foreach (Task task in tasks)
                {
                    string scriptPath = "sudo sh /home/Markus/Scripts/";

                    switch (task.category)
                    {
                        case "CSharp":
                            var CSharpCommand = client.RunCommand(scriptPath + "CSharp.sh" + " '" + Code + "' " + "'" + userId + "'");
                            string CSharpOutput = CSharpCommand.Result;
                            Debug.WriteLine("ja her valgte CSharp da");
                            Debug.WriteLine(CSharpOutput);
                            client.Disconnect();
                            return CSharpOutput;

                        case "JavaScript":
                            var JavaScriptCommand = client.RunCommand(scriptPath + "JavaScript.sh" + " '" + Code + "' " + "'" + userId + "'");
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
                            client.RunCommand(scriptPath + "TypeScript.sh" + " '" + Code + "' " + "'" + userId + "'");
                            var TypeScriptCommand = client.RunCommand("sudo cat /home/Markus/OXXGame/TypeScript-" + userId + ".js");
                            var TypeScriptOutput = TypeScriptCommand.Result;
                            client.Disconnect();
                            return TypeScriptOutput;

                            

                        case "DotNetFramework":
                            return "ikke laget";

                        case "React":
                            return "ikke laget";

                        case "Angular":
                            return "ikke laget";


                    }
                }
                return "No task specified";
            }
        }
    }
}