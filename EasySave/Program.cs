using Newtonsoft.Json;
using System;

namespace EasySave
{
    class Program
    {

        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            
            BackupJob newJob = new BackupJob("ma sauvegarde", "@C:\\Users\\ASUS\\Desktop\\file.txt", "@C:\\Users\\Domuments\\file.txt", "Complete");
           
            string json = JsonConvert.SerializeObject(newJob);

            Console.WriteLine(json);
        }
    }
}
