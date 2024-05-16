using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Configuration;
using System.Diagnostics;

namespace GT
{
    public partial class frmChangePass : Form
    {
        DBAccess dba = new DBAccess();
        public frmChangePass()
        {
            InitializeComponent();

            this.KeyPreview = true;
            this.txtCurrPass.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txt_TextChanged);
            this.txtNewPass.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txt_TextChanged);
            this.txtConfPass.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txt_TextChanged);
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            txtCurrPass.Text = "";
            txtNewPass.Text = "";
            txtConfPass.Text = "";
            chkShowPass.Checked = false;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (txtCurrPass.Text == "")
            {
                lblErrMsg.Text = "Enter current password";
                txtCurrPass.Focus();
            }
            else if (txtCurrPass.Text != Program.strCurrPass)
            {
                lblErrMsg.Text = "Current password incorrect";
                txtCurrPass.Focus();
            }
            else if (txtNewPass.Text == "")
            {
                lblErrMsg.Text = "Enter new password";
                txtNewPass.Focus();
            }
            else if (txtConfPass.Text == "")
            {
                lblErrMsg.Text = "Enter confirm password";
                txtConfPass.Focus();
            }
            else if (txtNewPass.Text != txtConfPass.Text)
                lblErrMsg.Text = "Password mismatch";
            else
            {
                SqlParameter[] sp1 = new SqlParameter[] { new SqlParameter("@UserName", Program.strUserName), new SqlParameter("@Password1", txtNewPass.Text) };
                int resp = dba.ExecuteNonQuery_SP("sp_upd_UserPasswrod", sp1);
                if (resp > 0)
                {
                    MessageBox.Show("Password Changed Successfully");
                    Application.Exit();
                    Process.Start(AppDomain.CurrentDomain.BaseDirectory + "GT.exe");
                }
            }
        }

        private void frmChangePass_Load(object sender, EventArgs e)
        {
            lblErrMsg.Text = "";
        }

        private void txt_TextChanged(object sender, EventArgs e)
        {
            TextBox txt = sender as TextBox;
            if (chkShowPass.Checked)
                txt.PasswordChar = '\0';
            else
                txt.PasswordChar = '*';
        }

        private void chkShowPass_CheckedChanged(object sender, EventArgs e)
        {
            txtCurrPass.PasswordChar = '*';
            txtNewPass.PasswordChar = '*';
            txtConfPass.PasswordChar = '*';
            if (chkShowPass.Checked)
            {
                txtCurrPass.PasswordChar = '\0';
                txtNewPass.PasswordChar = '\0';
                txtConfPass.PasswordChar = '\0';
            }
        }
    }
}
