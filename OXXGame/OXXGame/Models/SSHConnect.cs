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

                string command = string.Format("sudo sh /home/Markus/Testing/{0}.sh '{1}' '{2}' \"{3}\"",
                    testModel.task.category, 
                    testModel.singleTestResult.userId, 
                    testModel.task.testId, 
                    FormatCode(testModel.code,testModel.singleTestResult.userId.ToString(),testModel.task.testId.ToString())
                    );


                SshCommand runCommand = client.RunCommand(command);
                string output = runCommand.Result;

                Debug.WriteLine("ja her valgte {0} da", testModel.task.category);
                Debug.WriteLine(output);
                client.Disconnect();
                return output;
            }
        }

        private static string FormatCode(string code, string userId, string testId) 
        {
            string formattedCode = code;

            // Setter inn riktig namespace og klassenavn
            formattedCode.Replace("OxxTestId", "TId_" + testId);
            formattedCode.Replace("OxxUId", "UId_" + userId);

            // Fjerner flerlinje-kommentarer (/* lager tull i linux)
            while (formattedCode.Contains("/*"))
            {
                int startIndex = code.IndexOf("/*");
                int count = 0;

                for (int i = startIndex; i < formattedCode.Length; i++)
                {
                    if (formattedCode[i - 1] == '*' && formattedCode[i] == '/' && i != 0)
                    {
                        count = i - startIndex;
                        break;
                    }
                }

                code.Remove(startIndex, count);
            }

            if (formattedCode.Contains("\""))
            {
                formattedCode.Replace("\"",@"\" + "\"");
            }

            return formattedCode;
        }
    }
}