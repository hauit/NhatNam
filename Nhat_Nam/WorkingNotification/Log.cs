using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkingNotification
{
    public static class Log
    {
        public static void WriteErrorLog(Exception ex)
        {
            StreamWriter sw = null;
            try
            {
                sw = new StreamWriter(AppDomain.CurrentDomain.BaseDirectory + @"\LogFile.txt", true);
                sw.WriteLine(DateTime.Now.ToString() + ": " + ex.Message);
                sw.Flush();
                sw.Close();
            }
            catch
            {

            }
        }

        public static void WriteErrorLog(string message)
        {
            StreamWriter sw = null;
            try
            {
                sw = new StreamWriter(AppDomain.CurrentDomain.BaseDirectory + @"\LogFile.txt", true);
                sw.WriteLine(DateTime.Now.ToString() + ": " + message);
                sw.Flush();
                sw.Close();
            }
            catch
            {

            }
        }
    }
}
