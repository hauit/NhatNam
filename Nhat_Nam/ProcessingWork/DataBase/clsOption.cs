using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcessingWork.DataBase
{
    public class clsOption
    {
        private int iD = 0;
        private string deptCode = string.Empty;
        private string optionID = string.Empty;
        private string note = string.Empty;
        private DateTime inputTime = DateTime.Now;
        private bool deleted = false;
        public int ID { get{return iD;}set{iD = value;}}
        public string DeptCode { get{return deptCode;}set{deptCode = value;}}
        public string OptionID { get{return optionID;}set{optionID = value;}}
        public string Note { get{return note;}set{note = value;}}
        public DateTime InputTime { get{return inputTime;}set{inputTime = value;}}
        public bool Deleted { get{return deleted; }set{deleted = value;}}

    }
}
