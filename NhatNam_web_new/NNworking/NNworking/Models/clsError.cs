using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NNworking.Models
{
    public class clsError
    {
        public int Line { get; set; }
        public string Des { get; set; }
        public string Status { get; set; }

        public clsError(int line, string Status, string Des)
        {
            this.Line = line;
            this.Des = Des;
            this.Status = Status;
        }

    }
}