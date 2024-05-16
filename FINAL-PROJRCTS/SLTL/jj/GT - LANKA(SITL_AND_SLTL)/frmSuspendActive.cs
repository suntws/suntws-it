using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Reflection;

namespace GT
{
    public partial class frmSuspendActive : Form
    {
        DBAccess dbCon = new DBAccess();
        DataTable dtSuspend = new DataTable();
        public frmSuspendActive()
        {
            InitializeComponent();
        }

        private void frmSuspendActive_Load(object sender, EventArgs e)
        {
            try
            {
                rdoMould.Checked = false;
                rdoType.Checked = false;
                rdoFriction.Checked = false;
                rdoWtClass.Checked = false;

                cboName.DataSource = null;
                lblCaption.Text = "";
                lblValue.Text = "";
                txtRemarks.Text = "";
                lblRim.Text = "";
                btnActive.Visible = false;
                dtSuspend = (DataTable)dbCon.ExecuteReader_SP("sp_sel_suspendlist", DBAccess.Return_Type.DataTable);
            }
            catch (Exception ex)
            {
                Program.WriteToGtErrorLog("GT", "frmSuspendActive", MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        private void rdoSuspend_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                lblCaption.Text = "";
                cboName.DataSource = null;
                gvSuspendList.DataSource = null;
                lblValue.Text = "";
                txtRemarks.Text = "";
                lblRim.Text = "";
                btnActive.Visible = false;
                if (((RadioButton)sender).Checked)
                {
                    lblCaption.Text = ((RadioButton)sender).Text;
                    dtSuspend.DefaultView.RowFilter = "category='" + lblCaption.Text + "'";
                    dtSuspend.DefaultView.Sort = "caption ASC";
                    DataTable dt = dtSuspend.DefaultView.ToTable(true, "caption");

                    if (dt.Rows.Count > 0)
                    {
                        DataRow toInsert = dt.NewRow();
                        toInsert.ItemArray = new object[] { "CHOOSE" };
                        dt.Rows.InsertAt(toInsert, 0);

                        cboName.DataSource = dt;
                        cboName.DisplayMember = "caption";
                        cboName.ValueMember = "caption";

                        if (cboName.Items.Count == 2)
                            cboName.SelectedIndex = 1;
                    }
                }
            }
            catch (Exception ex)
            {
                Program.WriteToGtErrorLog("GT", "frmSuspendActive", MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        private void cboName_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                gvSuspendList.DataSource = null;
                lblValue.Text = "";
                txtRemarks.Text = "";
                lblRim.Text = "";
                btnActive.Visible = false;

                if (cboName.SelectedIndex > 0)
                {
                    SqlParameter[] spSel = new SqlParameter[] { new SqlParameter("@Val", cboName.SelectedValue.ToString()), new SqlParameter("@Category", lblCaption.Text) };
                    DataTable dtData = (DataTable)dbCon.ExecuteReader_SP("sp_sel_suspend_list_FilterWise", spSel, DBAccess.Return_Type.DataTable);
                    if (dtData.Rows.Count > 0)
                    {
                        gvSuspendList.DataSource = dtData;
                        if (dtData.Rows.Count == 1)
                        {
                            lblValue.Text = (gvSuspendList.Rows[0].Cells[0].Value).ToString();
                            if (lblCaption.Text == "MOULD SIZE")
                                lblRim.Text = (gvSuspendList.Rows[0].Cells[2].Value).ToString();
                            btnActive.Visible = true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Program.WriteToGtErrorLog("GT", "frmSuspendActive", MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        private void btnActive_Click(object sender, EventArgs e)
        {
            try
            {
                if (lblCaption.Text == "" || lblValue.Text == "")
                    MessageBox.Show("CHOOSE PROPER VALUES");
                else if (txtRemarks.Text == "")
                    MessageBox.Show("ENTER REMARKS");
                else
                {
                    SqlParameter[] spUpd = new SqlParameter[] { 
                        new SqlParameter("@Val1", lblValue.Text), 
                        new SqlParameter("@Category", lblCaption.Text), 
                        new SqlParameter("@Val2", lblCaption.Text == "MOULD SIZE" ? cboName.SelectedValue.ToString() : ""), 
                        new SqlParameter("@Val3", lblCaption.Text == "MOULD SIZE" ? lblRim.Text : ""),
                        new SqlParameter("@Username", Program.strUserName),
                        new SqlParameter("@Remarks", txtRemarks.Text)
                    };
                    int resp = dbCon.ExecuteNonQuery_SP("sp_upd_suspend_to_activate", spUpd);
                    if (resp > 0)
                    {
                        MessageBox.Show(lblCaption.Text + " ACTIVATED SUCCESSFULLY");
                        frmSuspendActive_Load(null, null);
                    }
                }
            }
            catch (Exception ex)
            {
                Program.WriteToGtErrorLog("GT", "frmSuspendActive", MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            frmSuspendActive_Load(null, null);
        }

        private void gvSuspendList_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                lblValue.Text = "";
                txtRemarks.Text = "";
                lblRim.Text = "";
                btnActive.Visible = false;
                if (e.RowIndex >= 0)
                {
                    lblValue.Text = (gvSuspendList.Rows[e.RowIndex].Cells[0].Value).ToString();
                    if (lblCaption.Text == "MOULD SIZE")
                        lblRim.Text = (gvSuspendList.Rows[e.RowIndex].Cells[2].Value).ToString();
                    btnActive.Visible = true;
                }
            }
            catch (Exception ex)
            {
                Program.WriteToGtErrorLog("GT", "frmSuspendActive", MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
    }
}
