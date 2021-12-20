using System;
using System.IO;
using System.Threading;
using System.Configuration;

namespace EasySave
{
    class Menu
    {
        private string Options;
        private string BackupNameEntry;
        private string SourcePathEntry;
        private string DestinationPathEntry;
        private string TypeEntry;
        private string SaveTypeEntry;
        private string SpecificBackupJob;


        public void ShowMenu()
        {
            ShowEasySaveLogo();
            ShowMenuOptions();

            Options = Console.ReadLine();

            switch (Options)
            {
                case "1":
                    this.AddBackupJob();
                    break;

                case "2":
                    this.ExecuteSpecificJob();
                    break;

                case "3":
                    this.ExecuteAllJobs();
                    break;

                case "4":
                    this.ShowCreatedJobs();
                    break;

                case "5":
                    this.DeleteBackupJob();
                    break;

                case "6":
                    Environment.Exit(0);
                    break;

                default:
                    ErrorMessage(" \n Invalid option, please choose between 1 and 5 \n ");
                    Thread.Sleep(1500);
                    Console.Clear();
                    ShowMenu();
                    break;
            }

        }


        public void ShowEasySaveLogo()
        {

            Console.WriteLine(@" 
                                ███████╗ █████╗ ███████╗██╗   ██╗███████╗ █████╗ ██╗   ██╗███████╗
                                ██╔════╝██╔══██╗██╔════╝╚██╗ ██╔╝██╔════╝██╔══██╗██║   ██║██╔════╝
                                █████╗  ███████║███████╗ ╚████╔╝ ███████╗███████║██║   ██║█████╗  
                                ██╔══╝  ██╔══██║╚════██║  ╚██╔╝  ╚════██║██╔══██║╚██╗ ██╔╝██╔══╝  
                                ███████╗██║  ██║███████║   ██║   ███████║██║  ██║ ╚████╔╝ ███████╗
                                ╚══════╝╚═╝  ╚═╝╚══════╝   ╚═╝   ╚══════╝╚═╝  ╚═╝  ╚═══╝  ╚══════╝");
        }



        public void ShowMenuOptions()
        {
                Console.WriteLine("\n");
                CenterOptionText("1- Create a Backup Job");
                CenterOptionText("2- Execute a specific Job");
                CenterOptionText("3- Execute all Backups Job");
                CenterOptionText("4- Show my created Backup Jobs");
                CenterOptionText("5- Delete a Backup Job");
                CenterOptionText("6- Exit");
        }


        public void CenterOptionText(string Text)
        {
            Console.WriteLine(String.Format("{0," + ((Console.WindowWidth / 2) + (Text.Length / 2)) + "}", Text));
            Console.WriteLine("\n");
        }



        public void SuccessMessage(string Text)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(Text);
            Console.ForegroundColor = ConsoleColor.White;
        }

        public void ErrorMessage(string Text)
        {
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine(Text);
            Console.ForegroundColor = ConsoleColor.White;
        }



        public void AddBackupJob()
        {

            Console.Write("\n Enter your backup name : ");
            BackupNameEntry = Console.ReadLine();

            // Check if the backup name is not empty
            while (String.IsNullOrEmpty(BackupNameEntry))
            {
                ErrorMessage("\n The backup name can't be empty ...\n");
                Console.Write("\n Enter your backup name :");
                BackupNameEntry = Console.ReadLine();
            }

            Console.Write("\n Enter the source path of the directory you want to copy :  ");
            SourcePathEntry = Console.ReadLine();


            // Check if the source path is not empty and valid
            while (String.IsNullOrEmpty(SourcePathEntry) || Directory.Exists(SourcePathEntry) != true)
            {
                ErrorMessage("\n Invalid source path, please try again  \n");
                Console.Write("\n Enter the source path of the directory you want to copy :  ");
                SourcePathEntry = Console.ReadLine();
            }

            Console.Write("\n Enter the destination path :  ");
            DestinationPathEntry = Console.ReadLine();

            // Check if the destination path is not empty and valid
            while (String.IsNullOrEmpty(DestinationPathEntry) || Directory.Exists(DestinationPathEntry) != true)
            {
                ErrorMessage("\n Invalid destination path, please try again  \n");
                Console.Write("\n Enter the destination path of the directory you want to copy :  ");
                DestinationPathEntry = Console.ReadLine();
            }


            Console.WriteLine("\n Type of save :  \n");
            Console.WriteLine(" 1- Complete Save \n");
            Console.WriteLine(" 2- Differential Save \n");

            Console.WriteLine("Enter the save type :  \n");
            SaveTypeEntry = Console.ReadLine();

            while (SaveTypeEntry != "1" && SaveTypeEntry != "2")   
            {
                ErrorMessage("\n Invalid save type ! \n");
                Console.WriteLine("Enter the save type :  \n");
                SaveTypeEntry = Console.ReadLine();
            }

            // if type = 1 (=> complete) else (=> Differential)
            TypeEntry = SaveTypeEntry == "1" ? "Complete" : "Differential";

            try
            {
                BackupJob newJob = new BackupJob(BackupNameEntry, SourcePathEntry, DestinationPathEntry, TypeEntry);
                FileDirectoryProcessing config = new FileDirectoryProcessing();
                newJob.WriteOnFile(config.BackupFile, newJob);
            }
            catch (Exception e)
            {
                ErrorMessage("\n An error occurred while creating backup job " + e.ToString());
            }

            SuccessMessage("\n Congratulations, your backup job has been created successfully ! \n");

            ReturnToMenu();
        }




