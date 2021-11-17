using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace EasySave
{
    // DataLog derives from Data

    class DataLog : Data
    {
        // Properties, Setters & Getters
        public int size { get; private set; }
        public DateTime timestamp { get; private set; }
        public int trasnferTime { get; private set; }


        // Constructor
        public DataLog(string backupName, string sourcePath, string destinationPath, DateTime timestamp, int size, int trasnferTime)
        {
            this.backupName = backupName;
            this.sourcePath = sourcePath;
            this.destinationPath = destinationPath;
            this.timestamp = timestamp;

            this.size = size;
            this.trasnferTime = trasnferTime;
        }
    }
}
