using ProcessingWork;
using ProcessingWork.WTS.InputWTS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using WorkingNotification;

namespace NT_Software
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            DevExpress.Data.CurrencyDataController.DisableThreadingProblemsDetection = true;
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new frm_Main());
        }
    }
}
