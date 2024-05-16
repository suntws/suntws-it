using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Configuration;
using System.Data.SqlClient;

namespace GT
{
    public partial class frmBlendMaster : Form
    {
        DBAccess dba = new DBAccess();
        public frmBlendMaster()
        {
            InitializeComponent();
        }

        private void frmBlendMaster_Load(object sender, EventArgs e)
        {
            try
            {
                DataTable dtCategory = (DataTable)dba.ExecuteReader_SP("SP_SEL_BlendMaster_Compound", DBAccess.Return_Type.DataTable);
                if (dtCategory.Rows.Count > 0)
                {
                    DataRow toInsert = dtCategory.NewRow();
                    toInsert.ItemArray = new object[] { "CHOOSE" };
                    dtCategory.Rows.InsertAt(toInsert, 0);

                    cboCategory.DataSource = dtCategory;
                    cboCategory.DisplayMember = "Compound";
                    cboCategory.ValueMember = "Compound";
                }
                DataTable dtData = (DataTable)dba.ExecuteReader_SP("sp_sel_Blendmaster", DBAccess.Return_Type.DataTable);
                dataGridView1.DataSource = dtData;
                btnClear_Click(null, null);
            }
            catch (Exception ex)
            {
                Program.WriteToGtErrorLog("GT", "frmBlendMaster", System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                lblErrorMsg.Text = "";
                double basePer = 0.0, LipPer = 0.0, centerPer = 0.0, treadPer = 0.0, frictionPer = 0.0, fsPer = 0.0;
                frictionPer = txtFrictionPer.Text != "" ? (Convert.ToDouble(txtFrictionPer.Text) > 0 ? Convert.ToDouble(txtFrictionPer.Text) : 0.0) : 0.0;
                fsPer = txtFsPer.Text != "" ? (Convert.ToDouble(txtFsPer.Text) > 0 ? Convert.ToDouble(txtFsPer.Text) : 0.0) : 0.0;
                basePer = txtBasePer.Text != "" ? (Convert.ToDouble(txtBasePer.Text) > 0 ? Convert.ToDouble(txtBasePer.Text) : 0.0) : 0.0;
                LipPer = txtLipPer.Text != "" ? (Convert.ToDouble(txtLipPer.Text) > 0 ? Convert.ToDouble(txtLipPer.Text) : 0.0) : 0.0;
                centerPer = txtCenterPer.Text != "" ? (Convert.ToDouble(txtCenterPer.Text) > 0 ? Convert.ToDouble(txtCenterPer.Text) : 0.0) : 0.0;
                treadPer = txtTreadPer.Text != "" ? (Convert.ToDouble(txtTreadPer.Text) > 0 ? Convert.ToDouble(txtTreadPer.Text) : 0.0) : 0.0;

                if (frictionPer + fsPer + LipPer + basePer + centerPer + treadPer != 100)
                {
                    lblErrorMsg.Text = "More or less percentage of components";
                    return;
                }

                //SqlParameter[] spC = new SqlParameter[] { new SqlParameter("@Category", cboCategory.Text) };
                //DataTable dtData = (DataTable)dba.ExecuteReader_SP("sp_chk_blendmaster_category", spC, DBAccess.Return_Type.DataTable);
                //if (dtData.Rows.Count == 1)
                //{
                //    SqlParameter[] sp = new SqlParameter[] { new SqlParameter("@category", cboCategory.Text) };
                //    dba.ExecuteNonQuery_SP("SP_UPD_BlendMaster_Status", sp);
                //}

                SqlParameter[] sp1 = new SqlParameter[]{
                    new SqlParameter("@category",cboCategory.Text ),
                    new SqlParameter("@FrictionPer",frictionPer.ToString()),
                    new SqlParameter("@FSPer",fsPer.ToString()),
                    new SqlParameter("@BasePer",basePer),
                    new SqlParameter("@LipPer",LipPer),
                    new SqlParameter("@CenterPer",centerPer.ToString()),
                    new SqlParameter("@TreadPer",treadPer.ToString()),
                    new SqlParameter("@UserName",Program.strUserName)  
                };
                dba.ExecuteNonQuery_SP("SP_INS_BlendMaster_Details", sp1);
                MessageBox.Show("Saved Successfully");
                frmBlendMaster_Load(null, null);
            }
            catch (Exception ex)
            {
                Program.WriteToGtErrorLog("GT", "frmBlendMaster", System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            cboCategory.SelectedIndex = 0;
            clearText();
            dataGridView1.ClearSelection();
        }

        private void clearText()
        {
            txtFrictionPer.Text = "";
            txtFsPer.Text = "";
            txtBasePer.Text = "";
            txtLipPer.Text = "";
            txtCenterPer.Text = "";
            txtTreadPer.Text = "";
            lblErrorMsg.Text = "";
        }

        private void dataGridView1_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex < 0) return;
            setGvdataOntext(e.RowIndex);
        }

        private void setGvdataOntext(int index)
        {
            DataGridViewRow gvRow = dataGridView1.Rows[index];
            string strCategory = gvRow.Cells["Category"].Value.ToString();
            int catIndex = cboCategory.FindStringExact(strCategory);
            cboCategory.SelectedIndex = catIndex;
            txtFrictionPer.Text = gvRow.Cells["FrictionPer"].Value.ToString();
            txtFsPer.Text = gvRow.Cells["FsPer"].Value.ToString();
            txtLipPer.Text = gvRow.Cells["LipPer"].Value.ToString();
            txtBasePer.Text = gvRow.Cells["BasePer"].Value.ToString();
            txtCenterPer.Text = gvRow.Cells["CenterPer"].Value.ToString();
            txtTreadPer.Text = gvRow.Cells["TreadPer"].Value.ToString();
        }

        private void txtNumericPercentage_KeyPress(object sender, KeyPressEventArgs e)
        {
            Program.digitonly(e);
        }

        private void cboCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                clearText();
                dataGridView1.ClearSelection();
                if (cboCategory.SelectedIndex > 0)
                {
                    //foreach (DataGridViewRow gv_Row in dataGridView1.Rows)
                    for (int i = 0; i < dataGridView1.Rows.Count; i++)
                    {
                        if (dataGridView1.Rows[i].Cells["Category"].Value.ToString() == cboCategory.SelectedValue.ToString())
                        {
                            dataGridView1.Rows[i].Selected = true;
                            setGvdataOntext(i);
                            return;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Program.WriteToGtErrorLog("GT", "frmBlendMaster", System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
    }
}
