using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Configuration;

namespace GT
{
    public partial class frmPressMaster : Form
    {
        DBAccess dba = new DBAccess();
        public frmPressMaster()
        {
            InitializeComponent();
        }
        private void frmPressMaster_Load(object sender, EventArgs e)
        {
            try
            {
                DataGridViewCellStyle style1 = this.dgv_PressMaster.ColumnHeadersDefaultCellStyle;
                style1.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                this.dgv_PressMaster.CellClick += new DataGridViewCellEventHandler(dgv_PressMaster_SelectionChanged);

                DataTable dtCategory = (DataTable)dba.ExecuteReader_SP("sp_sel_processid_category", DBAccess.Return_Type.DataTable);
                if (dtCategory.Rows.Count > 0)
                {
                    DataRow toInsert = dtCategory.NewRow();
                    toInsert.ItemArray = new object[] { "CHOOSE" };
                    dtCategory.Rows.InsertAt(toInsert, 0);

                    cmbCategory.DataSource = dtCategory;
                    cmbCategory.DisplayMember = "CATEGORY";
                    cmbCategory.ValueMember = "CATEGORY";

                    if (cmbCategory.Items.Count == 2)
                        cmbCategory.SelectedIndex = 1;
                }

                btnClear_Click(null, null);
            }
            catch (Exception ex)
            {
                Program.WriteToGtErrorLog("GT", "frmPressMaster", System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        private void Bind_GridView()
        {
            try
            {
                DataTable dt = (DataTable)dba.ExecuteReader_SP("sp_sel_CuringUnit", DBAccess.Return_Type.DataTable);
                dgv_PressMaster.DataSource = dt;
                dgv_PressMaster.Columns[0].HeaderText = "CURING UNIT";
            }
            catch (Exception ex)
            {
                Program.WriteToGtErrorLog("GT", "frmPressMaster", System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        private void selCheck()
        {
            try
            {
                if (cmbCategory.SelectedIndex > 0)
                {
                    SqlParameter[] sp = new SqlParameter[] { new SqlParameter("@Press", textBox1.Text), new SqlParameter("@Category", cmbCategory.SelectedValue.ToString()) };
                    DataTable dtTyreSize = (DataTable)dba.ExecuteReader_SP("SP_Sel_PressMaster_TyreSize", sp, DBAccess.Return_Type.DataTable);
                    if (dtTyreSize.Rows.Count > 0)
                    {
                        foreach (CheckBox chk in grpPanel.Controls)
                        {
                            List<string> lstTyreSize = dtTyreSize.AsEnumerable().Where(n => n.Field<string>("TyreSize").Equals(chk.Text)).Select(n =>
                                n.Field<string>("TyreSize").ToString()).ToList();
                            if (lstTyreSize.Count > 0)
                                chk.Checked = true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Program.WriteToGtErrorLog("GT", "frmPressMaster", System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
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
        private void dgv_PressMaster_SelectionChanged(object sender, DataGridViewCellEventArgs e)
        {
            deSelCheck();
            string value = dgv_PressMaster.Rows[e.RowIndex].Cells["CuringUnit"].Value.ToString();
            if (value != "")
            {
                textBox1.Text = dgv_PressMaster.Rows[e.RowIndex].Cells["CuringUnit"].Value.ToString();
                selCheck();
                btnSave.Visible = true;
            }
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (cmbCategory.SelectedIndex <= 0)
                {
                    lblErrMsg.Text = "Choose Category";
                    cmbCategory.Focus();
                }
                else
                {
                    updChkdType();
                    MessageBox.Show("Record successfully updated");
                    btnClear_Click(null, null);
                }
            }
            catch (Exception ex)
            {
                Program.WriteToGtErrorLog("GT", "frmPressMaster", System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        private void updChkdType()
        {
            try
            {
                List<string> checkedpress = new List<string>();
                foreach (CheckBox chk in grpPanel.Controls)
                {
                    if (chk.Checked == true)
                        checkedpress.Add(chk.Text);
                }

                SqlParameter[] sp = new SqlParameter[] { new SqlParameter("@Press", textBox1.Text), new SqlParameter("@Category", cmbCategory.SelectedValue.ToString()) };
                DataTable dtpress = (DataTable)dba.ExecuteReader_SP("SP_Sel_PressMaster_TyreSize", sp, DBAccess.Return_Type.DataTable);
                List<string> lstpress = dtpress.AsEnumerable().Select(n => n.Field<string>("TyreSize")).ToList();
                if (dtpress.Rows.Count > 0)
                {
                    List<string> lstToUncheck = dtpress.AsEnumerable().Where(n => !checkedpress.Contains(n.Field<string>("TyreSize"))).Select(n =>
                        n.Field<string>("TyreSize")).ToList();
                    foreach (string str in lstToUncheck)
                    {
                        SqlParameter[] sp1 = new SqlParameter[] { 
                            new SqlParameter("@Press", textBox1.Text), 
                            new SqlParameter("@TyreSize", str), 
                            new SqlParameter("@Category", cmbCategory.SelectedValue.ToString()) 
                        };
                        int resp = dba.ExecuteNonQuery_SP("SP_UPD_Pressmaster_TyreSizeWise", sp1);
                    }

                    List<string> lstToCheck = checkedpress.AsEnumerable().Where(n => !lstpress.Contains(n.ToString())).Select(n => n.ToString()).ToList();
                    foreach (string str in lstToCheck)
                    {
                        SqlParameter[] sp1 = new SqlParameter[] { 
                            new SqlParameter("@Press", textBox1.Text), 
                            new SqlParameter("@TyreSize", str), 
                            new SqlParameter("@Username", Program.strUserName), 
                            new SqlParameter("@Category", cmbCategory.SelectedValue.ToString())  
                        };
                        int resp = dba.ExecuteNonQuery_SP("SP_INS_PressMaster_TyreSizewise", sp1);
                    }
                }
                else
                {
                    foreach (string str in checkedpress)
                    {
                        SqlParameter[] sp1 = new SqlParameter[] { 
                            new SqlParameter("@Press", textBox1.Text), 
                            new SqlParameter("@TyreSize", str), 
                            new SqlParameter("@Username", Program.strUserName), 
                            new SqlParameter("@Category", cmbCategory.SelectedValue.ToString()) 
                        };
                        int resp = dba.ExecuteNonQuery_SP("SP_INS_PressMaster_TyreSizewise", sp1);
                    }
                }
            }
            catch (Exception ex)
            {
                Program.WriteToGtErrorLog("GT", "frmPressMaster", System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        private void btnClear_Click(object sender, EventArgs e)
        {
            textBox1.Text = "";
            lblErrMsg.Text = "";
            btnSave.Visible = false;
            Bind_GridView();
            deSelCheck();
            dgv_PressMaster.ClearSelection();
        }
        private void cmbCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                grpPanel.Controls.Clear();
                if (cmbCategory.SelectedIndex > 0)
                {
                    SqlParameter[] sp = new SqlParameter[] { new SqlParameter("@Category", cmbCategory.SelectedValue.ToString()) };
                    DataTable dtPress = (DataTable)dba.ExecuteReader_SP("SP_GET_ProcessID_TyreSize_List", sp, DBAccess.Return_Type.DataTable);
                    if (dtPress.Rows.Count > 0)
                    {
                        int maxHeight = grpPanel.Height - 20;
                        int maxWidth = grpPanel.Width - 20;
                        int curHeight = 0;
                        int curWidth = 5;

                        for (int i = 0; i < dtPress.Rows.Count; i++)
                        {
                            CheckBox chk = new CheckBox();
                            chk.Text = dtPress.Rows[i]["TyreSize"].ToString();
                            if (i > 0) curHeight += 30;
                            if (curHeight >= maxHeight)
                            {
                                curWidth += 230;
                                curHeight = 0;
                            }
                            chk.Location = new Point(curWidth, curHeight);
                            chk.Width = 225;
                            grpPanel.Controls.Add(chk);
                        }
                    }
                    if (textBox1.Text != "")
                        selCheck();
                }
            }
            catch (Exception ex)
            {
                Program.WriteToGtErrorLog("GT", "frmPressMaster", System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
    }
}
