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

        string user;
        string password;
        string host;

       

        public SSHConnect(string user, string password, string host)
        {
            this.user = user;
            this.password = password;
            this.host = host;

        }

        public void ConnectToVM()
        {
            string code = "'using System; namespace HelloWorld { public class HelloWorld { public static void Main(string[] args) { Console.WriteLine(\"Hva skjer da\"); } } }'";

            using (var client = new SshClient(new ConnectionInfo("51.140.218.174", "Markus", new PasswordAuthenticationMethod("Markus", "Plainsmuchj0urney"))))
            {
                client.Connect();
                client.RunCommand("rm HelloWorld.exe");
                client.RunCommand("rm HelloWorld.cs");
                client.RunCommand("sudo touch HelloWorld.cs");
                client.RunCommand("sudo chmod 777 HelloWorld.cs");
                client.RunCommand("echo " + code + ">> HelloWorld.cs");
                client.RunCommand("mcs HelloWorld.cs");
                var output = client.RunCommand("mono HelloWorld.exe");
                Debug.WriteLine(output.Result);
                client.Disconnect();
            }
        }

    }
}

