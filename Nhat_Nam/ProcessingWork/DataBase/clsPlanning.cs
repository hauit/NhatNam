using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcessingWork.DAO.Database
{
    public class clsPlanning
    {
        private int id = 0;
        private string k = string.Empty;
        private DateTime date = DateTime.Now.Date;
        private string shift = string.Empty;
        private string machine = string.Empty;
        private string optionID = string.Empty;
        private string mONo = string.Empty;
        private string dept = string.Empty;
        private string note = string.Empty;
        protected bool deleted = false;

        public int ID { get { return id; } set { id = value; } }
        public string K { get { return k ; } set { k  = value; } }
        public DateTime Date { get { return date ; } set { date  = value; } }
        public string Shift { get { return shift ; } set { shift  = value; } }
        public string MachineID { get { return machine ; } set { machine  = value; } }
        public string OptionID { get { return optionID; } set { optionID = value; } }
        public string MONo { get { return mONo ; } set { mONo  = value; } }
        public string Dept { get { return dept ; } set { dept  = value; } }
        public string Note { get { return note; } set { note  = value; } }

        public bool Deleted
        {
            get { return deleted; }
            set { deleted = value; }
        }
    }
}
