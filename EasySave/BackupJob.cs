using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;

namespace EasySave
{
    // BackupJob derives from Data
    class BackupJob : Data
    {
        // Properties, Setters & Getters
        public string type { get; set; }

        // Constructor
        public BackupJob() { }
        public BackupJob(string backupName, string sourcePath, string destinationPath, string type)
        {
            this.backupName = backupName;
            this.sourcePath = sourcePath;
            this.destinationPath = destinationPath;
            this.type = type;
        }

        // This method will return a specific backupJob 
        public string getSpecificJob(string backupName)
        {
            string[] All_Lines = File.ReadAllLines(@"C:\EasySave\Backup.txt");    // get all content of backupFile 
            foreach (string line in All_Lines)                                   // Loop line by line 
            {
                var backupJob = (JObject)JsonConvert.DeserializeObject(line);         // Deserialize the each line 

                string name = backupJob["backupName"].Value<string>();               // Extract the backup name from each line

                if (name == backupName)                                             // Compare if the name is the same with the backupName introduced by user 
                {
                    return line;
                }
            }
            return null;
        }


        // This method will return all backupJob 
        public string getAllJob()
        {
            return null;
        }
    }
}
