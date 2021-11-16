using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace EasySave
{
    class DataState: Data

    {
        public bool state { get; private set; }
        public int TotalFilesToCopy { get; private set; }
        public int TotalFilesSize { get; private set; }
        public int NbFilesLeftToDo { get; private set; }
        public int progression { get; private set; }


        public DataState(string backupName, string sourcePath, string destinationPath, DateTime timestamp, bool state,  int TotalFilesToCopy, int TotalFilesSize, int NbFilesLeftToDo, int progression)
        {
            this.backupName = backupName;
            this.timestamp = timestamp;
            this.state = state;

            if(state == true)
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
