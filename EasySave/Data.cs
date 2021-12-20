using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace EasySave
{
    class Data
    {
        // Properties, Setters & Getters
        public string BackupName { get; set; }
        public string SourcePath { get; set; }
        public string DestinationPath { get; set; }


        // Default Constructor  
        public Data()
        {
            this.BackupName = BackupName;
            this.SourcePath = SourcePath;
            this.DestinationPath = DestinationPath;
        }


        // Constructor
        public Data(string BackupNameEntry, string SourcePathEntry, string DestinationPathEntry)
        {
            this.BackupName = BackupNameEntry;
            this.SourcePath = SourcePathEntry;
            this.DestinationPath = DestinationPathEntry;
        }

        // This method will create the 'File' if it hasn't been created, and replenish it with the informations
        public void WriteOnFile(string Path, object Informations)
        {
            string JsonInformations = JsonConvert.SerializeObject(Informations);              // Convert object informations to JSON 

            try
            {
                if (File.Exists(Path) == true)                                              
                {
                    using (StreamWriter logFile = File.AppendText(Path))                    // If the 'File' exist just append the JSON informations
                    {
                        logFile.WriteLine(JsonInformations);
                    }
                }
                else
                {
                    using (StreamWriter logFile = File.CreateText(Path))                     // If the 'File' doesn't exist we create it and put ON Json informations
                    {
                        logFile.WriteLine(JsonInformations);
                    }
                }
            }
            catch (Exception e)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("An error occurred during" + e.ToString());
                Console.ForegroundColor = ConsoleColor.Gray;
            }
        }



        // This method will create the 'File' if it hasn't been created, and replenish it with the DataLog information
        public void WriteOnFile(string Path, DataLog Informations)
        {
            string JsonInformations = JsonConvert.SerializeObject(Informations, Formatting.Indented);              // Convert DataLog informations to JSON 

            try
            {
                if (File.Exists(Path) == true)
                {
                    using (StreamWriter logFile = File.AppendText(Path))                    // If the 'File' exist just append the JSON informations
                    {
                        logFile.WriteLine(JsonInformations);
                    }
                }
                else
                {
                    using (StreamWriter logFile = File.CreateText(Path))                     // If the 'File' doesn't exist we create it and put ON Json informations
                    {
                        logFile.WriteLine(JsonInformations);
                    }
                }
            }
            catch (Exception e)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("An error occurred during" + e.ToString());
                Console.ForegroundColor = ConsoleColor.Gray;
            }
        }

    }
}