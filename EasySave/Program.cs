using System;
using System.Text.Json;


namespace EasySave
{
    class Program
    {

        static void Main(string[] args)
        {
            string backupName = string.Empty;
            string sourcePath = string.Empty; ;
            string destinationPath = string.Empty; ;
            string type = string.Empty;
            string userChoice = string.Empty;

            Console.WriteLine("EASYSAVE");

            Console.WriteLine(
                                "1- Create a Backup Job" +
                                "2- Execute a Backup ");

            userChoice = Console.ReadLine();

            if(userChoice == "1")
            {
                Console.WriteLine("Enter your Backup name :");
                 backupName = Console.ReadLine();

                Console.WriteLine("Enter your source path :");
                sourcePath = Console.ReadLine();

                Console.WriteLine("Enter your destination path :");
                destinationPath = Console.ReadLine();

                Console.WriteLine("Enter your your backup type :");
                type = Console.ReadLine();
            }

            BackupJob newJob = new BackupJob(backupName, sourcePath, destinationPath, type);
            newJob.writeOnBackupFile(@"C:\\Users\\ASUS\\Desktop\\test.txt", newJob);

            string json = JsonSerializer.Serialize(newJob);

            Console.WriteLine(json);
        }
    }
}
