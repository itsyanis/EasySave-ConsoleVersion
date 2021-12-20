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

        private string LogFile = ConfigurationManager.AppSettings.Get("LogFile");              // Get the Log File Path from App.config
        private string StateFile = ConfigurationManager.AppSettings.Get("StateFile");         // Get State File Path from App.config



        public void CompleteSave(string BackupJobName, string SourcePath, string DestinationPath)
        {
            FileDirectoryProcessing processing = new FileDirectoryProcessing();
            Stopwatch TransferTime = new Stopwatch();                                              // Initialize the StopWatch timer 


            if (Directory.Exists(SourcePath) && !processing.IsDirectoryEmpty(SourcePath))        // Check if source path exist and isn't empty
            {
                string[] Files = Directory.GetFiles(SourcePath);                                 // Get all files contained in the source Path (put them in array)

                int NbrFiles = Directory.GetFiles(SourcePath).Length;                           // Get nbr of Files to copy
                int NbFilesLeftToDo = NbrFiles;
                long TotalFileSize = processing.GetDirectorySize(SourcePath);

                foreach (string File in Files)                                                    
                {

                    // Extract all informations about file (name, size, extention ...)
                    string FileName = Path.GetFileName(File);                                   
                    string Extension = Path.GetExtension(File);                                 
                    long Size = new FileInfo(File).Length;                                     

                    string DestinationFile = Path.Combine(DestinationPath, FileName);        // Combine the destination path with the file name  
                    bool State = false;

                    DataState StateInformations = new DataState(BackupJobName, SourcePath, DestinationPath, State, NbrFiles, TotalFileSize, NbFilesLeftToDo, DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss"), 100);

                    try
                    {
                        Parallel.Invoke
                                        (
                                            () => TransferTime.Start(),
                                            () => System.IO.File.Copy(File, DestinationFile, true),                         // Try to copy file to destination
                                            () => StateInformations.WriteOnFile(this.StateFile, StateInformations),        // Write state information on StateFile
                                            () => PBar.ProgBar(FileName)
                                        );

                        TransferTime.Stop();

                        DataLog LogInformations = new DataLog(BackupJobName, File, DestinationFile, DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss"), Size, TransferTime.Elapsed);
                        LogInformations.WriteOnFile(this.LogFile, LogInformations);               // Put Log informations on Log File
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("An error occurred during the save " + e.ToString());
                    }

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

                        try
                        {
                            Directory.CreateDirectory(DestinationDirectory);   // Create the directory
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine("An error occurred during the save " + e.ToString());
                        }

                        CompleteSave(BackupJobName, Dir, DestinationDirectory);                             // Recursivity : We call the method for sub sub ... directory and their files.
                    }
                }
            }
        }



        public void DifferentialSave(string BackupJobName, string SourcePath, string DestinationPath)
        {
            FileDirectoryProcessing processing = new FileDirectoryProcessing();
            Stopwatch TransferTime = new Stopwatch();                                                      // Initialize the StopWatch timer for transfert time


            if (Directory.Exists(SourcePath) == true && !processing.IsDirectoryEmpty(SourcePath))         
            {
                int NbrFiles = Directory.GetFiles(SourcePath).Length;                                     // Get nbr of Files to copy
                int NbFilesLeftToDo = NbrFiles;
                long TotalFileSize = processing.GetDirectorySize(SourcePath);


                if (Directory.GetFiles(SourcePath).Length > 0)                                          // Check if source path contain files
                {
                    if (Directory.GetFiles(DestinationPath).Length > 0)                                // Check if destination path contain files
                    {
                        // Get all source files and destination files
                        string[] SourceFiles = Directory.GetFiles(SourcePath);                     
                        string[] DestinationFiles = Directory.GetFiles(SourcePath);               

                        foreach (string SourceFile in SourceFiles)
                        {
                            string SourceFileName = Path.GetFileName(SourceFile);               // Extract name and source file size 
                            long SourceFileLength = new FileInfo(SourceFile).Length;

                            bool Find = false;
                            bool State = false;

                            foreach (string DestinationFile in DestinationFiles)
                            {
                                string DestinationFileName = Path.GetFileName(DestinationFile);    // Extract name and destination file size
                                long DestinationFilelength = new FileInfo(DestinationFile).Length;

                                if (string.Compare(SourceFileName, DestinationFile) == 0)         
                                {
                                    Find = true;       // We find one source file into destination

                                    if (SourceFileLength != DestinationFilelength)   // compare by sizing 
                                    {
                                       
                                        DataState StateInformations = new DataState(BackupJobName, SourcePath, DestinationPath, State, NbrFiles, TotalFileSize, NbFilesLeftToDo, DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss"), 100);

                                        try
                                        {
                                            Parallel.Invoke
                                                          (
                                                               () => TransferTime.Start(),
                                                               () => File.Copy(SourceFile, DestinationFile, true),   // Do the copy with true parametre for overwrite
                                                               () => StateInformations.WriteOnFile(this.StateFile, StateInformations),
                                                               () => PBar.ProgBar(SourceFileName)
                                                          );

                                            TransferTime.Stop();
                                        }
                                        catch (Exception e)
                                        {
                                            Console.WriteLine("An error occurred during the save " + e.ToString());
                                        }

                                        DataLog LogInformation = new DataLog(BackupJobName, SourcePath, DestinationPath, DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss"), SourceFileLength, TransferTime.Elapsed);
                                        LogInformation.WriteOnFile(this.LogFile, LogInformation);

                                        NbFilesLeftToDo--;
                                    }
                                }
                            }

                            if (Find == false)  // Case if we didn't find source file into destination we need to copy it 
                            {
                                string SrcName = Path.GetFileName(SourceFile);
                                string destination = Path.Combine(DestinationPath, SrcName);

                                DataState StateInformations = new DataState(BackupJobName, SourcePath, DestinationPath, State, NbrFiles, TotalFileSize, NbFilesLeftToDo, DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss"), 100);

                                try
                                {

                                    Parallel.Invoke
                                      (
                                           () => TransferTime.Start(),
                                           () => File.Copy(SourceFile, destination, true),   // Do the copy with true parametre for overwrite
                                           () => StateInformations.WriteOnFile(this.StateFile, StateInformations),
                                           () => PBar.ProgBar(SourceFileName)
                                      );
                                   
                                }
                                catch (Exception e)
                                {
                                    Console.WriteLine("An error occurred during the save " + e.ToString());
                                }

                                TransferTime.Stop();


                                DataLog LogInformation = new DataLog(BackupJobName, SourcePath, DestinationPath, DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss"), SourceFileLength, TransferTime.Elapsed);
                                LogInformation.WriteOnFile(this.LogFile, LogInformation);

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

                            try
                            {
                                System.IO.File.Copy(File, Destination, true);
                            }
                            catch(Exception e)
                            {
                                Console.WriteLine("An error occurred during the save " + e.ToString());
                            }
                        }
                    }
                }


                if (Directory.GetDirectories(SourcePath).Length > 0)                            // Check if the source Path contain sub directory 
                {
                    string[] SourceDirectories = Directory.GetDirectories(SourcePath);

                    foreach (string SubDirectory in SourceDirectories)
                    {
                        DirectoryInfo DirInfo = new DirectoryInfo(SubDirectory);
                        string DirName = DirInfo.Name;
                        string DestinationDir = Path.Combine(DestinationPath, DirName);

                        if (Directory.Exists(DestinationDir) == false)
                        {
                            Directory.CreateDirectory(DestinationDir);                              // Create the directory 
                        }

                        DifferentialSave(BackupJobName, SubDirectory, DestinationDir);              // Do the recursivity for files and sub directory 
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


    }
}



