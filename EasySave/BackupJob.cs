using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;

namespace EasySave
{
    class BackupJob
    {

        public string name { get; private set; }
        public string sourcePath { get; private set; }
        public string destinationPath { get; private set; }
        public string type { get; private set; }


        public BackupJob(string name, string sourcePath, string destinationPath, string type)
        {
            this.name = name;
            this.sourcePath = sourcePath;
            this.destinationPath = destinationPath;
            this.type = type;
        }

        // This method will create the 'backupFile' if it hasn't been created, and replenish it with the BackupJob informations
        public void writeOnBackupFile(string path, BackupJob JobInformations)
        {
            string JsonInformation =  JsonSerializer.Serialize(JobInformations);    // Convert BackupJob informations to JSON 

            if (File.Exists(path) == true)                                          // Check if 'backupFile' exist
            {
                using (StreamWriter file = File.AppendText(path))                   
                {
                    file.WriteLine(JsonInformation);                               // If the 'backupFile' exist just append the JSON informations
                } 

            }else
            {
                using (StreamWriter file = File.CreateText(path))                 // If the 'backupFile' doesn't exist we create it and put ON informations
                {
                    file.WriteLine(JsonInformation);                    
                }
            }
        }

    }
}
