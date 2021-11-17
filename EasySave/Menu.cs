using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Threading;

namespace EasySave
{
    class Menu
    {
        public string options;
        public string backupName;
        public string sourcePath;
        public string destinationPath;
        public string type;
        public string saveType;


        public void showMenu()
        {

            Console.WriteLine("EASYSAVE");

            Console.WriteLine("\n 1- Create a Backup Job \n");
            Console.WriteLine("\n 2- Execute a specific Job \n");
            Console.WriteLine("\n 3- Execute all Jobs \n");
            Console.WriteLine("\n 4- Show my created jobs \n");
            Console.WriteLine("\n 5- Exit \n");

            Console.WriteLine("\nChoose an option : \n");

            options = Console.ReadLine();

            switch (options)
            {
                case "1":

                    Console.Write("\n Enter your backup name : ");
                    backupName = Console.ReadLine();


                    // Check if the backup name is not empty
                    while (String.IsNullOrEmpty(backupName))
                    {
                        Console.ForegroundColor = ConsoleColor.DarkRed;
                        Console.Write("\n The backup name can't be empty ...\n");

                        Console.ForegroundColor = ConsoleColor.White;
                        Console.Write("\n Enter your backup name : ");
                        backupName = Console.ReadLine();
                    }

                    Console.Write("\n Enter the source path of the directory you want to copy :  ");
                    sourcePath = Console.ReadLine();


                    // Check if the source path is not empty and valid
                    while (String.IsNullOrEmpty(sourcePath) || Directory.Exists(sourcePath) != true)
                    {
                        Console.ForegroundColor = ConsoleColor.DarkRed;
                        Console.Write("\n Invalid source path, please try again  \n");

                        Console.ForegroundColor = ConsoleColor.White;
                        Console.Write("\n Enter the source path of the directory you want to copy :  ");

                        sourcePath = Console.ReadLine();
                    }

                    Console.Write("\n Enter the destination path :  ");
                    destinationPath = Console.ReadLine();

                    // Check if the destination path is not empty and valid
                    while (String.IsNullOrEmpty(destinationPath) || Directory.Exists(destinationPath) != true)
                    {
                        Console.ForegroundColor = ConsoleColor.DarkRed;
                        Console.Write("\n Invalid destination path, please try again  \n");

                        Console.ForegroundColor = ConsoleColor.White;
                        Console.Write("\n Enter the destination path of the directory you want to copy :  ");

                        destinationPath = Console.ReadLine();
                    }


                    Console.WriteLine("\n Type of save :  \n");
                    Console.WriteLine("1- Complete Save \n");
                    Console.WriteLine("2- Differential Save \n");
                    Console.ForegroundColor = ConsoleColor.White;

                    Console.WriteLine("Enter the save type :  \n");
                    saveType = Console.ReadLine();

                    while (saveType != "1" && saveType != "2")
                    {
                        Console.ForegroundColor = ConsoleColor.DarkRed;
                        Console.Write("\n Invalid save type ! \n");

                        Console.ForegroundColor = ConsoleColor.White;
                        Console.WriteLine("Enter the save type :  \n");
                        type = Console.ReadLine();
                    }

                    // if type = 1 (=> complete) else (=> Differential)
                    type = saveType == "1" ? "Complete" : "Differential";

                    try
                    {
                        BackupJob newJob = new BackupJob(backupName, sourcePath, destinationPath, type);
                        newJob.writeOnFile(@"C:\EasySave\Backup.txt", newJob);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("An error occurred while saving " + e);
                    }

                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("Congratulations, your backup job has been created successfully !");
                    Console.ForegroundColor = ConsoleColor.White;

                    break;

                case "2":
                    break;


                case "3":
                    break;

                case "4":

                    break;

                case "5":
                    Environment.Exit(0);
                    break;

                default:

                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.WriteLine(" \n Invalid option, please choose between 1 and 5 \n ");
                    Console.ForegroundColor = ConsoleColor.White;
                    Thread.Sleep(1000);
                    showMenu();
                    break;
            }

        }

        public void setLanguage()
        {

        }
    }
}
