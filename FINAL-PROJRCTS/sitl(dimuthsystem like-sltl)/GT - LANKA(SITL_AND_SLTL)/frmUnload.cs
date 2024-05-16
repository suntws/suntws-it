using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Data.Sql;
using System.Data.SqlClient;
using System.IO;
using System.Windows.Forms;
using System.Configuration;

namespace GT
{
    public partial class frmUnload : Form
    {
        DBAccess dba = new DBAccess();
        string strPress = "";
        public frmUnload()
        {
            InitializeComponent();
        }
        private void frmUnload_Load(object sender, EventArgs e)
        {
            try
            {
                txtStencilno.Text = "";
                ClearCntrls();
                Bind_Press();

                DataTable dtList = new DataTable();
                if (Program.strPlantName == "MMN"||Program.strPlantName == "SLTL")
                {
                    SqlParameter[] sp = new SqlParameter[] { new SqlParameter("@MachineType", "Unload") };
                    dtList = (DataTable)dba.ExecuteReader_SP("sp_sel_MachineList", sp, DBAccess.Return_Type.DataTable);

                    if (dtList.Rows.Count > 0)
                    {
                        DataRow toInsert = dtList.NewRow();
                        toInsert.ItemArray = new object[] { "CHOOSE" };
                        dtList.Rows.InsertAt(toInsert, 0);

                        cboUnload.DataSource = dtList;
                        cboUnload.DisplayMember = "MachineName";
                        cboUnload.ValueMember = "MachineName";
                    }
                }
                else
                {
                    dtList = (DataTable)dba.ExecuteReader_SP("sp_sel_CuringUnit", DBAccess.Return_Type.DataTable);
                    if (dtList.Rows.Count > 0)
                    {
                        DataRow toInsert = dtList.NewRow();
                        toInsert.ItemArray = new object[] { "CHOOSE" };
                        dtList.Rows.InsertAt(toInsert, 0);

                        cboUnload.DataSource = dtList;
                        cboUnload.DisplayMember = "CuringUnit";
                        cboUnload.ValueMember = "CuringUnit";
                    }
                }
            }
            catch (Exception ex)
            {
                Program.WriteToGtErrorLog("GT", "frmUnLoad", System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        private void Bind_Press()
        {
            try
            {
                DataTable dtPress = (DataTable)dba.ExecuteReader_SP("sp_sel_unload_press_waiting_v1", DBAccess.Return_Type.DataTable);
                if (dtPress != null && dtPress.Rows.Count > 0)
                {
                    DataRow dr = dtPress.NewRow();
                    dr.ItemArray = new object[] { "CHOOSE" };
                    dtPress.Rows.InsertAt(dr, 0);

                    cboPress.DataSource = dtPress;
                    cboPress.DisplayMember = "Press";
                    cboPress.ValueMember = "Press";

                    if (cboPress.Items.Count == 2)
                        cboPress.SelectedIndex = 1;
                }
            }
            catch (Exception ex)
            {
                Program.WriteToGtErrorLog("GT", "frmUnLoad", System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        private void cboPress_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                ClearCntrls();
                txtStencilno.Text = "";
                lstStencilNo.DataSource = null;
                if (cboPress.Items.Count > 0 && cboPress.SelectedIndex > 0)
                {
                    strPress = cboPress.Text;
                    string[] strsplit = cboPress.Text.Split('-');
                    SqlParameter[] spSel = new SqlParameter[] { 
                        new SqlParameter("@Press", strsplit[0].ToString()), 
                        new SqlParameter("@PressB", strsplit[1].ToString()) 
                    };
                    DataSet dt_StencilNo = (DataSet)dba.ExecuteReader_SP("sp_sel_unload_stencil_list_v1", spSel, DBAccess.Return_Type.DataSet);
                    if (dt_StencilNo.Tables.Count == 2)
                    {
                        lblMaxMsg.Text = "Loaded Stencil Based Maximum Curing Time Take From Type " + dt_StencilNo.Tables[0].Rows[0]["TyreType"].ToString();
                        lblMaxCureTime.Text = dt_StencilNo.Tables[0].Rows[0]["maxcuringtime"].ToString();

                        lstStencilNo.DataSource = dt_StencilNo.Tables[1];
                        lstStencilNo.DisplayMember = "StencilNo";
                        lstStencilNo.ValueMember = "StencilNo";
                        lstStencilNo.ClearSelected();

                        if (dt_StencilNo.Tables[1] != null && dt_StencilNo.Tables[1].Rows.Count == 1)
                            txtStencilno.Text = dt_StencilNo.Tables[1].Rows[0]["StencilNo"].ToString();
                    }
                    else
                    {
                        MessageBox.Show("NO RECORDS");
                        Bind_Press();
                    }
                }
            }
            catch (Exception ex)
            {
                Program.WriteToGtErrorLog("GT", "frmUnLoad", System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        private void lstStencilNo_Click(object sender, EventArgs e)
        {
            try
            {
                ClearCntrls();
                txtStencilno.Text = "";
                if (lstStencilNo.Items.Count > 0 && lstStencilNo.SelectedIndex >= 0)
                {
                    txtStencilno.Text = lstStencilNo.SelectedValue.ToString().ToString();
                    lstStencilNo.SetSelected(lstStencilNo.SelectedIndex, true);
                }
            }
            catch (Exception ex)
            {
                Program.WriteToGtErrorLog("GT", "frmUnLoad", System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);

            }
        }
        private void ClearCntrls()
        {
            try
            {
                lblPlatForm.Text = "";
                lblBrand.Text = "";
                lblSideWall.Text = "";
                lblType.Text = "";
                lblSize.Text = "";
                lblRimWidth.Text = "";
                lblProcessId.Text = "";
                lblMouldCode.Text = "";
                lblPress.Text = "";
                lblLoadedDate.Text = "";
                lblUser.Text = "";
                lblMouldTemp.Text = "";
                lblLoading.Text = "";
                lblPumping.Text = "";
                lblCuringTime.Text = "";
                lblCureEndOn.Text = "";
                lblDelayMsg.Text = "";
            }
            catch (Exception ex)
            {
                Program.WriteToGtErrorLog("GT", "frmUnLoad", System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);

            }
        }
        private void txtStencilno_TextChanged(object sender, EventArgs e)
        {
            try
            {
                ClearCntrls();
                if (txtStencilno.Text != "")
                {
                    SqlParameter[] sp = new SqlParameter[] { new SqlParameter("@StencilNo", txtStencilno.Text) };
                    DataTable dt = (DataTable)dba.ExecuteReader_SP("sp_sel_ProductDetails_Stage3", sp, DBAccess.Return_Type.DataTable);
                    if (dt.Rows.Count > 0)
                    {
                        lblPlatForm.Text = dt.Rows[0]["Config"].ToString();
                        lblBrand.Text = dt.Rows[0]["Brand"].ToString();
                        lblSideWall.Text = dt.Rows[0]["Sidewall"].ToString();
                        lblType.Text = dt.Rows[0]["TyreType"].ToString();
                        lblSize.Text = dt.Rows[0]["TyreSize"].ToString();
                        lblRimWidth.Text = dt.Rows[0]["RimSize"].ToString();
                        lblProcessId.Text = dt.Rows[0]["Process_Id"].ToString();
                        lblMouldCode.Text = dt.Rows[0]["MouldCode"].ToString();
                        lblPress.Text = dt.Rows[0]["Press"].ToString() + " - " + dt.Rows[0]["PressB"].ToString();
                        lblLoadedDate.Text = dt.Rows[0]["Loaded_Date"].ToString();
                        lblUser.Text = dt.Rows[0]["Loaded_By"].ToString();
                        lblMouldTemp.Text = dt.Rows[0]["MouldTemp"].ToString();
                        lblLoading.Text = dt.Rows[0]["Loading_Machine"].ToString();
                        lblPumping.Text = dt.Rows[0]["Pumping_Machine"].ToString();

                        lblCuringTime.Text = dt.Rows[0]["CuringTime"].ToString();
                        lblCureEndOn.Text = Convert.ToDateTime(lblLoadedDate.Text).AddHours(Convert.ToDateTime(lblMaxCureTime.Text).Hour).
                            AddMinutes(Convert.ToDateTime(lblMaxCureTime.Text).Minute).ToString();

                        btnSave.Visible = false;
                        lblDelayMsg.Text = "";
                        if ((Convert.ToDateTime(lblCureEndOn.Text).AddHours(1) >= System.DateTime.Now && Convert.ToDateTime(lblCureEndOn.Text) <= System.DateTime.Now)
                            || (Program.boolLoadUnloadDelayApproval)) //Convert.ToDateTime(lblCureEndOn.Text) <= System.DateTime.Now &&
                            btnSave.Visible = true;

                        if (Convert.ToDateTime(lblCureEndOn.Text).AddHours(1) <= System.DateTime.Now)
                            lblDelayMsg.Text = "LOADED ON " + lblLoadedDate.Text + " UNLOADING \nDELAY GET APPROVAL FROM SUPERVISOR";

                        cboUnload.SelectedIndex = cboUnload.FindStringExact(dt.Rows[0]["Press"].ToString());

                        txtRemarks.Focus();
                    }
                }
            }
            catch (Exception ex)
            {
                Program.WriteToGtErrorLog("GT", "frmUnLoad", System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtStencilno.Text == "")
                    MessageBox.Show("Choose stencil no.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                else if (cboUnload.SelectedIndex == 0 || cboUnload.SelectedIndex == -1)
                    MessageBox.Show("Choose unloading machine", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                else if (lblDelayMsg.Text != "" && txtRemarks.Text == "")
                    MessageBox.Show("Enter unloading delay remakrs", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                else
                {
                    SqlParameter[] sp = new SqlParameter[] { 
                        new SqlParameter("@StencilNo", txtStencilno.Text), 
                        new SqlParameter("@Unloaded_By", Program.strUserName), 
                        new SqlParameter("@Unloaded_Remarks", txtRemarks.Text), 
                        new SqlParameter("@Unloaded_status", 1),
                        new SqlParameter("@Unloading_Machine", cboUnload.SelectedValue.ToString())
                    };
                    int resp = dba.ExecuteNonQuery_SP("sp_upt_ProductionDetails_Stage3", sp);
                    if (resp > 0)
                    {
                        MessageBox.Show("Record Saved Successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        frmUnload_Load(null, null);
                        if (strPress != "" && cboPress.FindStringExact(strPress) != -1)
                            cboPress.SelectedIndex = cboPress.FindStringExact(strPress);
                        else
                        {
                            txtRemarks.Text = "";
                            cboPress.SelectedIndex = 0;
                        }
                    }
                    else
                        MessageBox.Show("Try Again", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                Program.WriteToGtErrorLog("GT", "frmUnLoad", System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        private void btnReset_Click(object sender, EventArgs e)
        {
            frmUnload_Load(sender, e);
        }
    }
}
