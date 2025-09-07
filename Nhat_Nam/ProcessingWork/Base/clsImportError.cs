using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcessingWork.Base
{
    public class clsImportError
    {
        public int Line { get;  }
        public string Obj { get; }
        public string Des { get;  }

        public clsImportError(int line, string des,string obj = "")
        {
            this.Obj = obj;
            this.Line = line;
            this.Des = des;
        }
    }
}
