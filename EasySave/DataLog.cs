using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace EasySave
{
    class DataLog
    {
        public string backupName { get; private set; }
        public string sourcePath { get; private set; }
        public string destinationPath { get; private set; }
        public int size { get; private set; }
        public DateTime timestamp { get; private set; }
        public int trasnferTime { get; private set; }



        public DataLog (string backupName, string sourcePath, string destinationPath, int size, DateTime timestamp, int trasnferTime)
        {
            this.backupName = backupName;
            this.sourcePath = sourcePath;
            this.destinationPath = destinationPath;
            this.size = size;
            this.timestamp = timestamp;
            this.trasnferTime = trasnferTime;
        }


        // This method will create the 'logFile' if it hasn't been created, and replenish it with the BackupJob informations
        public void writeOnLogFile(string path, DataLog LogInformations )
        {
            string JsonInformations = JsonConvert.SerializeObject(LogInformations);     // Convert DataLog informations to JSON 

            if (File.Exists(path) == true)                                              // Check if 'logFile' exist
            {
                using (StreamWriter logFile = File.AppendText(path))                    // If the 'logFile' exist just append the JSON informations
                {
                    logFile.WriteLine(JsonInformations);
                }
            }else
            {
                using (StreamWriter logFile = File.CreateText(path))                     // If the 'logFile' doesn't exist we create it and put ON Json informations
                {
                    logFile.WriteLine(JsonInformations);
                }
            }

        }
    }
}
