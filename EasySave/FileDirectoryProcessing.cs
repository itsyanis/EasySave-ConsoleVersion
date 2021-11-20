using System;
using System.Configuration;
using System.IO;

namespace EasySave
{
    class FileDirectoryProcessing
    {

        // Properties, Setters & Getters

        public string ConfigDirectory { get; set; }
        public string BackupFile { get; set; }
        public string LogFile { get; set; }
        public string StateFile { get; set; }


        // Default Constructor
        public FileDirectoryProcessing()
        {
            this.ConfigDirectory = ConfigurationManager.AppSettings.Get("EasySave");   // Get the Directory EsaySave Path from App.config
            this.BackupFile = ConfigurationManager.AppSettings.Get("BackupFile");      // Get the Backup File Path from App.config
            this.LogFile = ConfigurationManager.AppSettings.Get("LogFile");            // Get the Log File Path from App.config
            this.StateFile = ConfigurationManager.AppSettings.Get("StateFile");        // Get State File Path from App.config
        }


        // This method will create all configs File (EasySave, State, Log)
        public void GenerateConfigFiles()
        {
            this.CreateDirectory(ConfigDirectory);           // Create the EasySave directory that will contain log,state and backup file
            this.CreateFile(BackupFile);                    
            this.CreateFile(LogFile);                      
            this.CreateFile(StateFile);                   
        }



        // This method will create a new file
        public void CreateFile(string Path)
        {
            //  Check if file already exists
            if (File.Exists(Path) == false)                  
            {
                try
                {
                    // Try to create the file.
                    using (File.Create(Path)) { }         
                }
                catch (Exception e)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("An error occurred while creating a file, the path does not exist or is invalid ... " + e.ToString());
                    Console.ForegroundColor = ConsoleColor.White;
                }
            }
        }


        // This method will create a new directory
        public void CreateDirectory(string Path)
        {
            try
            {
                // Check if the directory exists.
                if (Directory.Exists(Path) == false)
                {
                    // Try to create the directory.
                    DirectoryInfo dir = Directory.CreateDirectory(Path);
                }
            }
            catch (Exception e)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("An error occurred while creating a directory, the path does not exist or is invalid ...", e.ToString());
                Console.ForegroundColor = ConsoleColor.White;
            }
        }


        // This method will check if the directory is empty
        public bool IsDirectoryEmpty(string SourcePath)
        {
            if (Directory.GetFileSystemEntries(SourcePath).Length == 0)
            {
                return true;
            }
            return false;
        }


        // This method will return the Directory Size
        public long GetDirectorySize(string DirectoryPath)
        {
            string[] Files = Directory.GetFiles(DirectoryPath);    // Get all files contained in the directory (put them in array)

            long Size = 0;

            foreach (string File in Files)                        
            {
                FileInfo F = new FileInfo(File);                
                Size += F.Length;
            }

            return Size;                                        
        }

    }
}
