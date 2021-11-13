using System;
using System.Collections.Generic;
using System.Text;

namespace EasySave
{
    class BackupStateData
    {
        public string backupName { get; private set; }
        public DateTime timestamp { get; private set; }
        public bool state { get; private set; }
        public string sourcePath { get; private set; }
        public string destinationPath { get; private set; }
        public int TotalFilesToCopy { get; private set; }
        public int TotalFilesSize { get; private set; }
        public int NbFilesLeftToDo { get; private set; }
        public int progression { get; private set; }


        public BackupStateData(string backupName, DateTime timestamp, bool state, string sourcePath, string destinationPath, int TotalFilesToCopy, int TotalFilesSize, int NbFilesLeftToDo, int progression)
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


        public void writeOnStateFile(string path, BackupStateData StateInformations)
        {

        }


    }
}
