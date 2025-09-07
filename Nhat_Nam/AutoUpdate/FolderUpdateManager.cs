using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoUpdate
{
    public class FolderUpdateManager
    {
        public string GetFolderUpdate()
        {
            string folder = ReadFile();
            return folder;
        }

        private string ReadFile()
        {
            FileStream myFStream = new FileStream("Info.ini", FileMode.OpenOrCreate, FileAccess.ReadWrite);
            StreamReader ReadFile = new StreamReader(myFStream);
            string folder = ReadFile.ReadLine();
            ReadFile.Close();
            return folder;
        }
    }
}
