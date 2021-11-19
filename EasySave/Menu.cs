using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Threading;
using Newtonsoft.Json;

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


        public void ShowMenu()
        {

            Console.WriteLine("EASYSAVE");

            Console.WriteLine("\n 1- Create a Backup Job \n");
            Console.WriteLine("\n 2- Execute a specific Job \n");
            Console.WriteLine("\n 3- Execute all Jobs \n");
            Console.WriteLine("\n 4- Show my created jobs \n");
            Console.WriteLine("\n 5- Show my saved jobs \n");
            Console.WriteLine("\n 6- Exit \n");

            Console.WriteLine("\nChoose an option : \n");

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
                    this.ShowSavedJobs();
                    break;

                case "6":
                    Environment.Exit(0);
                    break;

                default:
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.WriteLine(" \n Invalid option, please choose between 1 and 5 \n ");
                    Console.ForegroundColor = ConsoleColor.White;
                    Thread.Sleep(1000);
                    ShowMenu();
                    break;
            }

        }

        public void AddBackupJob()
        {

            Console.Write("\n Enter your backup name : ");
            BackupNameEntry = Console.ReadLine();

            // Check if the backup name is not empty
            while (String.IsNullOrEmpty(BackupNameEntry))
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.Write("\n The backup name can't be empty ...\n");

                Console.ForegroundColor = ConsoleColor.White;
                Console.Write("\n Enter your backup name :");
                BackupNameEntry = Console.ReadLine();
            }

            Console.Write("\n Enter the source path of the directory you want to copy :  ");
            SourcePathEntry = Console.ReadLine();


            // Check if the source path is not empty and valid
            while (String.IsNullOrEmpty(SourcePathEntry) || Directory.Exists(SourcePathEntry) != true)
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.Write("\n Invalid source path, please try again  \n");

                Console.ForegroundColor = ConsoleColor.White;
                Console.Write("\n Enter the source path of the directory you want to copy :  ");

                SourcePathEntry = Console.ReadLine();
            }

            Console.Write("\n Enter the destination path :  ");
            DestinationPathEntry = Console.ReadLine();

            // Check if the destination path is not empty and valid
            while (String.IsNullOrEmpty(DestinationPathEntry) || Directory.Exists(DestinationPathEntry) != true)
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.Write("\n Invalid destination path, please try again  \n");

                Console.ForegroundColor = ConsoleColor.White;
                Console.Write("\n Enter the destination path of the directory you want to copy :  ");

                DestinationPathEntry = Console.ReadLine();
            }


            Console.WriteLine("\n Type of save :  \n");
            Console.WriteLine("1- Complete Save \n");
            Console.WriteLine("2- Differential Save \n");
            Console.ForegroundColor = ConsoleColor.White;

            Console.WriteLine("Enter the save type :  \n");
            SaveTypeEntry = Console.ReadLine();

            while (SaveTypeEntry != "1" && SaveTypeEntry != "2")   // A verifier
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.Write("\n Invalid save type ! \n");

                Console.ForegroundColor = ConsoleColor.White;
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
                Console.WriteLine("An error occurred while saving " + e.ToString());
            }

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Congratulations, your backup job has been created successfully !");
            Console.ForegroundColor = ConsoleColor.White;
        }


        public void ExecuteSpecificJob()
        {

        }

        public void ExecuteAllJobs()
        {

        }

        public void ShowCreatedJobs()
        {
            BackupJob Jobs = new BackupJob();
            Jobs.ShowAllJobs();
        }

        public void ShowSavedJobs()
        {

        }

        public void SetLanguage()
        {

        }
    }
}
