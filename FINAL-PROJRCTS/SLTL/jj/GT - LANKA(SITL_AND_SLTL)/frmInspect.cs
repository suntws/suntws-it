using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Configuration;
using System.Reflection;
using System.Diagnostics;

namespace GT
{
    public partial class frmInspect : Form
    {
        struct structMouldOpen
        {
            public decimal IFrom;
            public decimal ITo;
            public string Grade;
        }
        List<structMouldOpen> lstMouldOpen = new List<structMouldOpen>();
        DBAccess dba = new DBAccess();
        bool stopPnlGrow = false;
        Label lblRGrade = new Label();
        public static string strImgMethod = "";
        public frmInspect()
        {
            InitializeComponent();
            //if (Process.GetProcessesByName(System.IO.Path.GetFileNameWithoutExtension(
            //    System.Reflection.Assembly.GetEntryAssembly().Location.Replace("GT.exe", "\\Debug test\\TrayApp.exe"))).Count() == 0)
            //{
            //    ProcessStartInfo startInfo = new ProcessStartInfo();
            //    startInfo.FileName = AppDomain.CurrentDomain.BaseDirectory + "\\Debug test\\TrayApp.exe";
            //    startInfo.Arguments = Program.strUserName;
            //    Process.Start(startInfo);
            //}
        }
        private void frmInspect_Load(object sender, EventArgs e)
        {
            try
            {
                lblDelayMsg.Text = "";
                bindCboObservation();
                bindEventHandler();
                pnlBase.Location = new Point(0, 0);
                lblStencilNo.Text = "";
                gvTyreInspection.DefaultCellStyle.SelectionBackColor = gvTyreInspection.DefaultCellStyle.BackColor;
                gvTyreInspection.DefaultCellStyle.SelectionForeColor = gvTyreInspection.DefaultCellStyle.ForeColor;
                bindGV("");
                pnlObservation.Visible = false;
                btnInspect.Enabled = false;
                gpDefectConfirmation.Visible = false;
                pnlCntrl.Visible = false;
                txtStencilNo.Text = (Program.strPlantName == "SLTL" ? "L" : Program.strPlantName == "MMN" ? "C" : Program.strPlantName.Substring(0, 1)) +
                    DateTime.Now.Year.ToString().Substring(2, 2) + DateTime.Now.Month.ToString("00");
                txtStencilNo.SelectionStart = txtStencilNo.Text.Length;
            }
            catch (Exception ex)
            {
                Program.WriteToGtErrorLog("GT", "frmInspect", MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        private void bindCboObservation()
        {
            try
            {
                DataTable dtObservation = (DataTable)dba.ExecuteReader_SP("SP_LST_TyreInspect_Observations", DBAccess.Return_Type.DataTable);
                if (dtObservation.Rows.Count > 0)
                {
                    var lstCodes = dtObservation.AsEnumerable().Select(n => n.Field<string>("DefectCode")).Distinct();
                    foreach (string code in lstCodes)
                    {
                        DataRow[] rowCollection = dtObservation.Select("DefectCode='" + code + "'");
                        DataTable dtObs = rowCollection.CopyToDataTable();
                        if (dtObs.Rows.Count > 0)
                        {
                            DataRow dr = dtObs.NewRow();
                            dr.ItemArray = new object[] { "", "CHOOSE", "" };
                            dtObs.Rows.InsertAt(dr, 0);
                            ComboBox cbo;
                            if (code == "D5" || code == "D6" || code == "D9")
                                cbo = (ComboBox)pnlObservation.Controls.Find("cbo" + code.Insert(1, "0") + "Obs", false)[0];
                            else
                                cbo = (ComboBox)pnlObservation.Controls.Find("cbo" + code + "Obs", false)[0];
                            cbo.DataSource = dtObs;
                            cbo.DisplayMember = "Observation";
                            cbo.ValueMember = "Grade";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Program.WriteToGtErrorLog("GT", "frmInspect", MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        private void bindEventHandler()
        {
            try
            {
                cboD05Obs.SelectedIndexChanged += new EventHandler(cboObservation_SelectedIndexChanged);
                cboD06Obs.SelectedIndexChanged += new EventHandler(cboObservation_SelectedIndexChanged);
                cboD09Obs.SelectedIndexChanged += new EventHandler(cboObservation_SelectedIndexChanged);
                cboD10Obs.SelectedIndexChanged += new EventHandler(cboObservation_SelectedIndexChanged);
                cboD11Obs.SelectedIndexChanged += new EventHandler(cboObservation_SelectedIndexChanged);
                cboD12Obs.SelectedIndexChanged += new EventHandler(cboObservation_SelectedIndexChanged);
                cboD15Obs.SelectedIndexChanged += new EventHandler(cboObservation_SelectedIndexChanged);
                cboD16Obs.SelectedIndexChanged += new EventHandler(cboObservation_SelectedIndexChanged);
                cboD21Obs.SelectedIndexChanged += new EventHandler(cboObservation_SelectedIndexChanged);
                cboD22Obs.SelectedIndexChanged += new EventHandler(cboObservation_SelectedIndexChanged);
                cboD23Obs.SelectedIndexChanged += new EventHandler(cboObservation_SelectedIndexChanged);
                cboD24Obs.SelectedIndexChanged += new EventHandler(cboObservation_SelectedIndexChanged);
                cboD25Obs.SelectedIndexChanged += new EventHandler(cboObservation_SelectedIndexChanged);
                cboD30Obs.SelectedIndexChanged += new EventHandler(cboObservation_SelectedIndexChanged);
                cboD31Obs.SelectedIndexChanged += new EventHandler(cboObservation_SelectedIndexChanged);

                rdoD01Yes.CheckedChanged += new EventHandler(rdoObservation_CheckedChanged);
                rdoD01No.CheckedChanged += new EventHandler(rdoObservation_CheckedChanged);
                rdoD02Yes.CheckedChanged += new EventHandler(rdoObservation_CheckedChanged);
                rdoD02No.CheckedChanged += new EventHandler(rdoObservation_CheckedChanged);
                rdoD03Yes.CheckedChanged += new EventHandler(rdoObservation_CheckedChanged);
                rdoD03No.CheckedChanged += new EventHandler(rdoObservation_CheckedChanged);
                rdoD07Yes.CheckedChanged += new EventHandler(rdoObservation_CheckedChanged);
                rdoD07No.CheckedChanged += new EventHandler(rdoObservation_CheckedChanged);
                rdoD08Yes.CheckedChanged += new EventHandler(rdoObservation_CheckedChanged);
                rdoD08No.CheckedChanged += new EventHandler(rdoObservation_CheckedChanged);
                rdoD13Yes.CheckedChanged += new EventHandler(rdoObservation_CheckedChanged);
                rdoD13No.CheckedChanged += new EventHandler(rdoObservation_CheckedChanged);
                rdoD14Yes.CheckedChanged += new EventHandler(rdoObservation_CheckedChanged);
                rdoD14No.CheckedChanged += new EventHandler(rdoObservation_CheckedChanged);
                rdoD17Yes.CheckedChanged += new EventHandler(rdoObservation_CheckedChanged);
                rdoD17No.CheckedChanged += new EventHandler(rdoObservation_CheckedChanged);
                rdoD18Yes.CheckedChanged += new EventHandler(rdoObservation_CheckedChanged);
                rdoD18No.CheckedChanged += new EventHandler(rdoObservation_CheckedChanged);
                rdoD19Yes.CheckedChanged += new EventHandler(rdoObservation_CheckedChanged);
                rdoD19No.CheckedChanged += new EventHandler(rdoObservation_CheckedChanged);
                rdoD20Yes.CheckedChanged += new EventHandler(rdoObservation_CheckedChanged);
                rdoD20No.CheckedChanged += new EventHandler(rdoObservation_CheckedChanged);
                rdoD26Yes.CheckedChanged += new EventHandler(rdoObservation_CheckedChanged);
                rdoD26No.CheckedChanged += new EventHandler(rdoObservation_CheckedChanged);
                rdoD27Yes.CheckedChanged += new EventHandler(rdoObservation_CheckedChanged);
                rdoD27No.CheckedChanged += new EventHandler(rdoObservation_CheckedChanged);
                rdoD28Yes.CheckedChanged += new EventHandler(rdoObservation_CheckedChanged);
                rdoD28No.CheckedChanged += new EventHandler(rdoObservation_CheckedChanged);
                rdoD29Yes.CheckedChanged += new EventHandler(rdoObservation_CheckedChanged);
                rdoD29No.CheckedChanged += new EventHandler(rdoObservation_CheckedChanged);
                rdoD32Yes.CheckedChanged += new EventHandler(rdoObservation_CheckedChanged);
                rdoD32No.CheckedChanged += new EventHandler(rdoObservation_CheckedChanged);
                rdoD33Yes.CheckedChanged += new EventHandler(rdoObservation_CheckedChanged);
                rdoD33No.CheckedChanged += new EventHandler(rdoObservation_CheckedChanged);

                // 3rd parameter is id part of the combo box.
                lblD01Grade.TextChanged += (sender, e) => lblGrade_TextChanged(sender, e, "D15");
                lblD02Grade.TextChanged += (sender, e) => lblGrade_TextChanged(sender, e, "");
                lblD03Grade.TextChanged += (sender, e) => lblGrade_TextChanged(sender, e, "D09");
                lblD04Grade.TextChanged += (sender, e) => lblGrade_TextChanged(sender, e, "D05");
                lblD05Grade.TextChanged += (sender, e) => lblGrade_TextChanged(sender, e, "D06");
                lblD06Grade.TextChanged += (sender, e) => lblGrade_TextChanged(sender, e, "");
                lblD08Grade.TextChanged += (sender, e) => lblGrade_TextChanged(sender, e, "D10");
                lblD09Grade.TextChanged += (sender, e) => lblGrade_TextChanged(sender, e, "D11");
                lblD10Grade.TextChanged += (sender, e) => lblGrade_TextChanged(sender, e, "D12");
                lblD11Grade.TextChanged += (sender, e) => lblGrade_TextChanged(sender, e, "");
                lblD12Grade.TextChanged += (sender, e) => lblGrade_TextChanged(sender, e, "");
                lblD13Grade.TextChanged += (sender, e) => lblGrade_TextChanged(sender, e, "");
                lblD14Grade.TextChanged += (sender, e) => lblGrade_TextChanged(sender, e, "");
                lblD15Grade.TextChanged += (sender, e) => lblGrade_TextChanged(sender, e, "D16");
                lblD16Grade.TextChanged += (sender, e) => lblGrade_TextChanged(sender, e, "");
                lblD17Grade.TextChanged += (sender, e) => lblGrade_TextChanged(sender, e, "");
                lblD18Grade.TextChanged += (sender, e) => lblGrade_TextChanged(sender, e, "");
                lblD19Grade.TextChanged += (sender, e) => lblGrade_TextChanged(sender, e, "");
                lblD20Grade.TextChanged += (sender, e) => lblGrade_TextChanged(sender, e, "D21");
                lblD21Grade.TextChanged += (sender, e) => lblGrade_TextChanged(sender, e, "D22");
                lblD22Grade.TextChanged += (sender, e) => lblGrade_TextChanged(sender, e, "D23");
                lblD23Grade.TextChanged += (sender, e) => lblGrade_TextChanged(sender, e, "D24");
                lblD24Grade.TextChanged += (sender, e) => lblGrade_TextChanged(sender, e, "D25");
                lblD25Grade.TextChanged += (sender, e) => lblGrade_TextChanged(sender, e, "");
                lblD26Grade.TextChanged += (sender, e) => lblGrade_TextChanged(sender, e, "");
                lblD27Grade.TextChanged += (sender, e) => lblGrade_TextChanged(sender, e, "");
                lblD28Grade.TextChanged += (sender, e) => lblGrade_TextChanged(sender, e, "");
                lblD29Grade.TextChanged += (sender, e) => lblGrade_TextChanged(sender, e, "");
                lblD30Grade.TextChanged += (sender, e) => lblGrade_TextChanged(sender, e, "");
                lblD31Grade.TextChanged += (sender, e) => lblGrade_TextChanged(sender, e, "");
                lblD32Grade.TextChanged += (sender, e) => lblGrade_TextChanged(sender, e, "");
                lblD33Grade.TextChanged += (sender, e) => lblGrade_TextChanged(sender, e, "");
                txtD4MouldOpen.KeyPress += new KeyPressEventHandler(txtNumericPercentage_KeyPress);
                txtD4MouldOpen.LostFocus += new EventHandler(txtD4MouldOpen_TextChanged);
            }
            catch (Exception ex)
            {
                Program.WriteToGtErrorLog("GT", "frmInspect", MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        void cboObservation_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                string itemName = ((ComboBox)sender).Name;
                Label lbl = (Label)pnlObservation.Controls.Find("lbl" + itemName.Substring(3, 3) + "Grade", false)[0];
                if (((ComboBox)sender).SelectedIndex > 0)
                {
                    lbl.Text = ((ComboBox)sender).SelectedValue.ToString();
                    if (lbl.Text == "R") lbl.ForeColor = Color.Red;
                    else lbl.ForeColor = Color.Black;
                }
                else
                    lbl.Text = "";
            }
            catch (Exception ex)
            {
                Program.WriteToGtErrorLog("GT", "frmInspect", MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        void rdoObservation_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                string strGrade = "";
                if (rdoGradeAccept_Aplus.Checked) // Accept A+
                    strGrade = "A+";
                else if (rdoGradeReject.Checked) // Reject
                    strGrade = "A+";
                string itemName = ((RadioButton)sender).Name;
                Label lbl = (Label)pnlObservation.Controls.Find("lbl" + itemName.Substring(3, 3) + "Grade", false)[0];
                if (((RadioButton)sender).Name.Length == 9)// name for radio "yes" has 9 character "rdoD01Yes"
                {
                    // Defect 18 grade is invert of other defect code. refer requirement excel sheet.
                    if (itemName.Substring(3, 3) != "D18" && itemName.Substring(3, 3) != "D32" && itemName.Substring(3, 3) != "D33")
                    {
                        lbl.Text = "R";
                        lbl.ForeColor = Color.Red;
                    }
                    else if (itemName.Substring(3, 3) == "D32")
                    {
                        lbl.Text = "C";
                        lbl.ForeColor = Color.DarkOrange;
                    }
                    else if (itemName.Substring(3, 3) == "D33")
                    {
                        lbl.Text = "E";
                        lbl.ForeColor = Color.DarkOrange;
                    }
                    else
                    {
                        lbl.Text = strGrade;
                        lbl.ForeColor = Color.Black;
                    }
                }
                else if (((RadioButton)sender).Name.Length == 8) // name for radio "No" has 8 character "rdoD01No"
                {
                    // Defect 18 grade is invert of other defect code. refer requirement excel sheet.
                    if (itemName.Substring(3, 3) == "D18")
                    {
                        lbl.Text = "R";
                        lbl.ForeColor = Color.Red;
                    }
                    else
                    {
                        lbl.Text = strGrade;
                        lbl.ForeColor = Color.Black;
                    }
                }
            }
            catch (Exception ex)
            {
                Program.WriteToGtErrorLog("GT", "frmInspect", MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        void lblGrade_TextChanged(object sender, EventArgs e, string nxtEle)
        {
            try
            {
                Label lbl = ((Label)sender);
                if (lbl.Text == "R")
                {
                    lblRGrade = lbl;
                    stopPnlGrow = true;
                }
                else
                {
                    if (lbl == lblRGrade) stopPnlGrow = false;
                    if (!(pnlObservation.Height >= lbl.Location.Y + (lbl.Height * 2)) && stopPnlGrow == false)
                    {
                        // combobox to show list without click dropdown.
                        if (nxtEle != "")
                        {
                            ComboBox cbo = (ComboBox)pnlObservation.Controls.Find("cbo" + nxtEle + "Obs", false)[0];
                            cbo.Focus();
                            cbo.DroppedDown = true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Program.WriteToGtErrorLog("GT", "frmInspect", MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        void txtD4MouldOpen_TextChanged(object sender, EventArgs e)
        {
            try
            {
                TextBox txt = (TextBox)sender;
                if (txt.Text.Trim() == "") return;
                decimal value = Convert.ToDecimal(txt.Text);
                if (lstMouldOpen.Count > 0)
                {
                    lblD04Grade.Text = "";
                    for (int i = 0; i < lstMouldOpen.Count; i++)
                    {
                        if (value >= lstMouldOpen[i].IFrom && value <= lstMouldOpen[i].ITo)
                        {
                            lblD04Grade.Text = lstMouldOpen[i].Grade;
                            return;
                        }
                    }
                    if (lblD04Grade.Text == "")
                    {
                        SqlParameter[] spVal = new SqlParameter[] { new SqlParameter("@TyreSize", lbl_D4TyreSize.Text) };
                        DataTable dtD4Range = (DataTable)dba.ExecuteReader_SP("sp_sel_DefectD4_Allowance", spVal, DBAccess.Return_Type.DataTable);
                        MessageBox.Show("Enter mould open from " + dtD4Range.Rows[0]["MinVal"].ToString() + " to " + dtD4Range.Rows[0]["MaxVal"].ToString());
                        txtD4MouldOpen.Text = "";
                        txtD4MouldOpen.Focus();
                    }
                }
                else
                {
                    MessageBox.Show("Contact administrator for mould open range", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtD4MouldOpen.Text = "";
                    txtD4MouldOpen.Focus();
                }
            }
            catch (Exception ex)
            {
                Program.WriteToGtErrorLog("GT", "frmInspect", MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        private void clear()
        {
            try
            {
                lstMouldOpen.Clear();
                lblProcessID.Text = "PROCESS-ID";
                lblStencilNo.Text = "";
                txtBrand.Text = "";
                txtCuringTime.Text = "__:__";
                txtRimSize.Text = "";
                txtMouldCode.Text = "";
                txtPlatform.Text = "";
                txtSidewall.Text = "";
                txtTyreSize.Text = "";
                txtType.Text = "";
                txtLoadedOn.Text = "";
                txtLoadedBy.Text = "";
                txtUnloadedOn.Text = "";
                txtUnloadedBy.Text = "";
                txtRemarks.Text = "";
                btnReset.Visible = false;
                btnInspect.Enabled = false;
                txtCycleStart.Text = "";
                txtExpCureEnd.Text = "";
                txtActCureEnd.Text = "";
                txtActCutOff.Text = "";
                txtTempTop.Text = "";
                txtTempBottom.Text = "";
                txtPress.Text = "";
                txtPressSeq.Text = "";
                txtLoadMachine.Text = "";
                txtPumbMachine.Text = "";
                txtTempTop.Text = "";
                txtTempBottom.Text = "";
                gpDefectConfirmation.Visible = false;
                pnlObservation.Visible = false;
                pnlCntrl.Visible = false;
                rdoGradeAccept_Aplus.Checked = false;
                rdoGradeReject.Checked = false;
                rdo_IDGuage_No.Checked = false;
                rdo_IDGuage_Yes.Checked = false;
                rdoMetelYes.Checked = false;
                rdoMetelNo.Checked = false;
                txt_HardnessBase.Text = "";
                txt_HardnessTread.Text = "";
                txtD4MouldOpen.Text = "";
                lblDelayMsg.Text = "";

                foreach (Control ctrl in pnlObservation.Controls)
                {
                    switch (ctrl.GetType().ToString())
                    {
                        case "System.Windows.Forms.Label":
                            Label lbl = (Label)ctrl;
                            if (lbl.Name.Contains("lbl")) lbl.Text = "";
                            break;
                        case "System.Windows.Forms.GroupBox":
                            GroupBox grp = (GroupBox)ctrl;
                            foreach (RadioButton rdo in grp.Controls.OfType<RadioButton>())
                                rdo.Checked = false;
                            break;
                        case "System.Windows.Forms.ComboBox":
                            ComboBox cbo = (ComboBox)ctrl;
                            cbo.SelectedIndex = 0;
                            break;
                        case "System.Windows.Forms.TextBox":
                            TextBox txt = (TextBox)ctrl;
                            txt.Text = "";
                            break;
                    }
                }
                pnlObservation.Visible = false;

                txtStencilNo.Text = (Program.strPlantName == "SLTL" ? "L" : Program.strPlantName == "MMN" ? "C" : Program.strPlantName.Substring(0, 1)) +
                    DateTime.Now.Year.ToString().Substring(2, 2) + DateTime.Now.Month.ToString("00");
                txtStencilNo.SelectionStart = txtStencilNo.Text.Length;
                txtStencilNo.Focus();
            }
            catch (Exception ex)
            {
                Program.WriteToGtErrorLog("GT", "frmInspect", MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        private void bindGV(string strStencilNo)
        {
            try
            {
                SqlParameter[] spList = new SqlParameter[] { new SqlParameter("@StencilNo", strStencilNo) };
                DataTable dtStencil = (DataTable)dba.ExecuteReader_SP("sp_sel_Inspect_StencilList", spList, DBAccess.Return_Type.DataTable);
                if (dtStencil.Rows.Count > 0)
                {
                    gvTyreInspection.DataSource = dtStencil;
                    if (strStencilNo != "")
                        gvTyreInspection_CellContentClick(null, null);
                }
                else
                    MessageBox.Show("NO RECORDS");
            }
            catch (Exception ex)
            {
                Program.WriteToGtErrorLog("GT", "frmInspect", MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        private void gvTyreInspection_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                clear();
                gvTyreInspection.DefaultCellStyle.SelectionBackColor = gvTyreInspection.DefaultCellStyle.BackColor;
                gvTyreInspection.DefaultCellStyle.SelectionForeColor = gvTyreInspection.DefaultCellStyle.ForeColor;
                Int32 rowindex = 0;
                if (e != null)
                    rowindex = e.RowIndex;
                gvTyreInspection.Rows[rowindex].DefaultCellStyle.SelectionBackColor = ColorTranslator.FromHtml("24, 152, 178");
                gvTyreInspection.Rows[rowindex].DefaultCellStyle.SelectionForeColor = Color.White;

                if (rowindex == 0 || rowindex > 0)
                {
                    lblStencilNo.Text = (gvTyreInspection.Rows[rowindex].Cells[0].Value).ToString();
                    bindTextFields();
                }
                gpDefectConfirmation.Visible = true;
                btnReset.Visible = true;
            }
            catch (Exception ex)
            {
                Program.WriteToGtErrorLog("GT", "frmInspect", MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        private void bindTextFields()
        {
            try
            {
                SqlParameter[] sp = new SqlParameter[] { new SqlParameter("@StencilNo", lblStencilNo.Text) };
                DataTable dt = (DataTable)dba.ExecuteReader_SP("SP_SEL_TyreInspect_StencilDetails", sp, DBAccess.Return_Type.DataTable);
                if (dt.Rows.Count > 0)
                {
                    lblProcessID.Text = dt.Rows[0]["Process_Id"].ToString();
                    txtMouldCode.Text = dt.Rows[0]["MouldCode"].ToString();
                    txtPlatform.Text = dt.Rows[0]["Config"].ToString();
                    txtBrand.Text = dt.Rows[0]["Brand"].ToString();
                    txtSidewall.Text = dt.Rows[0]["Sidewall"].ToString();
                    txtTyreSize.Text = dt.Rows[0]["TyreSize"].ToString();
                    lbl_D4TyreSize.Text = dt.Rows[0]["D4Tyresize"].ToString();
                    txtRimSize.Text = dt.Rows[0]["RimSize"].ToString();
                    txtType.Text = dt.Rows[0]["TyreType"].ToString();
                    txtLoadedOn.Text = Convert.ToDateTime(dt.Rows[0]["Loaded_Date"]).ToString("dd/MM/yyyy") + " " + Convert.ToDateTime(dt.Rows[0]["Loaded_Date"].ToString()).ToString("HH:mm");
                    txtLoadedBy.Text = dt.Rows[0]["Loaded_By"].ToString();
                    txtTempTop.Text = dt.Rows[0]["MouldTemp"].ToString();
                    txtTempBottom.Text = dt.Rows[0]["MouldTempBottom"].ToString();
                    txtUnloadedOn.Text = Convert.ToDateTime(dt.Rows[0]["Unloaded_Date"]).ToString("dd/MM/yyyy") + " " + Convert.ToDateTime(dt.Rows[0]["Unloaded_Date"].ToString()).ToString("HH:mm");
                    txtUnloadedBy.Text = dt.Rows[0]["unloaded_By"].ToString();
                    txtPress.Text = dt.Rows[0]["Press"].ToString();
                    txtPressSeq.Text = dt.Rows[0]["PressB"].ToString();
                    txtLoadMachine.Text = dt.Rows[0]["Loading_Machine"].ToString();
                    txtPumbMachine.Text = dt.Rows[0]["Pumping_Machine"].ToString();

                    btnInspect.Visible = false;
                    lblDelayMsg.Text = "";
                    if (Convert.ToDateTime(dt.Rows[0]["Unloaded_Date"].ToString()).AddHours(96) >= System.DateTime.Now || Program.boolInspectDelayApproval)
                        btnInspect.Visible = true;

                    if (Convert.ToDateTime(dt.Rows[0]["Unloaded_Date"].ToString()).AddHours(96) <= System.DateTime.Now)
                        lblDelayMsg.Text = "TYRE SHOULD INSPECT UPTO 4 DAYS. GET APPROVAL FROM SUPERVISOR";

                    SqlParameter[] spD4 = new SqlParameter[] { new SqlParameter("@TyreSize", lbl_D4TyreSize.Text) };
                    DataTable dtD4List = (DataTable)dba.ExecuteReader_SP("sp_DefectD4_Tyresize_Wise", spD4, DBAccess.Return_Type.DataTable);
                    if (dtD4List != null && dtD4List.Rows.Count > 0)
                    {
                        for (int i = 0; i < dtD4List.Rows.Count; i++)
                        {
                            structMouldOpen obj = new structMouldOpen();
                            obj.IFrom = dtD4List.Rows[i]["d4From"].ToString() != "" ? Convert.ToDecimal(dtD4List.Rows[i]["d4From"].ToString()) : -1;
                            obj.ITo = dtD4List.Rows[i]["d4To"].ToString() != "" ? Convert.ToDecimal(dtD4List.Rows[i]["d4To"].ToString()) : -1;
                            obj.Grade = dtD4List.Rows[i]["GRADE"].ToString();
                            lstMouldOpen.Add(obj);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Contact administrator for D4 defect master mould open value to tyresize " + lbl_D4TyreSize.Text, "Error", MessageBoxButtons.OK,
                            MessageBoxIcon.Error);
                        pnlObservation.Visible = false;
                    }
                }
            }
            catch (Exception ex)
            {
                Program.WriteToGtErrorLog("GT", "frmInspect", MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        private void btnInspect_Click(object sender, EventArgs e)
        {
            try
            {
                bool boolSave = false;
                bool boolSaveLabel = false;
                string strErrMsg = "";
                if (lblStencilNo.Text != "")
                {
                    if (rdoGradeAccept_Aplus.Checked)
                    {
                        boolSave = true;
                        foreach (Label lbl in pnlObservation.Controls.OfType<Label>())
                        {
                            if (lbl.Name.StartsWith("lblD") && lbl.Text.Trim() != "")
                            {
                                if (lbl.Text != "A+" && lbl.Text != "A") // if any one grade != A+ or A then break the loop
                                {
                                    boolSaveLabel = false;
                                    break;
                                }
                                else if (lbl.Text == "A+" || lbl.Text == "A") // continue loop to chk if all grade is A or A+
                                    boolSaveLabel = true;
                            }
                            else if (lbl.Name.StartsWith("lblD") && lbl.Text.Trim() == "")
                                strErrMsg = "Enter defect " + lbl.Name.Replace("lbl", "").Replace("Grade", "");
                        }
                    }
                    else if (rdoGradeReject.Checked)
                    {
                        boolSaveLabel = false;
                        foreach (Label lbl in pnlObservation.Controls.OfType<Label>())
                        {
                            if (lbl.Name.StartsWith("lblD") && lbl.Text.Trim() != "")
                            {
                                if (lbl.Text == "A+" || lbl.Text == "A") // continue loop to chk all grade having A+ or A
                                    boolSave = false;
                                else
                                {
                                    boolSave = true; // if any one grade is != A+ or A then break the loop
                                    break;
                                }
                            }
                            else if (lbl.Name.StartsWith("lblD") && lbl.Text.Trim() == "")
                                strErrMsg = "Enter defect " + lbl.Name.Replace("lbl", "").Replace("Grade", "");
                        }
                    }
                    if (strErrMsg.Length > 0)
                        MessageBox.Show(strErrMsg, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    else
                    {
                        //bool imgUpload = true;
                        //if (!boolSaveLabel)
                        //{
                        //    SqlParameter[] sp = new SqlParameter[] { new SqlParameter("@stencilno", lblStencilNo.Text) };
                        //    DataTable dtImg = (DataTable)dba.ExecuteReader_SP("sp_sel_defect_images", sp, DBAccess.Return_Type.DataTable);
                        //    if (dtImg.Rows.Count != 2)
                        //        imgUpload = false;
                        //}
                        //if (!imgUpload)
                        //{
                        //    MessageBox.Show("Upload stencil & defect images");
                        //    if (lblStencilNo.Text != "")
                        //    {
                        //        Form childForm = new frmDefectImage();
                        //        childForm.Text = lblStencilNo.Text;
                        //        strImgMethod = "BEFORE";
                        //        childForm.Show();
                        //        childForm.WindowState = FormWindowState.Maximized;
                        //    }
                        //}
                        //else 
                        if (rdoGradeReject.Checked && !boolSave)
                            MessageBox.Show("You choosed '" + rdoGradeReject.Text.ToUpper() + "' option. \n So you must choose atleast any one grade Instead of A+ and A !!..",
                                              "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        else if (txtD4MouldOpen.Text.Trim() == "")
                            MessageBox.Show("Enter MouldOpen", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        else if (rdo_IDGuage_No.Checked == false && rdo_IDGuage_Yes.Checked == false)
                            MessageBox.Show("Choose ID Gauge Check", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        else if (rdoMetelYes.Checked == false && rdoMetelNo.Checked == false)
                            MessageBox.Show("Choose Metel Detector", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        else if (txt_HardnessTread.Text.Trim() == "")
                            MessageBox.Show("Enter Tread Hardness", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        else if (txt_HardnessBase.Text.Trim() == "")
                            MessageBox.Show("Enter Base Hardness", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        else if (txtRemarks.Text.Trim() == "" && (boolSaveLabel == false || lblDelayMsg.Text != ""))
                            MessageBox.Show("Enter Remarks", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        else if (boolSave)
                        {
                            SqlParameter[] sp = new SqlParameter[] { 
                                new SqlParameter("@StencilNo", lblStencilNo.Text), new SqlParameter("@Username", Program.strUserName.ToString()), 
                                new SqlParameter("@D1", lblD01Grade.Text.Trim()), new SqlParameter("@D2", lblD02Grade.Text.Trim()), 
                                new SqlParameter("@D3", lblD03Grade.Text.Trim()), new SqlParameter("@D4", lblD04Grade.Text.Trim()), 
                                new SqlParameter("@D5", lblD05Grade.Text.Trim()), new SqlParameter("@D6", lblD06Grade.Text.Trim()), 
                                new SqlParameter("@D7", lblD07Grade.Text.Trim()), new SqlParameter("@D8", lblD08Grade.Text.Trim()), 
                                new SqlParameter("@D9", lblD09Grade.Text.Trim()), new SqlParameter("@D10", lblD10Grade.Text.Trim()), 
                                new SqlParameter("@D11", lblD11Grade.Text.Trim()), new SqlParameter("@D12", lblD12Grade.Text.Trim()), 
                                new SqlParameter("@D13", lblD13Grade.Text.Trim()), new SqlParameter("@D14", lblD14Grade.Text.Trim()), 
                                new SqlParameter("@D15", lblD15Grade.Text.Trim()), new SqlParameter("@D16", lblD16Grade.Text.Trim()), 
                                new SqlParameter("@D17", lblD17Grade.Text.Trim()), new SqlParameter("@D18", lblD18Grade.Text.Trim()), 
                                new SqlParameter("@D19", lblD19Grade.Text.Trim()), new SqlParameter("@D20", lblD20Grade.Text.Trim()), 
                                new SqlParameter("@D21", lblD21Grade.Text.Trim()), new SqlParameter("@D22", lblD22Grade.Text.Trim()), 
                                new SqlParameter("@D23", lblD23Grade.Text.Trim()), new SqlParameter("@D24", lblD24Grade.Text.Trim()), 
                                new SqlParameter("@D25", lblD25Grade.Text.Trim()), new SqlParameter("@D26", lblD26Grade.Text.Trim()), 
                                new SqlParameter("@D27", lblD27Grade.Text.Trim()), new SqlParameter("@D28", lblD28Grade.Text.Trim()), 
                                new SqlParameter("@D29", lblD29Grade.Text.Trim()), new SqlParameter("@Remarks", txtRemarks.Text), 
                                new SqlParameter("@Inspected_Status", Convert.ToBoolean(0)), new SqlParameter("@D4MouldOpen", txtD4MouldOpen.Text),
                                new SqlParameter("@IDGauge",Convert.ToBoolean(rdo_IDGuage_Yes.Checked ? 1 : 0)),
                                new SqlParameter("@MetelDetector",Convert.ToBoolean(rdoMetelYes.Checked ? 1 : 0)),
                                new SqlParameter("@HardenessTread", txt_HardnessTread.Text),new SqlParameter("@HardnessBase", txt_HardnessBase.Text), 
                                new SqlParameter("@D30", lblD30Grade.Text.Trim()), new SqlParameter("@D31", lblD31Grade.Text.Trim()), 
                                new SqlParameter("@D32", lblD32Grade.Text.Trim()), new SqlParameter("@D33", lblD33Grade.Text.Trim()) 
                            };
                            int resp = dba.ExecuteNonQuery_SP("SP_UPD_TyreInspect_InspectionDetails", sp);
                            if (resp > 0 && boolSaveLabel)
                            {
                                string strBarcode = (lblProcessID.Text + lblStencilNo.Text + "A").ToUpper();
                                SqlParameter[] sp1 = new SqlParameter[] { 
                                    new SqlParameter("@StencilNo", lblStencilNo.Text), 
                                    new SqlParameter("@Barcode", strBarcode), 
                                    new SqlParameter("@Grade", "A"),
                                    new SqlParameter("@username", Program.strUserName)
                                };
                                dba.ExecuteNonQuery_SP("sp_ins_tbQualityCntrl_V2", sp1);
                                Program.PreparePrintLable(strBarcode);

                                sp = new SqlParameter[] { 
                                    new SqlParameter("@StencilNo", lblStencilNo.Text), 
                                    new SqlParameter("@Approved_User", ""), 
                                    new SqlParameter("@Approved_Remarks", "") 
                                };
                                dba.ExecuteNonQuery_SP("sp_upt_TyreFinalinspect", sp);
                            }
                            MessageBox.Show("Inspected Succesfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            bindGV("");
                            pnlCntrl.Visible = false;
                            clear();
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Please select 'STENCIL NO' and 'GRADE'", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                Program.WriteToGtErrorLog("GT", "frmInspect", MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        private void txtNumericPercentage_KeyPress(object sender, KeyPressEventArgs e)
        {
            Program.digitonly(e);
        }

        private void rdoDefectYes_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                // Defect Confromation Check
                string strGrade = "";
                if (rdoGradeAccept_Aplus.Checked)// Accept A+
                    strGrade = "A+";
                else if (rdoGradeReject.Checked) // Reject
                    strGrade = "A+";
                foreach (Control ctrl in pnlObservation.Controls)
                {
                    switch (ctrl.GetType().ToString())
                    {
                        case "System.Windows.Forms.GroupBox":
                            GroupBox grp = (GroupBox)ctrl;
                            foreach (RadioButton rdo in grp.Controls.OfType<RadioButton>())
                            {
                                if (rdo.Text == "NO")
                                    rdo.Checked = true;
                                if (rdo.Name.Contains("rdoD18"))
                                {
                                    rdoD18Yes.Checked = true;
                                    rdoD18No.Checked = false;
                                }
                            }
                            break;
                        case "System.Windows.Forms.ComboBox":
                            ComboBox cbo = (ComboBox)ctrl;
                            cbo.SelectedIndex = strGrade == "A" ? 1 : 2;
                            break;
                        case "System.Windows.Forms.Label":
                            Label lbl = (Label)ctrl;
                            if (lbl.Name.StartsWith("lblD"))
                                lbl.Text = strGrade;
                            break;
                    }
                }
                txtD4MouldOpen.Text = "";
                SqlParameter[] spTo = new SqlParameter[] { new SqlParameter("@TyreSize", lbl_D4TyreSize.Text), new SqlParameter("@Grade", lblD04Grade.Text) };
                string strMouldOpen = (string)dba.ExecuteScalar_SP("sp_sel_MouldOpen_Range", spTo);
                if (strMouldOpen != null && strMouldOpen != null)
                    txtD4MouldOpen.Text = strMouldOpen;

                pnlObservation.Visible = true;
                pnlCntrl.Visible = true;
                btnInspect.Enabled = true;
                txtRemarks.Focus();
            }
            catch (Exception ex)
            {
                Program.WriteToGtErrorLog("GT", "frmInspect", MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        private void btnReset_Click_1(object sender, EventArgs e)
        {
            clear();
            bindGV("");
        }

        private void btn_Clear_Click(object sender, EventArgs e)
        {
            rdo_IDGuage_No.Checked = false;
            rdo_IDGuage_Yes.Checked = false;
            rdoMetelYes.Checked = false;
            rdoMetelNo.Checked = false;
            txt_HardnessBase.Text = "";
            txt_HardnessTread.Text = "";
            txtRemarks.Text = "";
        }

        private void txtStencilNo_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13 && txtStencilNo.Text.Length == 10)
                bindGV(txtStencilNo.Text);
        }
    }
}
