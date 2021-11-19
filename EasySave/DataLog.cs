using System;


namespace EasySave
{
    // DataLog derives from Data

    class DataLog : Data
    {
        // Properties, Setters & Getters
        public int Size { get; private set; }
        public DateTime Timestamp { get; private set; }
        public TimeSpan TrasnferTime { get; private set; }


        // Constructor
        public DataLog(string BackupName, string SourcePath, string DestinationPath, DateTime Timestamp, int Size, TimeSpan TrasnferTime)
        {
            this.BackupName = BackupName;
            this.SourcePath = SourcePath;
            this.DestinationPath = DestinationPath;
            this.Timestamp = Timestamp;

            this.Size = Size;
            this.TrasnferTime = TrasnferTime;
        }

        public int GetTransferTime()
        {
            return 0;
        }
    }
}
