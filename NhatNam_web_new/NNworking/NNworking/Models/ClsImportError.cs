using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NNworking.Models
{
    public class ClsImportError
    {
        public int Line { get; set; }
        public string Des { get; set; }

        public ClsImportError(int line, string Des)
        {
            this.Line = line;
            this.Des = Des;
        }
    }
}