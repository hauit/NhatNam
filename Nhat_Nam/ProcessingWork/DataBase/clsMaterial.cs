using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcessingWork.DataBase
{
    public class clsMaterial
    {
        public int id = 0;
        public string materialID = string.Empty;
        public float density = 0;
        public decimal unitPrice = 0;
        public string type = string.Empty;
        public DateTime dateModified = DateTime.Now;
        public string staffID = string.Empty;
        protected bool deleted = false;
        public int ID { get; set; }
        public string MaterialID { get { return materialID; } set { materialID = value; } }
        public float Density { get { return density; } set { density = value; } }
        public decimal UnitPrice { get { return unitPrice; } set { unitPrice = value; } }
        public string Type { get { return type; } set { type = value; } }
        public DateTime DateModified { get { return dateModified; } set { dateModified = value; } }
        public string StaffID { get { return staffID; } set { staffID = value; } }

        public bool Deleted
        {
            get { return deleted; }
            set { deleted = value; }
        }

    }
}
