using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace GT
{
    public partial class frmDefectD4Master : Form
    {
        DBAccess dba = new DBAccess();
        public frmDefectD4Master()
        {
            InitializeComponent();
        }
        private void frmDefectD4Master_Load(object sender, EventArgs e)
        {
            try
            {
                DataTable dtTyreSize = (DataTable)dba.ExecuteReader_SP("SP_GET_DefectD4Master_TyreSize", DBAccess.Return_Type.DataTable);
                if (dtTyreSize.Rows.Count > 0)
                {
                    DataRow dr;
                    dr = dtTyreSize.NewRow();
                    dr.ItemArray = new object[] { "CHOOSE" };
                    dtTyreSize.Rows.InsertAt(dr, 0);
                    cboTyreSize.DataSource = dtTyreSize;
                    cboTyreSize.DisplayMember = "TyreSize";
                    cboTyreSize.ValueMember = "TyreSize";
                }
                lblErrorMsg.Text = "";
                bindGrid();
                txtFrom.KeyPress += new KeyPressEventHandler(txtNumericPercentage_KeyPress);
                txtTo.KeyPress += new KeyPressEventHandler(txtNumericPercentage_KeyPress);
                txtTo.TextChanged += new EventHandler(checkValidEntry);
                txtFrom.TextChanged += new EventHandler(checkValidEntry);
            }
            catch (Exception ex)
            {
                Program.WriteToGtErrorLog("GT", "frmDefectD4Master", System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        private void checkValidEntry(object sender, EventArgs e)
        {
            try
            {
                if (txtFrom.Text.Trim() != "" && txtTo.Text.Trim() != "")
                {
                    decimal dcfrom = txtFrom.Text != "" ? Convert.ToDecimal(txtFrom.Text) : 0;
                    decimal dcTo = txtTo.Text != "" ? Convert.ToDecimal(txtTo.Text) : 0;
                    if (dcfrom > dcTo)
                    {
                        lblErrorMsg.Text = "From value must be less than to value";
                        txtFrom.Text = "";
                    }
                    else
                        lblErrorMsg.Text = "";
                }
            }
            catch (Exception ex)
            {
                Program.WriteToGtErrorLog("GT", "frmDefectD4Master", System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        private void bindGrid()
        {
            try
            {
                gvDefectDetails.DataSource = null;
                string strSize = cboTyreSize.SelectedValue.ToString() != "CHOOSE" ? cboTyreSize.SelectedValue.ToString() : "";
                SqlParameter[] sp = new SqlParameter[] { new SqlParameter("@TyreSize", strSize) };
                DataTable dtD4Details = (DataTable)dba.ExecuteReader_SP("SP_LST_DefectD4Master_D4Details", sp, DBAccess.Return_Type.DataTable);
                if (dtD4Details.Rows.Count > 0)
                {
                    gvDefectDetails.DataSource = dtD4Details;
                }
            }
            catch (Exception ex)
            {
                Program.WriteToGtErrorLog("GT", "frmDefectD4Master", System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        private void txtNumericPercentage_KeyPress(object sender, KeyPressEventArgs e)
        {
            Program.digitonly(e);
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (cboTyreSize.SelectedIndex <= 0)
                    lblErrorMsg.Text = "Choose Tyre Size";
                else if (txtFrom.Text.Trim() == "")
                    lblErrorMsg.Text = "Enter From Value";
                else if (txtTo.Text.Trim() == "")
                    lblErrorMsg.Text = "Enter To Value";
                else
                {
                    SqlParameter[] sp = new SqlParameter[]{
                    new SqlParameter("@TyreSize",cboTyreSize.Text),
                    new SqlParameter("@From",txtFrom.Text),
                    new SqlParameter("@To",txtTo.Text),
                    new SqlParameter("@Grade",cboGrade.Text),
                    new SqlParameter("@Username",Program.strUserName)
                    };
                    dba.ExecuteNonQuery_SP("SP_SAV_DefectD4Master_DefectD4", sp);
                    MessageBox.Show("Saved Successfully");
                    gvDefectDetails.DataSource = null;
                    lblErrorMsg.Text = "";
                    txtFrom.Text = "";
                    txtTo.Text = "";
                    cboGrade.SelectedIndex = 0;
                    cboTyreSize.SelectedIndex = cboTyreSize.FindStringExact(cboTyreSize.SelectedValue.ToString());
                    bindGrid();
                }
            }
            catch (Exception ex)
            {
                Program.WriteToGtErrorLog("GT", "frmDefectD4Master", System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        private void btnClear_Click(object sender, EventArgs e)
        {
            lblErrorMsg.Text = "";
            txtFrom.Text = "";
            txtTo.Text = "";
            cboGrade.SelectedIndex = 0;
            cboTyreSize.SelectedIndex = 0;
            bindGrid();
        }
        private void gvDefectDetails_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                lblErrorMsg.Text = "";
                txtFrom.Text = "";
                txtTo.Text = "";
                txtFrom.Text = (gvDefectDetails.Rows[e.RowIndex].Cells[1].Value).ToString();
                txtTo.Text = (gvDefectDetails.Rows[e.RowIndex].Cells[2].Value).ToString();
                cboGrade.SelectedIndex = cboGrade.FindStringExact(gvDefectDetails.Rows[e.RowIndex].Cells[3].Value.ToString());
                cboTyreSize.SelectedIndex = cboTyreSize.FindStringExact(gvDefectDetails.Rows[e.RowIndex].Cells[0].Value.ToString());
            }
            catch (Exception ex)
            {
                Program.WriteToGtErrorLog("GT", "frmDefectD4Master", System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        private void cboTyreSize_SelectedIndexChanged(object sender, EventArgs e)
        {
            bindGrid();
        }
    }
}
