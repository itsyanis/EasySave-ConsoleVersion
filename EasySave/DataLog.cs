using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace EasySave
{
    class DataLog
    {
        public string backupName { get; private set; }
        public string sourcePath { get; private set; }
        public string destinationPath { get; private set; }
        public int size { get; private set; }
        public DateTime timestamp { get; private set; }
        public int trasnferTime { get; private set; }



        public DataLog (string backupName, string sourcePath, string destinationPath, int size, DateTime timestamp, int trasnferTime)
        {
            this.backupName = backupName;
            this.sourcePath = sourcePath;
            this.destinationPath = destinationPath;
            this.size = size;
            this.timestamp = timestamp;
            this.trasnferTime = trasnferTime;
        }


        public void writeOnLogFile(string path, DataLog LogInformations )
        {
            string JsonInformations = JsonConvert.SerializeObject(LogInformations);

            using (StreamWriter LogFile = File.CreateText(path))    
            {
                LogFile.WriteLine(JsonInformations);
            }
        }
    }
}
