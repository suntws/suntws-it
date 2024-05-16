using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.Configuration;
using System.Data.SqlClient;
using System.Reflection;

namespace GT
{
    public partial class frmLogin : Form
    {
        DBAccess dba = new DBAccess();
        public frmLogin()
        {
            InitializeComponent();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            try
            {
                if (textBox1.Text == "")
                {
                    lblErr.Text = "Enter username";
                    textBox1.Focus();
                }
                else if (textBox2.Text == "")
                {
                    lblErr.Text = "Enter password";
                    textBox2.Focus();
                }
                else if (textBox1.Text != "" && textBox2.Text != "")
                {
                    SqlParameter[] sp = new SqlParameter[]{
                        new SqlParameter("@Username",textBox1.Text),
                        new SqlParameter("@password",textBox2.Text)
                    };
                    DataTable dt = (DataTable)dba.ExecuteReader_SP("SP_CHK_Login_USer", sp, DBAccess.Return_Type.DataTable);
                    if (dt.Rows.Count == 1)
                    {
                        Program.strUserName = dt.Rows[0]["UserName"].ToString();
                        Program.strCurrPass = dt.Rows[0]["Password1"].ToString();
                        this.Hide();
                        Form mainForm = new MdiForm();
                        mainForm.Show();
                        mainForm.WindowState = FormWindowState.Maximized;

                        DataTable dtTtsLog = (DataTable)dba.ExecuteReader_SP("sp_sel_ttslog", DBAccess.Return_Type.DataTable);
                        if (dtTtsLog.Rows.Count == 1)
                        {
                            Program.strServerDbPath1 = Program.strLocalDbPath != "." ? EncryptDecrypt.Decrypt(dtTtsLog.Rows[0]["ServerSource1"].ToString(), "STOCKMERGE") : ".";
                            Program.strServerDbPath2 = Program.strLocalDbPath != "." ? EncryptDecrypt.Decrypt(dtTtsLog.Rows[0]["ServerSource2"].ToString(), "STOCKMERGE") : ".";
                            Program.strServerDbPass = Program.strLocalDbPath != "." ? EncryptDecrypt.Decrypt(dtTtsLog.Rows[0]["ServerDbPass"].ToString(), "STOCKMERGE") : "abcd@abcd";
                        }

                        Program.dt_Wt_Com = (DataTable)dba.ExecuteReader_SP("sp_sel_Wt_Communication", DBAccess.Return_Type.DataTable);
                        DataRow dr = Program.dt_Wt_Com.NewRow();
                        dr.ItemArray = new object[] { "CHOOSE" };
                        Program.dt_Wt_Com.Rows.InsertAt(dr, 0);
                    }
                    else
                        lblErr.Text = "Invalid username / password";
                }
            }
            catch (Exception ex)
            {
                Program.WriteToGtErrorLog("GT", "frmLogin", System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        private void frmLogin_Load(object sender, EventArgs e)
        {
            this.KeyPreview = true;
            this.textBox1.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textBox1_KeyPress);
            this.textBox2.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textBox2_KeyPress);
            label4.Text = Program.strPlantName + " GT-SOFT {V" + FileVersionInfo.GetVersionInfo(Assembly.GetEntryAssembly().Location).FileVersion + "}";
            lblErr.Text = "";
        }

        private void textBox1_KeyPress(object sender, KeyEventArgs e)
        {
            if (textBox1.Text != "" && e.KeyValue == 13)
                textBox2.Focus();
        }

        private void textBox2_KeyPress(object sender, KeyEventArgs e)
        {
            if (textBox2.Text != "" && e.KeyValue == 13)
                btnLogin_Click(sender, e);
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
