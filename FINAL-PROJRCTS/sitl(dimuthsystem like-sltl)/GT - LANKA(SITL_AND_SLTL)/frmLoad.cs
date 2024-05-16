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
    public partial class frmLoad : Form
    {
        DBAccess dba = new DBAccess();
        DataTable dtProcessIdDetails;
        public frmLoad()
        {
            InitializeComponent();
        }
        private void frmLoad_Load(object sender, EventArgs e)
        {
            try
            {
                if (Program.strPlantName == "MMN")
                {
                    SqlParameter[] spLoad = new SqlParameter[] { new SqlParameter("@MachineType", "Load") };
                    DataTable dtLoad = (DataTable)dba.ExecuteReader_SP("sp_sel_MachineList", spLoad, DBAccess.Return_Type.DataTable);
                    if (dtLoad.Rows.Count > 0)
                    {
                        DataRow toInsert = dtLoad.NewRow();
                        toInsert.ItemArray = new object[] { "CHOOSE" };
                        dtLoad.Rows.InsertAt(toInsert, 0);

                        cboLoadMachine.DataSource = dtLoad;
                        cboLoadMachine.DisplayMember = "MachineName";
                        cboLoadMachine.ValueMember = "MachineName";
                    }
                    SqlParameter[] spBump = new SqlParameter[] { new SqlParameter("@MachineType", "Pumping") };
                    DataTable dtBump = (DataTable)dba.ExecuteReader_SP("sp_sel_MachineList", spBump, DBAccess.Return_Type.DataTable);
                    if (dtBump.Rows.Count > 0)
                    {
                        DataRow toInsert = dtBump.NewRow();
                        toInsert.ItemArray = new object[] { "CHOOSE" };
                        dtBump.Rows.InsertAt(toInsert, 0);

                        cboPumping.DataSource = dtBump;
                        cboPumping.DisplayMember = "MachineName";
                        cboPumping.ValueMember = "MachineName";
                    }
                }
                else
                {
                    cboLoadMachine.Visible = false;
                    cboPumping.Visible = false;
                    lblLoading.Visible = false;
                    lblBumping.Visible = false;

                    lblPress.Text = "Loading Unit";
                    lblPressSeq.Text = "Station";
                }
                Bind_StencilNo();
            }
            catch (Exception ex)
            {
                Program.WriteToGtErrorLog("GT", "frmLoad", System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        private void Bind_StencilNo()
        {
            try
            {
                ClearCntrls();
                lstStencilNo.DataSource = null;
                DataTable dt_StencilNo = (DataTable)dba.ExecuteReader_SP("sp_sel_load_stencil_list", DBAccess.Return_Type.DataTable);
                if (dt_StencilNo.Rows.Count > 0)
                {
                    lstStencilNo.DataSource = dt_StencilNo;
                    lstStencilNo.DisplayMember = "StencilNo";
                    lstStencilNo.ValueMember = "StencilNo";
                    lstStencilNo.ClearSelected();

                    if (dt_StencilNo.Rows.Count == 1)
                    {
                        lstStencilNo.SelectedIndex = 0;
                        txtStencilno.Text = dt_StencilNo.Rows[0]["StencilNo"].ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                Program.WriteToGtErrorLog("GT", "frmLoad", System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        private void lstStencilNo_Click(object sender, System.EventArgs e)
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
                Program.WriteToGtErrorLog("GT", "frmLoad", System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        private void txtStencilno_TextChanged(object sender, System.EventArgs e)
        {
            try
            {
                ClearCntrls();
                SqlParameter[] sp = new SqlParameter[] { new SqlParameter("@StencilNo", txtStencilno.Text) };
                DataTable dt = (DataTable)dba.ExecuteReader_SP("sp_sel_Stge3_ProductionSWspecDetails", sp, DBAccess.Return_Type.DataTable);
                if (dt.Rows.Count > 0)
                {
                    lblTyretype.Text = dt.Rows[0]["TyreType"].ToString();
                    lblTyreSize.Text = dt.Rows[0]["TyreSize"].ToString();
                    lblRimWidth.Text = dt.Rows[0]["RimSize"].ToString();
                    lblOperator.Text = dt.Rows[0]["Operator"].ToString();
                    lblDateShift.Text = dt.Rows[0]["ProductionDate"].ToString() + " / " + dt.Rows[0]["Shift"].ToString();
                    lblMouldCode.Text = dt.Rows[0]["MouldCode"].ToString();
                    lblFormerCode.Text = dt.Rows[0]["FormerCode"].ToString();
                    lblBuildingPressure.Text = dt.Rows[0]["BulidPressureFrom"].ToString() + " ~ " + dt.Rows[0]["BulidPressureTo"].ToString();
                    lblGTtemp.Text = dt.Rows[0]["GdTemp"].ToString();
                    lblNoOfBW.Text = dt.Rows[0]["NoofBW"].ToString();
                    lblBandBW.Text = dt.Rows[0]["BWSize"].ToString();
                    lblGTod.Text = dt.Rows[0]["GtOdOld"].ToString() + " / " + dt.Rows[0]["GtOdNew"].ToString();
                    lblGTwidth.Text = dt.Rows[0]["GtWidthOld"].ToString() + " / " + dt.Rows[0]["GtWitdthNew"].ToString();
                    lblBrand.Text = dt.Rows[0]["Brand"].ToString();
                    lblSidewall.Text = dt.Rows[0]["Sidewall"].ToString();

                    btnSave.Visible = false;
                    lblDelayMsg.Text = "";
                    if (Convert.ToDateTime(dt.Rows[0]["CreatedDate"].ToString()).AddMinutes(40) >= System.DateTime.Now || Program.boolLoadUnloadDelayApproval)
                        btnSave.Visible = true;

                    if (Convert.ToDateTime(dt.Rows[0]["CreatedDate"].ToString()).AddMinutes(40) <= System.DateTime.Now)
                        lblDelayMsg.Text = "GT BUILD ON " + dt.Rows[0]["CreatedDate"].ToString() + " LOADING \nDELAY GET APPROVAL FROM SUPERVISOR";

                    cmbPlatform.DataSource = null;
                    cmbBrand.DataSource = null;
                    cmbSideWall.DataSource = null;
                    cmbType.DataSource = null;
                    cmbSize.DataSource = null;
                    cmbRimWidth.DataSource = null;

                    cmbPlatform.Items.Clear();
                    cmbBrand.Items.Clear();
                    cmbSideWall.Items.Clear();
                    cmbType.Items.Clear();
                    cmbSize.Items.Clear();
                    cmbRimWidth.Items.Clear();
                    txtProcessId.Text = "";

                    cmbPlatform.Enabled = true;
                    cmbBrand.Enabled = true;
                    cmbSideWall.Enabled = true;
                    cmbType.Enabled = true;
                    cmbSize.Enabled = true;
                    cmbRimWidth.Enabled = true;

                    if (dt.Rows[0]["Prod_ItemID"].ToString() == "0")
                    {
                        string strDefaultTyreSize = lblTyreSize.Text.Substring(0, 1);
                        string strDefaultTyretype = lblTyretype.Text.Substring(0, 2);
                        string strDefaultTyreRim = lblRimWidth.Text.Substring(0, 1);

                        dtProcessIdDetails = (DataTable)dba.ExecuteReader_SP("SP_SEL_EditBarcode_ProcessIdDetails", DBAccess.Return_Type.DataTable);
                        List<string> lstConfig = dtProcessIdDetails.AsEnumerable().Where(
                               b => b.Field<string>("TyreType").StartsWith(strDefaultTyretype) &&
                                b.Field<string>("TyreSize").StartsWith(strDefaultTyreSize) &&
                                b.Field<string>("TyreRim").StartsWith(strDefaultTyreRim) &&
                                b.Field<string>("Brand").Equals(lblBrand.Text) &&
                                b.Field<string>("Sidewall").Equals(lblSidewall.Text)
                                ).Select(A => A.Field<string>("Config")).Distinct().ToList();
                        lstConfig.Insert(0, "CHOOSE");
                        cmbPlatform.DataSource = lstConfig;
                        if (cmbPlatform.Items.Count == 2)
                            cmbPlatform.SelectedIndex = 1;
                    }
                    else if (dt.Rows[0]["Prod_ItemID"].ToString() != "0")
                    {
                        SqlParameter[] spData = new SqlParameter[] { 
                            new SqlParameter("@ItemID", dt.Rows[0]["Prod_ItemID"].ToString()), 
                            new SqlParameter("@ProductionID", dt.Rows[0]["ProductionID"].ToString()) 
                        };
                        DataTable dtData = (DataTable)dba.ExecuteReader_SP("sp_Item_WeightTrack_v1", spData, DBAccess.Return_Type.DataTable);
                        if (dtData.Rows.Count == 1)
                        {
                            cmbPlatform.Enabled = false;
                            cmbBrand.Enabled = false;
                            cmbSideWall.Enabled = false;
                            cmbType.Enabled = false;
                            cmbSize.Enabled = false;
                            cmbRimWidth.Enabled = false;

                            cmbPlatform.Items.Add("CHOOSE");
                            cmbPlatform.Items.Add(dtData.Rows[0]["config"].ToString());
                            cmbPlatform.SelectedIndex = 1;

                            cmbBrand.Items.Add("CHOOSE");
                            cmbBrand.Items.Add(dtData.Rows[0]["brand"].ToString());
                            cmbBrand.SelectedIndex = 1;

                            cmbSideWall.Items.Add("CHOOSE");
                            cmbSideWall.Items.Add(dtData.Rows[0]["sidewall"].ToString());
                            cmbSideWall.SelectedIndex = 1;

                            cmbType.Items.Add("CHOOSE");
                            cmbType.Items.Add(dtData.Rows[0]["tyretype"].ToString());
                            cmbType.SelectedIndex = 1;

                            cmbSize.Items.Add("CHOOSE");
                            cmbSize.Items.Add(dtData.Rows[0]["tyresize"].ToString());
                            cmbSize.SelectedIndex = 1;

                            cmbRimWidth.Items.Add("CHOOSE");
                            cmbRimWidth.Items.Add(dtData.Rows[0]["rimsize"].ToString());
                            cmbRimWidth.SelectedIndex = 1;

                            txtProcessId.Text = dtData.Rows[0]["processid"].ToString();

                            cmbPress.SelectedIndex = cmbPress.FindStringExact(dtData.Rows[0]["Prod_Press"].ToString());
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Program.WriteToGtErrorLog("GT", "frmLoad", System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        private void cmbPlatform_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            try
            {
                cmbBrand.DataSource = null;
                cmbSideWall.DataSource = null;
                cmbType.DataSource = null;
                cmbSize.DataSource = null;
                cmbRimWidth.DataSource = null;
                if (cmbPlatform.SelectedIndex > 0 && cmbPlatform.Enabled)
                {
                    string strDefaultTyreSize = lblTyreSize.Text.Substring(0, 1);
                    string strDefaultTyretype = lblTyretype.Text.Substring(0, 2);
                    string strDefaultTyreRim = lblRimWidth.Text.Substring(0, 1);
                    List<string> lstBrand = dtProcessIdDetails.AsEnumerable().Where(
                           b => b.Field<string>("TyreType").StartsWith(strDefaultTyretype) &&
                            b.Field<string>("TyreSize").StartsWith(strDefaultTyreSize) &&
                            b.Field<string>("TyreRim").StartsWith(strDefaultTyreRim) &&
                            b.Field<string>("Config").Equals(cmbPlatform.SelectedValue.ToString()) &&
                            b.Field<string>("Brand").Equals(lblBrand.Text) &&
                            b.Field<string>("Sidewall").Equals(lblSidewall.Text)
                            ).Select(A => A.Field<string>("Brand")).Distinct().ToList();
                    lstBrand.Insert(0, "CHOOSE");
                    cmbBrand.DataSource = lstBrand;
                    if (cmbBrand.Items.Count > 1)
                        cmbBrand.SelectedIndex = cmbBrand.FindStringExact(lblBrand.Text);
                }
            }
            catch (Exception ex)
            {
                Program.WriteToGtErrorLog("GT", "frmLoad", System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        private void cmbBrand_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            try
            {
                cmbSideWall.DataSource = null;
                cmbType.DataSource = null;
                cmbSize.DataSource = null;
                cmbRimWidth.DataSource = null;
                if (cmbBrand.SelectedIndex > 0 && cmbBrand.Enabled)
                {
                    string strDefaultTyreSize = lblTyreSize.Text.Substring(0, 1);
                    string strDefaultTyretype = lblTyretype.Text.Substring(0, 2);
                    string strDefaultTyreRim = lblRimWidth.Text.Substring(0, 1);
                    List<string> lstSidewall = dtProcessIdDetails.AsEnumerable().Where(
                        b => b.Field<string>("TyreType").StartsWith(strDefaultTyretype) &&
                            b.Field<string>("TyreSize").StartsWith(strDefaultTyreSize) &&
                            b.Field<string>("TyreRim").StartsWith(strDefaultTyreRim) &&
                            b.Field<string>("Config").Equals(cmbPlatform.SelectedValue.ToString()) &&
                        b.Field<string>("Brand").Equals(cmbBrand.SelectedItem.ToString())
                        ).Select(A => A.Field<string>("Sidewall")).Distinct().ToList();
                    lstSidewall.Insert(0, "CHOOSE");
                    cmbSideWall.DataSource = lstSidewall;
                    if (cmbSideWall.Items.Count > 1)
                        cmbSideWall.SelectedIndex = cmbSideWall.FindStringExact(lblSidewall.Text);
                }
            }
            catch (Exception ex)
            {
                Program.WriteToGtErrorLog("GT", "frmLoad", System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        private void cmbSideWall_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            try
            {
                cmbType.DataSource = null;
                cmbSize.DataSource = null;
                cmbRimWidth.DataSource = null;
                if (cmbSideWall.SelectedIndex > 0 && cmbSideWall.Enabled)
                {
                    string strDefaultTyreSize = lblTyreSize.Text.Substring(0, 1);
                    string strDefaultTyretype = lblTyretype.Text.Substring(0, 2);
                    string strDefaultTyreRim = lblRimWidth.Text.Substring(0, 1);
                    List<string> lstType = dtProcessIdDetails.AsEnumerable().Where(
                         b => b.Field<string>("TyreType").StartsWith(strDefaultTyretype) &&
                            b.Field<string>("TyreSize").StartsWith(strDefaultTyreSize) &&
                            b.Field<string>("TyreRim").StartsWith(strDefaultTyreRim) &&
                            b.Field<string>("Config").Equals(cmbPlatform.SelectedItem.ToString()) &&
                             b.Field<string>("Brand").Equals(cmbBrand.SelectedItem.ToString()) &&
                             b.Field<string>("Sidewall").Equals(cmbSideWall.SelectedItem.ToString())
                         ).Select(A => A.Field<string>("TyreType")).Distinct().ToList();
                    lstType.Insert(0, "CHOOSE");
                    cmbType.DataSource = lstType;
                    if (cmbType.Items.Count == 2)
                        cmbType.SelectedIndex = 1;
                    else
                        cmbType.SelectedIndex = cmbType.FindStringExact(lblTyretype.Text);
                }
            }
            catch (Exception ex)
            {
                Program.WriteToGtErrorLog("GT", "frmLoad", System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        private void cmbType_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            try
            {
                cmbSize.DataSource = null;
                cmbRimWidth.DataSource = null;
                if (cmbType.SelectedIndex > 0 && cmbType.Enabled)
                {
                    string strDefaultTyreSize = lblTyreSize.Text.Substring(0, 1);
                    string strDefaultTyreRim = lblRimWidth.Text.Substring(0, 1);
                    List<string> lstSize = dtProcessIdDetails.AsEnumerable().Where(
                         b => b.Field<string>("TyreSize").StartsWith(strDefaultTyreSize) &&
                            b.Field<string>("TyreRim").StartsWith(strDefaultTyreRim) &&
                            b.Field<string>("Config").Equals(cmbPlatform.SelectedItem.ToString()) &&
                         b.Field<string>("Brand").Equals(cmbBrand.SelectedItem.ToString()) &&
                         b.Field<string>("Sidewall").Equals(cmbSideWall.SelectedItem.ToString()) &&
                         b.Field<string>("TyreType").Equals(cmbType.SelectedItem.ToString())
                         ).Select(A => A.Field<string>("TyreSize")).Distinct().ToList();
                    lstSize.Insert(0, "CHOOSE");
                    cmbSize.DataSource = lstSize;
                    if (cmbSize.Items.Count == 2)
                        cmbSize.SelectedIndex = 1;
                    else
                        cmbSize.SelectedIndex = cmbSize.FindStringExact(lblTyreSize.Text);
                }
            }
            catch (Exception ex)
            {
                Program.WriteToGtErrorLog("GT", "frmLoad", System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        private void cmbSize_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            try
            {
                cmbRimWidth.DataSource = null;
                if (cmbSize.SelectedIndex > 0 && cmbSize.Enabled)
                {
                    string strDefaultTyreRim = lblRimWidth.Text.Substring(0, 1);
                    List<string> lstRim = dtProcessIdDetails.AsEnumerable().Where(
                            b => b.Field<string>("TyreRim").StartsWith(strDefaultTyreRim) &&
                                b.Field<string>("Config").Equals(cmbPlatform.SelectedItem.ToString()) &&
                            b.Field<string>("Brand").Equals(cmbBrand.SelectedItem.ToString()) &&
                            b.Field<string>("Sidewall").Equals(cmbSideWall.SelectedItem.ToString()) &&
                            b.Field<string>("TyreType").Equals(cmbType.SelectedItem.ToString()) &&
                            b.Field<string>("TyreSize").Equals(cmbSize.SelectedItem.ToString())
                            ).Select(A => A.Field<string>("TyreRim")).Distinct().ToList();
                    lstRim.Insert(0, "CHOOSE");
                    cmbRimWidth.DataSource = lstRim;
                    if (cmbRimWidth.Items.Count == 2)
                        cmbRimWidth.SelectedIndex = 1;
                    else
                        cmbRimWidth.SelectedIndex = cmbRimWidth.FindStringExact(lblRimWidth.Text);
                }
            }
            catch (Exception ex)
            {
                Program.WriteToGtErrorLog("GT", "frmLoad", System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        private void cmbRimWidth_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            try
            {
                txtProcessId.Text = "";
                if (cmbRimWidth.SelectedIndex > 0)
                {
                    if (cmbRimWidth.Enabled)
                    {
                        List<string> lst = dtProcessIdDetails.AsEnumerable().Where(
                                b => b.Field<string>("Config").Equals(cmbPlatform.SelectedItem.ToString()) &&
                                b.Field<string>("Brand").Equals(cmbBrand.SelectedItem.ToString()) &&
                                b.Field<string>("Sidewall").Equals(cmbSideWall.SelectedItem.ToString()) &&
                                b.Field<string>("TyreType").Equals(cmbType.SelectedItem.ToString()) &&
                                b.Field<string>("TyreSize").Equals(cmbSize.SelectedItem.ToString()) &&
                                b.Field<string>("TyreRim").Equals(cmbRimWidth.SelectedItem.ToString())
                                ).Select(A => A.Field<string>("ProcessID")).Distinct().ToList();
                        txtProcessId.Text = lst[0].ToString();
                    }

                    SqlParameter[] sp = new SqlParameter[] { new SqlParameter("@TyreSize", cmbSize.SelectedItem.ToString()) };
                    DataTable dtSize = (DataTable)dba.ExecuteReader_SP("sp_sel_CuringPressList_SizeWise", sp, DBAccess.Return_Type.DataTable);
                    if (dtSize.Rows.Count > 0)
                    {
                        DataRow toInsert = dtSize.NewRow();
                        toInsert.ItemArray = new object[] { "CHOOSE" };
                        dtSize.Rows.InsertAt(toInsert, 0);

                        cmbPress.DataSource = dtSize;
                        cmbPress.DisplayMember = "CuringUnit";
                        cmbPress.ValueMember = "CuringUnit";

                        if (cmbPress.Items.Count == 2)
                            cmbPress.SelectedIndex = 1;
                    }
                }
            }
            catch (Exception ex)
            {
                Program.WriteToGtErrorLog("GT", "frmLoad", System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        private void cmbPress_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            try
            {
                if (cmbPress.SelectedIndex > 0)
                {
                    cmbPressB.DataSource = null;
                    SqlParameter[] sp = new SqlParameter[] { new SqlParameter("@CuringUnit", cmbPress.SelectedValue.ToString()) };
                    DataTable dtStation = (DataTable)dba.ExecuteReader_SP("sp_sel_CuringStation", sp, DBAccess.Return_Type.DataTable);
                    if (dtStation.Rows.Count > 0)
                    {
                        DataRow toInsert = dtStation.NewRow();
                        toInsert.ItemArray = new object[] { "CHOOSE" };
                        dtStation.Rows.InsertAt(toInsert, 0);

                        cmbPressB.DataSource = dtStation;
                        cmbPressB.DisplayMember = "CuringStation";
                        cmbPressB.ValueMember = "CuringStation";

                        if (cmbPressB.Items.Count == 2)
                            cmbPressB.SelectedIndex = 1;
                    }
                }
            }
            catch (Exception ex)
            {
                Program.WriteToGtErrorLog("GT", "frmLoad", System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        private void cmbPressB_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (cmbPressB.SelectedIndex > 0)
                    txtRemarks.Focus();
            }
            catch (Exception ex)
            {
                Program.WriteToGtErrorLog("GT", "frmLoad", System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        private void btnSave_Click(object sender, System.EventArgs e)
        {
            try
            {
                string ErrMsg = "";
                if (lstStencilNo.SelectedIndex == -1)
                    ErrMsg += "Choose Stencil No \n";
                if (cmbPlatform.SelectedIndex == 0 || cmbPlatform.SelectedIndex == -1)
                    ErrMsg += "Choose Platform \n";
                if (cmbBrand.SelectedIndex == 0 || cmbBrand.SelectedIndex == -1)
                    ErrMsg += "Choose Brand  \n";
                if (cmbSideWall.SelectedIndex == 0 || cmbSideWall.SelectedIndex == -1)
                    ErrMsg += "Choose Sidewall \n";
                if (cmbType.SelectedIndex == 0 || cmbType.SelectedIndex == -1)
                    ErrMsg += "Choose Type \n";
                if (cmbSize.SelectedIndex == 0 || cmbSize.SelectedIndex == -1)
                    ErrMsg += "Choose Size \n";
                if (cmbRimWidth.SelectedIndex == 0 || cmbRimWidth.SelectedIndex == -1)
                    ErrMsg += "Choose RimWidth \n";
                if (cmbPress.SelectedIndex == 0 || cmbPress.SelectedIndex == -1)
                    ErrMsg += "Choose " + lblPress.Text + "\n";
                if (cmbPressB.SelectedIndex == 0 || cmbPressB.SelectedIndex == -1)
                    ErrMsg += "Choose " + lblPressSeq.Text + " \n";
                if ((cboLoadMachine.SelectedIndex == 0 || cboLoadMachine.SelectedIndex == -1) && Program.strPlantName == "MMN")
                    ErrMsg += "Choose Loading Machine \n";
                if ((cboPumping.SelectedIndex == 0 || cboPumping.SelectedIndex == -1) && Program.strPlantName == "MMN")
                    ErrMsg += "Choose Bumping Machine \n";
                if (txtMouldTempTop.Text == "")
                    ErrMsg += "Enter mould temperature top \n";
                if (txtMouldTempBottom.Text == "")
                    ErrMsg += "Enter mould temperature bottom \n";
                if (cmbPress.SelectedIndex > 0)
                {
                    SqlParameter[] spChk = new SqlParameter[] { 
                        new SqlParameter("@Unit", cmbPress.SelectedValue.ToString()), 
                        new SqlParameter("@Station", cmbPressB.SelectedValue.ToString()) 
                    };
                    bool boolSlot = (bool)dba.ExecuteScalar_SP("sp_chk_Aval_MouldSlot_Loading_Stage", spChk);
                    if (!boolSlot)
                        ErrMsg += "Slot already full. Kindly unload the stencil\n";
                }
                if (lblDelayMsg.Text != "" && txtRemarks.Text == "")
                    ErrMsg += "Enter remarks for delay loading";

                if (ErrMsg.Length > 0)
                    MessageBox.Show(ErrMsg, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                else
                {
                    Save_LoadData();
                }
            }
            catch (Exception ex)
            {
                Program.WriteToGtErrorLog("GT", "frmLoad", System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        private void ClearCntrls()
        {
            try
            {
                cmbPlatform.DataSource = null;
                cmbBrand.DataSource = null;
                cmbSideWall.DataSource = null;
                cmbType.DataSource = null;
                cmbSize.DataSource = null;
                cmbRimWidth.DataSource = null;

                cmbPlatform.Items.Clear();
                cmbBrand.Items.Clear();
                cmbSideWall.Items.Clear();
                cmbType.Items.Clear();
                cmbSize.Items.Clear();
                cmbRimWidth.Items.Clear();

                cmbPlatform.Enabled = true;
                cmbBrand.Enabled = true;
                cmbSideWall.Enabled = true;
                cmbType.Enabled = true;
                cmbSize.Enabled = true;
                cmbRimWidth.Enabled = true;

                cmbPress.DataSource = null;
                cmbPressB.DataSource = null;

                txtProcessId.Text = "";
                txtRemarks.Text = "";
                lblTyretype.Text = "";
                lblTyreSize.Text = "";
                lblRimWidth.Text = "";
                lblOperator.Text = "";
                lblDateShift.Text = "";
                lblMouldCode.Text = "";
                lblFormerCode.Text = "";
                lblBuildingPressure.Text = "";
                lblGTtemp.Text = "";
                lblNoOfBW.Text = "";
                lblBandBW.Text = "";
                lblGTod.Text = "";
                lblGTwidth.Text = "";
                lblBrand.Text = "";
                lblSidewall.Text = "";
                txtMouldTempTop.Text = "";
                txtMouldTempBottom.Text = "";
                lblDelayMsg.Text = "";

                if (Program.strPlantName == "MMN")
                {
                    cboLoadMachine.SelectedIndex = 0;
                    cboPumping.SelectedIndex = 0;
                }
            }
            catch (Exception ex)
            {
                Program.WriteToGtErrorLog("GT", "frmLoad", System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        private void btnReset_Click(object sender, System.EventArgs e)
        {
            frmLoad_Load(sender, e);
        }
        private void Save_LoadData()
        {
            try
            {
                SqlParameter[] sp = new SqlParameter[] { 
                    new SqlParameter("@StencilNo", txtStencilno.Text), 
                    new SqlParameter("@Config", cmbPlatform.SelectedItem.ToString()), 
                    new SqlParameter("@Brand", cmbBrand.SelectedItem.ToString()), 
                    new SqlParameter("@Sidewall", cmbSideWall.SelectedItem.ToString()), 
                    new SqlParameter("@TyreType", cmbType.SelectedItem.ToString()), 
                    new SqlParameter("@TyreSize", cmbSize.SelectedItem.ToString()), 
                    new SqlParameter("@RimSize", cmbRimWidth.SelectedItem.ToString()), 
                    new SqlParameter("@Process_Id", txtProcessId.Text), 
                    new SqlParameter("@Press", cmbPress.SelectedValue.ToString()), 
                    new SqlParameter("@Loaded_By", Program.strUserName), 
                    new SqlParameter("@Loaded_Remarks", txtRemarks.Text), 
                    new SqlParameter("@MouldTemp", txtMouldTempTop.Text), 
                    new SqlParameter("@MouldTempBottom", txtMouldTempBottom.Text), 
                    new SqlParameter("@Loading_Machine", Program.strPlantName == "MMN" ? cboLoadMachine.SelectedValue.ToString() : cmbPress.SelectedValue.ToString()), 
                    new SqlParameter("@Pumping_Machine", Program.strPlantName == "MMN" ? cboPumping.SelectedValue.ToString() : ""), 
                    new SqlParameter("@PressB", cmbPressB.SelectedValue.ToString()) 
                };
                int resp = dba.ExecuteNonQuery_SP("sp_ins_ProductionDetails_Stage3_V1", sp);
                if (resp > 0)
                {
                    SqlParameter[] spUpd = new SqlParameter[] { new SqlParameter("@StencilNo", txtStencilno.Text) };
                    dba.ExecuteNonQuery_SP("sp_upd_Loading_Status", spUpd);
                    MessageBox.Show("Record Saved Successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Bind_StencilNo();
                }
                else
                    MessageBox.Show("Try Again", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                Program.WriteToGtErrorLog("GT", "frmLoad", System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
    }
}
