using System;
using System.Collections.Generic;
using System.IO;

namespace OXXGame.Models
{
    public class FileHandler
    {
        private string path = "/home/submission_files"; // Hardkodet default path for fillagring
        private bool windows = false; // variabel for enkel konvertering til windows-path ( / -> \ )

        //Default constructor
        public FileHandler() { }

        // Constructor primært for testing lokalt
        // Denne vil overskrive default path, og lar deg velge om path skal konverteres til windows-format
        public FileHandler(string overridePath, bool windows)
        {
            this.path = overridePath;
            this.windows = windows;
        }

        // Tar inn relativ path ("/userId"), filnavn, filinnhold og fil-extension
        // Oppretter og skriver innhold til fil. Returnerer absolute path til den nye filen.
        public string saveFile(string relativePath, string fileName, List<string> fileContent, string extension)
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

        // Leser inn string linje for linje og legger til i liste som returneres.
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

        // Setter passende extension basert på oppgavekategori
        public static string getFileExtension(string category)
        {
            switch (category)
            {
                case "C#":
                    return ".cs";
                case "TypeScript":
                    return ".ts";
                default:
                    return ".html";
            }
        }
    }
}
