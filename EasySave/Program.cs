using System;

namespace EasySave
{
    class Program
    {
        static void Main(string[] args)
        {
            // Generate Config Files
            FileDirectoryProcessing config = new FileDirectoryProcessing();
            config.GenerateConfigFiles();

            // Show Menu 
            Menu menu = new Menu();
            menu.ShowMenu();

        }
    }
}