        public void ExecuteSpecificJob()
        {
            ShowBackupsJobs();

            Console.WriteLine("Please choose which job you want to execute : ");
            SpecificBackupJob = Console.ReadLine();
            BackupJob job = new BackupJob();

            while (job.GetSpecificJob(SpecificBackupJob) == null)
            {
                ErrorMessage("The backup name" + SpecificBackupJob + " not found ...");
                Console.WriteLine("Please choose which job you want to execute : ");
                SpecificBackupJob = Console.ReadLine();
            }

            Save SaveJob = new Save();
            SaveJob.SpecificSave(SpecificBackupJob);

            SuccessMessage("\n Backup has been performed ! \n");

            ReturnToMenu();
        }


        public void ExecuteAllJobs()
        {
            if (new FileInfo(ConfigurationManager.AppSettings.Get("BackupFile")).Length != 0)
            {
                Save BackupSave = new Save();
                BackupSave.SaveAlljobs();
            }
            else
            {
                ErrorMessage("\n No backup job found \n");
            }

            SuccessMessage("\n All backup jobs have been backed up. \n");
            ReturnToMenu();
        }

        public void ShowCreatedJobs()
        {

            if (new FileInfo(ConfigurationManager.AppSettings.Get("BackupFile")).Length != 0)
            {
                BackupJob Jobs = new BackupJob();
                Jobs.ShowAllJobs();
            }
            else
            {
                ErrorMessage("\n No created jobs \n");
            }

            ReturnToMenu();
        }


        public void ShowBackupsJobs()
        {

            if (new FileInfo(ConfigurationManager.AppSettings.Get("BackupFile")).Length != 0)
            {
                BackupJob Jobs = new BackupJob();
                Jobs.ShowAllJobs();
            }
            else
            {
                ErrorMessage(" \n No created jobs \n");
            }
        }

        public void DeleteBackupJob()
        {
            ShowBackupsJobs();

            Console.WriteLine("Please choose which BackupJob you want to delete : ");
            SpecificBackupJob = Console.ReadLine();
            BackupJob Job = new BackupJob();

            while (Job.GetSpecificJob(SpecificBackupJob) == null)
            {
                ErrorMessage("The backup name" + SpecificBackupJob + " not found ...");
                Console.WriteLine("Please choose which job you want to execute : ");
                SpecificBackupJob = Console.ReadLine();
            }

            BackupJob Backup = new BackupJob();
            Backup.DeleteSpecificBackup(SpecificBackupJob);

            ShowCreatedJobs();
            SuccessMessage("Backup Job : " + SpecificBackupJob + " deleted successfully.");

            ReturnToMenu();
        }




        public void ReturnToMenu()
        {
            Console.WriteLine("Press 'Enter' to return to the menu");

            while (Console.ReadKey().Key != ConsoleKey.Enter)
            {
                Console.WriteLine("Press 'Enter' to return to the menu");
            }

            Thread.Sleep(1000);
            Console.Clear();

            ShowMenu();           // Return to Menu
        }

    }
}
