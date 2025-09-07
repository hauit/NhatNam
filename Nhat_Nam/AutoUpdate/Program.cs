using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Configuration;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using Base.Connect;

namespace AutoUpdate
{
    class Program
    {
        private const int MF_BYCOMMAND = 0x00000000;
        public const int SC_CLOSE = 0xF060;

        [DllImport("user32.dll")]
        public static extern int DeleteMenu(IntPtr hMenu, int nPosition, int wFlags);

        [DllImport("user32.dll")]
        private static extern IntPtr GetSystemMenu(IntPtr hWnd, bool bRevert);

        [DllImport("kernel32.dll", ExactSpelling = true)]
        private static extern IntPtr GetConsoleWindow();

        static void Main(string[] args)
        {
            try
            {
                Thread.Sleep(2000);
                DeleteMenu(GetSystemMenu(GetConsoleWindow(), false), SC_CLOSE, MF_BYCOMMAND);
                FetchCurrentUrl();
                OpenSoftware();
                Environment.Exit(0);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private static void OpenSoftware()
        {
            try
            {
                var curentPath = Directory.GetCurrentDirectory();
                var logSVPath = curentPath + "\\WEBWTS.exe";
                if (logSVPath.Length == 0)
                {
                    return;
                }

                Process.Start(logSVPath);
            }
            catch (Exception ex)
            {
                throw new ArgumentException(ex.Message);
            }
        }

        /// <summary>
        /// function get curent url of folder which include in exe file
        /// </summary>
        private static void FetchCurrentUrl()
        {
            string selectedFolder = GetServerFolder();
            if (string.IsNullOrEmpty(selectedFolder))
            {
                return;
            }

            string currentFolder = Directory.GetCurrentDirectory();
            var tartgetPart = Path.GetDirectoryName(selectedFolder);
            string[] fileList = Directory.GetFiles(tartgetPart);
            if (CheckSoftwareRunning())
            {
                KillProcessRunning();
            }

            Thread.Sleep(2000);
            foreach (var item in fileList)
            {
                var filename = Path.GetFileName(item).Trim();
                if (filename.StartsWith("AutoUpdate") || filename.StartsWith("WEBWTS.vshost.exe") || filename.StartsWith("Base"))
                {
                    continue;
                }
                File.Copy(item, currentFolder + "\\" + filename, true);
                Console.WriteLine("Copy " + filename + " completed.");
            }

            Console.WriteLine("All files are copied.");
            MessageBox.Show("Đã cập nhật xong");
        }

        private static bool CheckSoftwareRunning()
        {
            bool status = true;
            try
            {
                Process[] pname = Process.GetProcessesByName("WEBWTS");
                if (pname.Length == 0)
                {
                    status = false;
                }

            }
            catch (Exception ex)
            {
                status = false;
            }
            return status;
        }

        private static bool KillProcessRunning()
        {
            try
            {
                Process[] pname = Process.GetProcessesByName("WEBWTS");
                foreach (var item in pname)
                {
                    item.Kill();
                }

                return true;
            }
            catch
            {
                return false;
            }
        }

        private static string GetServerFolder()
        {

            string processEnviroment = "32bit";
            if (Environment.Is64BitProcess)
            {
                processEnviroment = "64bit";
            }


            ClsConnect connect = new ClsConnect();
            string sql = $"Select * from [222_PathOfSofware] where ProcessFile = 'WEBWTS' and ProcessEnviroment = '{processEnviroment}' ";
            string svFolder = connect.LoadGridByStr(sql).Rows[0]["Path"].ToString().Trim();
            return svFolder;
        }
    }
}
