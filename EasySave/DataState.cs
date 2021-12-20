using System;


namespace EasySave
{
    // DataState derives from Data
    class DataState : Data

    {
        // Properties, Setters & Getters
        public bool State { get; private set; }
        public int TotalFilesToCopy { get; private set; }
        public long TotalFilesSize { get; private set; }
        public int NbFilesLeftToDo { get; private set; }
        public string Timestamp { get; set; }

        public int Progression { get; private set; }


        // Constructor
        public DataState(string BackupName, string SourcePath, string DestinationPath, bool State, int TotalFilesToCopy, long TotalFilesSize, int NbFilesLeftToDo, string Timestamp, int Progression)
        {
            this.BackupName = BackupName;
            this.State = State;
            this.Timestamp = Timestamp;
            this.Progression = Progression;
            this.SourcePath = SourcePath;
            this.DestinationPath = DestinationPath;
            this.TotalFilesToCopy = TotalFilesToCopy;
            this.TotalFilesSize = TotalFilesSize;
            this.NbFilesLeftToDo = NbFilesLeftToDo;
        }
     
    }
}
