using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.IO.Ports;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using Modbus.Device;
using Modbus.Utility;

namespace GT
{
    public partial class frmGtWeigh : Form
    {
        DBAccess dba = new DBAccess();
        private DataTable dtWT;
        private string strWtClass = "", strFriction = "", strCommunication = "";
        public static int intRemainQty = -1;
        Label lblBlink;
        //string custcode;
        int result;
        public frmGtWeigh()
        {
            try
            {
                InitializeComponent();
               
                cboShift.Items.Add("CHOOSE");
                cboShift.Items.Add("SHIFT 1");
                cboShift.Items.Add("SHIFT 2");
                //cboShift.Items.Add("SHIFT 3");
                cboShift.SelectedIndex = 0;

                this.dtpPrepareDate.MinDate = DateTime.Now.AddDays(-3);
                this.dtpPrepareDate.MaxDate = DateTime.Now.AddDays(0);

                if (frmWeighPlan.Weigh_Manual)
                    this.dtpPrepareDate.CustomFormat = String.Format("MMM/dd/yyyy hh:mm tt");

                cboCommunication.DataSource = Program.dt_Wt_Com;
                cboCommunication.ValueMember = "Com_Method";
                cboCommunication.DisplayMember = "Com_Method";
                cboCommunication.SelectedIndex = 0;

                string filePath = AppDomain.CurrentDomain.BaseDirectory + "\\comm.dll";
                FileInfo file1 = new FileInfo(filePath);
                if (file1.Exists)
                {
                    var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
                    using (var streamReader = new StreamReader(fileStream, Encoding.UTF8))
                    {
                        strCommunication = EncryptDecrypt.Decrypt(streamReader.ReadToEnd(), "COMMUNICATION");
                    }
                    cboCommunication.SelectedIndex = cboCommunication.FindStringExact(strCommunication);
                    
                }
                //**********************************************************************
                
         
            }
            catch (Exception ex)
            {
                Program.WriteToGtErrorLog("GT", "frmGtWeigh", System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        private void frmGtWeigh_Load(object sender, EventArgs e)
        {
            try
            {
                panel1.Location = new Point(5, 20);

                
                
                SqlParameter[] sp = new SqlParameter[] { new SqlParameter("@PlanID", frmWeighPlan.ProductionID) };
                DataTable dt = (DataTable)dba.ExecuteReader_SP("sp_Item_WeightTrack_MouldPlanWise", sp, DBAccess.Return_Type.DataTable);
                if (dt.Rows.Count == 1)
                {
                    Bind_PlannedPendingListGv();
                    //custcode = FindCustCode();
                    result = FindCustCode();
                    Ctrl_Clear();
                    lblPlatform.Text = dt.Rows[0]["Config"].ToString();
                    lblBrand.Text = dt.Rows[0]["brand"].ToString();
                    lblSidewall.Text = dt.Rows[0]["sidewall"].ToString();
                    lblType.Text = dt.Rows[0]["tyretype"].ToString();
                    lblSize.Text = dt.Rows[0]["tyresize"].ToString();
                    lblRim.Text = dt.Rows[0]["rimsize"].ToString();
                    lblQty.Text = dt.Rows[0]["RequiredQuantity"].ToString();
                   

                    dtWT = (DataTable)dba.ExecuteReader_SP("SP_SEL_WeightTrack_WeightVariantDetails", DBAccess.Return_Type.DataTable);
                    Make_DtpDate();
                    Bind_ComboBox();
                    lblOperator.Text = Program.strUserName;
                    
                    
                    //int i = 1;
                }
                else
                {
                    MessageBox.Show("KINLDY CHOOSE THE CORRECT ITEM");
                    this.Hide();

                    Form frm = new frmWeighPlan();
                    frm.Location = new Point(0, 0);
                    frm.MdiParent = this.MdiParent;
                    frm.WindowState = FormWindowState.Maximized;
                    frm.Show();
                }
            }
            catch (Exception ex)
            {
                Program.WriteToGtErrorLog("GT", "frmGtWeigh", System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        private void KeyEvent(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F8)
            {
                bool boolKeyAction = false;
                if (pnlHbw.Visible && lblStatusHbw.Text != "OK" && Program.boolManualWeighing)
                    boolKeyAction = true;
                else if (pnlBase.Visible && lblStatusBase.Text != "OK" && (Program.boolManualWeighing || lnkConcBase.Visible))
                    boolKeyAction = true;
                else if (pnlInterface.Visible && lblStatusInter.Text != "OK" && Program.boolManualWeighing)
                    boolKeyAction = true;
                else if (pnlCenter.Visible && lblStatusCenter.Text != "OK" && (Program.boolManualWeighing || lnkConcCenter.Visible))
                    boolKeyAction = true;
                else if (pnlTread.Visible && lblStatusTread.Text != "OK" && (Program.boolManualWeighing || lnkConcTread.Visible))
                    boolKeyAction = true;

                if (boolKeyAction)
                {
                    txtSkipText.Text = "";
                    pnlSkip.Visible = true;
                    txtSkipText.Focus();
                }
            }
        }

        private void Make_DtpDate()
        {
            DateTime endTime = Convert.ToDateTime("07:06:00");
            if (System.DateTime.Now.TimeOfDay < endTime.TimeOfDay)
            {
                DateTime result = DateTime.Now.Subtract(TimeSpan.FromDays(1));
                dtpPrepareDate.Value = result;
            }
        }

        private void Bind_ComboBox()
        {
            try
            {
                
                
                DateTime shift1Time = Convert.ToDateTime("19:06:00");
                //DateTime shift2Time = Convert.ToDateTime("22:06:00");
                DateTime shift2Time = Convert.ToDateTime("07:06:00");
                if ((System.DateTime.Now.TimeOfDay > shift2Time.TimeOfDay) && (System.DateTime.Now.TimeOfDay <= shift1Time.TimeOfDay))
                    cboShift.SelectedIndex = 1;
                else if ((System.DateTime.Now.TimeOfDay > shift1Time.TimeOfDay) || (System.DateTime.Now.TimeOfDay < shift2Time.TimeOfDay))
                    cboShift.SelectedIndex = 2;
               // else if (System.DateTime.Now.TimeOfDay > shift2Time.TimeOfDay || System.DateTime.Now.TimeOfDay < shift3Time.TimeOfDay)
                   // cboShift.SelectedIndex = 3;

                cboType.DataSource = null;
                cboMould.DataSource = null;

                SqlParameter[] spWClass = new SqlParameter[] {
                    new SqlParameter("@TyreSize", lblSize.Text),
                    new SqlParameter("@RimSize", lblRim.Text),
                    new SqlParameter("@TyreType", lblType.Text)

                
                
                //DateTime shift1Time = Convert.ToDateTime("14:06:00");
                //DateTime shift2Time = Convert.ToDateTime("22:06:00");
                //DateTime shift3Time = Convert.ToDateTime("06:06:00");
                //if ((System.DateTime.Now.TimeOfDay > shift3Time.TimeOfDay) && (System.DateTime.Now.TimeOfDay <= shift1Time.TimeOfDay))
                //    cboShift.SelectedIndex = 1;
                //else if ((System.DateTime.Now.TimeOfDay > shift1Time.TimeOfDay) && (System.DateTime.Now.TimeOfDay <= shift2Time.TimeOfDay))
                //    cboShift.SelectedIndex = 2;
                //else if (System.DateTime.Now.TimeOfDay > shift2Time.TimeOfDay || System.DateTime.Now.TimeOfDay < shift3Time.TimeOfDay)
                //    cboShift.SelectedIndex = 3;

                //cboType.DataSource = null;
                //cboMould.DataSource = null;

                //SqlParameter[] spWClass = new SqlParameter[] {
                //    new SqlParameter("@TyreSize", lblSize.Text),
                //    new SqlParameter("@RimSize", lblRim.Text),
                //    new SqlParameter("@TyreType", lblType.Text)
                };
                DataTable dtWClass = (DataTable)dba.ExecuteReader_SP("SP_SEL_WeightTrack_WtClass_V1", spWClass, DBAccess.Return_Type.DataTable);
                if (dtWClass.Rows.Count > 0)
                {
                    DataRow dr = dtWClass.NewRow();
                    dr.ItemArray = new object[] { "CHOOSE" };
                    dtWClass.Rows.InsertAt(dr, 0);

                    cboWtClass.DataSource = dtWClass;
                    cboWtClass.DisplayMember = "WtClass";
                    cboWtClass.ValueMember = "WtClass";

                    if (cboWtClass.Items.Count == 2)
                        cboWtClass.SelectedIndex = 1;
                    else if (strWtClass != "")
                        cboWtClass.SelectedIndex = cboWtClass.FindStringExact(strWtClass);
                }
            }
            catch (Exception ex)
            {
                Program.WriteToGtErrorLog("GT", "frmGtWeigh", System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        private void cboWtClass_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                cboFriction.DataSource = null;
                cboType.DataSource = null;
                cboMould.DataSource = null;
                if (cboWtClass.SelectedIndex > 0)
                {
                    SqlParameter[] spOrigin = new SqlParameter[] {
                        new SqlParameter("@TyreSize", lblSize.Text),
                        new SqlParameter("@RimSize", lblRim.Text),
                        new SqlParameter("@TyreType", lblType.Text),
                        new SqlParameter("@WtClass", cboWtClass.SelectedValue.ToString())
                    };
                    DataTable dtOrigin = (DataTable)dba.ExecuteReader_SP("SP_SEL_WeightTrack_FrictionOrigin_V1", spOrigin, DBAccess.Return_Type.DataTable);
                    if (dtOrigin.Rows.Count > 0)
                    {
                        DataRow dr = dtOrigin.NewRow();
                        dr.ItemArray = new object[] { "CHOOSE" };
                        dtOrigin.Rows.InsertAt(dr, 0);

                        cboFriction.DataSource = dtOrigin;
                        cboFriction.DisplayMember = "FrictionOrigin";
                        cboFriction.ValueMember = "FrictionOrigin";

                        if (cboFriction.Items.Count == 2)
                            cboFriction.SelectedIndex = 1;
                        else if (strFriction != "")
                            cboFriction.SelectedIndex = cboFriction.FindStringExact(strFriction);
                    }
                }
            }
            catch (Exception ex)
            {
                Program.WriteToGtErrorLog("GT", "frmGtWeigh", System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        private void cboFriction_IndexChange(object sender, EventArgs e)
        {
            try
            {
                cboType.DataSource = null;
                cboMould.DataSource = null;
                if (cboFriction.SelectedIndex > 0)
                {
                    SqlParameter[] sp = new SqlParameter[] {
                        new SqlParameter("@TyreSize", lblSize.Text),
                        new SqlParameter("@RimSize", lblRim.Text),
                        new SqlParameter("@TyreType", lblType.Text),
                        new SqlParameter("@WtClass", cboWtClass.SelectedValue.ToString()),
                        new SqlParameter("@FrictionOrigin", cboFriction.SelectedValue.ToString()),
                        new SqlParameter("@Prod_ItemID", frmWeighPlan.Prod_ItemID)
                    };
                    DataTable dtType = (DataTable)dba.ExecuteReader_SP("SP_SEL_WeightTrack_TyreType_Priority_V2", sp, DBAccess.Return_Type.DataTable);
                    if (dtType.Rows.Count > 0)
                    {
                        DataRow dr = dtType.NewRow();
                        dr.ItemArray = new object[] { "CHOOSE" };
                        dtType.Rows.InsertAt(dr, 0);

                        cboType.DataSource = dtType;
                        cboType.DisplayMember = "TyreType";
                        cboType.ValueMember = "TyreType";

                        if (cboType.Items.Count == 2)
                            cboType.SelectedIndex = 1;
                    }
                    else
                    {
                        MessageBox.Show("TyreType is not available for this origin. Please inform admin");
                        btnWeightStart.Enabled = false;
                    }
                }
            }
            catch (Exception ex)
            {
                Program.WriteToGtErrorLog("GT", "frmGtWeigh", System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        private void cboType_IndexChange(object sender, EventArgs e)
        {
            try
            {
                cboMould.DataSource = null;
                if (cboType.SelectedIndex > 0)
                {
                    lblNoOfBw.Text = "";
                    SqlParameter[] sp1 = new SqlParameter[] { new SqlParameter("@Tyretype", cboType.SelectedValue.ToString()) };
                    bool strBwStatus = (bool)dba.ExecuteScalar_SP("sp_get_gtwt_BwStatus", sp1);
                    if (strBwStatus)
                    {
                        lblHbw.Text = "HS + BW";
                        lblHbw.ForeColor = Color.DeepPink;
                        lblHbw.BackColor = Color.White;
                        lblNoOfBw.Text = "No Of BW: ";
                    }
                    else
                    {
                        lblHbw.Text = "HEEL STRIP";
                        lblHbw.ForeColor = Color.Maroon;
                        lblHbw.BackColor = Color.Gainsboro;
                    }

                    SqlParameter[] sp = new SqlParameter[] {
                        new SqlParameter("@WtClass", cboWtClass.SelectedValue.ToString()),
                        new SqlParameter("@TyreType", cboType.SelectedValue.ToString()),
                        new SqlParameter("@TyreSize", lblSize.Text),
                        new SqlParameter("@RimSize", lblRim.Text),
                        new SqlParameter("@Config", lblPlatform.Text),
                        new SqlParameter("@Prod_ItemID", frmWeighPlan.Prod_ItemID),
                        new SqlParameter("@PlanID", frmWeighPlan.ProductionID)
                    };
                    DataTable dt = (DataTable)dba.ExecuteReader_SP("SP_SEL_WeightTrack_MouldCode_v2", sp, DBAccess.Return_Type.DataTable);
                    if (dt.Rows.Count > 0)
                    {
                        DataRow dr = dt.NewRow();
                        dr.ItemArray = new object[] { "CHOOSE" };
                        dt.Rows.InsertAt(dr, 0);
                        cboMould.DataSource = dt;
                        cboMould.DisplayMember = "MouldCode";
                        if (cboMould.Items.Count == 2)
                            cboMould.SelectedIndex = 1;
                    }
                }
            }
            catch (Exception ex)
            {
                Program.WriteToGtErrorLog("GT", "frmGtWeigh", System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        private void Ctrl_Clear()
        {
            try
            {
                pnlHbw.Visible = false;
                pnlBase.Visible = false;
                pnlInterface.Visible = false;
                pnlCenter.Visible = false;
                pnlTread.Visible = false;
                pnlSaveBtn.Visible = false;
                lblCcodeHbw.Text = "-";
                lblCcodeBase.Text = "-";
                lblCcodeInter.Text = "-";
                lblCcodeCenter.Text = "-";
                lblCcodeTread.Text = "-";
                lblWtHbw.Text = "0";
                lblWtBase.Text = "0";
                lblBaseTolerance.Text = "0";
                lblWtInter.Text = "0";
                lblWtCenter.Text = "0";
                lblWtTread.Text = "0";
                lblTreadTolerance.Text = "0";
                

                Ctrl_Clear_Restart();

                lblHbwType.Text = "";
                lblSpecWtTot.Text = "0";
                lblActWtTot.Text = "0";

                //______________________________________________________________________________________________commented by siva
                
                //DateTime endTime = Convert.ToDateTime("06:06:00");
                                          

                //if (System.DateTime.Now.TimeOfDay < endTime.TimeOfDay)
                //{

                //    //if (custcode == "189" || custcode == "DE0061" || custcode == "276" || custcode == "471" || custcode == "DE0075" || custcode == "487" || custcode == "ME0222" ||
                //    //   custcode == "2701" || custcode == "3772" || custcode == "ME0216" || custcode == "4808" || custcode == "4936" || custcode == "284" ||
                //    //   custcode == "DE0042")
                //    if (result==1)
                //    {
                       
                //        if (Program.strPlantName == "MMN")
                //        {
                //            txtStencil.Text = "127" + Convert.ToDateTime(dtpPrepareDate.Value.ToShortDateString()).Year.ToString().Substring(2, 2);                            
                //        }
                //        else if (Program.strPlantName == "PDK")
                //        {
                //            txtStencil.Text = "163" + Convert.ToDateTime(dtpPrepareDate.Value.ToShortDateString()).Year.ToString().Substring(2, 2);
                //        }
                //        else
                //        {
                //             txtStencil.Text = (Program.strPlantName == "SLTL" ? "L" : Program.strPlantName == "MMN" ? "C" : Program.strPlantName == "PDK" ? "P" : Program.strPlantName.Substring(0, 1)) +
                //                    DateTime.Now.Year.ToString().Substring(2, 2) + DateTime.Now.AddDays(-1).Month.ToString("00");
                                                           
                //        }
                //    }
                //    else
                //    {
                //        txtStencil.Text = (Program.strPlantName == "SLTL" ? "L" : Program.strPlantName == "MMN" ? "C" : Program.strPlantName == "PDK" ? "P" : Program.strPlantName.Substring(0, 1)) +
                //            DateTime.Now.Year.ToString().Substring(2, 2) + DateTime.Now.AddDays(-1).Month.ToString("00");                     

                //    }
                //}
                //else
                //{
                //    //if (custcode == "189" || custcode == "DE0061" || custcode == "276" || custcode == "471" || custcode == "DE0075" || custcode == "487" || custcode == "ME0222" ||
                //    //  custcode == "2701" || custcode == "3772" || custcode == "ME0216" || custcode == "4808" || custcode == "4936" || custcode == "284" ||
                //    //  custcode == "DE0042")
                //    if(result ==1)
                //    {
                       
                //        if (Program.strPlantName == "MMN")
                //        {
                //            txtStencil.Text = "127" + Convert.ToDateTime(dtpPrepareDate.Value.ToShortDateString()).Year.ToString().Substring(2, 2);
                            
                //        }

                //        else if (Program.strPlantName == "PDK")
                //        {
                //            txtStencil.Text = "163" + Convert.ToDateTime(dtpPrepareDate.Value.ToShortDateString()).Year.ToString().Substring(2, 2);
                //        }
                //        else                                                   
                //            {
                //                txtStencil.Text = (Program.strPlantName == "SLTL" ? "L" : Program.strPlantName == "MMN" ? "C" : Program.strPlantName == "PDK" ? "P" : Program.strPlantName.Substring(0, 1)) +
                //                    DateTime.Now.Year.ToString().Substring(2, 2) + DateTime.Now.Month.ToString("00");

                //            }                       
                    
                //    }

                //    else
                //    {
                //        txtStencil.Text = (Program.strPlantName == "SLTL" ? "L" : Program.strPlantName == "MMN" ? "C" : Program.strPlantName == "PDK" ? "P" : Program.strPlantName.Substring(0, 1)) +
                //        DateTime.Now.Year.ToString().Substring(2, 2) + DateTime.Now.Month.ToString("00");

                //        DateTime endTime = Convert.ToDateTime("07:06:00");
                //if (System.DateTime.Now.TimeOfDay < endTime.TimeOfDay)
                //    txtStencil.Text = (Program.strPlantName == "SLTL" ? "L" : Program.strPlantName == "MMN" ? "C" : Program.strPlantName.Substring(0, 1)) +
                //        DateTime.Now.Year.ToString().Substring(2, 2) + DateTime.Now.AddDays(-1).Month.ToString("00");
                //else
                //    txtStencil.Text = (Program.strPlantName == "SLTL" ? "L" : Program.strPlantName == "MMN" ? "C" : Program.strPlantName.Substring(0, 1)) +
                //        DateTime.Now.Year.ToString().Substring(2, 2) + DateTime.Now.Month.ToString("00");
                                               
                //    }
                //}
              // '_____________________________________________________________________commented by siva


                    DateTime endTime = Convert.ToDateTime("07:06:00");
                if (System.DateTime.Now.TimeOfDay < endTime.TimeOfDay)
                    txtStencil.Text = (Program.strPlantName == "SLTL" ? "L" : Program.strPlantName == "MMN" ? "C" : Program.strPlantName.Substring(0, 1)) +
                        DateTime.Now.Year.ToString().Substring(2, 2) + DateTime.Now.AddDays(-1).Month.ToString("00");
                else
                    txtStencil.Text = (Program.strPlantName == "SLTL" ? "L" : Program.strPlantName == "MMN" ? "C" : Program.strPlantName.Substring(0, 1)) +
                        DateTime.Now.Year.ToString().Substring(2, 2) + DateTime.Now.Month.ToString("00");
                string sequence = null;
                //DataTable dataseq = new DataTable();
                DataTable sequenceofData = (DataTable)dba.ExecuteReader_SP("Sp_Fetch_the_Serial_No", DBAccess.Return_Type.DataTable);
                sequence = sequenceofData.Rows[0]["stnclsequence"].ToString();
                //sequence = string.Format("{0:D5}", sequence);
                txtStencil.Text = txtStencil.Text + sequence;

                txtManualValue.Text = "0";
                txtManualValue.Visible = false;
                dtpPrepareDate.Enabled = false;
                if (frmWeighPlan.Weigh_Manual)
                {
                    txtManualValue.Visible = true;
                    dtpPrepareDate.Enabled = true;
                }

                txtManualValue.Text = "0";
                txtManualValue.Visible = false;
                dtpPrepareDate.Enabled = false;
                if (frmWeighPlan.Weigh_Manual)
                {
                    txtManualValue.Visible = true;
                    dtpPrepareDate.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                Program.WriteToGtErrorLog("GT", "frmGtWeigh", System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        private void Ctrl_Clear_Restart()
        {
            try
            {
                lblCcodeHbw.Font = new Font("Verdana", 12, FontStyle.Bold);
                lblCcodeBase.Font = new Font("Verdana", 12, FontStyle.Bold);
                lblCcodeInter.Font = new Font("Verdana", 12, FontStyle.Bold);
                lblCcodeCenter.Font = new Font("Verdana", 12, FontStyle.Bold);
                lblCcodeTread.Font = new Font("Verdana", 12, FontStyle.Bold);
                lblMwtHbw.Text = "0";
                lblMwtBase.Text = "0";
                lblMwtInter.Text = "0";
                lblMwtCenter.Text = "0";
                lblMwtTread.Text = "0";
                lblAddWtHbw.Text = "0";
                lblAddWtBase.Text = "0";
                lblAddWtInter.Text = "0";
                lblAddWtCenter.Text = "0";
                lblAddWtTread.Text = "0";
                txtMachineWt.Text = "0.00";
                lblStatusHbw.Text = "NOT OK";
                lblStatusBase.Text = "NOT OK";
                lblStatusInter.Text = "NOT OK";
                lblStatusCenter.Text = "NOT OK";
                lblStatusTread.Text = "NOT OK";
                lblStatusHbw.ForeColor = Color.Red;
                lblStatusBase.ForeColor = Color.Red;
                lblStatusInter.ForeColor = Color.Red;
                lblStatusCenter.ForeColor = Color.Red;
                lblStatusTread.ForeColor = Color.Red;
                lblHbwRemarks.Text = "";
                lblBaseRemarks.Text = "";
                lblInterRemarks.Text = "";
                lblCenterRemarks.Text = "";
                lblTreadRemarks.Text = "";
                txtSkipText.Text = "";
                pnlSkip.Visible = false;
                txtBatchHbw.Text = "";
                txtBatchBase.Text = "";
                txtBatchInter.Text = "";
                txtBatchCenter.Text = "";
                txtBatchTread.Text = "";
                timerHbw.Stop();
                timerBase.Stop();
                timerInter.Stop();
                timerCenter.Stop();
                timerTread.Stop();
            }
            catch (Exception ex)
            {
                Program.WriteToGtErrorLog("GT", "frmGtWeigh", System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        private void cbo_Enter_Keypress(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 13)
                SendKeys.Send("{TAB}");
        }

        private void cboMould_IndexChange(object sender, EventArgs e)
        {
            try
            {
                Ctrl_Clear();
                lnkConcBase.Text = "";
                lnkConcCenter.Text = "";
                lnkConcTread.Text = "";
                lblBaseConcBal.Text = "";
                lblCenterConcBal.Text = "";
                lblTreadConcBal.Text = "";
                pnlHbw.Visible = false;
                pnlBase.Visible = false;
                pnlInterface.Visible = false;
                pnlCenter.Visible = false;
                pnlTread.Visible = false;
                btnWeightStart.Enabled = false;
                if (cboMould.SelectedIndex > 0)
                {
                    SqlParameter[] sp = new SqlParameter[]{
                        new SqlParameter("@WtClass",cboWtClass.SelectedValue.ToString()),
                        new SqlParameter("@TyreType",cboType.SelectedValue.ToString()),
                        new SqlParameter("@TyreSize",lblSize.Text),
                        new SqlParameter("@RimSize",lblRim.Text),
                        new SqlParameter("@MouldCode",cboMould.Text)
                    };
                    DataTable dtWtMaster = (DataTable)dba.ExecuteReader_SP("SP_SEL_WeightTrack_WeightDetails_v1", sp, DBAccess.Return_Type.DataTable);
                    if (dtWtMaster.Rows.Count > 0)
                    {
                        string strConcBase = "", strConcCenter = "", strConcTread = "", strCompination = "";
                        SqlParameter[] sp2 = new SqlParameter[] { new SqlParameter("@TyreType", cboType.SelectedValue.ToString()) };
                        DataSet dsTypeCode = (DataSet)dba.ExecuteReader_SP("SP_SEL_WeightTrack_TypeDetails_v1", sp2, DBAccess.Return_Type.DataSet);

                        if (dsTypeCode.Tables[1].Rows.Count > 0)
                        {
                            foreach (DataRow cCode in dsTypeCode.Tables[1].Rows)
                            {
                                if (cCode["Category"].ToString() == "BASE")
                                {
                                    strConcBase = cCode["concCode"].ToString();
                                    lblBaseConcBal.Text = cCode["limitbalance"].ToString();
                                    lblBaseConcBal.Visible = true;
                                }
                                else if (cCode["Category"].ToString() == "CENTER")
                                {
                                    strConcCenter = cCode["concCode"].ToString();
                                    lblCenterConcBal.Text = cCode["limitbalance"].ToString();
                                    lblCenterConcBal.Visible = true;
                                }
                                else if (cCode["Category"].ToString() == "TREAD")
                                {
                                    strConcTread = cCode["concCode"].ToString();
                                    lblTreadConcBal.Text = cCode["limitbalance"].ToString();
                                    lblTreadConcBal.Visible = true;
                                }
                            }
                        }

                        if (dsTypeCode.Tables[0].Rows.Count > 0 || dsTypeCode.Tables[1].Rows.Count > 0)
                        {
                            btnWeightStart.Enabled = true;
                            string lipCode, baseCode, interfaceCode, centerCode, treadCode;
                            lipCode = dsTypeCode.Tables[0].Rows[0]["Lip"].ToString();
                            baseCode = dsTypeCode.Tables[0].Rows[0]["Base"].ToString();
                            interfaceCode = dsTypeCode.Tables[0].Rows[0]["Interface"].ToString();
                            centerCode = dsTypeCode.Tables[0].Rows[0]["Center"].ToString();
                            treadCode = dsTypeCode.Tables[0].Rows[0]["Tread"].ToString();

                            bool boolBlendMasterExists = true;
                            DataTable dtBlendCategory = (DataTable)dba.ExecuteReader_SP("sp_sel_BlendMaster_CategoryList", DBAccess.Return_Type.DataTable);
                            if (lipCode != "")
                            {
                                boolBlendMasterExists = false;
                                foreach (DataRow lRow in dtBlendCategory.Select("Category='" + lipCode + "'"))
                                {
                                    boolBlendMasterExists = true;
                                }
                                if (!boolBlendMasterExists)
                                {
                                    MessageBox.Show(lipCode + " is not available in blend Master for this tyre type, Please contact Admin.");
                                    btnWeightStart.Enabled = false;
                                    return;
                                }
                            }

                            if (baseCode != "")
                            {
                                boolBlendMasterExists = false;
                                foreach (DataRow lRow in dtBlendCategory.Select("Category='" + baseCode + "'"))
                                {
                                    boolBlendMasterExists = true;
                                }
                                if (!boolBlendMasterExists)
                                {
                                    MessageBox.Show(baseCode + " is not available in blend Master, Please contact Admin.");
                                    btnWeightStart.Enabled = false;
                                    return;
                                }
                            }

                            if (interfaceCode != "")
                            {
                                boolBlendMasterExists = false;
                                foreach (DataRow lRow in dtBlendCategory.Select("Category='" + interfaceCode + "'"))
                                {
                                    boolBlendMasterExists = true;
                                }
                                if (!boolBlendMasterExists)
                                {
                                    MessageBox.Show(interfaceCode + " is not available in blend Master, Please contact Admin.");
                                    btnWeightStart.Enabled = false;
                                    return;
                                }
                            }

                            if (strConcBase != "")
                            {
                                boolBlendMasterExists = false;
                                foreach (DataRow lRow in dtBlendCategory.Select("Category='" + strConcBase + "'"))
                                {
                                    boolBlendMasterExists = true;
                                }
                                if (!boolBlendMasterExists)
                                {
                                    MessageBox.Show(strConcBase + " is not available in blend Master, Please contact Admin.");
                                    btnWeightStart.Enabled = false;
                                    return;
                                }
                            }
                            if (btnWeightStart.Enabled)
                                btnWeightStart.Focus();
                        }

                        DataTable dtTypeMaster = dsTypeCode.Tables[0];
                        if (dtWtMaster.Rows[0]["LipWt"].ToString() != "0" && dtTypeMaster.Rows[0]["Lip"].ToString() != "")
                        {
                            lblNoOfBw.Text = lblNoOfBw.Text != "" ? "No Of BW: " + dtWtMaster.Rows[0]["NoofBw"].ToString() : "";
                            lblAddWtCenter.Visible = false;
                            lnkAddCenter.Visible = false;
                            pnlHbw.Visible = true;
                            lblCcodeHbw.Text = dtTypeMaster.Rows[0]["Lip"].ToString();
                            lblWtHbw.Text = Convert.ToDecimal(dtWtMaster.Rows[0]["LipWt"].ToString()).ToString("0.00");
                            if (Convert.ToDecimal(lblWtHbw.Text) > 40)
                            {
                                lblAddWtHbw.Visible = true;
                                lnkAddHbw.Visible = true;
                            }
                            pnlHbw.Location = new Point(panel4.Left, panel4.Bottom);
                        }
                        if (dtWtMaster.Rows[0]["BaseWt"].ToString() != "0" && dtTypeMaster.Rows[0]["Base"].ToString() != "")
                        {
                            lblAddWtCenter.Visible = false;
                            lnkAddCenter.Visible = false;
                            pnlBase.Visible = true;
                            lblCcodeBase.Text = dtTypeMaster.Rows[0]["Base"].ToString();

                            if (strConcBase != "" && (Convert.ToDecimal(dtWtMaster.Rows[0]["BaseWt"].ToString()) +
                                (Convert.ToDecimal(dtWtMaster.Rows[0]["InterfaceWt"].ToString()) / 2)) <= Convert.ToDecimal(lblBaseConcBal.Text))
                            {
                                lblCcodeBase.Text = strConcBase;
                                lnkConcBase.Text = dtTypeMaster.Rows[0]["Base"].ToString();
                                lnkConcBase.Visible = true;
                            }

                            lblWtBase.Text = Convert.ToDecimal(dtWtMaster.Rows[0]["BaseWt"].ToString()).ToString("0.00");
                            lblBaseTolerance.Text = dtWtMaster.Rows[0]["BaseWt"].ToString();
                            if (Convert.ToDecimal(lblWtBase.Text) > 40)
                            {
                                lblAddWtBase.Visible = true;
                                lnkAddBase.Visible = true;
                            }
                            if (pnlHbw.Visible)
                                pnlBase.Location = new Point(pnlHbw.Left, pnlHbw.Bottom);
                            else
                                pnlBase.Location = new Point(panel4.Left, panel4.Bottom);
                        }
                        if (dtWtMaster.Rows[0]["InterfaceWt"].ToString() != "0" && dtTypeMaster.Rows[0]["Interface"].ToString() != "")
                        {
                            lblAddWtCenter.Visible = false;
                            lnkAddCenter.Visible = false;
                            pnlInterface.Visible = true;

                            if (strConcCenter != "" && (Convert.ToDecimal(dtWtMaster.Rows[0]["CenterWt"].ToString()) +
                                (Convert.ToDecimal(dtWtMaster.Rows[0]["InterfaceWt"].ToString()) / 2)) <= Convert.ToDecimal(lblCenterConcBal.Text))
                            {
                                strCompination = strConcCenter;
                            }
                            else if (dtTypeMaster.Rows[0]["Center"].ToString() != "")
                            {
                                strCompination += dtTypeMaster.Rows[0]["Center"].ToString();
                            }
                            else if (((strCompination == "" && strConcTread != "") && (Convert.ToDecimal(dtWtMaster.Rows[0]["TreadWt"].ToString()) +
                                 (Convert.ToDecimal(dtWtMaster.Rows[0]["InterfaceWt"].ToString()) / 2)) <= Convert.ToDecimal(lblTreadConcBal.Text)))
                            {
                                strCompination = strConcTread;
                            }
                            else if (strCompination == "")
                            {
                                strCompination = dtTypeMaster.Rows[0]["Tread"].ToString();
                            }
                            strCompination = lblCcodeBase.Text + "+" + strCompination;

                            lblCcodeInter.Text = strCompination != "-" ? strCompination : dtTypeMaster.Rows[0]["Interface"].ToString();
                            lblWtInter.Text = Convert.ToDecimal(dtWtMaster.Rows[0]["InterfaceWt"].ToString()).ToString("0.00");
                            if (Convert.ToDecimal(lblWtInter.Text) > 40)
                            {
                                lblAddWtInter.Visible = true;
                                lnkAddInter.Visible = true;
                            }
                            if (pnlBase.Visible)
                                pnlInterface.Location = new Point(pnlBase.Left, pnlBase.Bottom);
                            else if (pnlHbw.Visible)
                                pnlInterface.Location = new Point(pnlHbw.Left, pnlHbw.Bottom);
                            else
                                pnlInterface.Location = new Point(panel4.Left, panel4.Bottom);
                        }
                        if (dtWtMaster.Rows[0]["CenterWt"].ToString() != "0" && dtTypeMaster.Rows[0]["Center"].ToString() != "")
                        {
                            lblAddWtCenter.Visible = false;
                            lnkAddCenter.Visible = false;
                            pnlCenter.Visible = true;
                            lblCcodeCenter.Text = dtTypeMaster.Rows[0]["Center"].ToString();

                            if (((lblCcodeBase.Text != "-" && strConcCenter != "") && (Convert.ToDecimal(dtWtMaster.Rows[0]["CenterWt"].ToString()) +
                                (Convert.ToDecimal(dtWtMaster.Rows[0]["InterfaceWt"].ToString()) / 2)) <= Convert.ToDecimal(lblCenterConcBal.Text)) ||
                                (strConcCenter != "" && Convert.ToDecimal(dtWtMaster.Rows[0]["CenterWt"].ToString()) <= Convert.ToDecimal(lblCenterConcBal.Text)))
                            {
                                lblCcodeCenter.Text = strConcCenter;
                                lnkConcCenter.Text = dtTypeMaster.Rows[0]["Center"].ToString();
                                lnkConcCenter.Visible = true;
                            }

                            lblWtCenter.Text = Convert.ToDecimal(dtWtMaster.Rows[0]["CenterWt"].ToString()).ToString("0.00");
                            if (Convert.ToDecimal(lblWtCenter.Text) > 40)
                            {
                                lblAddWtCenter.Visible = true;
                                lnkAddCenter.Visible = true;
                            }
                            if (pnlInterface.Visible)
                                pnlCenter.Location = new Point(pnlInterface.Left, pnlInterface.Bottom);
                            else if (pnlBase.Visible)
                                pnlCenter.Location = new Point(pnlBase.Left, pnlBase.Bottom);
                            else if (pnlHbw.Visible)
                                pnlCenter.Location = new Point(pnlHbw.Left, pnlHbw.Bottom);
                            else
                                pnlCenter.Location = new Point(panel4.Left, panel4.Bottom);
                        }
                        if (dtWtMaster.Rows[0]["TreadWt"].ToString() != "0" && dtTypeMaster.Rows[0]["Tread"].ToString() != "")
                        {
                            lblAddWtTread.Visible = false;
                            lnkAddTread.Visible = false;
                            pnlTread.Visible = true;
                            lblCcodeTread.Text = dtTypeMaster.Rows[0]["Tread"].ToString();

                            if (((lblCcodeBase.Text != "-" && lblCcodeCenter.Text == "-" && strConcTread != "") &&
                                (Convert.ToDecimal(dtWtMaster.Rows[0]["TreadWt"].ToString()) +
                                (Convert.ToDecimal(dtWtMaster.Rows[0]["InterfaceWt"].ToString()) / 2)) <= Convert.ToDecimal(lblTreadConcBal.Text)) ||
                                (strConcTread != "" && Convert.ToDecimal(dtWtMaster.Rows[0]["TreadWt"].ToString()) <= Convert.ToDecimal(lblTreadConcBal.Text)))
                            {
                                lblCcodeTread.Text = strConcTread;
                                lnkConcTread.Text = dtTypeMaster.Rows[0]["Tread"].ToString();
                                lnkConcTread.Visible = true;
                            }

                            lblWtTread.Text = Convert.ToDecimal(dtWtMaster.Rows[0]["TreadWt"].ToString()).ToString("0.00");
                            lblTreadTolerance.Text = dtWtMaster.Rows[0]["TreadWt"].ToString();
                            if (Convert.ToDecimal(lblWtTread.Text) > 40)
                            {
                                lblAddWtTread.Visible = true;
                                lnkAddTread.Visible = true;
                            }
                            if (pnlCenter.Visible)
                                pnlTread.Location = new Point(pnlCenter.Left, pnlCenter.Bottom);
                            else if (pnlInterface.Visible)
                                pnlTread.Location = new Point(pnlInterface.Left, pnlInterface.Bottom);
                            else if (pnlBase.Visible)
                                pnlTread.Location = new Point(pnlBase.Left, pnlBase.Bottom);
                            else if (pnlHbw.Visible)
                                pnlTread.Location = new Point(pnlHbw.Left, pnlHbw.Bottom);
                            else
                                pnlTread.Location = new Point(panel4.Left, panel4.Bottom);
                        }
                        if (dtWtMaster.Rows[0]["LipWt"].ToString() != "0" && dtTypeMaster.Rows[0]["Lip"].ToString() != "")
                            lblHbwType.Text = "With H+BW";
                        else
                            lblHbwType.Text = "Without H+BW";
                    }
                }
            }
            catch (Exception ex)
            {
                Program.WriteToGtErrorLog("GT", "frmGtWeigh", System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        private void timerHbw_Tick(object sender, EventArgs e)
        {
            try
            {
                lblErrMsg.Text = "";
                decimal strMinusWt = 0;
                decimal strPlusWt = 0;
                if (dtWT.Rows.Count > 0)
                {
                    foreach (DataRow row in dtWT.Select("Layer='H+BW' and HBWTYPE='" + lblHbwType.Text + "'"))
                    {
                        strMinusWt = Convert.ToDecimal(row["MinusWt"].ToString());
                        strPlusWt = Convert.ToDecimal(row["PlusWt"].ToString());
                    }
                }
                if ((Convert.ToDecimal(lblWtHbw.Text) - (Convert.ToDecimal(lblWtHbw.Text) * Convert.ToDecimal(strMinusWt) / 100) <
                    Convert.ToDecimal(lblMwtHbw.Text)) && (Convert.ToDecimal(lblWtHbw.Text) + (Convert.ToDecimal(lblWtHbw.Text) *
                    Convert.ToDecimal(strPlusWt) / 100) > Convert.ToDecimal(lblMwtHbw.Text)) || (Convert.ToDecimal(lblMwtHbw.Text) ==
                    Convert.ToDecimal(lblWtHbw.Text)))
                {
                    lblStatusHbw.Text = "OK";
                    lblStatusHbw.ForeColor = Color.DarkGreen;
                    btnHbw.Visible = true;
                    if (!frmWeighPlan.Weigh_Manual)
                        btnHbw.Focus();
                }
                else
                {
                    lblStatusHbw.Text = "NOT OK";
                    lblStatusHbw.ForeColor = Color.Red;
                    btnHbw.Visible = false;
                }
                Bind_Lable_MachineWt(lblMwtHbw, lblAddWtHbw);
            }
            catch (Exception)
            {
                //Program.WriteToGtErrorLog("GT", "frmGtWeigh", System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        private void timerBase_Tick(object sender, EventArgs e)
        {
            try
            {
                lblErrMsg.Text = "";
                decimal strMinusWt = 0;
                decimal strPlusWt = 0;
                if (dtWT.Rows.Count > 0)
                {
                    foreach (DataRow row in dtWT.Select("Layer='BASE' and HBWTYPE='" + lblHbwType.Text + "'"))
                    {
                        strMinusWt = Convert.ToDecimal(row["MinusWt"].ToString());
                        strPlusWt = Convert.ToDecimal(row["PlusWt"].ToString());
                    }
                }
                if ((Convert.ToDecimal(lblWtBase.Text) - Convert.ToDecimal(strMinusWt) < Convert.ToDecimal(lblMwtBase.Text)) &&
                    (Convert.ToDecimal(lblWtBase.Text) + (Convert.ToDecimal(strPlusWt)) > Convert.ToDecimal(lblMwtBase.Text)) ||
                    (Convert.ToDecimal(lblMwtBase.Text) == Convert.ToDecimal(lblWtBase.Text)))
                {
                    lblStatusBase.Text = "OK";
                    lblStatusBase.ForeColor = Color.DarkGreen;
                    btnBase.Visible = true;
                    if (!frmWeighPlan.Weigh_Manual)
                        btnBase.Focus();
                }
                else
                {
                    lblStatusBase.Text = "NOT OK";
                    lblStatusBase.ForeColor = Color.Red;
                    btnBase.Visible = false;
                }
                Bind_Lable_MachineWt(lblMwtBase, lblAddWtBase);
            }
            catch (Exception)
            {
                //Program.WriteToGtErrorLog("GT", "frmGtWeigh", System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        private void timerInter_Tick(object sender, EventArgs e)
        {
            try
            {
                lblErrMsg.Text = "";
                decimal strMinusWt = 0;
                decimal strPlusWt = 0;
                if (dtWT.Rows.Count > 0)
                {
                    foreach (DataRow row in dtWT.Select("Layer='INTERFACE' and HBWTYPE='" + lblHbwType.Text + "'"))
                    {
                        strMinusWt = Convert.ToDecimal(row["MinusWt"].ToString());
                        strPlusWt = Convert.ToDecimal(row["PlusWt"].ToString());
                    }
                }
                if ((Convert.ToDecimal(lblWtInter.Text) - Convert.ToDecimal(strMinusWt) < Convert.ToDecimal(lblMwtInter.Text)) && (Convert.ToDecimal(lblWtInter.Text) + (Convert.ToDecimal(strPlusWt)) > Convert.ToDecimal(lblMwtInter.Text)) || (Convert.ToDecimal(lblMwtInter.Text) == Convert.ToDecimal(lblWtInter.Text)))
                {
                    lblStatusInter.Text = "OK";
                    lblStatusInter.ForeColor = Color.DarkGreen;
                    btnInter.Visible = true;
                    if (!frmWeighPlan.Weigh_Manual)
                        btnInter.Focus();
                }
                else
                {
                    lblStatusInter.Text = "NOT OK";
                    lblStatusInter.ForeColor = Color.Red;
                    btnInter.Visible = false;
                }
                Bind_Lable_MachineWt(lblMwtInter, lblAddWtInter);
            }
            catch (Exception)
            {
                //Program.WriteToGtErrorLog("GT", "frmGtWeigh", System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        private void timerCenter_Tick(object sender, EventArgs e)
        {
            try
            {
                lblErrMsg.Text = "";
                decimal strMinusWt = 0;
                decimal strPlusWt = 0;
                if (dtWT.Rows.Count > 0)
                {
                    foreach (DataRow row in dtWT.Select("Layer='CENTER' and HBWTYPE='" + lblHbwType.Text + "'"))
                    {
                        strMinusWt = Convert.ToDecimal(row["MinusWt"].ToString());
                        strPlusWt = Convert.ToDecimal(row["PlusWt"].ToString());
                    }
                }
                if ((Convert.ToDecimal(lblWtCenter.Text) - Convert.ToDecimal(strMinusWt) < Convert.ToDecimal(lblMwtCenter.Text)) && (Convert.ToDecimal(lblWtCenter.Text) + (Convert.ToDecimal(strPlusWt)) > Convert.ToDecimal(lblMwtCenter.Text)) || (Convert.ToDecimal(lblMwtCenter.Text) == Convert.ToDecimal(lblWtCenter.Text)))
                {
                    lblStatusCenter.Text = "OK";
                    lblStatusCenter.ForeColor = Color.DarkGreen;
                    btnCenter.Visible = true;
                    if (!frmWeighPlan.Weigh_Manual)
                        btnCenter.Focus();
                }
                else
                {
                    lblStatusCenter.Text = "NOT OK";
                    lblStatusCenter.ForeColor = Color.Red;
                    btnCenter.Visible = false;
                }
                Bind_Lable_MachineWt(lblMwtCenter, lblAddWtCenter);
            }
            catch (Exception)
            {
                //Program.WriteToGtErrorLog("GT", "frmGtWeigh", System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        private void timerTread_Tick(object sender, EventArgs e)
        {
            try
            {
                lblErrMsg.Text = "";
                decimal strMinusWt = 0;
                decimal strPlusWt = 0;
                if (dtWT.Rows.Count > 0)
                {
                    string strAboveBelow = Convert.ToDecimal(lblWtTread.Text) < 58 ? "Below 58 Kg" : "Above 58 Kg";
                    foreach (DataRow row in dtWT.Select("Layer='TREAD' and HBWTYPE='" + lblHbwType.Text + "' and SpecType='" + strAboveBelow + "'"))
                    {
                        strMinusWt = Convert.ToDecimal(row["MinusWt"].ToString());
                        strPlusWt = Convert.ToDecimal(row["PlusWt"].ToString());
                    }
                }
                if ((Convert.ToDecimal(lblWtTread.Text) - Convert.ToDecimal(strMinusWt) < Convert.ToDecimal(lblMwtTread.Text)) && (Convert.ToDecimal(lblWtTread.Text) + (Convert.ToDecimal(strPlusWt)) > Convert.ToDecimal(lblMwtTread.Text)) || (Convert.ToDecimal(lblMwtTread.Text) == Convert.ToDecimal(lblWtTread.Text)))
                {
                    lblStatusTread.Text = "OK";
                    lblStatusTread.ForeColor = Color.DarkGreen;
                    btnTread.Visible = true;
                    if (!frmWeighPlan.Weigh_Manual)
                        btnTread.Focus();
                }
                else
                {
                    lblStatusTread.Text = "NOT OK";
                    lblStatusTread.ForeColor = Color.Red;
                    btnTread.Visible = false;
                }
                Bind_Lable_MachineWt(lblMwtTread, lblAddWtTread);
            }
            catch (Exception)
            {
                //Program.WriteToGtErrorLog("GT", "frmGtWeigh", System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        private void Bind_Lable_MachineWt(Label lblM, Label lblAddWt)
        {
            lblErrMsg.Text = "";
            decimal strVal = 0;
            if (frmWeighPlan.Weigh_Manual)
                strVal = txtManualValue.Text != "" ? Convert.ToDecimal(txtManualValue.Text) : 0;
            else
            {
                if (strCommunication != "COM4")
                {
                    TcpClient tcpClient = new TcpClient();
                    try
                    {
                        IPAddress iPAddress = System.Net.IPAddress.Parse("192.168.5." + Convert.ToInt32(strCommunication));
                        int tcpPort = 502;
                        tcpClient = new TcpClient(iPAddress.ToString(), tcpPort);
                        tcpClient.ReceiveTimeout = 1000;

                        if (!tcpClient.Connected)
                            lblErrMsg.Text = "ETHERNET NOT CONNECTED";
                        else
                        {
                            try
                            {
                                IModbusSerialMaster master1 = ModbusSerialMaster.CreateRtu(tcpClient);

                                byte slaveID = 1;
                                ushort startAddress = 1000;
                                ushort numRegisters = 2;
                                ushort[] registers1 = master1.ReadHoldingRegisters(slaveID, startAddress, numRegisters);
                                uint value1 = (ModbusUtility.GetUInt32(registers1[0], registers1[1]));
                                int readPow = unchecked((Int32)value1);

                                int intDivide = 10;
                                if (readPow > 0)
                                    intDivide = Convert.ToInt32(Math.Pow(10, readPow));
                                IModbusSerialMaster master = ModbusSerialMaster.CreateRtu(tcpClient);

                                startAddress = 1002;
                                ushort[] registers = master.ReadHoldingRegisters(slaveID, startAddress, numRegisters);
                                uint value = (ModbusUtility.GetUInt32(registers[0], registers[1]));
                                int readVal = unchecked((Int32)value);

                                strVal = Convert.ToDecimal(readVal) / Convert.ToInt32(intDivide);
                                master1.Dispose();
                                master.Dispose();
                            }
                            catch (TimeoutException exp)
                            {
                                lblErrMsg.Text = exp.Message.ToUpper();
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        lblErrMsg.Text = ex.Message.ToUpper();
                    }
                    tcpClient.Close();
                }
                else
                {
                   // SerialPort port = new SerialPort("COM4");
                    try
                    {
                       // port.BaudRate = 9600;
                        //port.DataBits = 8;
                       // port.Parity = Parity.None;
                        //port.StopBits = StopBits.One;
                        //port.ReadTimeout = 300;
                        //port.WriteTimeout = 300;
                        //port.Open();

                        //if (!port.IsOpen)
                           // lblErrMsg.Text = "PORT NOT OPENED";
                        //else
                        {
                            try
                            {
                                if ((Program.strPlantName == "mmn"))
                                {
                                    //IModbusSerialMaster master = ModbusSerialMaster.CreateRtu(port);

                                    //byte slaveId = 1;
                                    //ushort startAddress = 1002;
                                    //ushort numRegisters = 2;
                                    //// ushort[] registers = master.ReadHoldingRegisters(slaveId, startAddress, numRegisters);
                                    //uint value = (ModbusUtility.GetUInt32(registers[0], registers[1]));
                                    //int readVal = unchecked((Int32)value);

                                    //strVal = Convert.ToDecimal(value) / Convert.ToInt32(1000);
                                    //master.Dispose();
                                }
                            else
                                    
                                {
                                    //string strRtxt = port.ReadLine();
                                    //if (strRtxt.Length > 0)
                                    //{
                                    //    string numberOnly = Regex.Replace(strRtxt, "[^0-9.]", "");
                                    //    if (numberOnly.ToString() != "")
                                    //    {
                                    //        strVal = Convert.ToDecimal(numberOnly);
                                    //    }
                                    //}

                                    {

                                        //HttpWebRequest request = (HttpWebRequest)WebRequest.Create(@"http://192.168.8.103:4000/sc");
                                        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(@"http://localhost:4000/sc");
                                        HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                                        string content = new StreamReader(response.GetResponseStream()).ReadToEnd();
                                        string left_content = content.Substring(0, 26);
                                        int fetch_length = (((content.Length) - 3) - (left_content.Length));//left_content.Length;
                                        content = content.Substring(26, fetch_length);
                                        //string left_content= con
                                        strVal = Convert.ToDecimal(content);


                                        if (strVal.ToString() != "")
                                        {
                                            if (lblAddWt.Text == "0")
                                            {
                                                lblM.Text = (Convert.ToDecimal(strVal).ToString("0.00"));
                                                txtMachineWt.Text = (Convert.ToDecimal(strVal).ToString("0.00"));
                                            }
                                            else
                                            {
                                                lblM.Text = (Convert.ToDecimal(lblAddWt.Text) + (Convert.ToDecimal(strVal))).ToString("0.00");
                                                txtMachineWt.Text = (Convert.ToDecimal(strVal).ToString("0.00"));
                                            }
                                        }
                                        else
                                            txtMachineWt.Text = "0.000";

                                    }


                                }
                            }
                            catch (TimeoutException exp)
                            {
                                lblErrMsg.Text = exp.Message.ToUpper();
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        lblErrMsg.Text = ex.Message.ToUpper();
                    }
                    //port.Close();
                }
            }

            if (strVal.ToString() != "")
            {
                if (lblAddWt.Text == "0")
                {
                    lblM.Text = (Convert.ToDecimal(strVal).ToString("0.00"));
                    txtMachineWt.Text = (Convert.ToDecimal(strVal).ToString("0.00"));
                }
                else
                {
                    lblM.Text = (Convert.ToDecimal(lblAddWt.Text) + (Convert.ToDecimal(strVal))).ToString("0.00");
                    txtMachineWt.Text = (Convert.ToDecimal(strVal).ToString("0.00"));
                }
            }
            else
                txtMachineWt.Text = "0.00";
        }

        private void btnHbw_Click(object sender, EventArgs e)
        {
            try
            {
                lblCcodeHbw.Font = new Font("Verdana", 12, FontStyle.Bold);
                lblErrMsg.Text = "";
                lblStatusHbw.Text = "OK";
                lblStatusHbw.ForeColor = Color.DarkGreen;
                timerHbw.Stop();
                txtMachineWt.Text = "0.00";
                btnHbw.Visible = false;
                if (pnlBase.Visible)
                {
                    lblWtBase.Text = (Convert.ToDecimal(lblWtBase.Text) + ((Convert.ToDecimal(lblWtHbw.Text) - Convert.ToDecimal(lblMwtHbw.Text)) *
                        (Convert.ToDecimal(0.94)))).ToString("0.00");
                    timerBase.Start();
                    lblCcodeBase.Font = new Font("Verdana", 16, FontStyle.Bold);
                }
                else if (pnlInterface.Visible)
                {
                    timerInter.Start();
                    lblCcodeInter.Font = new Font("Verdana", 13, FontStyle.Bold);
                }
                else if (pnlCenter.Visible)
                {
                    timerCenter.Start();
                    lblCcodeCenter.Font = new Font("Verdana", 16, FontStyle.Bold);
                }
                else if (pnlTread.Visible)
                {
                    timerTread.Start();
                    lblCcodeTread.Font = new Font("Verdana", 16, FontStyle.Bold);
                }
                else
                    pnlSaveBtn.Visible = true;
                lblActWtTot.Text = Convert.ToDecimal(Convert.ToDecimal(lblMwtHbw.Text) + Convert.ToDecimal(lblMwtBase.Text) + Convert.ToDecimal(lblMwtInter.Text) + Convert.ToDecimal(lblMwtCenter.Text) + Convert.ToDecimal(lblMwtTread.Text)).ToString();
                txtManualValue.Text = "0";
                if (frmWeighPlan.Weigh_Manual)
                    txtManualValue.Focus();
            }
            catch (Exception ex)
            {
                Program.WriteToGtErrorLog("GT", "frmGtWeigh", System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        private void btnBase_Click(object sender, EventArgs e)
        {
            try
            {
                lblCcodeBase.Font = new Font("Verdana", 12, FontStyle.Bold);
                lblErrMsg.Text = "";
                lblStatusBase.Text = "OK";
                lblStatusBase.ForeColor = Color.DarkGreen;
                timerBase.Stop();
                txtMachineWt.Text = "0.00";
                btnBase.Visible = false;
                lnkConcBase.Visible = false;
                if (pnlInterface.Visible)
                {
                    timerInter.Start();
                    lblCcodeInter.Font = new Font("Verdana", 13, FontStyle.Bold);
                }
                else if (pnlCenter.Visible)
                {
                    timerCenter.Start();
                    lblCcodeCenter.Font = new Font("Verdana", 16, FontStyle.Bold);
                }
                else if (pnlTread.Visible)
                {
                    timerTread.Start();
                    lblCcodeTread.Font = new Font("Verdana", 16, FontStyle.Bold);
                }
                else
                    pnlSaveBtn.Visible = true;
                decimal tolerance = Convert.ToDecimal((Convert.ToDecimal(lblWtBase.Text) - Convert.ToDecimal(lblMwtBase.Text)));
                lblWtTread.Text = (Convert.ToDecimal(lblWtTread.Text) + (tolerance)).ToString("0.00");
                lblActWtTot.Text = Convert.ToDecimal(Convert.ToDecimal(lblMwtHbw.Text) + Convert.ToDecimal(lblMwtBase.Text) + Convert.ToDecimal(lblMwtInter.Text) + Convert.ToDecimal(lblMwtCenter.Text) + Convert.ToDecimal(lblMwtTread.Text)).ToString();
                txtManualValue.Text = "0";
                if (frmWeighPlan.Weigh_Manual)
                    txtManualValue.Focus();
            }
            catch (Exception ex)
            {
                Program.WriteToGtErrorLog("GT", "frmGtWeigh", System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        private void btnInter_Click(object sender, EventArgs e)
        {
            try
            {
                lblCcodeInter.Font = new Font("Verdana", 12, FontStyle.Bold);
                lblErrMsg.Text = "";
                lblStatusInter.Text = "OK";
                lblStatusInter.ForeColor = Color.DarkGreen;
                timerInter.Stop();
                txtMachineWt.Text = "0.00";
                btnInter.Visible = false;
                if (pnlCenter.Visible)
                {
                    timerCenter.Start();
                    lblCcodeCenter.Font = new Font("Verdana", 16, FontStyle.Bold);
                }
                else if (pnlTread.Visible)
                {
                    timerTread.Start();
                    lblCcodeTread.Font = new Font("Verdana", 16, FontStyle.Bold);
                }
                else
                    pnlSaveBtn.Visible = true;
                decimal tolerance = Convert.ToDecimal((Convert.ToDecimal(lblWtInter.Text) - Convert.ToDecimal(lblMwtInter.Text)));
                lblWtTread.Text = (Convert.ToDecimal(lblWtTread.Text) + (tolerance)).ToString("0.00");
                lblActWtTot.Text = Convert.ToDecimal(Convert.ToDecimal(lblMwtHbw.Text) + Convert.ToDecimal(lblMwtBase.Text) + Convert.ToDecimal(lblMwtInter.Text) + Convert.ToDecimal(lblMwtCenter.Text) + Convert.ToDecimal(lblMwtTread.Text)).ToString();
                txtManualValue.Text = "0";
                if (frmWeighPlan.Weigh_Manual)
                    txtManualValue.Focus();
            }
            catch (Exception ex)
            {
                Program.WriteToGtErrorLog("GT", "frmGtWeigh", System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        private void btnCenter_Click(object sender, EventArgs e)
        {
            try
            {
                lblCcodeCenter.Font = new Font("Verdana", 12, FontStyle.Bold);
                lblErrMsg.Text = "";
                lblStatusCenter.Text = "OK";
                lblStatusCenter.ForeColor = Color.DarkGreen;
                timerCenter.Stop();
                txtMachineWt.Text = "0.00";
                btnCenter.Visible = false;
                lnkConcCenter.Visible = false;
                if (pnlTread.Visible)
                {
                    timerTread.Start();
                    lblCcodeTread.Font = new Font("Verdana", 16, FontStyle.Bold);
                }
                else
                    pnlSaveBtn.Visible = true;
                decimal tolerance = Convert.ToDecimal((Convert.ToDecimal(lblWtCenter.Text) - Convert.ToDecimal(lblMwtCenter.Text)));
                lblWtTread.Text = (Convert.ToDecimal(lblWtTread.Text) + (tolerance)).ToString("0.00");
                lblActWtTot.Text = Convert.ToDecimal(Convert.ToDecimal(lblMwtHbw.Text) + Convert.ToDecimal(lblMwtBase.Text) + Convert.ToDecimal(lblMwtInter.Text) + Convert.ToDecimal(lblMwtCenter.Text) + Convert.ToDecimal(lblMwtTread.Text)).ToString();
                txtManualValue.Text = "0";
                if (frmWeighPlan.Weigh_Manual)
                    txtManualValue.Focus();
            }
            catch (Exception ex)
            {
                Program.WriteToGtErrorLog("GT", "frmGtWeigh", System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        private void btnTread_Click(object sender, EventArgs e)
        {
            try
            {
                lblCcodeTread.Font = new Font("Verdana", 12, FontStyle.Bold);
                lblErrMsg.Text = "";
                lblStatusTread.Text = "OK";
                lblStatusTread.ForeColor = Color.DarkGreen;
                timerTread.Stop();
                txtMachineWt.Text = "0.00";
                btnTread.Visible = false;
                lnkConcTread.Visible = false;
                pnlSaveBtn.Visible = true;
                lblActWtTot.Text = Convert.ToDecimal(Convert.ToDecimal(lblMwtHbw.Text) + Convert.ToDecimal(lblMwtBase.Text) + Convert.ToDecimal(lblMwtInter.Text) + Convert.ToDecimal(lblMwtCenter.Text) + Convert.ToDecimal(lblMwtTread.Text)).ToString();
                txtManualValue.Text = "0";
                txtStencil.Focus();
                txtStencil.SelectionStart = txtStencil.Text.Length;
            }
            catch (Exception ex)
            {
                Program.WriteToGtErrorLog("GT", "frmGtWeigh", System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                lblErrMsg.Text = "";
                if (cboShift.SelectedIndex <= 0)
                {
                    lblErrMsg.Text = "Select shift";
                    cboShift.Focus();
                }
                else if (cboWtClass.SelectedIndex <= 0)
                {
                    lblErrMsg.Text = "Select weight class";
                    cboWtClass.Focus();
                }
                else if (cboFriction.SelectedIndex <= 0)
                {
                    lblErrMsg.Text = "Select friction source";
                    cboFriction.Focus();
                }
                else if (cboType.SelectedIndex <= 0)
                {
                    lblErrMsg.Text = "Select type";
                    cboType.Focus();
                }
                else if (cboMould.SelectedIndex <= 0)
                {
                    lblErrMsg.Text = "Select mould id";
                    cboMould.Focus();
                }
                else if (txtStencil.Text == "")
                {
                    lblErrMsg.Text = "Enter stencil no.";
                    txtStencil.Focus();
                    txtStencil.SelectionStart = txtStencil.Text.Length;
                }
                else if (txtStencil.Text.Length < 10)
                {
                    lblErrMsg.Text = "Enter proper stencil no.";
                    txtStencil.Focus();
                    txtStencil.SelectionStart = txtStencil.TextLength;
                }
                else if (pnlCenter.Visible && txtBatchCenter.Text == "")
                {
                    MessageBox.Show("Enter Center Compound Batch No.");
                    txtBatchCenter.Focus();
                }
                else if (pnlTread.Visible && txtBatchTread.Text == "")
                {
                    MessageBox.Show("Enter Tread Compound Batch No.");
                    txtBatchTread.Focus();
                }
                else
                {
                    string strQuery = string.Empty;
                    SqlParameter[] sp = new SqlParameter[] { new SqlParameter("@StencilNo", txtStencil.Text) };
                    DataTable dt = (DataTable)dba.ExecuteReader_SP("SP_SEL_WeightTrack_ProductionDetails", sp, DBAccess.Return_Type.DataTable);
                    if (dt.Rows.Count == 1)
                        lblErrMsg.Text = "Stencil no. already existing on " + dt.Rows[0]["ProductionDate"].ToString() + " " +
                            dt.Rows[0]["Shift"].ToString() + " by " + dt.Rows[0]["Operator"].ToString();
                    else
                    {
                        string strInterface = "";
                        if (lblCcodeBase.Text != "-" && (lblCcodeCenter.Text != "-" || lblCcodeTread.Text != "-"))
                        {
                            strInterface = (!lnkConcBase.Visible && lnkConcBase.Text != "" ? lnkConcBase.Text : lblCcodeBase.Text) + "+";
                            if (strInterface != "" && (lnkConcCenter.Text != "" || lblCcodeCenter.Text != "-"))
                                strInterface += (!lnkConcCenter.Visible && lnkConcCenter.Text != "" ? lnkConcCenter.Text : lblCcodeCenter.Text);
                            else if (strInterface != "" && (lnkConcTread.Text != "" || lblCcodeTread.Text != "-"))
                                strInterface += (!lnkConcTread.Visible && lnkConcTread.Text != "" ? lnkConcTread.Text : lblCcodeTread.Text);
                        }

                        SqlParameter[] spBlend = new SqlParameter[] {
                            new SqlParameter("@lipCategory", lblCcodeHbw.Text),
                            new SqlParameter("@baseCategory", lblCcodeBase.Text),
                            new SqlParameter("@interfaceCategory", strInterface)
                        };
                        DataTable dtBlendMaster = (DataTable)dba.ExecuteReader_SP("sp_sel_blendmaster_percentage", spBlend, DBAccess.Return_Type.DataTable);

                        double v1 = 0, v2 = 0, v3 = 0, v4 = 0, v5 = 0, v6 = 0;
                        if (dtBlendMaster.Rows.Count > 0)
                        {
                            foreach (DataRow drBlend in dtBlendMaster.Select("category = '" + lblCcodeHbw.Text + "'"))
                            {
                                v1 = Convert.ToDouble(lblMwtHbw.Text) * Convert.ToDouble(drBlend["FrictionPer"]) / 100;
                                v2 = Convert.ToDouble(lblMwtHbw.Text) * Convert.ToDouble(drBlend["FsPer"]) / 100;
                                v3 = Convert.ToDouble(lblMwtHbw.Text) * Convert.ToDouble(drBlend["BasePer"]) / 100;
                                v4 = Convert.ToDouble(lblMwtHbw.Text) * Convert.ToDouble(drBlend["LipPer"]) / 100;
                                v5 = Convert.ToDouble(lblMwtHbw.Text) * Convert.ToDouble(drBlend["CenterPer"]) / 100;
                                v6 = Convert.ToDouble(lblMwtHbw.Text) * Convert.ToDouble(drBlend["TreadPer"]) / 100;
                            }
                            foreach (DataRow drBlend in dtBlendMaster.Select("category = '" + lblCcodeBase.Text + "'"))
                            {
                                v1 += Convert.ToDouble(lblMwtBase.Text) * Convert.ToDouble(drBlend["FrictionPer"]) / 100;
                                v2 += Convert.ToDouble(lblMwtBase.Text) * Convert.ToDouble(drBlend["FsPer"]) / 100;
                                v3 += Convert.ToDouble(lblMwtBase.Text) * Convert.ToDouble(drBlend["BasePer"]) / 100;
                                v4 += Convert.ToDouble(lblMwtBase.Text) * Convert.ToDouble(drBlend["LipPer"]) / 100;
                                v5 += Convert.ToDouble(lblMwtBase.Text) * Convert.ToDouble(drBlend["CenterPer"]) / 100;
                                v6 += Convert.ToDouble(lblMwtBase.Text) * Convert.ToDouble(drBlend["TreadPer"]) / 100;
                            }
                            foreach (DataRow drBlend in dtBlendMaster.Select("category = '" + strInterface + "'"))
                            {
                                v1 += Convert.ToDouble(lblMwtInter.Text) * Convert.ToDouble(drBlend["FrictionPer"]) / 100;
                                v2 += Convert.ToDouble(lblMwtInter.Text) * Convert.ToDouble(drBlend["FsPer"]) / 100;
                                v3 += Convert.ToDouble(lblMwtInter.Text) * Convert.ToDouble(drBlend["BasePer"]) / 100;
                                v4 += Convert.ToDouble(lblMwtInter.Text) * Convert.ToDouble(drBlend["LipPer"]) / 100;
                                v5 += Convert.ToDouble(lblMwtInter.Text) * Convert.ToDouble(drBlend["CenterPer"]) / 100;
                                v6 += Convert.ToDouble(lblMwtInter.Text) * Convert.ToDouble(drBlend["TreadPer"]) / 100;
                            }
                        }

                        SqlParameter spProductionDate;
                        SqlParameter spProductionTime;
                        try
                        {
                            spProductionDate = new SqlParameter("@ProductionDate", DateTime.Parse(dtpPrepareDate.Value.ToString()));
                            if (!frmWeighPlan.Weigh_Manual)
                                spProductionTime = new SqlParameter("@ProductionTime", DateTime.Parse(DateTime.Now.ToString("HH:mm")));
                            else
                                spProductionTime = new SqlParameter("@ProductionTime", DateTime.Parse(dtpPrepareDate.Value.ToShortTimeString()));
                        }
                        catch (Exception ex)
                        {
                            spProductionDate = new SqlParameter("@ProductionDate", DateTime.ParseExact(dtpPrepareDate.Value.ToString(), "dd/MM/yyyy", null));
                            if (!frmWeighPlan.Weigh_Manual)
                                spProductionTime = new SqlParameter("@ProductionTime", DateTime.ParseExact(DateTime.Now.ToString(), "HH:mm", null));
                            else
                                spProductionTime = new SqlParameter("@ProductionTime", DateTime.ParseExact(dtpPrepareDate.Value.ToShortTimeString(), "HH:mm", null));
                            Program.WriteToGtErrorLog("GT", "frmGtWeigh", MethodBase.GetCurrentMethod().Name, 1, ex.Message);
                        }

                        SqlParameter[] sp1 = new SqlParameter[] {
                                spProductionDate, spProductionTime,
                                new SqlParameter("@Shift", cboShift.Text), new SqlParameter("@Operator", Program.strUserName),
                                new SqlParameter("@WtClass", cboWtClass.SelectedValue.ToString()), new SqlParameter("@TyreType", cboType.SelectedValue.ToString()),
                                new SqlParameter("@TyreSize", lblSize.Text), new SqlParameter("@RimSize", lblRim.Text),
                                new SqlParameter("@MouldCode", cboMould.Text), new SqlParameter("@StencilNo", txtStencil.Text.ToUpper()),
                                new SqlParameter("@LipCode", lblCcodeHbw.Text), new SqlParameter("@LipRqWt", lblWtHbw.Text),
                                new SqlParameter("@LipScale", lblMwtHbw.Text), new SqlParameter("@BaseCode", lblCcodeBase.Text),
                                new SqlParameter("@BaseRqWt", lblWtBase.Text), new SqlParameter("@BaseScale", lblMwtBase.Text),
                                new SqlParameter("@InterfaceCode", lblCcodeInter.Text), new SqlParameter("@InterfaceRqWt", lblWtInter.Text),
                                new SqlParameter("@InterfaceScale", lblMwtInter.Text), new SqlParameter("@CenterCode", lblCcodeCenter.Text),
                                new SqlParameter("@CenterRqWt", lblWtCenter.Text), new SqlParameter("@CenterScale", lblMwtCenter.Text),
                                new SqlParameter("@TreadCode", lblCcodeTread.Text), new SqlParameter("@TreadRqWt", lblWtTread.Text),
                                new SqlParameter("@TreadScale", lblMwtTread.Text), new SqlParameter("@UserName", (Program.strUserName + (frmWeighPlan.Weigh_Manual ? " -MANUAL" : ""))),
                                new SqlParameter("@LipBatch", txtBatchHbw.Text), new SqlParameter("@BaseBatch", cboFriction.SelectedValue.ToString()),
                                new SqlParameter("@InterfaceBatch", "Null"), new SqlParameter("@CenterBatch", txtBatchCenter.Text),
                                new SqlParameter("@BaseBatchno", txtBatchBase.Text),
                                new SqlParameter("@TreadBatch", txtBatchTread.Text), new SqlParameter("@LipRemarks", lblHbwRemarks.Text),
                                new SqlParameter("@BaseRemarks", lblBaseRemarks.Text), new SqlParameter("@InterfaceRemarks", lblInterRemarks.Text),
                                new SqlParameter("@CenterRemarks", lblCenterRemarks.Text), new SqlParameter("@TreadRemarks", lblTreadRemarks.Text),
                                new SqlParameter("@BaseTolerance", lblBaseTolerance.Text), new SqlParameter("@TreadTolerance", lblTreadTolerance.Text),
                                new SqlParameter("@GTToNext", "1"), new SqlParameter("@BlendFriction", v1), new SqlParameter("@BlendFs", v2),
                                new SqlParameter("@BlendLip", v4), new SqlParameter("@BlendBase", v3), new SqlParameter("@BlendCenter", v5),
                                new SqlParameter("@BlendTread", v6), new SqlParameter("@Prod_ItemID", frmWeighPlan.Prod_ItemID),
                                new SqlParameter("@ProductionID", frmWeighPlan.ProductionID)
                            };
                        int resp = dba.ExecuteNonQuery_SP("SP_INS_WeightTrack_ProductionDetails_V3", sp1);
                        if (resp > 0)
                        {
                            decimal decConcUsage = 0;
                            if (lnkConcBase.Text != "" && lnkConcBase.Text != lblCcodeBase.Text && !lnkConcBase.Visible)
                            {
                                decConcUsage = Convert.ToDecimal(lblMwtBase.Text) +
                                    (lblCcodeInter.Text.Contains(lblCcodeBase.Text) ? Convert.ToDecimal(lblMwtInter.Text) / 2 : 0);
                                SqlParameter[] spB = new SqlParameter[] {
                                        new SqlParameter("@category", "BASE"),
                                        new SqlParameter("@masterCompcode", lnkConcBase.Text),
                                        new SqlParameter("@concCode", lblCcodeBase.Text),
                                        new SqlParameter("@Usage", decConcUsage ),
                                        new SqlParameter("@ModifiedBy", Program.strUserName)
                                    };
                                dba.ExecuteNonQuery_SP("sp_upd_cons_CompoundLimitChart_Usage", spB);
                            }
                            if (lnkConcCenter.Text != "" && lnkConcCenter.Text != lblCcodeCenter.Text && !lnkConcCenter.Visible)
                            {
                                decConcUsage = Convert.ToDecimal(lblMwtCenter.Text) +
                                       (lblCcodeInter.Text.Contains(lblCcodeCenter.Text) ? Convert.ToDecimal(lblMwtInter.Text) / 2 : 0);
                                SqlParameter[] spB = new SqlParameter[] {
                                        new SqlParameter("@category", "CENTER"),
                                        new SqlParameter("@masterCompcode", lnkConcCenter.Text),
                                        new SqlParameter("@concCode", lblCcodeCenter.Text),
                                        new SqlParameter("@Usage", decConcUsage),
                                        new SqlParameter("@ModifiedBy", Program.strUserName)
                                    };
                                dba.ExecuteNonQuery_SP("sp_upd_cons_CompoundLimitChart_Usage", spB);
                            }
                            if (lnkConcTread.Text != "" && lnkConcTread.Text != lblCcodeTread.Text && !lnkConcTread.Visible)
                            {
                                decConcUsage = Convert.ToDecimal(lblMwtTread.Text) +
                                       (lblCcodeInter.Text.Contains(lblCcodeTread.Text) ? Convert.ToDecimal(lblMwtInter.Text) / 2 : 0);
                                SqlParameter[] spB = new SqlParameter[] {
                                        new SqlParameter("@category", "TREAD"),
                                        new SqlParameter("@masterCompcode", lnkConcTread.Text),
                                        new SqlParameter("@concCode", lblCcodeTread.Text),
                                        new SqlParameter("@Usage", decConcUsage),
                                        new SqlParameter("@ModifiedBy", Program.strUserName)
                                    };
                                dba.ExecuteNonQuery_SP("sp_upd_cons_CompoundLimitChart_Usage", spB);
                            }

                            MessageBox.Show("Records successfully " + btnSave.Text.ToLower() + "d");
                            lblQty.Text = (Convert.ToInt32(lblQty.Text) - 1).ToString();
                            intRemainQty = Convert.ToInt32(lblQty.Text);
                            if (intRemainQty <= 3)
                            {
                                if (intRemainQty == 0)
                                    MessageBox.Show("MOULD CODE " + cboMould.Text + " PLANNED QTY COMPLETED.\nINFORM TO PLANNING TEAM.");

                                SqlParameter[] spSel = new SqlParameter[] { new SqlParameter("@Prod_Press", frmWeighPlan.Prod_Press) };
                                DataTable dtSeq = (DataTable)dba.ExecuteReader_SP("sp_sel_mould_prod_sequence_v1", spSel, DBAccess.Return_Type.DataTable);
                                if (dtSeq.Rows.Count > 0)
                                {
                                    Form frm = new frmGtNextSequence();
                                    frm.Location = new Point(0, 0);
                                    frm.MdiParent = this.MdiParent;
                                    frm.WindowState = FormWindowState.Maximized;
                                    frm.Show();
                                    this.Close();
                                }
                                else
                                    MessageBox.Show("NEXT PRODUCTION SEQUENCE NOT AVAILABLE.\nINFORM TO PLANNING TEAM.");
                            }
                            Bind_PlannedPendingListGv();
                            Ctrl_Clear();
                            Make_DtpDate();
                            cboType_IndexChange(null, null);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Program.WriteToGtErrorLog("GT", "frmGtWeigh", System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnWeightStart_Click(object sender, EventArgs e)
        {
            try
            {
                lblErrMsg.Text = "";
                txtManualValue.Text = "0";

                Ctrl_Clear_Restart();

                pnlSkip.Visible = false;
                pnlSaveBtn.Visible = false;
                if (pnlHbw.Visible)
                {
                    timerHbw.Start();
                    lblCcodeHbw.Font = new Font("Verdana", 16, FontStyle.Bold);
                }
                else if (pnlBase.Visible)
                {
                    timerBase.Start();
                    lblCcodeBase.Font = new Font("Verdana", 16, FontStyle.Bold);
                }
                else if (pnlInterface.Visible)
                {
                    timerInter.Start();
                    lblCcodeInter.Font = new Font("Verdana", 13, FontStyle.Bold);
                }
                else if (pnlCenter.Visible)
                {
                    timerCenter.Start();
                    lblCcodeCenter.Font = new Font("Verdana", 16, FontStyle.Bold);
                }
                else if (pnlTread.Visible)
                {
                    timerTread.Start();
                    lblCcodeTread.Font = new Font("Verdana", 16, FontStyle.Bold);
                }
                lblSpecWtTot.Text = Convert.ToDecimal(Convert.ToDecimal(lblWtHbw.Text) + Convert.ToDecimal(lblWtBase.Text) + Convert.ToDecimal(lblWtInter.Text) + Convert.ToDecimal(lblWtCenter.Text) + Convert.ToDecimal(lblWtTread.Text)).ToString();
                if (frmWeighPlan.Weigh_Manual)
                    txtManualValue.Focus();
            }
            catch (Exception ex)
            {
                Program.WriteToGtErrorLog("GT", "frmGtWeigh", System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        private void lnkAddHbw_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                lblErrMsg.Text = "";
                lblAddWtHbw.Text = lblMwtHbw.Text;
            }
            catch (Exception ex)
            {
                Program.WriteToGtErrorLog("GT", "frmGtWeigh", System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        private void lnkAddBase_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                lblErrMsg.Text = "";
                lblAddWtBase.Text = lblMwtBase.Text;
            }
            catch (Exception ex)
            {
                Program.WriteToGtErrorLog("GT", "frmGtWeigh", System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        private void lnkAddInter_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                lblErrMsg.Text = "";
                lblAddWtInter.Text = lblMwtInter.Text;
            }
            catch (Exception ex)
            {
                Program.WriteToGtErrorLog("GT", "frmGtWeigh", System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        private void lnkAddCenter_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                lblErrMsg.Text = "";
                lblAddWtCenter.Text = lblMwtCenter.Text;
            }
            catch (Exception ex)
            {
                Program.WriteToGtErrorLog("GT", "frmGtWeigh", System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        private void lnkAddTread_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                lblErrMsg.Text = "";
                lblAddWtTread.Text = lblMwtTread.Text;
            }
            catch (Exception ex)
            {
                Program.WriteToGtErrorLog("GT", "frmGtWeigh", System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            pnlSkip.Visible = false;
            txtSkipText.Text = "";
        }

        private void btnSkip_Click(object sender, EventArgs e)
        {
            if (txtSkipText.Text == "")
            {
                MessageBox.Show("Enter skip remarks");
                txtSkipText.Focus();
            }
            else
            {
                if (pnlHbw.Visible && lblStatusHbw.Text != "OK")
                {
                    lblHbwRemarks.Text = txtSkipText.Text;
                    btnHbw.Visible = true;
                    timerHbw.Stop();
                }
                else if (pnlBase.Visible && lblStatusBase.Text != "OK")
                {
                    lblBaseRemarks.Text = txtSkipText.Text;
                    btnBase.Visible = true;
                    timerBase.Stop();
                }
                else if (pnlInterface.Visible && lblStatusInter.Text != "OK")
                {
                    lblInterRemarks.Text = txtSkipText.Text;
                    btnInter.Visible = true;
                    timerInter.Stop();
                }
                else if (pnlCenter.Visible && lblStatusCenter.Text != "OK")
                {
                    lblCenterRemarks.Text = txtSkipText.Text;
                    btnCenter.Visible = true;
                    timerCenter.Stop();
                }
                else if (pnlTread.Visible && lblStatusTread.Text != "OK")
                {
                    lblTreadRemarks.Text = txtSkipText.Text;
                    btnTread.Visible = true;
                    timerTread.Stop();
                }
                pnlSkip.Visible = false;
            }
        }

        private void txtStencil_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 13 && txtStencil.Text.Length == 10)
                btnSave_Click(sender, e);
        }

        private void txtManualValue_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (frmWeighPlan.Weigh_Manual)
                Program.digitonly(e);
        }

        private void dtpPrepareDate_ValueChanged(object sender, EventArgs e)
        {

            //if (custcode == "189" || custcode == "DE0061" || custcode == "276" || custcode == "471" || custcode == "DE0075" || custcode == "4807" || custcode == "ME0222" ||
            //            custcode == "2701" || custcode == "3772" || custcode == "ME0216" || custcode == "4808" || custcode == "4936" || custcode == "284" ||
            //            custcode == "DE0042")
            if(result == 1)
           {
              
               if (Program.strPlantName == "MMN")
               {
                   txtStencil.Text = "127" + Convert.ToDateTime(dtpPrepareDate.Value.ToShortDateString()).Year.ToString().Substring(2, 2);
               }
               else if (Program.strPlantName == "PDK")
               {
                   txtStencil.Text = "163" + Convert.ToDateTime(dtpPrepareDate.Value.ToShortDateString()).Year.ToString().Substring(2, 2);
               }
               else
               {
                   txtStencil.Text = (Program.strPlantName == "SLTL" ? "L" : Program.strPlantName == "MMN" ? "C" : Program.strPlantName == "PDK" ? "P" : Program.strPlantName.Substring(0, 1)) +
                   Convert.ToDateTime(dtpPrepareDate.Value.ToShortDateString()).Year.ToString().Substring(2, 2) +
                   Convert.ToDateTime(dtpPrepareDate.Value.ToShortDateString()).Month.ToString("00");
               }
           }
           else
           {
              string sequence = null;
              DataTable sequenceofData = (DataTable)dba.ExecuteReader_SP("Sp_Fetch_the_Serial_No",  DBAccess.Return_Type.DataTable);
              sequence = sequenceofData.Rows[0]["stnclsequence"].ToString();
               

               txtStencil.Text = (Program.strPlantName == "SLTL" ? "L" : Program.strPlantName == "MMN" ? "C" : Program.strPlantName == "PDK" ? "P" : Program.strPlantName.Substring(0, 1)) +
               Convert.ToDateTime(dtpPrepareDate.Value.ToShortDateString()).Year.ToString().Substring(2, 2) +
               Convert.ToDateTime(dtpPrepareDate.Value.ToShortDateString()).Month.ToString("00") + sequence;
           }
              
            
           

            DateTime dtpTime = Convert.ToDateTime(dtpPrepareDate.Value.ToShortTimeString());
            //DateTime shift1Time = Convert.ToDateTime("14:06:00");
            //DateTime shift2Time = Convert.ToDateTime("22:06:00");
            //DateTime shift3Time = Convert.ToDateTime("06:06:00");
            //if ((dtpTime.TimeOfDay > shift3Time.TimeOfDay) && (dtpTime.TimeOfDay <= shift1Time.TimeOfDay))
            //    cboShift.SelectedIndex = 1;
            //else if ((dtpTime.TimeOfDay > shift1Time.TimeOfDay) && (dtpTime.TimeOfDay <= shift2Time.TimeOfDay))
            //    cboShift.SelectedIndex = 2;
            //else if (dtpTime.TimeOfDay > shift2Time.TimeOfDay || dtpTime.TimeOfDay < shift3Time.TimeOfDay)
            //    cboShift.SelectedIndex = 3;

            
            DateTime shift1Time = Convert.ToDateTime("19:06:00");
            //DateTime shift2Time = Convert.ToDateTime("22:06:00");
            DateTime shift2Time = Convert.ToDateTime("07:06:00");
            if ((dtpTime.TimeOfDay > shift2Time.TimeOfDay) && (dtpTime.TimeOfDay <= shift1Time.TimeOfDay))
                cboShift.SelectedIndex = 1;
            else if ((dtpTime.TimeOfDay > shift1Time.TimeOfDay) && (dtpTime.TimeOfDay <= shift2Time.TimeOfDay))
                cboShift.SelectedIndex = 2;
            //else if (dtpTime.TimeOfDay > shift2Time.TimeOfDay || dtpTime.TimeOfDay < shift3Time.TimeOfDay)
            //  cboShift.SelectedIndex = 3;

            
            
            //if ((dtpTime.TimeOfDay > shift2Time.TimeOfDay) && (dtpTime.TimeOfDay <= shift1Time.TimeOfDay))
            //    cboShift.SelectedIndex = 1;
            //else if ((dtpTime.TimeOfDay > shift1Time.TimeOfDay) && (dtpTime.TimeOfDay <= shift2Time.TimeOfDay))
            //    cboShift.SelectedIndex = 2;
            //else if (dtpTime.TimeOfDay > shift2Time.TimeOfDay || dtpTime.TimeOfDay < shift3Time.TimeOfDay)
            //    cboShift.SelectedIndex = 3;

           
            
        }

        private void txtManualValue_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 13 && txtManualValue.Text.Length > 0)
            {
                if (btnHbw.Visible)
                    btnHbw.Focus();
                else if (btnBase.Visible)
                    btnBase.Focus();
                else if (btnInter.Visible)
                    btnInter.Focus();
                else if (btnCenter.Visible)
                    btnCenter.Focus();
                else if (btnTread.Visible)
                    btnTread.Focus();
            }
            else
            {
                txtManualValue.Focus();
                txtManualValue.SelectionStart = txtManualValue.Text.Length;
            }
        }


       public int FindCustCode()
        {
            SqlParameter[] sp = new SqlParameter[] { 
            //new SqlParameter("@TyreSize", lblSize.Text),
            //            new SqlParameter("@RimSize", lblRim.Text),
            //            new SqlParameter("@TyreType", lblType.Text),                       
            //            new SqlParameter("@Config",lblPlatform.Text),
            //            new SqlParameter("@Brand",lblBrand.Text),
            //            new SqlParameter("@Sidewall",lblSidewall.Text),
            //            new SqlParameter("@RequiredQuantity",lblQty.Text),
            //            new SqlParameter("@Prod_ItemID", frmWeighPlan.Prod_ItemID)};
           new SqlParameter("@Prod_ItemID", frmWeighPlan.Prod_ItemID)};

            DataTable dtPlantItem = (DataTable)dba.ExecuteReader_SP("Sp_Find_Cust_Code_v2", sp, DBAccess.Return_Type.DataTable);

           
            //string rtrun;
            int results;
                   // rtrun = dtPlantItem.Rows[0]["customercode"].ToString();
                    var table = dtPlantItem;
                    if (table.Rows.Count > 0)
                    {
                        results = 1;
                    }
                    else
                    {
                        results = 0;
                    }
            
            //return rtrun;  
                    return results;
        }


        private void Bind_PlannedPendingListGv()
        {
            try
            {
                dgv_PlanItems.DataSource = null;
                dgv_PlanItems.Columns.Clear();
                dgv_PlanItems.Rows.Clear();

                SqlParameter[] spDate = new SqlParameter[] { new SqlParameter("@Plant", Program.strPlantName) };
                DataTable dtPlantItem = (DataTable)dba.ExecuteReader_SP("sp_sel_Prod_Weighing_PlanList_v1", spDate, DBAccess.Return_Type.DataTable);
                if (dtPlantItem != null && dtPlantItem.Rows.Count > 0)
                {
                    DataView view = new DataView(dtPlantItem);
                    view.RowFilter = "Prod_Press='" + frmWeighPlan.Prod_Press + "' and Prod_PlanID<>'" + frmWeighPlan.ProductionID + "'";
                    view.Sort = "Prod_MouldID ASC";
                    DataTable dtList = view.ToTable(true);

                    if (dtList != null && dtList.Rows.Count > 0)
                    {
                        dgv_PlanItems.DataSource = dtList;
                        dgv_PlanItems.Columns[0].Visible = false; //Prod_ItemID
                        dgv_PlanItems.Columns[1].Visible = false; //Prod_PlanID
                        dgv_PlanItems.Columns[2].Visible = false; //Prod_Press
                        dgv_PlanItems.Columns[4].Visible = false; //Config
                        dgv_PlanItems.Columns[8].Visible = false; //brand
                        dgv_PlanItems.Columns[9].Visible = false; //sidewall
                        dgv_PlanItems.Columns[10].Visible = false; //processid
                        dgv_PlanItems.Columns[11].Visible = false; //Prod_ReqQty
                        dgv_PlanItems.Columns[12].Visible = false; //WeighingQty
                        dgv_PlanItems.Columns[13].Visible = false; //RejectQty
                        dgv_PlanItems.Columns[14].Visible = false; //Prod_RemainQty
                        dgv_PlanItems.Columns[3].Width = 80; //Prod_MouldID
                        dgv_PlanItems.Columns[3].HeaderText = "MOULD";
                        dgv_PlanItems.Columns[3].ReadOnly = true;
                        dgv_PlanItems.Columns[5].Width = 150; //tyresize
                        dgv_PlanItems.Columns[5].HeaderText = "TYRE SIZE";
                        dgv_PlanItems.Columns[5].ReadOnly = true;
                        dgv_PlanItems.Columns[6].Width = 40; //rim
                        dgv_PlanItems.Columns[6].HeaderText = "RIM";
                        dgv_PlanItems.Columns[6].ReadOnly = true;
                        dgv_PlanItems.Columns[7].Width = 60; //type
                        dgv_PlanItems.Columns[7].HeaderText = "TYPE";
                        dgv_PlanItems.Columns[7].ReadOnly = true;

                        DataGridViewButtonColumn btn = new DataGridViewButtonColumn();
                        dgv_PlanItems.Columns.Add(btn);
                        btn.Text = "WT";
                        btn.Name = "btn_weightMaster";
                        btn.UseColumnTextForButtonValue = true;

                        foreach (DataGridViewColumn dgvc in dgv_PlanItems.Columns)
                        {
                            dgvc.SortMode = DataGridViewColumnSortMode.NotSortable;
                        }
                        dgv_PlanItems.Focus();
                    }
                }
            }
            catch (Exception ex)
            {
                Program.WriteToGtErrorLog("GT", "frmGtWeigh", System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        private void dgv_PlanItems_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex >= 0 && dgv_PlanItems.Columns[e.ColumnIndex].Name == "btn_weightMaster")
                {
                    frmWeighPlan.Prod_ItemID = Convert.ToInt32(dgv_PlanItems.Rows[e.RowIndex].Cells["Prod_ItemID"].Value.ToString());
                    frmWeighPlan.ProductionID = Convert.ToInt32(dgv_PlanItems.Rows[e.RowIndex].Cells["Prod_PlanID"].Value.ToString());
                    strWtClass = cboWtClass.SelectedIndex <= 0 ? "" : cboWtClass.SelectedValue.ToString();
                    strFriction = cboFriction.SelectedIndex <= 0 ? "" : cboFriction.SelectedValue.ToString();

                    frmGtWeigh_Load(sender, e);
                }
            }
            catch (Exception ex)
            {
                Program.WriteToGtErrorLog("GT", "frmGtWeigh", System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        private void cboCommunication_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (cboCommunication.SelectedIndex > 0 && cboCommunication.SelectedValue.ToString() != "System.Data.DataRowView")
                {
                    strCommunication = cboCommunication.SelectedValue.ToString();
                    string filePath = AppDomain.CurrentDomain.BaseDirectory + "\\comm.dll";
                    FileInfo file1 = new FileInfo(filePath);
                    if (file1.Exists)
                        file1.Delete();

                    if (!Directory.Exists(AppDomain.CurrentDomain.BaseDirectory + "\\"))
                        Directory.CreateDirectory(AppDomain.CurrentDomain.BaseDirectory + "\\");

                    StreamWriter SWrtiter = System.IO.File.AppendText(filePath);
                    SWrtiter.WriteLine(EncryptDecrypt.Encrypt(strCommunication, "COMMUNICATION"));
                    SWrtiter.Close();
                }
            }
            catch (Exception)
            {
                //Program.WriteToGtErrorLog("GT", "frmGtWeigh", System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        private void lblCcode_FontChanged(object sender, EventArgs e)
        {
            try
            {
                lblBlink = (Label)sender;
                if (lblBlink.Font.Size == 16 || lblBlink.Font.Size == 13)
                    tmrBlink.Start();
                else
                {
                    tmrBlink.Stop();
                    lblBlink.BackColor = Color.Transparent;
                }
            }
            catch (Exception ex)
            {
                Program.WriteToGtErrorLog("GT", "frmGtWeigh", System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        private void lnk_RemoveConc_Click(object sender, EventArgs e)
        {
            try
            {
                LinkLabel lnk = (LinkLabel)sender;
                if (lnk.Name == "lnkConcBase" && Convert.ToDecimal(lblMwtBase.Text) == 0)
                {
                    lblCcodeInter.Text = lblCcodeInter.Text.Replace(lblCcodeBase.Text, lnk.Text);
                    lblCcodeBase.Text = lnk.Text;
                }
                else if (lnk.Name == "lnkConcCenter" && Convert.ToDecimal(lblMwtCenter.Text) == 0)
                {
                    lblCcodeInter.Text = lblCcodeInter.Text.Replace(lblCcodeCenter.Text, lnk.Text);
                    lblCcodeCenter.Text = lnk.Text;
                }
                else if (lnk.Name == "lnkConcTread" && Convert.ToDecimal(lblMwtTread.Text) == 0)
                {
                    lblCcodeInter.Text = lblCcodeInter.Text.Replace(lblCcodeTread.Text, lnk.Text);
                    lblCcodeTread.Text = lnk.Text;
                }
                lnk.Text = "";
                lnk.Visible = false;
            }
            catch (Exception ex)
            {
                Program.WriteToGtErrorLog("GT", "frmGtWeigh", System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        private void tmrBlink_Tick(object sender, EventArgs e)
        {
            if (lblBlink.BackColor == Color.White)
                lblBlink.BackColor = Color.Transparent;
            else
                lblBlink.BackColor = Color.White;
        }

        private void txtBatchTread_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13 && txtBatchTread.Text != "")
            {
                txtStencil.Focus();
                txtStencil.SelectionStart = txtStencil.Text.Length;
            }
        }

        private void txtStencil_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
