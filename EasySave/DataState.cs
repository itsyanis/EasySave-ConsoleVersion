using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace EasySave
{
    // DataState derives from Data
    class DataState : Data

    {
        // Properties, Setters & Getters
        public bool state { get; private set; }
        public int TotalFilesToCopy { get; private set; }
        public int TotalFilesSize { get; private set; }
        public int NbFilesLeftToDo { get; private set; }
        protected DateTime timestamp { get; set; }

        public int progression { get; private set; }


        // Constructor
        public DataState(string backupName, string sourcePath, string destinationPath, bool state, int TotalFilesToCopy, int TotalFilesSize, int NbFilesLeftToDo, DateTime timestamp, int progression)
        {
            this.backupName = backupName;
            this.state = state;
            this.timestamp = timestamp;

            if (state == true)
            {
                this.sourcePath = sourcePath;
                this.destinationPath = destinationPath;
                this.TotalFilesToCopy = TotalFilesToCopy;
                this.TotalFilesSize = TotalFilesSize;
                this.NbFilesLeftToDo = NbFilesLeftToDo;
                this.progression = progression;
            }
        }
    }
}
