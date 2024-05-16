using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.IO;
using System.Windows.Forms;
using System.Data.OleDb;
using System.Configuration;
using System.Reflection;
using System.IO.Ports;
using Modbus.Device;
using Modbus.Utility;
using System.Text.RegularExpressions;
using System.Net.Sockets;
using System.Net;
//using System.Net.Sockets;
//using System.Net;

namespace GT
{
    public partial class frmGtBuild : Form
    {
        DBAccess dbCon = new DBAccess();
        DataTable dtProcessIdDetails;
        string strCommunication = "";
        public frmGtBuild()
        {
            try
            {
                InitializeComponent();

                cboShift.Items.Clear();
                cboShift.Items.Add("--SELECT--");
                cboShift.Items.Add("SHIFT 1");
                cboShift.Items.Add("SHIFT 2");
               // cboShift.Items.Add("SHIFT 3");//done by siva
                cboShift.Text = "--SELECT--";

                panel1.Location = new Point(0, 0);
                this.KeyPreview = true;
                this.txtgttemp.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtNumericPercentage_KeyPress);
                this.txtGDWidth.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtNumericPercentage_KeyPress);
                this.txtGTOD.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtNumericPercentage_KeyPress);
                //this.txtgttemp.GotFocus += textBox1_GotFocus;
                lblCurrentOperatorName.Text = Program.strUserName;

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
            }
            catch (Exception ex)
            {
                Program.WriteToGtErrorLog("GT", "frmGtBuild", System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        private void frmGtBuild_Load(object sender, EventArgs e)
        {
            try
            {
                dtpPrepareDate.MinDate = DateTime.Now.AddDays(-3);
                dtpPrepareDate.MaxDate = DateTime.Now.AddDays(0);
                dtpPrepareDate.Value = dtpPrepareDate.MaxDate;

                dtpPrepareDate.CustomFormat = String.Format("MMM/dd/yyyy");
                dtpPrepareDate.Enabled = false;

                DataTable dtList = (DataTable)dbCon.ExecuteReader_SP("SP_LST_GreenWt_StencilNo", DBAccess.Return_Type.DataTable);
                listBox1.DataSource = dtList;
                listBox1.DisplayMember = "StencilNo";
                listBox1.ValueMember = "StencilNo";

                Ctrl_Clear();
            }
            catch (Exception ex)
            {
                Program.WriteToGtErrorLog("GT", "frmGtBuild", System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        private void txtNumericPercentage_KeyPress(object sender, KeyPressEventArgs e)
        {
            Program.digitonly(e);
        }
        private void Make_DtpDate()
        {
            try
            {
                DateTime endTime = Convert.ToDateTime("07:06:00");
                if (DateTime.Now.TimeOfDay < endTime.TimeOfDay)
                {
                    DateTime result = DateTime.Now.Subtract(TimeSpan.FromDays(1));
                    dtpPrepareDate.Value = result;
                }
            }
            catch (Exception ex)
            {
                Program.WriteToGtErrorLog("GT", "frmGtBuild", System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        private void btnsave_Click(object sender, EventArgs e)
        {
            try
            {
                lblErrMsg.Text = "";
                if (cboShift.Text == "--SELECT--")
                {
                    lblErrMsg.Text = "Select shift";
                    cboShift.Focus();
                }
               // else if ((txtgttemp.Text == "") || ((59 < Convert.ToDecimal(txtgttemp.Text)) && (71 > Convert.ToDecimal(txtgttemp.Text))))
                else if (txtgttemp.Text == "")
                {
                    lblErrMsg.Text = "Enter GT Temperature";
                    txtgttemp.Focus();
                }
                else if (cboBrand.Text == "CHOOSE" || cboBrand.Text == "")
                {
                    lblErrMsg.Text = "Choose brand";
                    cboBrand.Focus();
                }
                else if (cboSidewall.Text == "CHOOSE" || cboSidewall.Text == "")
                {
                    lblErrMsg.Text = "Choose sidewall";
                    cboSidewall.Focus();
                }
                else
                {
                    tmrActWtNew.Stop();
                    tmrBandWtNew.Stop();
                    string strQuery = string.Empty;
                    SqlParameter[] sp = new SqlParameter[] { new SqlParameter("@stencilNo", txtStencilno.Text) };
                    DataTable dt = (DataTable)dbCon.ExecuteReader_SP("sp_sel_gtwt_productionDetails", sp, DBAccess.Return_Type.DataTable);
                    if (dt.Rows.Count == 1)
                        lblErrMsg.Text = "Stencil no. already existing " + dt.Rows[0]["ProductionDate"].ToString() + " " + dt.Rows[0]["Shift"].ToString() + " " + dt.Rows[0]["Operator"].ToString();
                    else
                    {
                        DateTime prepareDate;
                        DateTime prepareTime;
                        try
                        {
                            prepareDate = DateTime.Parse(dtpPrepareDate.Text);
                            if (lblPageTitle.Text == "GT BUILD")
                                prepareTime = DateTime.Parse(DateTime.Now.ToString("HH:mm"));
                            else
                                prepareTime = DateTime.Parse(dtpPrepareDate.Value.ToShortTimeString());
                        }
                        catch (Exception)
                        {
                            prepareDate = DateTime.ParseExact(dtpPrepareDate.Text, "dd/MM/yyyy", null);
                            if (lblPageTitle.Text == "GT BUILD")
                                prepareTime = DateTime.ParseExact(DateTime.Now.ToString(), "HH:mm", null);
                            else
                                prepareTime = DateTime.ParseExact(dtpPrepareDate.Value.ToShortTimeString(), "HH:mm", null);
                        }
                        decimal strBandWt = 0, strCushionGum = 0;
                        if (lblRimWidth.Text == "0" || (lbltyresize.Text == "100/80-8MOULDON" && lblRimWidth.Text == "2.50") || (lbltyresize.Text == "100/80-8EDC701" && lblRimWidth.Text == "2.50") || (lbltyresize.Text == "100/80-8EDC703" && lblRimWidth.Text == "2.50"))
                        {
                            string[] strSplit = lbllipcode.Text.Split('+');
                            strBandWt = Convert.ToDecimal(strSplit[0].ToString());
                            strCushionGum = Convert.ToDecimal(strSplit[1].ToString());
                        }
                        SqlParameter[] sp1 = new SqlParameter[]{
                            new SqlParameter("@ProductionDate",prepareDate),
                            new SqlParameter("@ProductionTime",prepareTime),
                            new SqlParameter("@Shift",cboShift.Text),
                            new SqlParameter("@StencilNo",txtStencilno.Text),
                            new SqlParameter("@Operator",lblCurrentOperatorName.Text),
                            new SqlParameter("@MouldCode",lblmoulduniqcode.Text),
                            new SqlParameter("@BWSize",lblRimWidth.Text == "0" || (lbltyresize.Text == "100/80-8MOULDON" && lblRimWidth.Text == "2.50") || (lbltyresize.Text == "100/80-8EDC701" && lblRimWidth.Text == "2.50") || (lbltyresize.Text == "100/80-8EDC701" && lblRimWidth.Text == "2.50") ? "0" : lblBWSize.Text),
                            //new SqlParameter("@BWSize",lblRimWidth.Text == "0" || (lbltyresize.Text == "100/80-8MOULDON" && lblRimWidth.Text == "2.50") ? "0" : lblBWSize.Text),
                            new SqlParameter("@NoofBW",lblRimWidth.Text == "0" || (lbltyresize.Text == "100/80-8MOULDON" && lblRimWidth.Text == "2.50")|| (lbltyresize.Text == "100/80-8EDC701" && lblRimWidth.Text == "2.50") || (lbltyresize.Text == "100/80-8EDC701" && lblRimWidth.Text == "2.50") ? "0" : lblNoofBw.Text),
                            //new SqlParameter("@NoofBW",lblRimWidth.Text == "0" || (lbltyresize.Text == "100/80-8MOULDON" && lblRimWidth.Text == "2.50") ? "0" : lblNoofBw.Text),
                            new SqlParameter("@BulidPressureFrom",lblbuildPressurefrom.Text),
                            new SqlParameter("@BulidPressureTo",lblbuildPressureTo.Text),
                            new SqlParameter("@lipSwSpec",lbllipswspec.Text),
                            new SqlParameter("@BaseSwSpec",lblbaseswspec.Text),
                            new SqlParameter("@InterfaceSwSpec",lblinterfaceswspec.Text),
                            new SqlParameter("@CenterSwSpec",lblCenterswspec.Text),
                            new SqlParameter("@TreadSwSpec",lbltreadswspec.Text),
                            new SqlParameter("@TotalWtSpec",lblSpecTotal.Text),
                            new SqlParameter("@GtOdOld",lblGTOD.Text),
                            new SqlParameter("@GtOdNew",txtGTOD.Text),
                            new SqlParameter("@GtWidthOld",lblGDWidth.Text),
                            new SqlParameter("@GtWitdthNew",txtGDWidth.Text),
                            new SqlParameter("@ActualGtWeight",txtMachineWt.Text),
                            new SqlParameter("@UserName",(Program.strUserName + (lblPageTitle.Text == "GT BUILD - MANUAL" ? " -MANUAL" : ""))),
                            new SqlParameter("@GdTemp",txtgttemp.Text),
                            new SqlParameter("@FormerCode",lblformercode.Text),
                            new SqlParameter("@BandSize",lblRimWidth.Text=="0"|| (lbltyresize.Text == "100/80-8MOULDON" && lblRimWidth.Text == "2.50")?lblBWSize.Text :""),
                            new SqlParameter("@BandWt",strBandWt),
                            new SqlParameter("@CushionGumWt",strCushionGum),
                            new SqlParameter("@ActBandCushionWt",lblRimWidth.Text=="0"|| (lbltyresize.Text == "100/80-8MOULDON" && lblRimWidth.Text == "2.50")?lblhbwweighed.Text:"0"),
                            new SqlParameter("@Brand",cboBrand.Text),
                            new SqlParameter("@Sidewall",cboSidewall.Text)
                        };
                        int resp = dbCon.ExecuteNonQuery_SP("SP_Ins_GreenWt_ProductionDetails", sp1);
                        if (resp > 0)
                        {
                            update_stencil("0");
                            MessageBox.Show("Records successfully Save");
                            frmGtBuild_Load(null, null);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Program.WriteToGtErrorLog("GT", "frmGtBuild", System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
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

                dtProcessIdDetails = (DataTable)dbCon.ExecuteReader_SP("SP_SEL_EditBarcode_ProcessIdDetails", DBAccess.Return_Type.DataTable);

                txtStencilno.TextChanged -= new EventHandler(txtStencilno_TextChanged);
                btnsave.Visible = false;
                listBox1.Enabled = true;
                lblbuildPressureTo.Text = "";
                lblbuildPressurefrom.Text = "";
                lblhbwspec.Text = "";
                lblbasespec.Text = "";
                txtgttemp.Text = "";
                lblinterspec.Text = "";
                lblcenterspec.Text = "";
                lbltreadspec.Text = "";
                lblhbwweighed.Text = "";
                lblbaseweighed.Text = "";
                lblinterfaceweighed.Text = "";
                lblcenterweighed.Text = "";
                lbltreadweighed.Text = "";
                lblStencilno.Text = "";
                lblmoulduniqcode.Text = "";
                lblType.Text = "";
                lbltyresize.Text = "";
                lblRimWidth.Text = "";
                lblDate.Text = "";
                lblShift.Text = "";
                lblOperatorname.Text = "";
                lbllipcode.Text = "";
                lblbasecode.Text = "";
                lblInterfaceCode.Text = "";
                lblCenterCode.Text = "";
                lblTreadCode.Text = "";
                lblformercode.Text = "";
                txtStencilno.Text = "";
                lblBWSize.Text = "0";
                lblNoofBw.Text = "0";
                lblSpecTotal.Text = "";
                txtGDWidth.Text = "";
                txtGTOD.Text = "";
                lbllipswspec.Text = "";
                lblbaseswspec.Text = "";
                lblinterfaceswspec.Text = "";
                lblCenterswspec.Text = "";
                lbltreadswspec.Text = "";
                lblGTOD.Text = "";
                lblGDWidth.Text = "";
                lblErrMsg.Text = "";
                lblActualTotal.Text = "";

                lblAdjuTot.Text = "";
                lblAdjuHbw.Text = "";
                lblAdjuBase.Text = "";
                lblAdjuInter.Text = "";
                lblAdjuCenter.Text = "";
                lblAdjuTread.Text = "";

                txtgttemp.Text = "";
                lblF8Usage.Text = "";
                lblLipF8.Text = "";
                lblBaseF8.Text = "";
                lblInterF8.Text = "";
                lblCenterF8.Text = "";
                lblTreadF8.Text = "";
                lblStatusGTWidth.Text = "NOT OK";
                lblStatusGTWidth.ForeColor = Color.Red;
                label28.Text = "NOT OK";
                label28.ForeColor = Color.Red;
                lblStatusGTOD.Text = "NOT OK";
                lblStatusGTOD.ForeColor = Color.Red;
                lblStatusActualGt.Text = "NOT OK";
                lblStatusActualGt.ForeColor = Color.Red;
                tmrBandWtNew.Stop();
                tmrActWtNew.Stop();
                txtMachineWt.Text = "0.000";

                cboBrand.DataSource = null;
                cboBrand.Items.Clear();
                cboSidewall.DataSource = null;
                cboSidewall.Items.Clear();

                lblPageTitle.Text = "GT BUILD";
                txtManualValue.Visible = false;
                txtManualValue.Text = "0";


                Make_DtpDate();

                txtStencilno.TextChanged += new EventHandler(txtStencilno_TextChanged);
            }
            catch (Exception ex)
            {
                Program.WriteToGtErrorLog("GT", "frmGtBuild", System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        private void txtStencilno_TextChanged(object sender, EventArgs e)
        {
            try
            {
                lblF8Usage.Text = "";
                lblLipF8.Text = "";
                lblBaseF8.Text = "";
                lblInterF8.Text = "";
                lblCenterF8.Text = "";
                lblTreadF8.Text = "";

                SqlParameter[] sp = new SqlParameter[] { new SqlParameter("@stencilNo", txtStencilno.Text) };
                DataTable dtdetails = (DataTable)dbCon.ExecuteReader_SP("sp_get_gtwt_StencilNoDetails", sp, DBAccess.Return_Type.DataTable);
                if (dtdetails.Rows.Count > 0)
                {
                    lblStencilno.Text = dtdetails.Rows[0]["StencilNo"].ToString();
                    lblmoulduniqcode.Text = dtdetails.Rows[0]["MouldCode"].ToString();
                    lblType.Text = dtdetails.Rows[0]["TyreType"].ToString();
                    lbltyresize.Text = dtdetails.Rows[0]["TyreSize"].ToString();
                    lblRimWidth.Text = dtdetails.Rows[0]["RimSize"].ToString();
                    lblDate.Text = dtdetails.Rows[0]["ProductionDate"].ToString() + " " + dtdetails.Rows[0]["ProductionTime"].ToString();
                    lblShift.Text = dtdetails.Rows[0]["Shift"].ToString().Replace("SHIFT", "/");
                    lblOperatorname.Text = dtdetails.Rows[0]["Operator"].ToString();
                    lbllipcode.Text = dtdetails.Rows[0]["LipCode"].ToString();
                    lblbasecode.Text = dtdetails.Rows[0]["BaseCode"].ToString();
                    lblInterfaceCode.Text = dtdetails.Rows[0]["InterfaceCode"].ToString();
                    lblCenterCode.Text = dtdetails.Rows[0]["CenterCode"].ToString();
                    lblTreadCode.Text = dtdetails.Rows[0]["TreadCode"].ToString();

                    lblhbwspec.Text = Convert.ToDecimal(dtdetails.Rows[0]["LipRqWt"].ToString()).ToString("0.000");
                    lblbasespec.Text = Convert.ToDecimal(dtdetails.Rows[0]["BaseTolerance"].ToString()).ToString("0.000");
                    lblinterspec.Text = Convert.ToDecimal(dtdetails.Rows[0]["InterfaceRqWt"].ToString()).ToString("0.000");
                    lblcenterspec.Text = Convert.ToDecimal(dtdetails.Rows[0]["CenterRqWt"].ToString()).ToString("0.000");
                    lbltreadspec.Text = Convert.ToDecimal(dtdetails.Rows[0]["TreadTolerance"].ToString()).ToString("0.000");

                    lblAdjuHbw.Text = Convert.ToDecimal(dtdetails.Rows[0]["LipScale"].ToString()).ToString("0.000");
                    lblAdjuBase.Text = Convert.ToDecimal(dtdetails.Rows[0]["BaseRqWt"].ToString()).ToString("0.000");
                    lblAdjuInter.Text = Convert.ToDecimal(dtdetails.Rows[0]["InterfaceRqWt"].ToString()).ToString("0.000");
                    lblAdjuCenter.Text = Convert.ToDecimal(dtdetails.Rows[0]["CenterRqWt"].ToString()).ToString("0.000");
                    lblAdjuTread.Text = Convert.ToDecimal(dtdetails.Rows[0]["TreadRqWt"].ToString()).ToString("0.000");

                    lblhbwweighed.Text = Convert.ToDecimal(dtdetails.Rows[0]["LipScale"].ToString()).ToString("0.000");
                    lblbaseweighed.Text = Convert.ToDecimal(dtdetails.Rows[0]["BaseScale"].ToString()).ToString("0.000");
                    lblinterfaceweighed.Text = Convert.ToDecimal(dtdetails.Rows[0]["InterfaceScale"].ToString()).ToString("0.000");
                    lblcenterweighed.Text = Convert.ToDecimal(dtdetails.Rows[0]["CenterScale"].ToString()).ToString("0.000");
                    lbltreadweighed.Text = Convert.ToDecimal(dtdetails.Rows[0]["TreadScale"].ToString()).ToString("0.000");

                    if (dtdetails.Rows[0]["LipRqWt"].ToString() != "0" && dtdetails.Rows[0]["LipRqWt"].ToString() != "")
                    {
                        pnlHbw.Visible = true;
                        pnlHbw.Location = new Point(panel7.Left, panel7.Bottom);
                        label10.Text = "H+BW";
                    }
                    else if (lblRimWidth.Text == "0" || (lbltyresize.Text == "100/80-8MOULDON" && lblRimWidth.Text == "2.50") || (lbltyresize.Text == "100/80-8EDC703" && lblRimWidth.Text != "2.50") || ((lbltyresize.Text == "100/80-8EDC701" && lblRimWidth.Text == "2.50")))
                    {
                        pnlHbw.Visible = true;
                        pnlHbw.Location = new Point(panel7.Left, panel7.Bottom);
                        label10.Text = "BAND + CG";
                    }

                    if (dtdetails.Rows[0]["BaseRqWt"].ToString() != "0" && dtdetails.Rows[0]["BaseRqWt"].ToString() != "")
                    {
                        pnlBase.Visible = true;
                        if (pnlHbw.Visible)
                            pnlBase.Location = new Point(pnlHbw.Left, pnlHbw.Bottom);
                        else
                            pnlBase.Location = new Point(panel7.Left, panel7.Bottom);
                    }
                    if (dtdetails.Rows[0]["InterfaceRqWt"].ToString() != "0" && dtdetails.Rows[0]["InterfaceRqWt"].ToString() != "")
                    {
                        pnlInterface.Visible = true;
                        if (pnlBase.Visible)
                            pnlInterface.Location = new Point(pnlBase.Left, pnlBase.Bottom);
                        else if (pnlHbw.Visible)
                            pnlInterface.Location = new Point(pnlHbw.Left, pnlHbw.Bottom);
                        else
                            pnlInterface.Location = new Point(panel7.Left, panel7.Bottom);
                    }
                    if (dtdetails.Rows[0]["CenterRqWt"].ToString() != "0" && dtdetails.Rows[0]["CenterRqWt"].ToString() != "")
                    {
                        pnlCenter.Visible = true;
                        if (pnlInterface.Visible)
                            pnlCenter.Location = new Point(pnlInterface.Left, pnlInterface.Bottom);
                        else if (pnlBase.Visible)
                            pnlCenter.Location = new Point(pnlBase.Left, pnlBase.Bottom);
                        else if (pnlHbw.Visible)
                            pnlCenter.Location = new Point(pnlHbw.Left, pnlHbw.Bottom);
                        else
                            pnlCenter.Location = new Point(panel7.Left, panel7.Bottom);
                    }
                    if (dtdetails.Rows[0]["TreadRqWt"].ToString() != "0" && dtdetails.Rows[0]["TreadRqWt"].ToString() != "")
                    {
                        pnlTread.Visible = true;
                        if (pnlCenter.Visible)
                            pnlTread.Location = new Point(pnlCenter.Left, pnlCenter.Bottom);
                        else if (pnlInterface.Visible)
                            pnlTread.Location = new Point(pnlInterface.Left, pnlInterface.Bottom);
                        else if (pnlBase.Visible)
                            pnlTread.Location = new Point(pnlBase.Left, pnlBase.Bottom);
                        else if (pnlHbw.Visible)
                            pnlTread.Location = new Point(pnlHbw.Left, pnlHbw.Bottom);
                        else
                            pnlTread.Location = new Point(panel7.Left, panel7.Bottom);
                    }
                    SqlParameter[] sp1 = new SqlParameter[] { new SqlParameter("@TyreType", lblType.Text) };
                    DataTable dtdetails1 = (DataTable)dbCon.ExecuteReader_SP("sp_get_gtwt_BwStatus", sp1, DBAccess.Return_Type.DataTable);
                    if (dtdetails1.Rows.Count > 0)
                    {
                        if (lblRimWidth.Text != "0" )
                        
                        //if (lblRimWidth.Text != "0" && (lbltyresize.Text != "100/80-8MOULDON" && lblRimWidth.Text != "2.50") || (lbltyresize.Text != "100/80-8EDC703" && lblRimWidth.Text != "2.50") || ((lbltyresize.Text != "100/80-8EDC701" && lblRimWidth.Text != "2.50")))
                        {
                            if (dtdetails1.Rows[0]["BwStatus"].ToString() == "True")
                            {
                                label10.Text = "HS + BW";
                                label10.ForeColor = Color.DeepPink;
                                label10.BackColor = Color.White;
                            }
                            else
                            {
                                label10.Text = "HEEL STRIP";
                                label10.ForeColor = Color.Maroon;
                                label10.BackColor = Color.Gainsboro;
                            }
                        }
                        SqlParameter[] sp2 = new SqlParameter[] {
                            new SqlParameter("@TyreSize", lbltyresize.Text),
                            new SqlParameter("@RimSize", lblRimWidth.Text),
                            new SqlParameter("@MouldCode", lblmoulduniqcode.Text)
                        };
                        DataTable dtdetails2 = (DataTable)dbCon.ExecuteReader_SP("sp_get_gtwt_mouldDetails", sp2, DBAccess.Return_Type.DataTable);
                        if (dtdetails2.Rows.Count > 0)
                        {
                            lblformercode.Text = dtdetails2.Rows[0]["FormerCode"].ToString();
                            if (dtdetails1.Rows[0]["BwStatus"].ToString() == "True")
                            {
                                lblBWSize.Text = dtdetails2.Rows[0]["BWSize"].ToString();
                                lblNoofBw.Text = dtdetails2.Rows[0]["NoofBw"].ToString();
                            }
                            if (lblRimWidth.Text == "0" || (lbltyresize.Text == "100/80-8MOULDON" && lblRimWidth.Text == "2.50") || (lbltyresize.Text == "100/80-8EDC703" && lblRimWidth.Text == "2.50") || ((lbltyresize.Text == "100/80-8EDC701" && lblRimWidth.Text == "2.50")))
                            {
                                lblBWSize.Text = dtdetails2.Rows[0]["BandSize"].ToString();
                                lbllipcode.Text = dtdetails2.Rows[0]["BandWt"].ToString() + "+" + dtdetails2.Rows[0]["CushionGumWt"].ToString();
                                lblhbwspec.Text = (Convert.ToDecimal(dtdetails2.Rows[0]["BandWt"].ToString()) + Convert.ToDecimal(dtdetails2.Rows[0]["CushionGumWt"].ToString())).ToString("0.000");
                                tmrBandWtNew.Start();
                            }
                            else
                                tmrActWtNew.Start();
                            lbllipswspec.Text = dtdetails2.Rows[0]["LipSwSpec"].ToString();
                            lblbaseswspec.Text = dtdetails2.Rows[0]["BaseSwSpec"].ToString();
                            lblinterfaceswspec.Text = dtdetails2.Rows[0]["InterfaceSwSpec"].ToString();
                            lblCenterswspec.Text = dtdetails2.Rows[0]["CenterSwSpec"].ToString();
                            lbltreadswspec.Text = dtdetails2.Rows[0]["TreadSwSpec"].ToString();
                            lblGTOD.Text = dtdetails2.Rows[0]["GTOD"].ToString();
                            lblGDWidth.Text = dtdetails2.Rows[0]["GTWidth"].ToString();
                            lblbuildPressurefrom.Text = dtdetails2.Rows[0]["BuildPressureFrom"].ToString();
                            lblbuildPressureTo.Text = dtdetails2.Rows[0]["BuildPressureTo"].ToString();
                        }
                    }
                    lblActualTotal.Text = Convert.ToDecimal(Convert.ToDecimal(lblhbwweighed.Text) + Convert.ToDecimal(lblbaseweighed.Text) +
                        Convert.ToDecimal(lblinterfaceweighed.Text) + Convert.ToDecimal(lblcenterweighed.Text) + Convert.ToDecimal(lbltreadweighed.Text)).ToString("0.000");
                    lblSpecTotal.Text = Convert.ToDecimal(Convert.ToDecimal(lblhbwspec.Text) + Convert.ToDecimal(lblbasespec.Text) + Convert.ToDecimal(lblinterspec.Text) +
                        Convert.ToDecimal(lblcenterspec.Text) + Convert.ToDecimal(lbltreadspec.Text)).ToString("0.000");
                    lblAdjuTot.Text = Convert.ToDecimal(Convert.ToDecimal(lblAdjuHbw.Text) + Convert.ToDecimal(lblAdjuBase.Text) + Convert.ToDecimal(lblAdjuInter.Text) +
                        Convert.ToDecimal(lblAdjuCenter.Text) + Convert.ToDecimal(lblAdjuTread.Text)).ToString("0.000");

                    if (dtdetails.Rows[0]["LipRemarks"].ToString() != "" || dtdetails.Rows[0]["BaseRemarks"].ToString() != "" || dtdetails.Rows[0]["InterfaceRemarks"].ToString() != ""
                        || dtdetails.Rows[0]["CenterRemarks"].ToString() != "" || dtdetails.Rows[0]["TreadRemarks"].ToString() != "")
                    {
                        lblF8Usage.Text = "F8 KEY USED";
                        lblLipF8.Text = dtdetails.Rows[0]["LipRemarks"].ToString() != "" ? dtdetails.Rows[0]["LipRemarks"].ToString() : "";
                        lblBaseF8.Text = dtdetails.Rows[0]["BaseRemarks"].ToString() != "" ? dtdetails.Rows[0]["BaseRemarks"].ToString() : "";
                        lblInterF8.Text = dtdetails.Rows[0]["InterfaceRemarks"].ToString() != "" ? dtdetails.Rows[0]["InterfaceRemarks"].ToString() : "";
                        lblCenterF8.Text = dtdetails.Rows[0]["CenterRemarks"].ToString() != "" ? dtdetails.Rows[0]["CenterRemarks"].ToString() : "";
                        lblTreadF8.Text = dtdetails.Rows[0]["TreadRemarks"].ToString() != "" ? dtdetails.Rows[0]["TreadRemarks"].ToString() : "";
                    }

                    cboBrand.DataSource = null;
                    cboBrand.Items.Clear();
                    if (dtdetails.Rows[0]["Prod_ItemID"].ToString() == "0")
                    {
                        string strDefaultTyreSize = lbltyresize.Text.Substring(0, 1);
                        string strDefaultTyretype = lblType.Text.Substring(0, 2);
                        string strDefaultTyreRim = lblRimWidth.Text.Substring(0, 1);

                        List<string> lstBrand = dtProcessIdDetails.AsEnumerable().Where(
                            b => b.Field<string>("TyreType").StartsWith(strDefaultTyretype) &&
                                b.Field<string>("TyreSize").StartsWith(strDefaultTyreSize) &&
                                b.Field<string>("TyreRim").StartsWith(strDefaultTyreRim)
                                ).Select(A => A.Field<string>("Brand")).Distinct().ToList();
                        lstBrand.Insert(0, "CHOOSE");
                        cboBrand.DataSource = lstBrand;
                        if (cboBrand.Items.Count == 2)
                        {
                            cboBrand.SelectedIndex = 1;
                            cboBrand_SelectedIndexChanged(null, null);
                        }
                    }
                    else if (dtdetails.Rows[0]["Prod_ItemID"].ToString() != "0")
                    {
                        SqlParameter[] spData = new SqlParameter[] { new SqlParameter("@ItemID", dtdetails.Rows[0]["Prod_ItemID"].ToString()) };
                        DataTable dt = (DataTable)dbCon.ExecuteReader_SP("sp_Item_WeightTrack", spData, DBAccess.Return_Type.DataTable);
                        if (dt.Rows.Count == 1)
                        {
                            cboBrand.Items.Add(dt.Rows[0]["brand"].ToString());
                            cboBrand.SelectedIndex = 0;
                            cboSidewall.Items.Add(dt.Rows[0]["sidewall"].ToString());
                            cboSidewall.SelectedIndex = 0;
                        }
                    }
                }
                if ((lbllipswspec.Text == "" || lbllipswspec.Text == "0") && (lblbaseswspec.Text == "" || lblbaseswspec.Text == "0") && (lblinterfaceswspec.Text == "" || lblinterfaceswspec.Text == "0") && (lblCenterswspec.Text == "" || lblCenterswspec.Text == "0") && (lbltreadswspec.Text == "" || lbltreadswspec.Text == "0"))
                    lblErrMsg.Text = "Sheet width spec is empty or zero. please enter in mould master";
                else if ((lblGTOD.Text == "" || lblGTOD.Text == "0"))
                    lblErrMsg.Text = "GT OD is empty or zero. please enter in mould master";
                else if ((lblGDWidth.Text == "" || lblGDWidth.Text == "0"))
                    lblErrMsg.Text = "GT width is empty or zero. please enter in mould master";
            }
            catch (Exception ex)
            {
                Program.WriteToGtErrorLog("GT", "frmGtBuild", MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
            dtpPrepareDate.MinDate = Convert.ToDateTime(lblDate.Text);
        }
        private void listBox1_Click(object sender, EventArgs e)
        {
            try
            {
                if (listBox1.Items.Count > 0)
                {
                    txtStencilno.Text = listBox1.SelectedValue.ToString();
                    listBox1.SetSelected(listBox1.SelectedIndex, true);
                    listBox1.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                Program.WriteToGtErrorLog("GT", "frmGtBuild", System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        private void update_stencil(string value)
        {
            try
            {
                SqlParameter[] sp = new SqlParameter[]{
                    new SqlParameter("@StencilNo",txtStencilno.Text)
                };
                dbCon.ExecuteNonQuery_SP("SP_UPD_GreenWt_GTStatus", sp);
            }
            catch (Exception ex)
            {
                Program.WriteToGtErrorLog("GT", "frmGtBuild", System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        private void txtGTOD_TextChanged(object sender, EventArgs e)
        {
            try
            {
                lblErrMsg.Text = "";
                lblStatusGTOD.Text = "NOT OK";
                lblStatusGTOD.ForeColor = Color.Red;
                if (lblGTOD.Text != "" && txtGTOD.Text != "" && Convert.ToDecimal(txtGTOD.Text) > 0)
                {
                    if (((Convert.ToDecimal(lblGTOD.Text) - 10) <= Convert.ToDecimal(txtGTOD.Text)) && ((Convert.ToDecimal(lblGTOD.Text) + 10) >= Convert.ToDecimal(txtGTOD.Text)))
                    {

                        lblStatusGTOD.Text = "OK";
                        lblStatusGTOD.ForeColor = Color.DarkGreen;

                    }
                }
            }
            catch (Exception ex)
            {
                Program.WriteToGtErrorLog("GT", "frmGtBuild", System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        private void Save_Visible()
        {
            try
            {
                if (lblStatusActualGt.Text == "OK" && lblStatusGTOD.Text == "OK" && lblStatusGTWidth.Text == "OK" )
                    btnsave.Visible = true;
                else
                    btnsave.Visible = false;
            }
            catch (Exception ex)
            {
                Program.WriteToGtErrorLog("GT", "frmGtBuild", System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        private void txtGDWidth_TextChanged(object sender, EventArgs e)
        {
            try
            {
                lblErrMsg.Text = "";
                lblStatusGTWidth.Text = "NOT OK";
                lblStatusGTWidth.ForeColor = Color.Red;
                if (lblGDWidth.Text != "" && txtGDWidth.Text != "" && Convert.ToDecimal(txtGDWidth.Text) > 0)
                {
                    //if (Convert.ToDecimal(lblGDWidth.Text) > Convert.ToDecimal(txtGDWidth.Text) &&
                    //    (Convert.ToDecimal(lblGDWidth.Text) - Convert.ToDecimal(50)) < Convert.ToDecimal(txtGDWidth.Text))
                    if (((Convert.ToDecimal(lblGDWidth.Text) - 10) <= Convert.ToDecimal(txtGDWidth.Text)) && ((Convert.ToDecimal(lblGDWidth.Text) + 10) >= Convert.ToDecimal(txtGDWidth.Text)))
                    {
                        lblStatusGTWidth.Text = "OK";
                        lblStatusGTWidth.ForeColor = Color.DarkGreen;
                    }
                    Save_Visible();
                }
            }
            catch (Exception ex)
            {
                Program.WriteToGtErrorLog("GT", "frmGtBuild", System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        //private void btnBandWt_Click(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        tmrBandWtNew.Stop();
        //        txtMachineWt.Text = "0.000";
        //        btnBandWt.Visible = false;
        //        lblAdjuHbw.Text = (Convert.ToDecimal(lblhbwweighed.Text)).ToString("0.000");
        //        lblAdjuTot.Text = Convert.ToDecimal(Convert.ToDecimal(lblAdjuHbw.Text) + Convert.ToDecimal(lblAdjuBase.Text) + Convert.ToDecimal(lblAdjuInter.Text) +
        //           Convert.ToDecimal(lblAdjuCenter.Text) + Convert.ToDecimal(lblAdjuTread.Text)).ToString("0.000");
        //        lblActualTotal.Text = Convert.ToDecimal(Convert.ToDecimal(lblhbwweighed.Text) + Convert.ToDecimal(lblbaseweighed.Text) + Convert.ToDecimal(lblinterfaceweighed.Text) + Convert.ToDecimal(lblcenterweighed.Text) + Convert.ToDecimal(lbltreadweighed.Text)).ToString();
        //        tmrActWtNew.Start();
        //    }
        //    catch (Exception ex)
        //    {
        //        Program.WriteToGtErrorLog("GT", "frmGtBuild", System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
        //    }
        //}
        private void btnBandWt_Click(object sender, EventArgs e)
        {
            try
            {
                tmrBandWtNew.Stop();
                txtMachineWt.Text = "0.000";
                btnBandWt.Visible = false;
                if (label10.Text == "BAND + CG")
                {
                    decimal checkvalue = (Convert.ToDecimal(lblhbwspec.Text) - Convert.ToDecimal(lblhbwweighed.Text)) * Convert.ToDecimal(0.15);
                    if (checkvalue != 0)
                    {
                        lblAdjuHbw.Text = (Convert.ToDecimal(lblhbwweighed.Text)).ToString("0.000");

                        lblAdjuTot.Text = Convert.ToDecimal(Convert.ToDecimal(lblAdjuHbw.Text) + Convert.ToDecimal(lblAdjuBase.Text) + Convert.ToDecimal(lblAdjuInter.Text) +
                         Convert.ToDecimal(lblAdjuCenter.Text) + Convert.ToDecimal(lblAdjuTread.Text)).ToString("0.000");
                        lblActualTotal.Text = Convert.ToDecimal(Convert.ToDecimal(lblhbwweighed.Text) + Convert.ToDecimal(lblbaseweighed.Text) + Convert.ToDecimal(lblinterfaceweighed.Text) + Convert.ToDecimal(lblcenterweighed.Text) + Convert.ToDecimal(lbltreadweighed.Text) + checkvalue).ToString("0.000");
                        tmrActWtNew.Start();

                        //        lbltreadweighed.Text = (Convert.ToDecimal(lbltreadweighed.Text) + checkvalue).ToString("0.000");
                    }
                    else
                    {
                        lblAdjuHbw.Text = (Convert.ToDecimal(lblhbwweighed.Text)).ToString("0.000");
                        lblAdjuTot.Text = Convert.ToDecimal(Convert.ToDecimal(lblAdjuHbw.Text) + Convert.ToDecimal(lblAdjuBase.Text) + Convert.ToDecimal(lblAdjuInter.Text) +
                           Convert.ToDecimal(lblAdjuCenter.Text) + Convert.ToDecimal(lblAdjuTread.Text)).ToString("0.000");
                        lblActualTotal.Text = Convert.ToDecimal(Convert.ToDecimal(lblhbwweighed.Text) + Convert.ToDecimal(lblbaseweighed.Text) + Convert.ToDecimal(lblinterfaceweighed.Text) + Convert.ToDecimal(lblcenterweighed.Text) + Convert.ToDecimal(lbltreadweighed.Text)).ToString();
                        tmrActWtNew.Start();
                    }








                }
                else
                {
                    lblAdjuHbw.Text = (Convert.ToDecimal(lblhbwweighed.Text)).ToString("0.000");
                    lblAdjuTot.Text = Convert.ToDecimal(Convert.ToDecimal(lblAdjuHbw.Text) + Convert.ToDecimal(lblAdjuBase.Text) + Convert.ToDecimal(lblAdjuInter.Text) +
                       Convert.ToDecimal(lblAdjuCenter.Text) + Convert.ToDecimal(lblAdjuTread.Text)).ToString("0.000");
                    lblActualTotal.Text = Convert.ToDecimal(Convert.ToDecimal(lblhbwweighed.Text) + Convert.ToDecimal(lblbaseweighed.Text) + Convert.ToDecimal(lblinterfaceweighed.Text) + Convert.ToDecimal(lblcenterweighed.Text) + Convert.ToDecimal(lbltreadweighed.Text)).ToString();
                    tmrActWtNew.Start();
                }
            }
            catch (Exception ex)
            {
                Program.WriteToGtErrorLog("GT", "frmGtBuild", System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        //private void tmrActWtNew_Tick(object sender, EventArgs e)
        //{
        //    lblErrMsg.Text = "";
        //    decimal strVal = 0;
        //    if (lblPageTitle.Text == "GT BUILD - MANUAL")
        //        strVal = txtManualValue.Text != "" ? Convert.ToDecimal(txtManualValue.Text) : 0;
        //    else
        //    {
        //        if (strCommunication != "COM4")
        //        {
        //            TcpClient tcpClient = new TcpClient();
        //            try
        //            {
        //                IPAddress iPAddress = System.Net.IPAddress.Parse("192.168.5." + Convert.ToInt32(strCommunication));
        //                int tcpPort = 502;
        //                tcpClient = new TcpClient(iPAddress.ToString(), tcpPort);
        //                tcpClient.ReceiveTimeout = 1000;

        //                if (!tcpClient.Connected)
        //                    lblErrMsg.Text = "ETHERNET NOT CONNECTED";
        //                else
        //                {
        //                    try
        //                    {
        //                        IModbusSerialMaster master1 = ModbusSerialMaster.CreateRtu(tcpClient);

        //                        byte slaveID = 1;
        //                        ushort startAddress = 1000;
        //                        ushort numRegisters = 2;
        //                        ushort[] registers1 = master1.ReadHoldingRegisters(slaveID, startAddress, numRegisters);
        //                        uint value1 = (ModbusUtility.GetUInt32(registers1[0], registers1[1]));
        //                        int readPow = unchecked((Int32)value1);

        //                        int intDivide = 10;
        //                        if (readPow > 0)
        //                            intDivide = Convert.ToInt32(Math.Pow(10, readPow));
        //                        IModbusSerialMaster master = ModbusSerialMaster.CreateRtu(tcpClient);

        //                        startAddress = 1002;
        //                        ushort[] registers = master.ReadHoldingRegisters(slaveID, startAddress, numRegisters);
        //                        uint value = (ModbusUtility.GetUInt32(registers[0], registers[1]));
        //                        int readVal = unchecked((Int32)value);

        //                        strVal = Convert.ToDecimal(readVal) / Convert.ToInt32(intDivide);
        //                        master1.Dispose();
        //                        master.Dispose();
        //                    }
        //                    catch (TimeoutException exp)
        //                    {
        //                        lblErrMsg.Text = exp.Message.ToUpper();
        //                    }
        //                }
        //            }
        //            catch (Exception ex)
        //            {
        //                lblErrMsg.Text = ex.Message.ToUpper();
        //            }
        //            tcpClient.Close();
        //        }



        //        else
        //        {
        //            try
        //            {

        //                //    SerialPort port = new SerialPort("COM4");
        //                //    try
        //                //    {
        //                //        if (lblSpecTotal.Text != "")
        //                //        {
        //                //            port.BaudRate = 9600;
        //                //            port.DataBits = 8;
        //                //            port.Parity = Parity.None;
        //                //            port.StopBits = StopBits.One;
        //                //            port.ReadTimeout = 300;
        //                //            port.WriteTimeout = 300;
        //                //            port.Open();

        //                //            if (!port.IsOpen)
        //                //                lblErrMsg.Text = "PORT NOT CONNECTED";
        //                //            else
        //                //            {
        //                //                try
        //                //                {
        //                //                    if (Program.strPlantName != "SITL")
        //                //                    {
        //                //                        if (Convert.ToInt32(lblGTOD.Text) < 900 || Convert.ToDecimal(lblActualTotal.Text) < 125 || Program.strPlantName == "PDK")
        //                //                        {
        //                //                            IModbusSerialMaster master = ModbusSerialMaster.CreateRtu(port);

        //                //                            byte slaveId = 1;
        //                //                            ushort startAddress = 1002;
        //                //                            ushort numRegisters = 2;

        //                //                            ushort[] registers = master.ReadHoldingRegisters(slaveId, startAddress, numRegisters);
        //                //                            uint value = (ModbusUtility.GetUInt32(registers[0], registers[1]));
        //                //                            int readVal = unchecked((Int32)value);

        //                //                            strVal = Convert.ToDecimal(value) / Convert.ToInt32(1000);
        //                //                        }
        //                //                        else if (Convert.ToInt32(lblGTOD.Text) >= 900 && Program.strPlantName == "MMN")
        //                //                        {
        //                //                            IModbusSerialMaster master = ModbusSerialMaster.CreateRtu(port);

        //                //                            byte slaveId = 1;
        //                //                            ushort startAddress = 1002;
        //                //                            ushort numRegisters = 2;

        //                //                            ushort[] registers = master.ReadHoldingRegisters(slaveId, startAddress, numRegisters);
        //                //                            uint value = (ModbusUtility.GetUInt32(registers[0], registers[1]));
        //                //                            int readVal = unchecked((Int32)value);

        //                //                            strVal = Convert.ToDecimal(value) / Convert.ToInt32(10);

        //                //                        }

        //                //                        else
        //                //                        {
        //                //                            IModbusSerialMaster master = ModbusSerialMaster.CreateRtu(port);

        //                //                            byte slaveId = 2;
        //                //                            ushort startAddress = 1002;
        //                //                            ushort numRegisters = 2;

        //                //                            ushort[] registers = master.ReadHoldingRegisters(slaveId, startAddress, numRegisters);
        //                //                            uint value = (ModbusUtility.GetUInt32(registers[0], registers[1]));
        //                //                            int readVal = unchecked((Int32)value);

        //                //                            strVal = Convert.ToDecimal(value);
        //                //                            master.Dispose();
        //                //                        }
        //                //                    }
        //                //                    else
        //                //                    {
        //                //                        string strRtxt = port.ReadLine();
        //                //                        if (strRtxt.Length > 0)
        //                //                        {
        //                //                            string numberOnly = Regex.Replace(strRtxt, "[^0-9.]", "");
        //                //                            if (numberOnly.ToString() != "")
        //                //                                strVal = Convert.ToDecimal(numberOnly);
        //                //                        }
        //                //                    }
        //                //                }
        //                //                catch (TimeoutException exp)
        //                //                {
        //                //                    lblErrMsg.Text = exp.Message.ToUpper();
        //                //                }
        //                //            }
        //                //        }
        //                // }
        //                //catch (Exception ex)
        //                //{
        //                //    lblErrMsg.Text = ex.Message.ToUpper();
        //                //}
        //                //port.Close();
        //                // }
        //                //}
        //                //{

        //                //if (Convert.ToInt32(lblGTOD.Text) < 900 || Convert.ToDecimal(lblActualTotal.Text) < 125 || Program.strPlantName == "PDK")
        //                //{
        //                //    //HttpWebRequest request = (HttpWebRequest)WebRequest.Create(@"http://192.168.8.103:4000/sc");
        //                //    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(@"http://localhost:4000/sc");
        //                //    HttpWebResponse response = (HttpWebResponse)request.GetResponse();
        //                //    string content = new StreamReader(response.GetResponseStream()).ReadToEnd();
        //                //    string left_content = content.Substring(0, 26);
        //                //    int fetch_length = (((content.Length) - 3) - (left_content.Length));//left_content.Length;
        //                //    content = content.Substring(26, fetch_length);
        //                //    //string left_content= con
        //                //    strVal = Convert.ToDecimal(content) / Convert.ToInt32(1000);

        //                //}
        //                //else if (Convert.ToInt32(lblGTOD.Text) >= 900 && Program.strPlantName == "MMN")



        //                //HttpWebRequest request = (HttpWebRequest)WebRequest.Create(@"http://192.168.8.103:4000/sc");
        //                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(@"http://localhost:4000/sc");
        //                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
        //                string content = new StreamReader(response.GetResponseStream()).ReadToEnd();
        //                string left_content = content.Substring(0, 26);
        //                int fetch_length = (((content.Length) - 3) - (left_content.Length));//left_content.Length;
        //                content = content.Substring(26, fetch_length);
        //                //string left_content= con
        //                //strVal = Convert.ToDecimal(content) / Convert.ToInt32(10);
        //                strVal = Convert.ToDecimal(content);
        //            }


        //            //if (strVal.ToString() != "")
        //            //{
        //            //   // if (lblAddWt.Text == "0")
        //            //    {
        //            //        //lblM.Text = (Convert.ToDecimal(strVal).ToString("0.00"));
        //            //        txtMachineWt.Text = (Convert.ToDecimal(strVal).ToString("0.00"));
        //            //    }
        //            //    //else
        //            //    //{
        //            //    //    lblM.Text = (Convert.ToDecimal(lblAddWt.Text) + (Convert.ToDecimal(strVal))).ToString("0.00");
        //            //    //    txtMachineWt.Text = (Convert.ToDecimal(strVal).ToString("0.00"));
        //            //    //}
        //            //}
        //            //else
        //            //    txtMachineWt.Text = "0.000";


        //            catch (TimeoutException exp)
        //            {
        //                lblErrMsg.Text = exp.Message.ToUpper();
        //            }
        //        }
        //    }
        



        //        if (strVal.ToString() != "")
        //        {
        //            txtMachineWt.Text = (strVal).ToString("0.000");

        //            lblErrMsg.Text = "";
        //            lblStatusActualGt.Text = "NOT OK";
        //            lblStatusActualGt.ForeColor = Color.Red;
        //            bool boolWtValue = false;
        //            if (lblF8Usage.Text != "" || lblRimWidth.Text == "0" || (lbltyresize.Text == "100/80-8MOULDON" && lblRimWidth.Text == "2.50") || (lbltyresize.Text == "100/80-8EDC703" && lblRimWidth.Text == "2.50") || ((lbltyresize.Text == "100/80-8EDC701" && lblRimWidth.Text == "2.50")))
        //            {
        //                if ((Convert.ToDecimal(lblActualTotal.Text) <= Convert.ToDecimal(txtMachineWt.Text) &&
        //                    (Convert.ToDecimal(lblActualTotal.Text) + Convert.ToDecimal(0.060)) >= Convert.ToDecimal(txtMachineWt.Text)))
        //                    boolWtValue = true;
        //            }
        //            else if (lblF8Usage.Text == "")
        //            {
        //                if (Convert.ToDecimal(lblSpecTotal.Text) < Convert.ToDecimal(lblActualTotal.Text))
        //                {
        //                    if (Convert.ToDecimal(txtMachineWt.Text) == Convert.ToDecimal(lblActualTotal.Text) ||
        //                        Convert.ToDecimal(lblSpecTotal.Text) == Convert.ToDecimal(txtMachineWt.Text) ||
        //                        (Convert.ToDecimal(txtMachineWt.Text) < Convert.ToDecimal(lblActualTotal.Text)) &&
        //                        (Convert.ToDecimal(lblSpecTotal.Text) < Convert.ToDecimal(txtMachineWt.Text)))
        //                        boolWtValue = true;
        //                }
        //                else if (Convert.ToDecimal(lblSpecTotal.Text) > Convert.ToDecimal(lblActualTotal.Text))
        //                {
        //                    if (Convert.ToDecimal(txtMachineWt.Text) == Convert.ToDecimal(lblActualTotal.Text) ||
        //                        Convert.ToDecimal(lblSpecTotal.Text) == Convert.ToDecimal(txtMachineWt.Text) ||
        //                        (Convert.ToDecimal(txtMachineWt.Text) > Convert.ToDecimal(lblActualTotal.Text)) &&
        //                        (Convert.ToDecimal(lblSpecTotal.Text) > Convert.ToDecimal(txtMachineWt.Text)))
        //                        boolWtValue = true;
        //                }
        //                else
        //                {
        //                    if (Convert.ToDecimal(txtMachineWt.Text) == Convert.ToDecimal(lblActualTotal.Text) ||
        //                        Convert.ToDecimal(lblSpecTotal.Text) == Convert.ToDecimal(txtMachineWt.Text))
        //                        boolWtValue = true;
        //                }
        //            }
        //            if (boolWtValue)
        //            {
        //                lblStatusActualGt.Text = "OK";
        //                lblStatusActualGt.ForeColor = Color.DarkGreen;
        //            }
        //            Save_Visible();
        //        }
        //        else
        //            txtMachineWt.Text = "0.000";
        //    }

        private void tmrActWtNew_Tick(object sender, EventArgs e)
        {
            lblErrMsg.Text = "";
            decimal strVal = 0;
            if (lblPageTitle.Text == "GT BUILD - MANUAL")
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

                    try
                    {
                        try
                        {

                            //    SerialPort port = new SerialPort("COM4");
                            //    try
                            //    {
                            //        if (lblSpecTotal.Text != "")
                            //        {
                            //            port.BaudRate = 9600;
                            //            port.DataBits = 8;
                            //            port.Parity = Parity.None;
                            //            port.StopBits = StopBits.One;
                            //            port.ReadTimeout = 300;
                            //            port.WriteTimeout = 300;
                            //            port.Open();

                            //            if (!port.IsOpen)
                            //                lblErrMsg.Text = "PORT NOT CONNECTED";
                            //            else
                            //            {
                            //                try
                            //                {
                            //                    if (Program.strPlantName != "SITL")
                            //                    {
                            //                        if (Convert.ToInt32(lblGTOD.Text) < 900 || Convert.ToDecimal(lblActualTotal.Text) < 125 || Program.strPlantName == "PDK")
                            //                        {
                            //                            IModbusSerialMaster master = ModbusSerialMaster.CreateRtu(port);

                            //                            byte slaveId = 1;
                            //                            ushort startAddress = 1002;
                            //                            ushort numRegisters = 2;

                            //                            ushort[] registers = master.ReadHoldingRegisters(slaveId, startAddress, numRegisters);
                            //                            uint value = (ModbusUtility.GetUInt32(registers[0], registers[1]));
                            //                            int readVal = unchecked((Int32)value);

                            //                            strVal = Convert.ToDecimal(value) / Convert.ToInt32(1000);
                            //                        }
                            //                        else if (Convert.ToInt32(lblGTOD.Text) >= 900 && Program.strPlantName == "MMN")
                            //                        {
                            //                            IModbusSerialMaster master = ModbusSerialMaster.CreateRtu(port);

                            //                            byte slaveId = 1;
                            //                            ushort startAddress = 1002;
                            //                            ushort numRegisters = 2;

                            //                            ushort[] registers = master.ReadHoldingRegisters(slaveId, startAddress, numRegisters);
                            //                            uint value = (ModbusUtility.GetUInt32(registers[0], registers[1]));
                            //                            int readVal = unchecked((Int32)value);

                            //                            strVal = Convert.ToDecimal(value) / Convert.ToInt32(10);

                            //                        }

                            //                        else
                            //                        {
                            //                            IModbusSerialMaster master = ModbusSerialMaster.CreateRtu(port);

                            //                            byte slaveId = 2;
                            //                            ushort startAddress = 1002;
                            //                            ushort numRegisters = 2;

                            //                            ushort[] registers = master.ReadHoldingRegisters(slaveId, startAddress, numRegisters);
                            //                            uint value = (ModbusUtility.GetUInt32(registers[0], registers[1]));
                            //                            int readVal = unchecked((Int32)value);

                            //                            strVal = Convert.ToDecimal(value);
                            //                            master.Dispose();
                            //                        }
                            //                    }
                            //                    else
                            //                    {
                            //                        string strRtxt = port.ReadLine();
                            //                        if (strRtxt.Length > 0)
                            //                        {
                            //                            string numberOnly = Regex.Replace(strRtxt, "[^0-9.]", "");
                            //                            if (numberOnly.ToString() != "")
                            //                                strVal = Convert.ToDecimal(numberOnly);
                            //                        }
                            //                    }
                            //                }
                            //                catch (TimeoutException exp)
                            //                {
                            //                    lblErrMsg.Text = exp.Message.ToUpper();
                            //                }
                            //            }
                            //        }
                            // }
                            //catch (Exception ex)
                            //{
                            //    lblErrMsg.Text = ex.Message.ToUpper();
                            //}
                            //port.Close();
                            // }
                            //}
                            //{

                            //if (Convert.ToInt32(lblGTOD.Text) < 900 || Convert.ToDecimal(lblActualTotal.Text) < 125 || Program.strPlantName == "PDK")
                            //{
                            //    //HttpWebRequest request = (HttpWebRequest)WebRequest.Create(@"http://192.168.8.103:4000/sc");
                            //    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(@"http://localhost:4000/sc");
                            //    HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                            //    string content = new StreamReader(response.GetResponseStream()).ReadToEnd();
                            //    string left_content = content.Substring(0, 26);
                            //    int fetch_length = (((content.Length) - 3) - (left_content.Length));//left_content.Length;
                            //    content = content.Substring(26, fetch_length);
                            //    //string left_content= con
                            //    strVal = Convert.ToDecimal(content) / Convert.ToInt32(1000);

                            //}
                            //else if (Convert.ToInt32(lblGTOD.Text) >= 900 && Program.strPlantName == "MMN")



                            //HttpWebRequest request = (HttpWebRequest)WebRequest.Create(@"http://192.168.8.103:4000/sc");
                            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(@"http://localhost:4000/sc");
                            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                            string content = new StreamReader(response.GetResponseStream()).ReadToEnd();
                            string left_content = content.Substring(0, 26);
                            int fetch_length = (((content.Length) - 3) - (left_content.Length));//left_content.Length;
                            content = content.Substring(26, fetch_length);
                            //string left_content= con
                            //strVal = Convert.ToDecimal(content) / Convert.ToInt32(10);
                            strVal = Convert.ToDecimal(content);
                        }


                        //if (strVal.ToString() != "")
                        //{
                        //   // if (lblAddWt.Text == "0")
                        //    {
                        //        //lblM.Text = (Convert.ToDecimal(strVal).ToString("0.00"));
                        //        txtMachineWt.Text = (Convert.ToDecimal(strVal).ToString("0.00"));
                        //    }
                        //    //else
                        //    //{
                        //    //    lblM.Text = (Convert.ToDecimal(lblAddWt.Text) + (Convert.ToDecimal(strVal))).ToString("0.00");
                        //    //    txtMachineWt.Text = (Convert.ToDecimal(strVal).ToString("0.00"));
                        //    //}
                        //}
                        //else
                        //    txtMachineWt.Text = "0.000";


                        catch (TimeoutException exp)
                        {
                            lblErrMsg.Text = exp.Message.ToUpper();
                        }
                    }
                    catch (Exception ex)
                    {
                        lblErrMsg.Text = ex.Message.ToUpper();
                    }
                }
            }




            if (strVal.ToString() != "")
            {
                txtMachineWt.Text = (strVal).ToString("0.000");

                lblErrMsg.Text = "";
                lblStatusActualGt.Text = "NOT OK";
                lblStatusActualGt.ForeColor = Color.Red;
                bool boolWtValue = false;
                if (lblF8Usage.Text != "" || lblRimWidth.Text == "0" || (lbltyresize.Text == "100/80-8MOULDON" && lblRimWidth.Text == "2.50") || (lbltyresize.Text == "100/80-8EDC703" && lblRimWidth.Text == "2.50") || ((lbltyresize.Text == "100/80-8EDC701" && lblRimWidth.Text == "2.50")))
                {
                    if ((Convert.ToDecimal(lblActualTotal.Text) <= Convert.ToDecimal(txtMachineWt.Text) &&
                        (Convert.ToDecimal(lblActualTotal.Text) + Convert.ToDecimal(0.060)) >= Convert.ToDecimal(txtMachineWt.Text)))
                        boolWtValue = true;
                }
                else if (lblF8Usage.Text == "")
                {
                    if (Convert.ToDecimal(lblSpecTotal.Text) < Convert.ToDecimal(lblActualTotal.Text))
                    {
                        if (Convert.ToDecimal(txtMachineWt.Text) == Convert.ToDecimal(lblActualTotal.Text) ||
                            Convert.ToDecimal(lblSpecTotal.Text) == Convert.ToDecimal(txtMachineWt.Text) ||
                            (Convert.ToDecimal(txtMachineWt.Text) < Convert.ToDecimal(lblActualTotal.Text)) &&
                            (Convert.ToDecimal(lblSpecTotal.Text) < Convert.ToDecimal(txtMachineWt.Text)))
                            boolWtValue = true;
                    }
                    else if (Convert.ToDecimal(lblSpecTotal.Text) > Convert.ToDecimal(lblActualTotal.Text))
                    {
                        if (Convert.ToDecimal(txtMachineWt.Text) == Convert.ToDecimal(lblActualTotal.Text) ||
                            Convert.ToDecimal(lblSpecTotal.Text) == Convert.ToDecimal(txtMachineWt.Text) ||
                            (Convert.ToDecimal(txtMachineWt.Text) > Convert.ToDecimal(lblActualTotal.Text)) &&
                            (Convert.ToDecimal(lblSpecTotal.Text) > Convert.ToDecimal(txtMachineWt.Text)))
                            boolWtValue = true;
                    }
                    else
                    {
                        if (Convert.ToDecimal(txtMachineWt.Text) == Convert.ToDecimal(lblActualTotal.Text) ||
                            Convert.ToDecimal(lblSpecTotal.Text) == Convert.ToDecimal(txtMachineWt.Text))
                            boolWtValue = true;
                    }
                }
                if (boolWtValue)
                {
                    lblStatusActualGt.Text = "OK";
                    lblStatusActualGt.ForeColor = Color.DarkGreen;
                }
                Save_Visible();
            }
            else
                txtMachineWt.Text = "0.000";
        }
        
        private void tmrBandWtNew_Tick(object sender, EventArgs e)
        {
            if (label10.Text == "BAND + CG")
            {
                lblErrMsg.Text = "";
                decimal strVal = 0;
                if (lblPageTitle.Text == "GT BUILD - MANUAL")
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
                        //SerialPort port = new SerialPort("COM4");
                        try
                        {
                            //port.BaudRate = 9600;
                            //port.DataBits = 8;
                            //port.Parity = Parity.None;
                            //port.StopBits = StopBits.One;
                            //port.ReadTimeout = 300;
                            //port.WriteTimeout = 300;
                            //port.Open();

                            ////if (!port.IsOpen)
                                //lblErrMsg.Text = "PORT NOT CONNECTED";
                            
                            {
                                try
                                {
                                    if (Program.strPlantName != "SITL" && Program.strPlantName != "SLTL")
                                    {
                                        //IModbusSerialMaster master = ModbusSerialMaster.CreateRtu();

                                        //byte slaveId = 1;
                                        //ushort startAddress = 1002;
                                        //ushort numRegisters = 2;

                                        //ushort[] registers = master.ReadHoldingRegisters(slaveId, startAddress, numRegisters);
                                        //uint value = (ModbusUtility.GetUInt32(registers[0], registers[1]));
                                        //int readVal = unchecked((Int32)value);

                                        //strVal = Convert.ToDecimal(readVal) / Convert.ToInt32(1000);
                                        //master.Dispose();
                                    }
                                    else
                                    {
                                        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(@"http://localhost:4000/sc");
                                        HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                                        string content = new StreamReader(response.GetResponseStream()).ReadToEnd();
                                        string left_content = content.Substring(0, 26);
                                        int fetch_length = (((content.Length) - 3) - (left_content.Length));//left_content.Length;
                                        content = content.Substring(26, fetch_length);
                                        //string left_content= con
                                        //strVal = Convert.ToDecimal(content) / Convert.ToInt32(10);
                                        strVal = Convert.ToDecimal(content);
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
                    lblhbwweighed.Text = (Convert.ToDecimal(strVal).ToString("0.000"));
                    txtMachineWt.Text = (Convert.ToDecimal(strVal).ToString("0.000"));

                    decimal strMinusWt = Convert.ToDecimal(lblhbwspec.Text) - (Convert.ToDecimal(lblhbwspec.Text) * Convert.ToDecimal(10) / 100);
                    decimal strPlusWt = Convert.ToDecimal(lblhbwspec.Text) + (Convert.ToDecimal(lblhbwspec.Text) * Convert.ToDecimal(10) / 100);
                    if ((strMinusWt < Convert.ToDecimal(lblhbwweighed.Text)) && (strPlusWt > Convert.ToDecimal(lblhbwweighed.Text)) || (Convert.ToDecimal(lblhbwweighed.Text) == Convert.ToDecimal(lblhbwspec.Text)))
                        btnBandWt.Visible = true;
                    else
                        btnBandWt.Visible = false;
                }
                else
                    txtMachineWt.Text = "0.000";
            }
        }
        private void cboBrand_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                cboSidewall.DataSource = null;
                cboSidewall.Items.Clear();
                if (cboBrand.SelectedIndex > 0)
                {
                    string strDefaultTyreSize = lbltyresize.Text.Substring(0, 1);
                    string strDefaultTyretype = lblType.Text.Substring(0, 2);
                    string strDefaultTyreRim = lblRimWidth.Text.Substring(0, 1);

                    List<string> lstSidewall = dtProcessIdDetails.AsEnumerable().Where(b => b.Field<string>("TyreType").StartsWith(strDefaultTyretype) &&
                            b.Field<string>("TyreSize").StartsWith(strDefaultTyreSize) && b.Field<string>("TyreRim").StartsWith(strDefaultTyreRim) &&
                            b.Field<string>("Brand").Equals(cboBrand.SelectedItem.ToString())).Select(A => A.Field<string>("Sidewall")).Distinct().ToList();
                    if (lstSidewall.Count > 0)
                    {
                        lstSidewall.Insert(0, "CHOOSE");
                        cboSidewall.DataSource = lstSidewall;
                        if (cboSidewall.Items.Count == 2)
                            cboSidewall.SelectedIndex = 1;
                    }
                }
            }
            catch (Exception ex)
            {
                Program.WriteToGtErrorLog("GT", "frmGtBuild", System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        private void btnReload_Click(object sender, EventArgs e)
        {
            Ctrl_Clear();
        }
        private void frmGtBuild_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.F8 && txtStencilno.Text == "")
                    MessageBox.Show("CHOOSE STENCIL");
                else
                {
                    if (e.KeyCode == Keys.F8 && Program.boolManualWeighing)
                    {
                        lblPageTitle.Text = lblPageTitle.Text == "GT BUILD" ? "GT BUILD - MANUAL" : "GT BUILD";
                        txtManualValue.Visible = lblPageTitle.Text == "GT BUILD" ? false : true;
                        txtManualValue.Text = "0";
                        txtManualValue.Focus();
                        dtpPrepareDate.Enabled = lblPageTitle.Text == "GT BUILD" ? false : true;
                        dtpPrepareDate.CustomFormat = String.Format(lblPageTitle.Text == "GT BUILD" ? "MMM/dd/yyyy" : "MMM/dd/yyyy hh:mm tt");
                        dtpPrepareDate.MinDate = Convert.ToDateTime(lblDate.Text);
                        dtpPrepareDate.MaxDate = DateTime.Now.AddDays(0);
                        dtpPrepareDate.Value = dtpPrepareDate.MaxDate;
                        if (lblPageTitle.Text == "GT BUILD")
                            Make_DtpDate();
                    }
                    else if (e.KeyCode == Keys.F8)
                        MessageBox.Show("MANUAL ENTRY PRIVILEGE DISABLED. CONTACT YOUR SUPERVISOR.", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                Program.WriteToGtErrorLog("GT", "frmGtBuild", System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        private void txtManualValue_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 13 && txtManualValue.Text.Length > 0)
            {
                if (btnBandWt.Visible)
                    btnBandWt.Focus();
                else if (btnsave.Visible)
                    btnsave.Focus();
                else if (txtGTOD.Text.Length == 0)
                    txtGTOD.Focus();
                else if (txtGDWidth.Text.Length == 0)
                    txtGDWidth.Focus();
                else if (txtgttemp.Text.Length == 0)
                    txtgttemp.Focus();
            }
            else
            {
                txtManualValue.Focus();
                txtManualValue.SelectionStart = txtManualValue.Text.Length;
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
                //Program.WriteToGtErrorLog("GT", "frmGtBuild", System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        private void dtpPrepareDate_ValueChanged(object sender, EventArgs e)
        {
            //DateTime dtpTime = Convert.ToDateTime(dtpPrepareDate.Value.ToShortTimeString());
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
                DateTime shift2Time = Convert.ToDateTime("07:06:00");
                if ((System.DateTime.Now.TimeOfDay > shift2Time.TimeOfDay) && (System.DateTime.Now.TimeOfDay <= shift1Time.TimeOfDay))
                    cboShift.SelectedIndex = 1;
                else if ((System.DateTime.Now.TimeOfDay > shift1Time.TimeOfDay) || (System.DateTime.Now.TimeOfDay < shift2Time.TimeOfDay))
                    cboShift.SelectedIndex = 2;
             



        }

        private void txtgttemp_TextChanged(object sender, EventArgs e)
        {
            try
            {
                lblErrMsg.Text = "";
                label28.Text = "NOT OK";
                label28.ForeColor = Color.Red;
                if (txtgttemp.Text != "" && txtgttemp.Text != "" && Convert.ToDecimal(txtgttemp.Text) > 0)
                {
                   // if ((59 < Convert.ToDecimal(txtgttemp.Text)) && (71 > Convert.ToDecimal(txtgttemp.Text)))
                    {
                        label28.Text = "ok";
                        label28.ForeColor = Color.Green;
                        //txtgttemp.Focus();
                    }
                }
            }
            catch (Exception ex)
            {
                Program.WriteToGtErrorLog("GT", "frmGtBuild", System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        
    }
}

            
            
               
            
                

            
        
        
        
    

            
        

        //private void txtgttemp_TextChanged(object sender, EventArgs e)
        //{
        //    try
        //    {


        //        if ((60 <= Convert.ToDecimal(txtgttemp.Text)) && (70 >= Convert.ToDecimal(txtgttemp.Text)))
        //            {
                       
        //            }
        //            else
        //            {
        //                lblErrMsg.Text = "Enter Proper GT Temperature";
        //                txtgttemp.Focus();
        //            }
                
        //    }
        //    catch (Exception ex)
        //    {
        //        Program.WriteToGtErrorLog("GT", "frmGtBuild", System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
        //    }
        //}

       

        
        //private void textBox1_GotFocus(object sender, EventArgs e)
        //{
        //    //SerialPort port = new SerialPort("COM4");
        //    //try
        //    //{
        //    //    port.BaudRate = 9600;
        //    //    port.DataBits = 8;
        //    //    port.Parity = Parity.None;
        //    //    port.StopBits = StopBits.One;
        //    //    port.ReadTimeout = 300;
        //    //    port.WriteTimeout = 300;
        //    //    port.Open();

        //    //    if (!port.IsOpen)
        //    //        lblErrMsg.Text = "PORT NOT CONNECTED";
        //    //    else
        //    //    {
        //    //        IModbusSerialMaster master = ModbusSerialMaster.CreateRtu(port);

        //    //        byte slaveId = 1;
        //    //        ushort startAddress = 0;
        //    //        ushort numRegisters = 1;

        //    //        ushort[] registers = master.ReadInputRegisters(slaveId, startAddress, numRegisters);
        //    //        int strVal = unchecked(registers[0]);

        //    //        if (strVal.ToString() != "")
        //    //            txtgttemp.Text = strVal.ToString();

        //    //        master.Dispose();
        //    //    }
        //    //}
        //    //catch (Exception ex)
        //    //{
        //    //    lblErrMsg.Text = ex.Message.ToUpper();
        //    //}
        //    //port.Close();
        //}
    

