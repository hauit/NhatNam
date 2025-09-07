using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcessingWork.DataBase
{
    public class clsDepartment
    {
        private int id = 0;
        private string deptCode = string.Empty;
        private string deptName = string.Empty;
        private bool deleted = false;

        public int ID { get { return id; } set { id = value; } }
        public string DeptCode { get { return deptCode; } set { deptCode = value; } }
        public string DeptName { get { return deptName; } set { deptName = value; } }
        public bool Deleted { get { return deleted; } set { deleted = value; } }
    }
}
