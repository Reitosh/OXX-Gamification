using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using System.Text;
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
            switch (testModel.task.category)
            {
                case "TypeScript":
                    return RunTypeScript(testModel);

                case "C#":
                    return RunCsharp(testModel);

                default:
                    throw new Exception("Kategori er ikke definert");
            }
        }

        public string RunTypeScript(TestModel testModel)
        {
            using (SshClient client = new SshClient(new ConnectionInfo(
               host, user, new PasswordAuthenticationMethod(user, password))))
            {

                client.Connect();
                string tsScript = string.Format("cd /home/Markus/Scripts && ./{0}.sh '{1}' '{2}'", testModel.task.category, testModel.code, testModel.singleTestResult.userId);
                SshCommand run = client.RunCommand(tsScript);
                string output = run.Result;

                if (output.Contains("error TS"))
                {
                    List<string> allLines = FileHandler.stringToList(output);
                    List<string> errors = new List<string>();
                    StringBuilder errorStrBuilder = new StringBuilder();

                    foreach (string error in allLines)
                    {
                        if (error.Contains("error TS"))
                        {
                            errors.Add(error);
                            errorStrBuilder.AppendLine(error.ToString());
                        }
                    }
                    client.Disconnect();
                    return errorStrBuilder.ToString() + "\nFant " + errors.Count() + " feil.";
                }

                else
                {
                    client.Disconnect();
                    return output;
                }
            }
        }

        private static string FormatCode(string code, string userId, string testId)
        {
            string formattedCode = code;

            // Setter inn riktig namespace og klassenavn
            formattedCode = formattedCode
                .Replace("OxxTestId", "TId_" + testId)
                .Replace("OxxUId", "UId_" + userId);

            formattedCode = formattedCode.Replace("OxxUId", "UId_" + userId);

            // Fjerner flerlinje-kommentarer (/* lager tull i linux)
            while (formattedCode.Contains("/*"))
            {
                int startIndex = formattedCode.IndexOf("/*");
                int endIndex = formattedCode.IndexOf("*/") + 2;

                int count = endIndex - startIndex;

                formattedCode = formattedCode.Remove(startIndex, count);
            }

            // Legger til \ foran "
            if (formattedCode.Contains("\""))
            {
                formattedCode = formattedCode.Replace("\"", @"\" + "\"");
            }

            return formattedCode;
        }

        public string RunCsharp(TestModel testModel)
        {
            using (SshClient client = new SshClient(new ConnectionInfo(
                    host, user, new PasswordAuthenticationMethod(user, password))))
            {

                client.Connect();

                string command = string.Format("sudo sh /home/Markus/Testing/{0}.sh '{1}' '{2}' \"{3}\"",
                       testModel.task.category,
                       testModel.singleTestResult.userId,
                       testModel.task.testId,
                       FormatCode(testModel.code, testModel.singleTestResult.userId.ToString(), testModel.task.testId.ToString())
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