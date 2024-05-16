using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Configuration;
using System.Reflection;

namespace GT
{
    public partial class frmUserCreate : Form
    {
        DBAccess dbCon = new DBAccess();
        public frmUserCreate()
        {
            InitializeComponent();
        }
        private void frmUserCreate_Load(object sender, EventArgs e)
        {
            try
            {
                panel1.Location = new Point(0, 0);
                DataGridViewCellStyle style = this.gvUserDetails.ColumnHeadersDefaultCellStyle;
                style.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                this.KeyPreview = true;
                this.txtMobile.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtNumericPercentage_KeyPress);
                gvUserDetails.CellClick += new DataGridViewCellEventHandler(GridViewRow_SelectionChanged);
                Bind_Gridview();
            }
            catch (Exception ex)
            {
                Program.WriteToGtErrorLog("GT", "frmUserCreate", MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        private void Bind_Gridview()
        {
            try
            {
                DataTable dt = (DataTable)dbCon.ExecuteReader_SP("SP_SEL_UserCreate_UserDetails", DBAccess.Return_Type.DataTable);
                gvUserDetails.DataSource = dt;
                gvUserDetails.Columns[0].HeaderText = "USER NAME";
                gvUserDetails.Columns[1].HeaderText = "MOBILE";
                gvUserDetails.Columns[2].HeaderText = "EMP ID";
            }
            catch (Exception ex)
            {
                Program.WriteToGtErrorLog("GT", "frmUserCreate", MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        private void txtNumericPercentage_KeyPress(object sender, KeyPressEventArgs e)
        {
            Program.digitonly(e);
        }
        private void GridViewRow_SelectionChanged(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex == 0 || e.RowIndex > 0)
                {
                    txtUserName.Text = (gvUserDetails.Rows[e.RowIndex].Cells[0].Value).ToString();
                    txtMobile.Text = (gvUserDetails.Rows[e.RowIndex].Cells[1].Value).ToString();
                    txtEmpID.Text = (gvUserDetails.Rows[e.RowIndex].Cells[2].Value).ToString();
                    btnSave.Text = "UPDATE";
                    txtUserName.Enabled = false;
                    txtEmpID.Enabled = false;
                    btnDelete.Visible = true;

                    SqlParameter[] sp = new SqlParameter[] { new SqlParameter("@UserName", txtUserName.Text) };
                    DataTable dt = (DataTable)dbCon.ExecuteReader_SP("sp_sel_UserDetails_menu_privilege", sp, DBAccess.Return_Type.DataTable);
                    if (dt.Rows.Count > 0)
                    {
                        txtPassword.Text = dt.Rows[0]["Password1"].ToString();
                        txtConfirmPass.Text = dt.Rows[0]["Password1"].ToString();
                        chkList1.SetItemChecked(0, Convert.ToBoolean(dt.Rows[0]["mnuWtVariantMaster"].ToString().ToLower()));
                        chkList1.SetItemChecked(1, Convert.ToBoolean(dt.Rows[0]["mnuFrictionMaster"].ToString().ToLower()));
                        chkList1.SetItemChecked(2, Convert.ToBoolean(dt.Rows[0]["mnuTypeMaster"].ToString().ToLower()));
                        chkList1.SetItemChecked(3, Convert.ToBoolean(dt.Rows[0]["mnuMouldMaster"].ToString().ToLower()));
                        chkList1.SetItemChecked(4, Convert.ToBoolean(dt.Rows[0]["mnuBlendMaster"].ToString().ToLower()));
                        chkList1.SetItemChecked(5, Convert.ToBoolean(dt.Rows[0]["mnuWtTolerance"].ToString().ToLower()));
                        chkList1.SetItemChecked(6, Convert.ToBoolean(dt.Rows[0]["mnuD4Defect"].ToString().ToLower()));
                        chkList1.SetItemChecked(7, Convert.ToBoolean(dt.Rows[0]["mnuWeightStandard"].ToString().ToLower()));
                        chkList1.SetItemChecked(8, Convert.ToBoolean(dt.Rows[0]["mnuSizePressStandard"].ToString().ToLower()));
                        chkList1.SetItemChecked(9, Convert.ToBoolean(dt.Rows[0]["mnuProdAllocate"].ToString().ToLower()));
                        chkList1.SetItemChecked(10, Convert.ToBoolean(dt.Rows[0]["mnuRunningAllocate"].ToString().ToLower()));
                        chkList1.SetItemChecked(11, Convert.ToBoolean(dt.Rows[0]["mnuProdRevoke"].ToString().ToLower()));

                        chkList1.SetItemChecked(12, Convert.ToBoolean(dt.Rows[0]["mnuWeightTrack"].ToString().ToLower()));
                        chkList1.SetItemChecked(13, Convert.ToBoolean(dt.Rows[0]["mnuGtBuild"].ToString().ToLower()));
                        chkList1.SetItemChecked(14, Convert.ToBoolean(dt.Rows[0]["mnuTyreCure"].ToString().ToLower()));
                        chkList1.SetItemChecked(15, Convert.ToBoolean(dt.Rows[0]["mnuInspection"].ToString().ToLower()));
                        chkList1.SetItemChecked(16, Convert.ToBoolean(dt.Rows[0]["mnuInspectionReview"].ToString().ToLower()));

                        chkList1.SetItemChecked(17, Convert.ToBoolean(dt.Rows[0]["mnuStencilUpdate"].ToString().ToLower()));
                        chkList1.SetItemChecked(18, Convert.ToBoolean(dt.Rows[0]["mnuStencilCancel"].ToString().ToLower()));

                        chkList1.SetItemChecked(19, Convert.ToBoolean(dt.Rows[0]["mnuStencilRpt"].ToString().ToLower()));
                        chkList1.SetItemChecked(20, Convert.ToBoolean(dt.Rows[0]["mnuGeneralRpt"].ToString().ToLower()));
                        chkList1.SetItemChecked(21, Convert.ToBoolean(dt.Rows[0]["mnuCompoundConsumrpt"].ToString().ToLower()));
                        chkList1.SetItemChecked(22, Convert.ToBoolean(dt.Rows[0]["mnuGtBuildRpt"].ToString().ToLower()));
                        chkList1.SetItemChecked(23, Convert.ToBoolean(dt.Rows[0]["mnuLoadUnloadRpt"].ToString().ToLower()));
                        chkList1.SetItemChecked(24, Convert.ToBoolean(dt.Rows[0]["mnuBarcodedRpt"].ToString().ToLower()));
                        chkList1.SetItemChecked(25, Convert.ToBoolean(dt.Rows[0]["mnuDefectedRpt"].ToString().ToLower()));
                        chkList1.SetItemChecked(26, Convert.ToBoolean(dt.Rows[0]["mnuStencilCancelRpt"].ToString().ToLower()));
                        chkList1.SetItemChecked(27, Convert.ToBoolean(dt.Rows[0]["mnuDispatchedList"].ToString().ToLower()));
                        chkList1.SetItemChecked(28, Convert.ToBoolean(dt.Rows[0]["mnuDashboard"].ToString().ToLower()));

                        chkList1.SetItemChecked(29, Convert.ToBoolean(dt.Rows[0]["mnuDelayApproval"].ToString().ToLower()));
                        chkList1.SetItemChecked(30, Convert.ToBoolean(dt.Rows[0]["mnuManualWeighEntry"].ToString().ToLower()));

                        chkList1.SetItemChecked(31, Convert.ToBoolean(dt.Rows[0]["mnuConcessionChart"].ToString().ToLower()));
                        chkList1.SetItemChecked(32, Convert.ToBoolean(dt.Rows[0]["mnuConcessionPlan"].ToString().ToLower()));

                        chkList1.SetItemChecked(33, Convert.ToBoolean(dt.Rows[0]["mnuRimInward"].ToString().ToLower()));
                        chkList1.SetItemChecked(34, Convert.ToBoolean(dt.Rows[0]["mnuRimStockConvert"].ToString().ToLower()));
                    }
                }
            }
            catch (Exception ex)
            {
                Program.WriteToGtErrorLog("GT", "frmUserCreate", MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        private void btnClear_Click(object sender, EventArgs e)
        {
            txtUserName.Text = "";
            txtPassword.Text = "";
            txtConfirmPass.Text = "";
            txtMobile.Text = "";
            txtEmpID.Text = "";
            lblErrMsg.Text = "";
            btnSave.Text = "SAVE";
            txtUserName.Enabled = true;
            txtEmpID.Enabled = true;
            txtPassword.Enabled = true;
            txtConfirmPass.Enabled = true;
            btnDelete.Visible = false;
            for (int j = 0; j < chkList1.Items.Count; j++)
            {
                chkList1.SetItemChecked(j, false);
            }
            Bind_Gridview();
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                bool mnuWtVariantMaster = false;
                bool mnuFrictionMaster = false;
                bool mnuTypeMaster = false;
                bool mnuMouldMaster = false;
                bool mnuBlendMaster = false;
                bool mnuWtTolerance = false;
                bool mnuD4Defect = false;
                bool mnuWeightStandard = false;
                bool mnuSizePressStandard = false;
                bool mnuProdAllocate = false;
                bool mnuRunningAllocate = false;
                bool mnuProdRevoke = false;
                bool mnuWeightTrack = false;
                bool mnuGtBuild = false;
                bool mnuTyreCure = false;
                bool mnuInspection = false;
                bool mnuInspectionReview = false;
                bool mnuStencilUpdate = false;
                bool mnuStencilCancel = false;
                bool mnuStencilRpt = false;
                bool mnuGeneralReport = false;
                bool mnuCompoundConsumrpt = false;
                bool mnuGtBuildRpt = false;
                bool mnuLoadUnloadRpt = false;
                bool mnuBarcodedRpt = false;
                bool mnuDefectedRpt = false;
                bool mnuStencilCancelRpt = false;
                bool mnuDispatchedList = false;
                bool mnuDashboard = false;
                bool mnuDelayApproval = false;
                bool mnuManualWeighEntry = false;
                bool mnuConcessionChart = false;
                bool mnuConcessionPlan = false;
                bool mnuRimInward = false;
                bool mnuRimStockConvert = false;

                for (int j = 0; j < chkList1.CheckedItems.Count; j++)
                {
                    if (chkList1.CheckedItems[j].ToString() == "WEIGHT VARIANT MASTER")
                        mnuWtVariantMaster = true;
                    else if (chkList1.CheckedItems[j].ToString() == "FRICTION ORIGIN MASTER")
                        mnuFrictionMaster = true;
                    else if (chkList1.CheckedItems[j].ToString() == "TYPE MASTER")
                        mnuTypeMaster = true;
                    else if (chkList1.CheckedItems[j].ToString() == "MOULD MASTER")
                        mnuMouldMaster = true;
                    else if (chkList1.CheckedItems[j].ToString() == "BLEND MASTER")
                        mnuBlendMaster = true;
                    else if (chkList1.CheckedItems[j].ToString() == "WEIGHT TOLERANCE")
                        mnuWtTolerance = true;
                    else if (chkList1.CheckedItems[j].ToString() == "D4 DEFECT ALLOWANCE")
                        mnuD4Defect = true;
                    else if (chkList1.CheckedItems[j].ToString() == "WEIGHT STANDARD")
                        mnuWeightStandard = true;
                    else if (chkList1.CheckedItems[j].ToString() == "TYRE SIZE-PRESS STANDARD")
                        mnuSizePressStandard = true;
                    else if (chkList1.CheckedItems[j].ToString() == "PRODUCTION ALLOCATE")
                        mnuProdAllocate = true;
                    else if (chkList1.CheckedItems[j].ToString() == "RUNNING MOULD ALLOCATE")
                        mnuRunningAllocate = true;
                    else if (chkList1.CheckedItems[j].ToString() == "PRODUCTION REVOKE")
                        mnuProdRevoke = true;
                    else if (chkList1.CheckedItems[j].ToString() == "WEIGH (STAGE-I)")
                        mnuWeightTrack = true;
                    else if (chkList1.CheckedItems[j].ToString() == "BUILD (STAGE-II)")
                        mnuGtBuild = true;
                    else if (chkList1.CheckedItems[j].ToString() == "CURE (STAGE-III)")
                        mnuTyreCure = true;
                    else if (chkList1.CheckedItems[j].ToString() == "INSPECTION (STAGE-IV)")
                        mnuInspection = true;
                    else if (chkList1.CheckedItems[j].ToString() == "INSPECTION REVIEW")
                        mnuInspectionReview = true;
                    else if (chkList1.CheckedItems[j].ToString() == "STENCIL-NO UPDATE")
                        mnuStencilUpdate = true;
                    else if (chkList1.CheckedItems[j].ToString() == "STENCIL-NO CANCEL")
                        mnuStencilCancel = true;
                    else if (chkList1.CheckedItems[j].ToString() == "STENCIL WISE REPORT")
                        mnuStencilRpt = true;
                    else if (chkList1.CheckedItems[j].ToString() == "COMMON REPORT")
                        mnuGeneralReport = true;
                    else if (chkList1.CheckedItems[j].ToString() == "COMPOUND CONSUMPTION REPORT")
                        mnuCompoundConsumrpt = true;
                    else if (chkList1.CheckedItems[j].ToString() == "GT BUILD REPORT")
                        mnuGtBuildRpt = true;
                    else if (chkList1.CheckedItems[j].ToString() == "LOAD/UNLOAD REPORT")
                        mnuLoadUnloadRpt = true;
                    else if (chkList1.CheckedItems[j].ToString() == "BARCODED REPORT")
                        mnuBarcodedRpt = true;
                    else if (chkList1.CheckedItems[j].ToString() == "DEFECTED REPORT")
                        mnuDefectedRpt = true;
                    else if (chkList1.CheckedItems[j].ToString() == "REJECTED REPORT")
                        mnuStencilCancelRpt = true;
                    else if (chkList1.CheckedItems[j].ToString() == "DISPATCHED REPORT")
                        mnuDispatchedList = true;
                    else if (chkList1.CheckedItems[j].ToString() == "DASHBOARD")
                        mnuDashboard = true;
                    else if (chkList1.CheckedItems[j].ToString() == "LOAD/UNLOD DELAY APPROVAL")
                        mnuDelayApproval = true;
                    else if (chkList1.CheckedItems[j].ToString() == "MANUAL WEIGHING ENTRY")
                        mnuManualWeighEntry = true;
                    else if (chkList1.CheckedItems[j].ToString() == "COMP CONCESSION CHART")
                        mnuConcessionChart = true;
                    else if (chkList1.CheckedItems[j].ToString() == "COMP CONCESSION PLAN")
                        mnuConcessionPlan = true;
                    else if (chkList1.CheckedItems[j].ToString() == "RIM STOCK INWARD")
                        mnuRimInward = true;
                    else if (chkList1.CheckedItems[j].ToString() == "RIM STOCK CONVERT")
                        mnuRimStockConvert = true;
                }

                if (txtUserName.Text == "")
                {
                    lblErrMsg.Text = "Enter username";
                    txtUserName.Focus();
                }
                else if (txtPassword.Text == "")
                {
                    lblErrMsg.Text = "Enter password";
                    txtPassword.Focus();
                }
                else if (txtConfirmPass.Text == "")
                {
                    lblErrMsg.Text = "Enter confirm password";
                    txtConfirmPass.Focus();
                }
                else if (txtPassword.Text != txtConfirmPass.Text)
                {
                    lblErrMsg.Text = "Password mismatch - Enter correct password";
                    txtConfirmPass.Focus();
                }
                else if (txtMobile.Text == "")
                {
                    lblErrMsg.Text = "Enter mobile no.";
                    txtMobile.Focus();
                }
                else if (txtEmpID.Text == "")
                {
                    lblErrMsg.Text = "Enter employee id";
                    txtEmpID.Focus();
                }
                else
                {
                    int mode = 0;
                    if (btnSave.Text == "SAVE")
                    {
                        SqlParameter[] sp = new SqlParameter[] { new SqlParameter("@UserName", txtUserName.Text) };
                        DataTable dt = (DataTable)dbCon.ExecuteReader_SP("sp_chk_username", sp, DBAccess.Return_Type.DataTable);
                        if (dt.Rows.Count > 0)
                            lblErrMsg.Text = "Username already created to Employee ID: " + dt.Rows[0]["EmpID"].ToString() + " ~ Contact No:" + dt.Rows[0]["ContactNo"].ToString();
                        else
                            mode = 1;
                    }
                    else if (btnSave.Text == "UPDATE")
                        mode = 2;

                    if (mode > 0)
                    {
                        SqlParameter[] sp1 = new SqlParameter[]{
                            new SqlParameter("@UserName", txtUserName.Text), 
                            new SqlParameter("@Password1", txtPassword.Text), 
                            new SqlParameter("@ContactNo", txtMobile.Text), 
                            new SqlParameter("@EmpID", txtEmpID.Text), 
                            new SqlParameter("@mode", mode), 
                            new SqlParameter("@mnuWtVariantMaster", mnuWtVariantMaster), 
                            new SqlParameter("@mnuFrictionMaster", mnuFrictionMaster), 
                            new SqlParameter("@mnuTypeMaster", mnuTypeMaster), 
                            new SqlParameter("@mnuMouldMaster", mnuMouldMaster), 
                            new SqlParameter("@mnuBlendMaster", mnuBlendMaster), 
                            new SqlParameter("@mnuWtTolerance", mnuWtTolerance),  
                            new SqlParameter("@mnuD4Defect", mnuD4Defect), 
                            new SqlParameter("@mnuWeightStandard", mnuWeightStandard), 
                            new SqlParameter("@mnuSizePressStandard", mnuSizePressStandard), 
                            new SqlParameter("@mnuProdAllocate", mnuProdAllocate),
                            new SqlParameter("@mnuRunningAllocate", mnuRunningAllocate),
                            new SqlParameter("@mnuProdRevoke", mnuProdRevoke),
                            new SqlParameter("@mnuWeightTrack", mnuWeightTrack), 
                            new SqlParameter("@mnuGtBuild", mnuGtBuild), 
                            new SqlParameter("@mnuTyreCure", mnuTyreCure),
                            new SqlParameter("@mnuInspection", mnuInspection), 
                            new SqlParameter("@mnuInspectionReview", mnuInspectionReview), 
                            new SqlParameter("@mnuStencilUpdate", mnuStencilUpdate), 
                            new SqlParameter("@mnuStencilCancel", mnuStencilCancel),
                            new SqlParameter("@mnuStencilRpt", mnuStencilRpt), 
                            new SqlParameter("@mnuGeneralRpt", mnuGeneralReport), 
                            new SqlParameter("@mnuCompoundConsumrpt", mnuCompoundConsumrpt), 
                            new SqlParameter("@mnuGtBuildRpt", mnuGtBuildRpt), 
                            new SqlParameter("@mnuLoadUnloadRpt", mnuLoadUnloadRpt), 
                            new SqlParameter("@mnuBarcodedRpt", mnuBarcodedRpt),
                            new SqlParameter("@mnuDefectedRpt", mnuDefectedRpt), 
                            new SqlParameter("@mnuStencilCancelRpt", mnuStencilCancelRpt), 
                            new SqlParameter("@mnuDispatchedList", mnuDispatchedList), 
                            new SqlParameter("@mnuDashboard", mnuDashboard),
                            new SqlParameter("@mnuDelayApproval", mnuDelayApproval),
                            new SqlParameter("@mnuManualWeighEntry", mnuManualWeighEntry), 
                            new SqlParameter("@mnuConcessionChart", mnuConcessionChart), 
                            new SqlParameter("@mnuConcessionPlan", mnuConcessionPlan),
                            new SqlParameter("@mnuRimInward", mnuRimInward),
                            new SqlParameter("@mnuRimStockConvert", mnuRimStockConvert)
                        };
                        int resp = dbCon.ExecuteNonQuery_SP("SP_SAV_UserCreate_UserDetails_V1", sp1);
                        if (resp > 0)
                        {
                            MessageBox.Show("Details successfully " + btnSave.Text.ToLower() + "d");
                            btnClear_Click(sender, e);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Program.WriteToGtErrorLog("GT", "frmUserCreate", MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                SqlParameter[] sp = new SqlParameter[] { new SqlParameter("@UserName", txtUserName.Text), new SqlParameter("@EmpID", txtEmpID.Text) };
                int resp = dbCon.ExecuteNonQuery_SP("SP_UPD_UserCreate_UserStatus", sp);
                if (resp > 0)
                {
                    MessageBox.Show("User accounts successfully " + btnDelete.Text.ToLower() + "d");
                    btnClear_Click(sender, e);
                }
            }
            catch (Exception ex)
            {
                Program.WriteToGtErrorLog("GT", "frmUserCreate", MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
    }
}
