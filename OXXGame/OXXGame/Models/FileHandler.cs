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

        public FileHandler() { }

        // Primært for testing lokalt (kan være nyttig ...?)
        public FileHandler(string overridePath)
        {
            this.path = overridePath;
        }

        public string saveFile(string relativePath, string fileName, List<string> fileContent)
        {
            string absolutePath = path + relativePath;
            if (! Directory.Exists(absolutePath))
            {
                Directory.CreateDirectory(absolutePath);
            }

            string fileAbsolutePath = absolutePath + "/" + fileName;

            File.Create(fileAbsolutePath);

            using (StreamWriter file = new StreamWriter(fileAbsolutePath))
            {
                foreach (string line in fileContent)
                {
                    file.WriteLine(line);
                }
            }

            return absolutePath + fileName;
        }

        public List<string> getFile(string relativePath, string fileName)
        {
            string fileAbsolutePath = path + relativePath + "/" + fileName;
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
            StringReader reader = new StringReader(inputString);

            string line = reader.ReadLine();
            while ( line != null)
            {
                codeLines.Add(line);
                line = reader.ReadLine();
            }

            return codeLines;
        }
    }
}
