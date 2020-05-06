using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;

namespace OXXGame.Models
{
    public class FileHandler
    {
        private string path = "/home/submission_files";
        private string extension = ".txt";
        private bool windows = false;

        public FileHandler() { }

        // Primært for testing lokalt (kan være nyttig ...?)
        public FileHandler(string overridePath, bool windows)
        {
            this.path = overridePath;
            this.windows = windows;
        }

        public string saveFile(string relativePath, string fileName, List<string> fileContent)
        {
            string absolutePath = path + relativePath;
            if (! Directory.Exists(absolutePath))
            {
                Directory.CreateDirectory(absolutePath);
            }

            string fileAbsolutePath = absolutePath + "/" + fileName + extension;

            using (StreamWriter file = new StreamWriter(fileAbsolutePath))
            {
                foreach (string line in fileContent)
                {
                    file.WriteLine(line);
                }
            }

            if (windows)
            {
                fileAbsolutePath.Replace("/", @"\");
            }

            return fileAbsolutePath;
        }

        public List<string> getFile(string relativePath, string fileName)
        {
            string fileAbsolutePath = path + relativePath + "/" + fileName + extension;
            List<string> fileContent = new List<string>();

            using (StreamReader file = new StreamReader(fileAbsolutePath))
            {
                while (!file.EndOfStream)
                {
                    fileContent.Add(file.ReadLine());
                }
            }

            return fileContent;
        }

        public static List<string> stringToList(string inputString)
        {
            List<string> codeLines = new List<string>();
            
            if (inputString != null)
            {
                StringReader reader = new StringReader(inputString);

                string line = reader.ReadLine();
                while (line != null)
                {
                    codeLines.Add(line);
                    line = reader.ReadLine();
                }
            }
            else
            {
                codeLines.Add("");
            }
            

            return codeLines;
        }
    }
}
