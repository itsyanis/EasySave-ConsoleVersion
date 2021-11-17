using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace EasySave
{
    class Data
    {
        // Properties, Setters & Getters
        public string backupName { get; set; }
        public string sourcePath { get; set; }
        public string destinationPath { get; set; }


        // Default Constructor  
        public Data()
        {
            backupName = backupName;
            sourcePath = sourcePath;
            destinationPath = destinationPath;
        }


        // Constructor
        public Data(string backupName, string sourcePath, string destinationPath)
        {
            this.backupName = backupName;
            this.sourcePath = sourcePath;
            this.destinationPath = destinationPath;
        }

        // This method will create the 'File' if it hasn't been created, and replenish it with the informations
        public void writeOnFile(string path, object Informations)
        {
            string JsonInformations = JsonConvert.SerializeObject(Informations);     // Convert DataLog informations to JSON 

         try
            {
                if (File.Exists(path) == true)                                              // Check if 'File' exist
                {
                    using (StreamWriter logFile = File.AppendText(path))                    // If the 'File' exist just append the JSON informations
                    {
                        logFile.WriteLine(JsonInformations);
                    }
                }
                else
                {
                    using (StreamWriter logFile = File.CreateText(path))                     // If the 'File' doesn't exist we create it and put ON Json informations
                    {
                        logFile.WriteLine(JsonInformations);
                    }
                }

            }catch(Exception e)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Une erreur est survenue ... ");
                Console.ForegroundColor = ConsoleColor.White;
            }
        }
    }
}
