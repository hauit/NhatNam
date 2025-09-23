using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Base.Connect
{
    public partial class frm_Connect : Form
    {
        ClsConnect Connect = new ClsConnect();

        public frm_Connect()
        {
            InitializeComponent();
        }

        private void btnTest_Click(object sender, EventArgs e)
        {
            if (Connect.CheckConnect(txtServer.Text, txtDatabase.Text, txtUser.Text, txtPass.Text) == true)
            {
                MessageBox.Show("Kết nối CSDL thành công");
            }
            else
            {
                MessageBox.Show("Kết nối CSDL thất bại");
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            Connect.WriteConfig(txtServer.Text, txtUser.Text, txtPass.Text, txtDatabase.Text);
            MessageBox.Show("Đã lưu cấu hình thành công!");
            Close();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            try
            {
                switch (keyData)
                {
                    case Keys.Escape:
                        Close();
                        return true;
                }
                return base.ProcessCmdKey(ref msg, keyData);
            }
            catch
            {
                return false;
            }
        }
    }
}
