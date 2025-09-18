using Base.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Base.Login
{
    public partial class frm_Login : Form
    {
        public event EventHandler Login;
        public frm_Login()
        {
            InitializeComponent();
        }

        private void btnClose_Click(object sender, EventArgs e)
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

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (txtTaiKhoan.Text == "")
            {
                MessageBox.Show("Chưa nhập tài khoản");
            }
            else if (txtMatKhau.Text == "")
            {
                MessageBox.Show("Chưa nhập mật khẩu");
            }
            else
            {
                try
                {
                    ClsLoginDAO login = new ClsLoginDAO();
                    DataTable dt;
                    dt = login.CheckLogin(txtTaiKhoan.Text, txtMatKhau.Text.Trim());
                    if (dt.Rows.Count > 0)
                    {
                        ClsSession.User = dt.Rows[0]["UserName"].ToString().Trim();
                        ClsSession.staffID = dt.Rows[0]["StaffID"].ToString().Trim();
                        DataTable dt1 = login.LoadGridByStr("select * from [222_Staff] where StaffID = N'" + ClsSession.staffID + "'");
                        ClsSession.StaffName = dt1.Rows[0]["StaffName"].ToString().Trim();
                        ClsSession.SecID = dt1.Rows[0]["SecID"].ToString().Trim();
                        ClsSession.lv = int.Parse(dt1.Rows[0]["level"].ToString().Trim().Length == 0 ? "0" : dt1.Rows[0]["level"].ToString().Trim());
                        Login(sender, e);
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("Đăng nhập không thành công");
                    }
                }
                catch(Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void txtTaiKhoan_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnSave_Click(sender, e);
            }
        }
    }
}
