using System;


namespace EasySave
{
    // DataState derives from Data
    class DataState : Data

    {
        // Properties, Setters & Getters
        public bool State { get; private set; }
        public int TotalFilesToCopy { get; private set; }
        public int TotalFilesSize { get; private set; }
        public int NbFilesLeftToDo { get; private set; }
        public DateTime Timestamp { get; set; }

        public int Progression { get; private set; }


        // Constructor
        public DataState(string BackupName, string SourcePath, string DestinationPath, bool State, int TotalFilesToCopy, int TotalFilesSize, int NbFilesLeftToDo, DateTime Timestamp, int Progression)
        {
            this.BackupName = BackupName;
            this.State = State;
            this.Timestamp = Timestamp;

            if (State == true)
            {
                this.SourcePath = SourcePath;
                this.DestinationPath = DestinationPath;
                this.TotalFilesToCopy = TotalFilesToCopy;
                this.TotalFilesSize = TotalFilesSize;
                this.NbFilesLeftToDo = NbFilesLeftToDo;
                this.Progression = Progression;
            }
        }

        public int GetFileToCopy()
        {
            return 0;
        }

        public int GetSizeLeft()
        {
            return 0;
        }

        public string GetCurrentFileAdressSource()
        {
            return "0";
        }

        public string GetFileAdressDestination()
        {
            return "0";
        }
    }
}
