using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcessingWork.DataBase
{
    public class clsOptionData
    {
        #region Members
        protected int iD = 0;
        protected DateTime inputDate = DateTime.Now;
        protected string partID = string.Empty;
        protected string machineID = string.Empty;
        protected string optionID = string.Empty;
        protected bool lastOption = false;
        protected string jigID = string.Empty;
        protected int toolQty = 0;
        protected double proTime = 0;
        protected double clampTime = 0;
        protected bool techDate = false;
        protected DateTime updateDay = DateTime.Now;
        protected string staffID = string.Empty;
        protected string note = string.Empty;
        protected string aondNote = string.Empty;
        protected string memo = string.Empty;
        protected DateTime cLUpdateday = DateTime.Now;
        protected bool jig = false;
        protected string jigType = string.Empty;
        protected float tich = 0;
        protected float doc = 0;
        protected float timeTreo = 0;
        protected float timeComplete = 0;
        protected bool deleted = false;
        #endregion
        #region Public Properties
        
        public bool Deleted
        {
            get { return deleted; }
            set { deleted = value; }
        }

        public float Tich
        {
            get { return tich; }
            set { tich = value; }
        }
        public float Doc
        {
            get { return doc; }
            set { doc = value; }
        }
        public float TimeTreo
        {
            get { return timeTreo; }
            set { timeTreo = value; }
        }
        public float TimeComplete
        {
            get { return timeComplete; }
            set { timeComplete = value; }
        }

        public bool Jig
        {
            get { return jig; }
            set { jig = value; }
        }

        public string JigType
        {
            get { return jigType; }
            set { jigType = value; }
        }

        public int ID
        {
            get { return iD; }
            set { iD = value; }
        }

        public DateTime InputDate
        {
            get { return inputDate; }
            set
            {

                inputDate = value;
            }
        }

        public string PartID
        {
            get { return partID; }
            set { partID = value; }
        }

        public string MachineID
        {
            get { return machineID; }
            set { machineID = value; }
        }

        public string OptionID
        {
            get { return optionID; }
            set { optionID = value; }
        }

        public bool LastOption
        {
            get { return lastOption; }
            set { lastOption = value; }
        }

        public string JigID
        {
            get { return jigID; }
            set { jigID = value; }
        }

        public int ToolQty
        {
            get { return toolQty; }
            set { toolQty = value; }
        }

        public double ProTime
        {
            get { return proTime; }
            set { proTime = value; }
        }

        public double ClampTime
        {
            get { return clampTime; }
            set { clampTime = value; }
        }

        public bool TechDate
        {
            get { return techDate; }
            set { techDate = value; }
        }

        public DateTime UpdateDay
        {
            get { return updateDay; }
            set { updateDay = value; }
        }

        public string StaffID
        {
            get { return staffID; }
            set { staffID = value; }
        }

        public string Note
        {
            get { return note; }
            set { note = value; }
        }

        public string AondNote
        {
            get { return aondNote; }
            set { aondNote = value; }
        }

        public string Memo
        {
            get { return memo; }
            set { memo = value; }
        }
        
        public DateTime CLUpdateday
        {
            get { return cLUpdateday; }
            set { cLUpdateday = value; }
        }
        #endregion

    }
}
