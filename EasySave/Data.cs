using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace EasySave
{
    class Data
    {
        protected string backupName { get; set; }
        protected string sourcePath { get; set; }
        protected string destinationPath { get; set; }
        protected DateTime timestamp { get;  set; }

        public Data()
        {
            backupName = backupName;
            sourcePath = sourcePath;
            timestamp = timestamp;
        }

        public Data(string backupName, string sourcePath, string destinationPath, DateTime timestamp)
        {
            this.backupName = backupName;
            this.sourcePath = sourcePath;
            this.destinationPath = destinationPath;
            this.timestamp = timestamp;
        }

        // This method will create the 'File' if it hasn't been created, and replenish it with the BackupJob informations
        public void writeOnFile(string path, Data Informations)
        {
            string JsonInformations = JsonConvert.SerializeObject(Informations);     // Convert DataLog informations to JSON 

            if (File.Exists(path) == true)                                              // Check if 'File' exist
            {
                using (StreamWriter logFile = File.AppendText(path))                    // If the 'File' exist just append the JSON informations
                {
                    logFile.WriteLine(JsonInformations);
                }
            }
            else
            {
                using (StreamWriter logFile = File.CreateText(path))                     // If the 'File' doesn't exist we create it and put ON Json informations
                {
                    logFile.WriteLine(JsonInformations);
                }
            }

        }
    }
}
