using ConsoleTables;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Configuration;
using System.Diagnostics;
using System.IO;

namespace EasySave
{
    class Save
    {
        private string BackupFile = ConfigurationManager.AppSettings.Get("BackupFile");        // Get the Backup File Path from App.config
        private string LogFile = ConfigurationManager.AppSettings.Get("LogFile");             // Get the Log File Path from App.config
        private string StateFile = ConfigurationManager.AppSettings.Get("StateFile");        // Get State File Path from App.config


        public void CompleteSave(string BackupJobName, string SourcePath, string DestinationPath)
        {
            FileDirectoryProcessing processing = new FileDirectoryProcessing();
            Stopwatch TransferTime = new Stopwatch();                                               


            if (Directory.Exists(SourcePath) && !processing.IsDirectoryEmpty(SourcePath))        // Check if source path exist and isn't empty
            {
                string[] Files = Directory.GetFiles(SourcePath);                                 

                foreach (string File in Files)                                                     
                {

                    // Extract all informations about file (name, size, extention ...)
                    string FileName = Path.GetFileName(File);                                    
                    string Extension = Path.GetExtension(File);                                 
                    long Size = new FileInfo(File).Length;                                     

                    string DestinationFile = Path.Combine(DestinationPath, FileName);        // Combine the destination path with the file name  

                    try
                    {
                        TransferTime.Start();
                        System.IO.File.Copy(File, DestinationFile);                             // Try to copy file to destination
                        TransferTime.Stop();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("En error occurred during the save " + e.ToString());
                    }

                    DataLog LogInformations = new DataLog(BackupJobName, SourcePath, DestinationPath, DateTime.Now, Size, TransferTime.Elapsed);
                    LogInformations.WriteOnFile(this.LogFile, LogInformations);               // Put Log informations on Log File
                }


                if (Directory.GetDirectories(SourcePath).Length > 0)      // Check if source path contain Sub Directory
                {
                    // In case the source path contains SubDirectory

                    string[] Directories = Directory.GetDirectories(SourcePath);   // Get all directories contained in the source Path (put them in array)


                    foreach (string Dir in Directories)
                    {
                        // Extract all informations about directory (name, size ...)
                        DirectoryInfo DirInfo = new DirectoryInfo(Dir);
                        string DirectoryName = DirInfo.Name;
                        long DirectorySize = processing.GetDirectorySize(Dir);

                        string DestinationDirectory = Path.Combine(DestinationPath, DirectoryName);

                        TransferTime.Reset();                                                   // Start the transfer time

                        try
                        {
                            TransferTime.Start();
                            Directory.CreateDirectory(DestinationDirectory);   // Create the directory 
                            TransferTime.Stop();
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine("En error occurred during the save " + e.ToString());
                        }

                        DataLog LogInformation = new DataLog(BackupJobName, SourcePath, DestinationPath, DateTime.Now, DirectorySize, TransferTime.Elapsed);
                        LogInformation.WriteOnFile(this.LogFile, LogInformation);

                        CompleteSave(BackupJobName, Dir, DestinationDirectory);      // Recursivity : We call the method for sub sub ... directory and their files.
                    }
                }
            }
        }


        public void DifferentialSave()
        {

        }



        public void SpecificSave(string BackupJobName)
        {
            BackupJob Backup = new BackupJob();
            string Job = Backup.GetSpecificJob(BackupJobName);               // Get the specific jb from BackupFile

            var data = (JObject)JsonConvert.DeserializeObject(Job);         // Deseriaize the backup informations

            string Type = data["Type"].Value<string>();                    //  Get the Type, Source Path and Destination Path
            string SourcePath = data["SourcePath"].Value<string>();
            string DestinationPath = data["DestinationPath"].Value<string>();

            switch (Type)
            {
                case "Complete":
                    CompleteSave(BackupJobName, SourcePath, DestinationPath);
                    break;

                case "Differential":
                    DifferentialSave();
                    break;
            }

        }


        // This method will save all created Backup Jobs
        public void SaveAlljobs()
        {
            string[] All_Lines = File.ReadAllLines(ConfigurationManager.AppSettings.Get("BackupFile"));    // get all content of backupFile 

            foreach (string line in All_Lines)                                   // Loop line by line 
            {
                var backupJob = (JObject)JsonConvert.DeserializeObject(line);         // Deserialize the each line 

                string BackupName = backupJob["BackupName"].Value<string>();               // Extract the backup name from each line
                SpecificSave(BackupName);
            }
        }


        public void ShowSavedJob()
        {
            string LogFile = ConfigurationManager.AppSettings.Get("LogFile");
            String[] GetAll = File.ReadAllLines(LogFile);
            int i = 1;
            var table = new ConsoleTable("Count", "Backup Job Name", "Source Path", "Destination Path", "Save Type");

            foreach (string line in GetAll)                                                    // Loop line by line 
            {
                var backupJob = (JObject)JsonConvert.DeserializeObject(line);                // Deserialize each line 

                table.AddRow(i, backupJob["BackupName"], backupJob["SourcePath"], backupJob["DestinationPath"], backupJob["Type"]);
                table.Write();
                i++;
            }

        }

    }
}



