using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.Base
{
    public static class ClsSession
    {
        //public static List<string> chucnang = new List<string> { };
        static string Year = DateTime.Now.Year.ToString();
        public static string User;
        public static string Pass;
        public static string staffID;
        public static string StaffName;
        public static string SecID;
        public static int lv;

        public static string FolderPO = @"\\192.168.0.5\Business\Production Control\IMPORT\Import\PURCHASE ODER\Purchase " + Year;

        public static string DomainMail = "outlook.office365.com";
    }
}
