using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Base.Connect
{
    public partial class Frm_FolderUpdate : Form
    {
        ClsConnect connect = new ClsConnect();
        public Frm_FolderUpdate()
        {
            InitializeComponent();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            connect.WriteFolderUpdate(txtFolder.Text);
            MessageBox.Show("OK");
        }
    }
}
