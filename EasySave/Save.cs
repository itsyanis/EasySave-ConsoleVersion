using ConsoleTables;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

namespace EasySave
{
    class Save
    {
        private string BackupFile = ConfigurationManager.AppSettings.Get("BackupFile");        // Get the Backup File Path from App.config
        private string LogFile = ConfigurationManager.AppSettings.Get("LogFile");            // Get the Log File Path from App.config
        private string StateFile = ConfigurationManager.AppSettings.Get("StateFile");       // Get State File Path from App.config


        public void CompleteSave(string BackupJobName, string SourcePath, string DestinationPath)
        {
            FileDirectoryProcessing processing = new FileDirectoryProcessing();
            Stopwatch TransferTime = new Stopwatch();                                              // Initialize the StopWatch timer 


            if (Directory.Exists(SourcePath) && !processing.IsDirectoryEmpty(SourcePath))        // Check if source path exist and isn't empty
            {
                string[] Files = Directory.GetFiles(SourcePath);                                 // Get all files contained in the source Path (put them in array)

                int NbrFiles = System.IO.Directory.GetFiles(SourcePath).Length;                 // Get nbr of Files to copy
                int NbFilesLeftToDo = NbrFiles;
                long TotalFileSize = processing.GetDirectorySize(SourcePath);

                foreach (string File in Files)                                                    // browse file by file 
                {

                    // Extract all informations about file (name, size, extention ...)
                    string FileName = Path.GetFileName(File);                                    // Get file Name
                    string Extension = Path.GetExtension(File);                                 //  Get extension
                    long Size = new FileInfo(File).Length;                                     //   Get size

                    string DestinationFile = Path.Combine(DestinationPath, FileName);        // Combine the destination path with the file name  
                    bool State = false;

                    DataState StateInformations = new DataState(BackupJobName, SourcePath, DestinationPath, State, NbrFiles, TotalFileSize, NbFilesLeftToDo, DateTime.Now, 15);

                    try
                    {
                        Parallel.Invoke
                                        (
                                            () => TransferTime.Start(),                        // Start the transfer time
                                            () => System.IO.File.Copy(File, DestinationFile)  // Try to copy file to destination
                                        );

                        TransferTime.Stop();
                        StateInformations.WriteOnFile(this.StateFile, StateInformations); // Stop the transfer time
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("En error occurred during the save " + e.ToString());
                    }

                    DataLog LogInformations = new DataLog(BackupJobName, SourcePath, DestinationPath, DateTime.Now, Size, TransferTime.Elapsed);
                    LogInformations.WriteOnFile(this.LogFile, LogInformations);               // Put Log informations on Log File

                    NbFilesLeftToDo--;
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
                            Parallel.Invoke
                                          (
                                               () => TransferTime.Start(),
                                               () => Directory.CreateDirectory(DestinationDirectory)   // Create the directory
                                          );

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


        public void DifferentialSave(string BackupJobName, string SourcePath, string DestinationPath)
        {
            FileDirectoryProcessing processing = new FileDirectoryProcessing();

            if (Directory.Exists(SourcePath) == true && !processing.IsDirectoryEmpty(SourcePath))         // Check if source path is a Directory and isn't empty
            {
                if (Directory.GetFiles(SourcePath).Length > 0)                                          // Check if source path contain files
                {
                    if (Directory.GetFiles(DestinationPath).Length > 0)                             // Check if destination path contain files
                    {
                        string[] SourceFiles = Directory.GetFiles(SourcePath);                     // Get all source files
                        string[] DestinationFiles = Directory.GetFiles(SourcePath);               // Get all destination files

                        foreach (string SourceFile in SourceFiles)
                        {
                            string SourceFileName = Path.GetFileName(SourceFile);               // Extract name and source file size 
                            long SourceFileLength = new FileInfo(SourceFile).Length;

                            bool Find = false;

                            foreach (string DestinationFile in DestinationFiles)
                            {
                                string DestinationFileName = Path.GetFileName(DestinationFile);    // Extract name and destination file size
                                long DestinationFilelength = new FileInfo(DestinationFile).Length;

                                if (string.Compare(SourceFileName, DestinationFile) == 0)         // Comparing
                                {
                                    Find = true;       // We find one source file into destination

                                    if (SourceFileLength != DestinationFilelength)   // compare by sizing 
                                    {
                                        File.Copy(SourceFile, DestinationFile, true);   // Do the copy with true parametre for overwrite

                                        
                                         // Write on LogFile
                                    }
                                }
                            }

                            if (Find == false)  // Case if we didn't find source file into destination we need to copy it 
                            {
                                string SrcName = Path.GetFileName(SourceFile);
                                string destination = Path.Combine(DestinationPath, SrcName);

                                File.Copy(SourceFile, destination, true);

                                   // Write on LogFile and state file
                            }

                        }
                    }
                    else       // Case when destination path doesn't contain files, we copy all source files to destination
                    {
                        string[] SourceFiles = Directory.GetFiles(SourcePath);

                        foreach (string File in SourceFiles)
                        {
                            string SrcName = Path.GetFileName(File);
                            string Destination = Path.Combine(DestinationPath, SrcName);
                            System.IO.File.Copy(File, Destination, true);

                                    // Write on LogFile and state file
                        }
                    }
                }


                if (Directory.GetDirectories(SourcePath).Length > 0)  // Check if the source Path contain sub directory 
                {
                    string[] SourceDirectories = Directory.GetDirectories(SourcePath);

                    foreach (string SubDirectory in SourceDirectories)
                    {
                        DirectoryInfo DirInfo = new DirectoryInfo(SubDirectory);
                        string DirName = DirInfo.Name;
                        string DestinationDir = Path.Combine(DestinationPath, DirName);

                        DifferentialSave(BackupJobName, SubDirectory, DestinationDir);  // Do the recursivity for files and sub directory 
                    }
                }
            }
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
                    DifferentialSave(BackupJobName, SourcePath, DestinationPath);
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



