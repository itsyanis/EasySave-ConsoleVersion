using ConsoleTables;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
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
            string[] All_Lines = File.ReadAllLines(ConfigurationManager.AppSettings.Get("BackupFile"));    // get all content of backupFile 

            foreach (string line in All_Lines)                                   
            {
                var backupJob = (JObject)JsonConvert.DeserializeObject(line);         // Deserialize the each line 

                string name = backupJob["BackupName"].Value<string>();               // Extract the backup name from each line

                if (name == BackupName)                                              
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
                int i = 1;      //Count the number of backup jobs
                var table = new ConsoleTable("Count", "Backup Job Name", "Source Path", "Destination Path", "Save Type");

                foreach (string line in All_Lines)
                {
                    var backupJob = (JObject)JsonConvert.DeserializeObject(line);         // Deserialize the each line 

                    table.AddRow(i, backupJob["BackupName"], backupJob["SourcePath"], backupJob["DestinationPath"], backupJob["Type"]);  //Add the informations in the table
                    i++;
                }
                table.Write();
                Console.WriteLine();
            }
            else     //In case of error
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("erreur");
                Console.ForegroundColor = ConsoleColor.Gray;
            }
        }

        public void DeleteSpecificBackup(string BackupName)
        {
            string[] All_Lines = File.ReadAllLines(ConfigurationManager.AppSettings.Get("BackupFile"));    // get all content of backup File 

            File.Delete(ConfigurationManager.AppSettings.Get("BackupFile"));

            using (StreamWriter Backup = new StreamWriter(ConfigurationManager.AppSettings.Get("BackupFile")))
            {
                foreach (string line in All_Lines)
                {
                    var backupJob = (JObject)JsonConvert.DeserializeObject(line);         // Deserialize the each line 

                    string name = backupJob["BackupName"].Value<string>();               // Extract the backup name from each line

                    if (name != BackupName)                                             // Compare if the name is the same with the backupName introduced by user 
                    {
                        Backup.WriteLine(line);
                    }
                }
            }
        }

    }
}
