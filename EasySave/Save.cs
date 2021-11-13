using System;
using System.Collections.Generic;
using System.IO;
using System.Text;


namespace EasySave
{
    class Save
    {
        public void completeSave(string sourcePath, string destinationPath)
        {
            try
            {
                File.Copy(sourcePath, destinationPath, true);

            }
            catch(Exception ex)
            {
                Console.WriteLine("Erreur",ex);
            }

        }
    }
}
