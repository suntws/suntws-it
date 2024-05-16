using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Configuration;
using System.Reflection;
using System.Linq;

namespace GT
{
    public partial class frmFrictionMaster : Form
    {
        DBAccess dbCon = new DBAccess();
        public frmFrictionMaster()
        {
            InitializeComponent();
        }

        private void frmFrictionMaster_Load(object sender, EventArgs e)
        {
            try
            {
                DataGridViewCellStyle style1 = this.gvOriginMaster.ColumnHeadersDefaultCellStyle;
                style1.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                this.gvOriginMaster.CellClick += new DataGridViewCellEventHandler(gvOriginMaster_SelectionChanged);
                bindType();
                btnClear_Click(null, null);
            }
            catch (Exception ex)
            {
                Program.WriteToGtErrorLog("GT", "frmFrictionMaster", MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        private void Bind_GridView()
        {
            try
            {
                DataTable dt = (DataTable)dbCon.ExecuteReader_SP("SP_LST_OriginMaster_FrictionOrigin", DBAccess.Return_Type.DataTable);
                gvOriginMaster.DataSource = dt;
                gvOriginMaster.Columns[0].HeaderText = "Friction Origin";
            }
            catch (Exception ex)
            {
                Program.WriteToGtErrorLog("GT", "frmFrictionMaster", MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        private void bindType()
        {
            try
            {
                DataTable dtType = (DataTable)dbCon.ExecuteReader_SP("SP_LST_OriginMaster_TyreType", DBAccess.Return_Type.DataTable);
                if (dtType.Rows.Count > 0)
                {
                    int maxHeight = grpPanel.Height - 20;
                    int maxWidth = grpPanel.Width - 20;
                    int curHeight = 0;
                    int curWidth = 5;

                    for (int i = 0; i < dtType.Rows.Count; i++)
                    {
                        CheckBox chk = new CheckBox();
                        chk.Text = dtType.Rows[i]["TyreType"].ToString();
                        if (i > 0) curHeight += 30;
                        if (curHeight >= maxHeight)
                        {
                            curWidth += 120;
                            curHeight = 0;
                        }
                        chk.Location = new Point(curWidth, curHeight);
                        grpPanel.Controls.Add(chk);
                    }
                }
            }
            catch (Exception ex)
            {
                Program.WriteToGtErrorLog("GT", "frmFrictionMaster", MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        private void selCheck()
        {
            try
            {
                SqlParameter[] sp = new SqlParameter[] { new SqlParameter("@FrictionOrigin", textBox1.Text) };
                DataTable dtType = (DataTable)dbCon.ExecuteReader_SP("SP_LST_OriginMaster_TypeOfOrigin", sp, DBAccess.Return_Type.DataTable);
                if (dtType.Rows.Count > 0)
                {
                    foreach (CheckBox chk in grpPanel.Controls)
                    {
                        List<string> lstType = dtType.AsEnumerable().Where(n => n.Field<string>("TyreType").Equals(chk.Text)).Select(n => n.Field<string>("TyreType").ToString()).ToList();
                        if (lstType.Count > 0) { chk.Checked = true; }
                    }
                }
            }
            catch (Exception ex)
            {
                Program.WriteToGtErrorLog("GT", "frmFrictionMaster", MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        private void deSelCheck()
        {
            foreach (CheckBox chk in grpPanel.Controls)
            {
                chk.Checked = false;
                chk.Enabled = true;
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            textBox1.Text = "";
            lblErrMsg.Text = "";
            progressBar1.Value = 0;
            textBox1.Enabled = true;
            btnSave.Text = "SAVE";
            btnSuspend.Visible = false;
            Bind_GridView();
            deSelCheck();
            gvOriginMaster.ClearSelection();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (textBox1.Text == "")
                {
                    lblErrMsg.Text = "Enter Origin";
                    textBox1.Focus();
                }
                else
                {
                    if (btnSave.Text == "SAVE")
                    {
                        SqlParameter[] sp = new SqlParameter[] { new SqlParameter("@FrictionOrigin", textBox1.Text) };
                        DataTable dt = (DataTable)dbCon.ExecuteReader_SP("SP_GET_OriginMaster_DetailOfOrigin", sp, DBAccess.Return_Type.DataTable);
                        gvOriginMaster.DataSource = dt;
                        if (dt.Rows.Count > 0)
                            lblErrMsg.Text = "Friction Origin Already Existing";
                        else
                        {
                            SqlParameter[] sp1 = new SqlParameter[] { new SqlParameter("@FrictionOrigin", textBox1.Text), new SqlParameter("@UserName", Program.strUserName) };
                            if (dbCon.ExecuteNonQuery_SP("sp_ins_OriginMaster", sp1) > 0)
                            {
                                updChkdType();
                                MessageBox.Show("Record successfully saved");
                                btnClear_Click(sender, e);
                            }
                        }
                    }
                    else
                    {
                        updChkdType();
                        MessageBox.Show("Record successfully saved");
                        btnClear_Click(null, null);
                    }
                }
            }
            catch (Exception ex)
            {
                Program.WriteToGtErrorLog("GT", "frmFrictionMaster", MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        private void updChkdType()
        {
            try
            {
                List<string> checkedOrigins = new List<string>();
                foreach (CheckBox chk in grpPanel.Controls)
                {
                    if (chk.Checked == true)
                    {
                        checkedOrigins.Add(chk.Text);
                    }
                }
                SqlParameter[] sp = new SqlParameter[] { new SqlParameter("@FrictionOrigin", textBox1.Text) };
                DataTable dtFrictionOrigin = (DataTable)dbCon.ExecuteReader_SP("SP_LST_OriginMaster_TypeOfOrigin", sp, DBAccess.Return_Type.DataTable);
                List<string> lstFrictionOrigin = dtFrictionOrigin.AsEnumerable().Select(n => n.Field<string>("TyreType")).ToList();
                if (dtFrictionOrigin.Rows.Count > 0)
                {
                    List<string> lstToUncheck = dtFrictionOrigin.AsEnumerable().Where(n => !checkedOrigins.Contains(n.Field<string>("TyreType"))).Select(n => n.Field<string>("TyreType")).ToList();
                    List<string> lstToCheck = checkedOrigins.AsEnumerable().Where(n => !lstFrictionOrigin.Contains(n.ToString())).Select(n => n.ToString()).ToList();
                    foreach (string str in lstToUncheck)
                    {
                        SqlParameter[] sp1 = new SqlParameter[] { new SqlParameter("@FrictionOrigin", textBox1.Text), new SqlParameter("@TyreType", str) };
                        int resp = dbCon.ExecuteNonQuery_SP("SP_UPD_OriginMaster_OriginWiseType", sp1);
                    }

                    foreach (string str in lstToCheck)
                    {
                        SqlParameter[] sp1 = new SqlParameter[] { 
                            new SqlParameter("@FrictionOrigin", textBox1.Text), 
                            new SqlParameter("@TyreType", str), 
                            new SqlParameter("@Username", Program.strUserName) 
                        };
                        int resp = dbCon.ExecuteNonQuery_SP("SP_INS_OriginMaster_OriginWiseType", sp1);
                    }
                }
                else
                {
                    foreach (string str in checkedOrigins)
                    {
                        SqlParameter[] sp1 = new SqlParameter[]{
                            new SqlParameter("@FrictionOrigin",textBox1.Text),
                            new SqlParameter("@TyreType",str),
                            new SqlParameter("@Username",Program.strUserName)
                        };
                        int resp = dbCon.ExecuteNonQuery_SP("SP_INS_OriginMaster_OriginWiseType", sp1);
                    }
                }
            }
            catch (Exception ex)
            {
                Program.WriteToGtErrorLog("GT", "frmFrictionMaster", MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        private void gvOriginMaster_SelectionChanged(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                deSelCheck();
                string value = gvOriginMaster.Rows[e.RowIndex].Cells["FrictionOrigin"].Value.ToString();
                if (value != "")
                {
                    textBox1.Text = gvOriginMaster.Rows[e.RowIndex].Cells["FrictionOrigin"].Value.ToString();
                    textBox1.Enabled = false;
                    selCheck();
                    btnSave.Text = "UPDATE";
                    btnSuspend.Visible = true;
                }
            }
            catch (Exception ex)
            {
                Program.WriteToGtErrorLog("GT", "frmFrictionMaster", MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        private void btnSuspend_Click(object sender, EventArgs e)
        {
            try
            {
                string strRemarks = Microsoft.VisualBasic.Interaction.InputBox("ENTER REASON FOR SUSPEND", "Title", "");
                if (strRemarks != "" && MessageBox.Show(strRemarks, "FRICTION ORIGIN SUSPEND", MessageBoxButtons.OKCancel) == DialogResult.OK)
                {
                    SqlParameter[] spDel = new SqlParameter[] { 
                        new SqlParameter("@Val1", textBox1.Text), 
                        new SqlParameter("@Val2", ""), 
                        new SqlParameter("@Val3", ""),  
                        new SqlParameter("@UserName", Program.strUserName),
                        new SqlParameter("@Category", "FRICTION ORIGIN"),
                        new SqlParameter("@Remarks", ("SUSPEND: " + strRemarks)) 
                    };
                    int resp = dbCon.ExecuteNonQuery_SP("sp_upd_masterdata_suspend", spDel);
                    if (resp > 0)
                    {
                        MessageBox.Show("Origin successfully " + btnSuspend.Text.ToLower() + "ed");
                        updChkdType();
                        btnClear_Click(null, null);
                    }
                }
            }
            catch (Exception ex)
            {
                Program.WriteToGtErrorLog("GT", "frmFrictionMaster", MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
    }
}
