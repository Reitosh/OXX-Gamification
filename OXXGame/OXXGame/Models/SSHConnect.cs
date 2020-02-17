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

        public SSHConnect(string user, string password, string host)
        {
            this.user = user;
            this.password = password;
            this.host = host;
        }
        public string ConnectToVM(string Code)
        {
             

            using (var client = new SshClient(new ConnectionInfo(
                "51.140.218.174", "Markus", new PasswordAuthenticationMethod("Markus", "Plainsmuchj0urney"))))
            {

                client.Connect();
                var command = client.RunCommand("sudo sh /home/Markus/CSharp.sh '" + Code + "'");
                string result = command.Result;
                client.Disconnect();
               
                return result;
            }
        }

    }
}