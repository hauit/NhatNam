using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcessingWork.DataBase
{
    public class clsP_NhapOrderlapKHGC
    {
        private DateTime date = DateTime.Now.Date;
        private string mONumber = string.Empty;
        private string note = string.Empty;

        public DateTime Date { get { return date; } set { date = value; } }
        public string MONumber { get { return mONumber; } set { mONumber = value; } }
        public string Note { get { return note; } set { note = value; } }

    }
}
