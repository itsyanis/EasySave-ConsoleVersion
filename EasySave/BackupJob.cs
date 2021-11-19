using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Configuration;
using System.IO;

namespace EasySave
{
    // BackupJob derives from Data
    class BackupJob : Data
    {
        // Properties, Setters & Getters
        public string Type { get; private set; }

        // Constructor
        public BackupJob() { }
        public BackupJob(string BackupNameEntry, string SourcePathEntry, string DestinationPathEntry, string TypeEntry)
        {
            this.BackupName = BackupNameEntry;
            this.SourcePath = SourcePathEntry;
            this.DestinationPath = DestinationPathEntry;
            this.Type = TypeEntry;
        }

        // This method will return a specific backupJob 
        public string GetSpecificJob(string BackupName)
        {
            string[] All_Lines = File.ReadAllLines(@"C:\EasySave\Backup.txt");    // get all content of backupFile 

            foreach (string line in All_Lines)                                   // Loop line by line 
            {
                var backupJob = (JObject)JsonConvert.DeserializeObject(line);         // Deserialize the each line 

                string name = backupJob["backupName"].Value<string>();               // Extract the backup name from each line

                if (name == BackupName)                                             // Compare if the name is the same with the backupName introduced by user 
                {
                    return line;
                }
            }
            return null;
        }


        // This method will return all backupJob 
        public void ShowAllJobs()
        {
            string BackupFile = ConfigurationManager.AppSettings.Get("BackupFile");   // Get the Backup file path

            if (new FileInfo(BackupFile).Length != 0)                 // Check if file is not empty
            {
                string[] All_Lines = File.ReadAllLines(BackupFile);    // get all content of backupFile 

                foreach (string line in All_Lines)
                {
                    var backupJob = (JObject)JsonConvert.DeserializeObject(line);         // Deserialize the each line 

                    Console.WriteLine("*********************************************************************");
                    Console.WriteLine("- Name : " + backupJob["BackupName"] + "\n- Source Path : " + backupJob["SourcePath"] + "\n- Destination Path : " + backupJob["DestinationPath"]);
                }
                Console.WriteLine("*********************************************************************");
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("No backup job has been created.");
                Console.ForegroundColor = ConsoleColor.White;
            }
        }
    }
}
