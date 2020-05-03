using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
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

        public SSHConnect(string user, string password, string host)
        {
            this.user = user;
            this.password = password;
            this.host = host;
        }

        public string RunCode(TestModel testModel)
        {
            using (SshClient client = new SshClient(new ConnectionInfo(
                host,user,new PasswordAuthenticationMethod(user,password))))
            {

                client.Connect();
                
                if (testModel.task.category == "TypeScript")
                {
                    string tsScript = string.Format("cd /home/Markus/Scripts && ./{0}.sh '{1}' '{2}'", testModel.task.category, testModel.code, testModel.singleTestResult.userId);
                    SshCommand run = client.RunCommand(tsScript);
                    string output = run.Result;


                    if (output.Contains("error TS"))
                    {
                        string error = new StringReader(output).ReadLine();
                        return error;
                    }
                    else
                    {
                        //string TScommand = string.Format("cat /home/Markus/OXXGame/TypeScript-{0}.js", testModel.singleTestResult.userId);
                       // SshCommand runTs = client.RunCommand(TScommand);
                        //string remove = string.Format("rm -f /home/Markus/OXXGame/TypeScript-{0}.ts TypeScript-{0}.js", testModel.singleTestResult.userId);
                        //client.RunCommand(remove);
                        
                        return output;
                    }
                } 
                else
                {
                    string command = string.Format("sudo sh /home/Markus/Testing/{0}.sh '{1}' '{2}' \"{3}\"",
                    testModel.task.category, 
                    testModel.singleTestResult.userId, 
                    testModel.task.testId, 
                    testModel.code
                    );

                    SshCommand runCommand = client.RunCommand(command);
                    string output = runCommand.Result;

                    Debug.WriteLine("ja her valgte {0} da", testModel.task.category);
                    Debug.WriteLine(output);
                    client.Disconnect();
                    return output;
                }
            }
        }
    }
}