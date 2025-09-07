using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcessingWork.DataBase
{
    public class clsMOOption
    {
        public int ID         { get; set; }
        public string MONo	   { get; set; }
        public string PartID	   { get; set; }
        public int MOQty	   { get; set; }
        public DateTime Deadline   { get; set; }
        public string MachineID  { get; set; }
        public string OptionID   { get; set; }
        public bool LastOption { get; set; }
        public string JigType	   { get; set; }
        public float ProTime	   { get; set; }
        public float ClampTime  { get; set; }
        public bool Finished   { get; set; }
        public DateTime Finishdate { get; set; }
        public int Shift	   { get; set; }
        public bool Note	   { get; set; }
        public int Deleted { get; set; }
    }
}
