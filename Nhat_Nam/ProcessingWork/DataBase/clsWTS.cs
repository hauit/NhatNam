using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcessingWork.DataBase
{
    public class clsWTS
    {
        private int id = 0;
        private int idPlan = 0;
        private DateTime date = DateTime.Now.Date;
        private string staffID = string.Empty;
        private string shift = string.Empty;
        private string machine = string.Empty;
        private string optionID = string.Empty;
        private string mONo = string.Empty;
        private string workID = string.Empty;
        private decimal time = 0;
        private int okQty = 0;
        private int ngQty = 0;
        private int nGNCTruoc = 0;
        private string note = string.Empty;
        private string note1 = string.Empty;
        private string note2 = string.Empty;
        private string partID = string.Empty;
        private string proTime = "0";
        private string clampTime = "0";
        protected bool status = false;
        protected bool deleted = false;

        public int ID { get { return id; } set { id = value; } }
        public int IDPlan { get { return idPlan; } set { idPlan = value; } }
        public DateTime Date { get { return date; } set { date = value; } }
        public string StaffID { get { return staffID; } set { staffID = value; } }
        public string Shift { get { return shift; } set { shift = value; } }
        public string MachineID { get { return machine; } set { machine = value; } }
        public string OptionID { get { return optionID; } set { optionID = value; } }
        public string MONo { get { return mONo; } set { mONo = value; } }
        public string WorkID { get { return workID; } set { workID = value; } }
        public decimal Time { get { return time; } set { time = value; } }
        public int OKQty { get { return okQty; } set { okQty = value; } }
        public int NGQty { get { return ngQty; } set { ngQty = value; } }
        public int NGNCTruoc { get { return nGNCTruoc; } set { nGNCTruoc = value; } }
        public string Note { get { return note; } set { note = value; } }
        public string Note1 { get { return note1; } set { note1 = value; } }
        public string Note2 { get { return note2; } set { note2 = value; } }
        public string PartID { get { return partID; } set { partID = value; } }
        public string ProTime { get { return proTime; } set { proTime = value; } }
        public string ClampTime { get { return clampTime; } set { clampTime = value; } }

        public bool Status
        {
            get { return status; }
            set { status = value; }

        }
        public bool Deleted
        {
            get { return deleted; }
            set { deleted = value; }
        }
    }
}
