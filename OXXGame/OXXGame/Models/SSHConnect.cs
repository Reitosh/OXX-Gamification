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
        //private OXXGameDBContext db;

        public SSHConnect(string user, string password, string host /*, OXXGameDBContext db*/)
        {
            this.user = user;
            this.password = password;
            this.host = host;
            //this.db = db;
        }

        public string RunCode2(TestModel testModel)
        {
            using (SshClient client = new SshClient(new ConnectionInfo(
                host,user,new PasswordAuthenticationMethod(user,password))))
            {

                client.Connect();

                string command = string.Format("sudo sh /home/Markus/Testing/{0}.sh '{1}' '{2}' '{3}'",
                    testModel.task.category, testModel.singleTestResult.userId, testModel.task.testId, testModel.code);


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