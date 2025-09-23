using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPF_Test.Models.Entity
{
    public class WebUrl
    {
        public int id = 0;
        public string controller = string.Empty;
        public string action = string.Empty;
        public string localPath = string.Empty;
        public bool updated = false;
        public string note = string.Empty;

        public int ID
        {
            get { return id; }
            set { id = value; }
        }
        public string Controller
        {
            get { return controller; }
            set { controller = value; }
        }
        public string Action
        {
            get { return action; }
            set { action = value; }
        }
        public string LocalPath
        {
            get { return localPath; }
            set { localPath = value; }
        }
        public bool Updated
        {
            get { return updated; }
            set { updated = value; }
        }
        public string Note
        {
            get { return note; }
            set { note = value; }
        }
    }
}
