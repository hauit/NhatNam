using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPF_Test.Models.Entity
{
    public class Products
    {
        private int id = 0;
        private string name = string.Empty;
        private string note = string.Empty;

        public int ID
        {
            get { return id; }
            set { id = value; }
        }

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public string Note
        {
            get { return note; }
            set { note = value; }
        }
    }
}
