using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace EasySave
{
    class BackupJob
    {

        public string name { get; private set; }
        public string sourcePath { get; private set; }
        public string destinationPath { get; private set; }
        public string type { get; private set; }


        public BackupJob(string name, string sourcePath, string destinationPath, string type)
        {
            this.name = name;
            this.sourcePath = sourcePath;
            this.destinationPath = destinationPath;
            this.type = type;
        }

    }
}
